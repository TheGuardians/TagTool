using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Resources;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Tags
{
    /// <summary>
    /// Builds a Halo Online (ms29/ms30) "zone" (cache_file_resource_gestalt) tag for a scenario.
    ///
    /// ms30 loads a map's resources through the zone tag named in the .map header (0x2E04): each map has its own zone whose
    /// TagResourceTable holds a copy of the inline PageableResource of every tag the map uses, with Page.Index replaced by the
    /// resource's byte offset in its .dat file. This command rebuilds that table from the tag graph:
    ///
    ///  - tags: depth-first walk from the cache_file_global_tags globals and the scenario (the scenario is walked before
    ///    matg). A tag's own resources are emitted when it is first visited, in its resource-pointer order. Its children are
    ///    visited in field order (RenderMethod.Parameters skipped - source data; RenderMethod.ShaderProperties visited last -
    ///    postprocess data), restricted to the tag's stored dependency list, followed by any remaining stored dependencies.
    ///    Structure BSPs are skipped during the walk and visited at the end.
    ///  - tags that nothing but zone tags reference and that most existing zones contain ("global orphans") are appended.
    ///
    /// On the 13 shipped ms30 maps this reproduces 99.4% of each zone's entries (the rest are empty placeholder entries and
    /// tags left over from earlier builds). The manifests are built from the other shipped zones' classification of each
    /// tag plus structural rules for tags they don't contain - see BuildManifests.
    /// </summary>
    class GenerateZoneCommand : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;
        private Stream CacheStream;

        public GenerateZoneCommand(GameCacheHaloOnlineBase cache)
            : base(false,
                  "GenerateZone",
                  "Builds a Halo Online (ms29/ms30) zone (cache_file_resource_gestalt) tag for a scenario.",

                  "GenerateZone <scenario> [compare <zone>] [write <zone>|new] [mapid <id>] [unseen required|learned] [optional votes|none] [add <tag>]...",

                  "Builds the per-map resource gestalt for <scenario> from the tag graph.\n" +
                  "  compare <zone>  compares the generated zone against an existing zone tag and prints a report (nothing is written)\n" +
                  "  write <zone>    overwrites an existing zone tag with the generated one\n" +
                  "  write new       allocates a new zone tag named after the scenario\n" +
                  "  mapid <id>      map id to store in the zone (default: taken from the compare/write zone, else the scenario's)\n" +
                  "  unseen <mode>   how to classify weapon/vehicle/armor content that no existing zone contains:\n" +
                  "                    required (default, safe) - always loaded with the map\n" +
                  "                    learned  - required or optional (loaded once the object is created), following the pattern\n" +
                  "                               the existing zones use for the object fields that reach it (roughly 85% accurate)\n" +
                  "  optional <mode> how resources are split between required and optional:\n" +
                  "                    votes (default) - follow the existing zones (and the unseen rule), as ms30 does\n" +
                  "                    none - nothing is optional and no zoneset objects are written, as H3/ODST tool.exe does\n" +
                  "                           (its s_tag_resource_definition::optional is a stub returning false). Loads everything\n" +
                  "                           up front: on the stock maps that overflows ms30's resource memory and the map crashes\n" +
                  "                           on load (s3d_turf, 2.5 GB required), so it only suits small maps\n" +
                  "  add <tag>       (repeatable) instead of replacing the zone, appends the resources of <tag> and the tags it\n" +
                  "                  references that the zone doesn't have yet (e.g. newly ported tags). Existing entries keep their\n" +
                  "                  indices and classification; the new ones are classified as a full generation would. Use with\n" +
                  "                  'write <zone>' to modify the map's zone, or 'compare <zone>' to only list what would be added.\n" +
                  "The zone tag index must be written to the map's .map header at 0x2E04.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);
            if (!Cache.TagCache.TryGetTag(args[0], out var scenarioTag) || !scenarioTag.IsInGroup("scnr"))
                return new TagToolError(CommandError.TagInvalid, args[0]);

            CachedTag compareTag = null, writeTag = null;
            bool writeNew = false;
            int mapId = int.MinValue;
            var addTags = new List<CachedTag>();
            for (int i = 1; i < args.Count; i += 2)
            {
                if (i + 1 >= args.Count)
                    return new TagToolError(CommandError.ArgCount);
                switch (args[i].ToLowerInvariant())
                {
                    case "compare":
                        if (!Cache.TagCache.TryGetTag(args[i + 1], out compareTag) || !compareTag.IsInGroup("zone"))
                            return new TagToolError(CommandError.TagInvalid, args[i + 1]);
                        break;
                    case "write":
                        if (args[i + 1].ToLowerInvariant() == "new") writeNew = true;
                        else if (!Cache.TagCache.TryGetTag(args[i + 1], out writeTag) || !writeTag.IsInGroup("zone"))
                            return new TagToolError(CommandError.TagInvalid, args[i + 1]);
                        break;
                    case "unseen":
                        UnseenMode = args[i + 1].ToLowerInvariant();
                        if (UnseenMode != "required" && UnseenMode != "learned")
                            return new TagToolError(CommandError.ArgInvalid, args[i + 1]);
                        break;
                    case "optional":
                        OptionalMode = args[i + 1].ToLowerInvariant();
                        if (OptionalMode != "votes" && OptionalMode != "none")
                            return new TagToolError(CommandError.ArgInvalid, args[i + 1]);
                        break;
                    case "add":
                        if (!Cache.TagCache.TryGetTag(args[i + 1], out var addTag) || addTag.IsInGroup("zone"))
                            return new TagToolError(CommandError.TagInvalid, args[i + 1]);
                        addTags.Add(addTag);
                        break;
                    case "mapid":
                        if (!int.TryParse(args[i + 1], out mapId))
                            return new TagToolError(CommandError.ArgInvalid, args[i + 1]);
                        break;
                    default:
                        return new TagToolError(CommandError.ArgInvalid, args[i]);
                }
            }

            using (CacheStream = Cache.OpenCacheReadWrite())
            {
                ResourceGestalt existing = null;
                if (compareTag != null) existing = Cache.Deserialize<ResourceGestalt>(CacheStream, compareTag);
                else if (writeTag != null) existing = Cache.Deserialize<ResourceGestalt>(CacheStream, writeTag);
                var template = existing ?? Cache.Deserialize<ResourceGestalt>(CacheStream, Cache.TagCache.NonNull().First(t => t.IsInGroup("zone")));
                // stock maps use the same id in the .map header (0x2DFC), the scenario and the zone
                if (mapId == int.MinValue) mapId = existing != null && existing.MapId != -1 ? existing.MapId : Cache.Deserialize<Scenario>(CacheStream, scenarioTag).MapId;

                TargetZone = compareTag ?? writeTag;
                LearnPlaceholders();
                var layout = BuildLayout(scenarioTag);
                var visitPath = Environment.GetEnvironmentVariable("GENERATEZONE_VISIT");
                if (visitPath != null) File.WriteAllLines(visitPath, layout.VisitOrder.Select(t => $"{t:X4},{Cache.TagCache.GetTag(t)?.Group.Tag}"));
                var edgesPath = Environment.GetEnvironmentVariable("GENERATEZONE_EDGES");
                if (edgesPath != null) DumpEdges(edgesPath);
                var zone = BuildZone(layout, template, scenarioTag, mapId);

                if (addTags.Count > 0)
                {
                    if (existing == null || writeNew)
                        return new TagToolError(CommandError.ArgInvalid, "add needs 'write <zone>' or 'compare <zone>' with the map's existing zone");
                    int added = AddToZone(existing, zone, layout, addTags);
                    if (writeTag != null && added > 0)
                    {
                        Cache.Serialize(CacheStream, writeTag, existing);
                        Console.WriteLine($"Wrote zone 0x{writeTag.Index:X4} ({existing.TagResourceTable.Count} resources, {added} added).");
                    }
                    else if (added > 0)
                        Console.WriteLine("Not written (use 'write <zone>' instead of 'compare <zone>' to add them).");
                    return true;
                }

                if (compareTag != null)
                {
                    Compare(zone, existing, layout);
                    return true;
                }

                if (writeTag == null && writeNew)
                {
                    writeTag = Cache.TagCache.AllocateTag<ResourceGestalt>(scenarioTag.Name);
                    Console.WriteLine($"Allocated zone tag 0x{writeTag.Index:X4} {writeTag.Name}.zone");
                }
                if (writeTag != null)
                {
                    Cache.Serialize(CacheStream, writeTag, zone);
                    Console.WriteLine($"Wrote zone 0x{writeTag.Index:X4} ({zone.TagResourceTable.Count} resources). Set the map header's zone index (0x2E04) to 0x{writeTag.Index:X}.");
                }
                else
                {
                    Console.WriteLine($"Generated {zone.TagResourceTable.Count} resource entries (not written; use 'write <zone>' or 'write new').");
                }
            }
            return true;
        }

        //
        // Tag walk
        //

        private class Layout
        {
            public List<(int Tag, PageableResource Resource)> Entries = new List<(int, PageableResource)>();
            public HashSet<int> GlobalEntries = new HashSet<int>();   // entry indices reached before the deferred BSP pass
            public HashSet<int> BspEntries = new HashSet<int>();      // entry indices reached from structure BSPs
            public List<int> Bsps = new List<int>();
            public int FailedTags;
            public Dictionary<int, int> Referrer = new Dictionary<int, int>();   // tag -> tag it was first reached from
            public List<int> Roots = new List<int>();                           // walk roots (globals, scenario, orphans)
            public List<int> VisitOrder = new List<int>();
        }

        private readonly Dictionary<int, int[]> StoredDependencies = new Dictionary<int, int[]>();
        private readonly Dictionary<int, List<int>> FieldReferences = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, List<(PageableResource Resource, bool Direct)>> TagResources = new Dictionary<int, List<(PageableResource, bool)>>();

        // Source-only fields the cache builder does not follow, and fields it resolves after the rest of the tag.
        private static readonly HashSet<string> SkippedFields = new HashSet<string> { "RenderMethod.Parameters" };
        private static readonly HashSet<string> LateFields = new HashSet<string> { "RenderMethod.ShaderProperties" };

        private int[] Dependencies(int index)
        {
            if (StoredDependencies.TryGetValue(index, out var d)) return d;
            // stored order matters, and CachedTagHaloOnline.Dependencies is a set, so read the list from the tag header
            var tag = (CachedTagHaloOnline)Cache.TagCache.GetTag(index);
            var list = new List<int>();
            if (tag != null)
            {
                var data = Cache.TagCacheGenHO.ExtractTagRaw(CacheStream, tag);
                int count = BitConverter.ToInt16(data, 0x08);
                for (int i = 0; i < count; i++)
                    list.Add(BitConverter.ToInt32(data, 0x24 + i * 4));
            }
            return StoredDependencies[index] = list.ToArray();
        }

        // Tags of groups TagTool has no definition for (e.g. ms30's sweg/swel) make Cache.Deserialize throw from deep inside
        // the serializer; return null for them instead, without an exception, so callers fall back to stored dependencies.
        private object TryDeserialize(CachedTag tag)
        {
            if (tag == null || Cache.TagCache.TagDefinitions.GetTagDefinitionType(tag.Group) == null)
                return null;
            return Cache.Deserialize(CacheStream, tag);
        }

        private List<int> References(int index, Layout layout)
        {
            if (FieldReferences.TryGetValue(index, out var refs)) return refs;
            refs = new List<int>();
            var tag = Cache.TagCache.GetTag(index);
            try
            {
                var definition = TryDeserialize(tag);
                if (definition == null)
                {
                    layout.FailedTags++;
                    return FieldReferences[index] = null;   // no definition: use the stored dependency order
                }
                var late = new List<int>();
                CollectReferences(definition, refs, late, 0);
                refs.AddRange(late);
            }
            catch
            {
                refs = null;   // definition not supported: fall back to the stored dependency order
                layout.FailedTags++;
            }
            return FieldReferences[index] = refs;
        }

        //
        // Blocks sorted by tag postprocessing
        //
        // The cache builder (tool) loads tags - and so orders the zone - by walking the SOURCE tags. Some tag groups sort
        // blocks during postprocess, so the cached element order is the sorted one. For those blocks the walk rebuilds the
        // source order by inverting the sort, assuming the source keys were already in sorted order (as authored); ties are
        // then placed exactly where Bungie's qsort_elements / shortsort_elements (ms30 0x5666E0 / 0x566920, the old CRT
        // quicksort) would have left them.
        //
        // comparer(a, b) = true when a sorts after b (qsort_elements' boolean comparison).
        private static readonly Dictionary<string, Func<object, object, bool>> SortedBlocks = new Dictionary<string, Func<object, object, bool>>
        {
            // model damage section instant responses: descending damage threshold (verified against the H3EK source
            // hornet.model and the H3EK build's tag load order)
            ["DamageSection.InstantResponses"] = (a, b) =>
                ((Model.GlobalDamageInfoBlock.DamageSection.InstantResponse)a).DamageThreshold < ((Model.GlobalDamageInfoBlock.DamageSection.InstantResponse)b).DamageThreshold,
        };

        private static IList SourceOrder(string fieldName, IList sorted)
        {
            if (sorted.Count < 2 || !SortedBlocks.TryGetValue(fieldName, out var after))
                return sorted;
            // sort the source positions 0..n-1, whose keys are the sorted keys: result[k] = source index of sorted element k
            var positions = Enumerable.Range(0, sorted.Count).ToArray();
            QSortElements(positions, (x, y) => after(sorted[x], sorted[y]));
            var source = new object[sorted.Count];
            for (int k = 0; k < positions.Length; k++)
                source[positions[k]] = sorted[k];
            return source;
        }

        // Port of Bungie's qsort_elements (old CRT qsort: middle pivot swapped to lo, explicit stack, smaller side first).
        private static void QSortElements(int[] a, Func<int, int, bool> after)
        {
            if (a.Length < 2) return;
            void Swap(int i, int j) { var t = a[i]; a[i] = a[j]; a[j] = t; }
            var loStack = new Stack<int>(); var hiStack = new Stack<int>();
            int lo = 0, hi = a.Length - 1;
            while (true)
            {
                int size = hi - lo + 1;
                if (size <= 8)
                {
                    ShortSortElements(a, lo, hi, after);
                }
                else
                {
                    Swap(lo + size / 2, lo);
                    int loguy = lo, higuy = hi + 1;
                    while (true)
                    {
                        do loguy++; while (loguy <= hi && !after(a[loguy], a[lo]));
                        do higuy--; while (higuy > lo && !after(a[lo], a[higuy]));
                        if (higuy < loguy) break;
                        Swap(loguy, higuy);
                    }
                    Swap(lo, higuy);
                    if (higuy - 1 - lo >= hi - loguy)
                    {
                        if (lo + 1 < higuy) { loStack.Push(lo); hiStack.Push(higuy - 1); }
                        if (loguy < hi) { lo = loguy; continue; }
                    }
                    else
                    {
                        if (loguy < hi) { loStack.Push(loguy); hiStack.Push(hi); }
                        if (lo + 1 < higuy) { hi = higuy - 1; continue; }
                    }
                }
                if (loStack.Count == 0) break;
                lo = loStack.Pop(); hi = hiStack.Pop();
            }
        }

        // Port of shortsort_elements: selection sort moving the first strictly-largest element to hi each pass.
        private static void ShortSortElements(int[] a, int lo, int hi, Func<int, int, bool> after)
        {
            while (hi > lo)
            {
                int max = lo;
                for (int p = lo + 1; p <= hi; p++)
                    if (after(a[p], a[max])) max = p;
                var t = a[max]; a[max] = a[hi]; a[hi] = t;
                hi--;
            }
        }

        // Debugging aid: every tag reference with the field path it is stored in.
        private static void CollectEdges(object obj, string path, List<(string Path, int Tag)> edges, int depth)
        {
            if (obj == null || depth > 64) return;
            foreach (var field in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                object value;
                try { value = field.GetValue(obj); } catch { continue; }
                if (value == null) continue;
                var name = path.Length == 0 ? field.Name : path + "." + field.Name;
                switch (value)
                {
                    case CachedTag t: if (t.Index >= 0) edges.Add((name, t.Index)); break;
                    case TagStructure s: CollectEdges(s, name, edges, depth + 1); break;
                    case IList list when !(value is byte[]):
                        foreach (var e in list)
                        {
                            if (e is CachedTag lt) { if (lt.Index >= 0) edges.Add((name, lt.Index)); }
                            else if (e is TagStructure) CollectEdges(e, name, edges, depth + 1);
                        }
                        break;
                }
            }
        }

        private void DumpEdges(string path)
        {
            using (var w = new StreamWriter(path))
            {
                w.WriteLine("parent,parent_group,field,child,child_group");
                foreach (var tag in Cache.TagCache.NonNull())
                {
                    if (tag.IsInGroup("zone")) continue;
                    var edges = new List<(string Path, int Tag)>();
                    bool ok = false;
                    try { var definition = TryDeserialize(tag); if (definition != null) { CollectEdges(definition, "", edges, 0); ok = true; } } catch { }
                    if (!ok) { edges.Clear(); foreach (var d in Dependencies(tag.Index)) edges.Add(("?", d)); }
                    foreach (var (p, c) in edges)
                        w.WriteLine($"0x{tag.Index:X4},{tag.Group.Tag},{p},0x{c:X4},{Cache.TagCache.GetTag(c)?.Group.Tag}");
                }
            }
        }

        private static void CollectReferences(object obj, List<int> refs, List<int> late, int depth)
        {
            if (obj == null || depth > 64) return;
            foreach (var field in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var name = field.DeclaringType.Name + "." + field.Name;
                if (SkippedFields.Contains(name)) continue;
                object value;
                try { value = field.GetValue(obj); } catch { continue; }
                if (value == null) continue;
                var target = late != null && LateFields.Contains(name) ? late : refs;
                var nextLate = target == late ? null : late;
                switch (value)
                {
                    case CachedTag t: if (t.Index >= 0) target.Add(t.Index); break;
                    case TagStructure s: CollectReferences(s, target, nextLate, depth + 1); break;
                    case IList list when !(value is byte[]):
                        foreach (var e in SourceOrder(name, list))
                        {
                            if (e is CachedTag lt) { if (lt.Index >= 0) target.Add(lt.Index); }
                            else if (e is TagStructure) CollectReferences(e, target, nextLate, depth + 1);
                        }
                        break;
                }
            }
        }

        // The tag's resources in its resource-pointer order, read straight from the tag data so that tags whose definition
        // doesn't deserialize still contribute. Empty or invalid slots are skipped. Direct = the resource reference is a field
        // of the tag's main struct (e.g. sbsp collision/pathfinding resources) rather than of a nested struct or block element.
        private List<(PageableResource Resource, bool Direct)> Resources(int index)
        {
            if (TagResources.TryGetValue(index, out var list)) return list;
            list = new List<(PageableResource, bool)>();
            var tag = (CachedTagHaloOnline)Cache.TagCache.GetTag(index);
            if (tag != null && tag.ResourcePointerOffsets.Count > 0 && !tag.IsInGroup("zone"))
            {
                // offsets of resource references declared directly on the main struct (not inside inline structs)
                var directOffsets = new HashSet<uint>();
                try
                {
                    var mainInfo = TagStructure.GetTagStructureInfo(Cache.TagCache.TagDefinitions.GetTagDefinitionType(tag.Group), Cache.Version, Cache.Platform);
                    foreach (var f in TagStructure.GetTagFieldEnumerable(mainInfo))
                        if (f.FieldType == typeof(TagResourceReference) || f.FieldType == typeof(PageableResource))
                            directOffsets.Add(tag.DefinitionOffset + f.Offset);
                }
                catch { }
                var context = new HaloOnlineSerializationContext(CacheStream, Cache, tag);
                var info = TagStructure.GetTagStructureInfo(typeof(PageableResource), Cache.Version, Cache.Platform);
                using (var reader = context.BeginDeserialize(info))
                {
                    foreach (var offset in tag.ResourcePointerOffsets)
                    {
                        reader.BaseStream.Position = offset;
                        var pointer = reader.ReadUInt32();
                        if (pointer == 0) continue;
                        reader.BaseStream.Position = tag.PointerToOffset(pointer);
                        var resource = (PageableResource)Cache.Deserializer.DeserializeStruct(reader, context, info);
                        bool direct = directOffsets.Contains(offset);
                        if (resource.Page != null && resource.Page.Index != -1)
                            list.Add((resource, direct));
                    }
                }
            }
            return TagResources[index] = list;
        }

        private Layout BuildLayout(CachedTag scenarioTag)
        {
            var layout = new Layout();
            var visited = new HashSet<int>();
            var deferred = new List<int>();
            bool inBspPass = false;
            var stack = new Stack<int>();

            void Visit(int index)
            {
                if (index < 0 || visited.Contains(index)) return;
                var tag = Cache.TagCache.GetTag(index);
                if (tag == null || tag.IsInGroup("zone")) return;
                if (!inBspPass && tag.IsInGroup("sbsp"))
                {
                    if (!deferred.Contains(index)) deferred.Add(index);
                    return;
                }
                visited.Add(index);
                layout.VisitOrder.Add(index);
                layout.Referrer[index] = stack.Count > 0 ? stack.Peek() : -1;
                stack.Push(index);
                try { VisitBody(index); } finally { stack.Pop(); }
            }

            void VisitBody(int index)
            {
                // resources in nested structs/blocks are emitted on visit, direct main-struct resources after the children
                void Emit(bool direct)
                {
                    var emitted = Resources(index).Where(r => r.Direct == direct).ToList();
                    if (emitted.Count == 0) return;
                    // empty entries the shipped zones keep around some tags' resources (culled animation groups)
                    for (int k = PlaceholdersBefore.TryGetValue(index, out var nb) ? nb : 0; k > 0; k--) layout.Entries.Add((-1, null));
                    foreach (var (resource, _) in emitted)
                    {
                        (inBspPass ? layout.BspEntries : layout.GlobalEntries).Add(layout.Entries.Count);
                        layout.Entries.Add((index, resource));
                    }
                    for (int k = PlaceholdersAfter.TryGetValue(index, out var na) ? na : 0; k > 0; k--) layout.Entries.Add((-1, null));
                }
                Emit(false);

                foreach (var child in Children(index, layout)) Visit(child);

                Emit(true);
            }

            // roots: cache_file_global_tags globals with the scenario walked before matg
            var cfgtTag = Cache.TagCache.NonNull().First(t => t.IsInGroup("cfgt"));
            var cfgt = Cache.Deserialize<CacheFileGlobalTags>(CacheStream, cfgtTag);
            var globals = cfgt.GlobalTags.Where(g => g.Instance != null).Select(g => g.Instance).ToList();
            var roots = globals.Where(g => !g.IsInGroup("matg")).Select(g => g.Index).ToList();
            roots.Add(scenarioTag.Index);
            roots.AddRange(globals.Where(g => g.IsInGroup("matg")).Select(g => g.Index));
            visited.Add(cfgtTag.Index);
            roots.AddRange(GlobalOrphans());   // tags only zone tags reference, present in most existing zones
            layout.Roots.AddRange(roots);
            foreach (var r in roots) Visit(r);

            inBspPass = true;
            layout.Bsps.AddRange(deferred);
            foreach (var bsp in deferred) Visit(bsp);
            return layout;
        }

        // A tag's children in walk order: its field references that are also stored dependencies, then the remaining stored
        // dependencies (may repeat; callers skip visited tags).
        private IEnumerable<int> Children(int index, Layout layout)
        {
            var stored = Dependencies(index);
            var storedSet = new HashSet<int>(stored);
            var refs = References(index, layout);
            if (refs != null)
                foreach (var r in refs)
                    if (storedSet.Contains(r)) yield return r;
            foreach (var d in stored) yield return d;
        }

        private List<int> GlobalOrphans()
        {
            var zones = Cache.TagCache.NonNull().Where(t => t.IsInGroup("zone")).Select(t => t.Index).ToList();
            if (zones.Count == 0) return new List<int>();
            var referenced = new HashSet<int>();
            foreach (var t in Cache.TagCache.NonNull())
                if (!t.IsInGroup("zone"))
                    foreach (var d in Dependencies(t.Index))
                        if (d != t.Index) referenced.Add(d);
            // present in most existing zones (0x3ABE is in 11 of the 13 shipped ms30 zones)
            var counts = new Dictionary<int, int>();
            foreach (var z in zones)
                foreach (var d in new HashSet<int>(Dependencies(z)))
                    counts[d] = counts.TryGetValue(d, out var c) ? c + 1 : 1;
            return counts.Where(kv => kv.Value * 2 > zones.Count && !referenced.Contains(kv.Key) && Resources(kv.Key).Count > 0)
                .Select(kv => kv.Key).OrderBy(i => i).ToList();
        }

        //
        // Zone construction
        //

        private uint DatOffset(PageableResource resource)
        {
            resource.GetLocation(out var location);
            // (InResourcesB resolves to video.dat in stock folders, see ResourceCachesHaloOnline)
            var cache = Cache.ResourceCaches.GetResourceCache(location);
            if (cache == null || resource.Page.Index < 0 || resource.Page.Index >= cache.Resources.Count)
                throw new InvalidOperationException($"resource page {resource.Page.Index} ({resource.Page.NewFlags}, location {location}, version {Cache.Version}) is outside its resource cache ({cache?.Resources.Count} resources)");
            return cache.Resources[resource.Page.Index].Offset;
        }

        private ResourceGestalt BuildZone(Layout layout, ResourceGestalt template, CachedTag scenarioTag, int mapId)
        {
            var zone = new ResourceGestalt
            {
                MapType = template.MapType,
                Flags = template.Flags,
                TagResourceTable = new List<ResourceGestalt.TagResourceData>(),
                CampaignId = -1,
                MapId = mapId,
                BspReferences = layout.Bsps.Select(b => new TagReferenceBlock { Instance = Cache.TagCache.GetTag(b) }).ToList(),
                ZoneSetZoneUsages = template.ZoneSetZoneUsages?.Take(1).ToList() ?? new List<ZoneSetZoneUsage>(),
                PredictionTable = new ResourcePredictionTable
                {
                    PredictionQuanta = new List<PredictionQuantum>(),
                    PredictionAtoms = new List<PredictionAtom>(),
                    PredictionMoleculeAtoms = new List<PredictionMoleculeAtom>(),
                    PredictionMolecules = new List<PredictionMolecule>(),
                    PredictionMoleculeKeys = new List<PredictionMoleculeKey>()
                },
            };
            foreach (var (tag, resource) in layout.Entries)
            {
                if (resource == null)
                {
                    zone.TagResourceTable.Add(PlaceholderTemplate?.DeepCloneV2() ?? new ResourceGestalt.TagResourceData { FileLocation = new ResourcePage(), RuntimeData = new ResourceData() });
                    continue;
                }
                var page = resource.Page.DeepCloneV2();
                page.Index = (int)DatOffset(resource);
                zone.TagResourceTable.Add(new ResourceGestalt.TagResourceData { FileLocation = page, RuntimeData = resource.Resource });
            }
            BuildManifests(zone, layout, template, scenarioTag);
            return zone;
        }

        private static uint PageRound(uint size) => (size + 0xFFFu) & ~0xFFFu;

        private static TagBlockBitVector BitVector(IEnumerable<int> bits, int count)
        {
            var words = new uint[(count + 31) / 32];
            foreach (var b in bits) words[b >> 5] |= 1u << (b & 31);
            return new TagBlockBitVector { Words = words.Select(w => new TagBlockBitVector.Word { Value = w }).ToList() };
        }

        //
        // Manifests
        //
        // Membership follows the H3EK cache builder (build_cache_file_attach_resources_to_zones): the global manifest is a walk
        // of the globals and the scenario (minus the scenario's bsp/sky/decal/character/decorator blocks), the default bsp
        // manifest is the scenario's structure bsp, lightmap and acoustics references, and the static/dynamic bsp manifests
        // are what the bsp game systems register (see BspRegistrations). ms30's second bitvector is H3's "deferred required"
        // one; H3's tool leaves it empty (its deferred predicate is a stub), and ms30's rule for it isn't in any data we have,
        // so required vs optional comes from:
        //
        //  - votes: how the other shipped zones classify each tag. A tag is classified the same way in 97% of the zones that
        //    contain it (the exceptions are map specific, e.g. vehicle parts that are required where the vehicle is placed).
        //  - structure: optional resources are exactly those some zoneset object lists; anything reachable without going
        //    through a zoneset object is required, and tags no other zone contains default to required.
        //
        // Global membership also falls back to structure for unseen tags, and content that only this map's bsp uses stays
        // out of the global manifest. The zones-only manifest is the union (required wins over optional) and the gestalt sizes
        // follow from the manifests (4 KB rounded uncompressed sizes; ResourceUsage[type] = {required, optional, both}).

        private const int ManifestCount = 4;   // global, default_bsp, static_bsp, dynamic_bsp
        private const int FewVotes = 3;         // fewer "none" votes than this don't override this map's structure
        private enum Membership { None, Required, Optional }

        private class ManifestVotes
        {
            public readonly Dictionary<int, int[][]> Counts = new Dictionary<int, int[][]>();   // tag -> [manifest][membership]
            // zoneset objects: tag -> {zones listing it as a global zoneset object, zones listing it as a dynamic one}
            public readonly Dictionary<int, int[]> Objects = new Dictionary<int, int[]>();
            public bool? DynamicObject(int tag) => Objects.TryGetValue(tag, out var o) ? o[1] * 2 > o[0] : (bool?)null;
            public Membership? Get(int tag, int manifest)
            {
                if (!Counts.TryGetValue(tag, out var c)) return null;
                var v = c[manifest];
                // ties: required, then optional, then none
                if (v[1] >= v[2] && v[1] >= v[0] && v[1] > 0) return Membership.Required;
                if (v[2] >= v[0] && v[2] > 0) return Membership.Optional;
                return Membership.None;
            }
        }

        private CachedTag TargetZone;   // the zone being compared against / overwritten: left out of the votes
        private string UnseenMode = "required";
        private string OptionalMode = "votes";

        private List<ResourceGestalt> OtherZonesCache;
        private List<ResourceGestalt> OtherZones()
        {
            if (OtherZonesCache != null) return OtherZonesCache;
            OtherZonesCache = new List<ResourceGestalt>();
            foreach (var zoneTag in Cache.TagCache.NonNull().Where(t => t.IsInGroup("zone")))
            {
                if (TargetZone != null && zoneTag.Index == TargetZone.Index) continue;
                try { OtherZonesCache.Add(Cache.Deserialize<ResourceGestalt>(CacheStream, zoneTag)); } catch { }
            }
            return OtherZonesCache;
        }

        // Empty entries (no parent tag, no page) in the shipped zones. They sit directly before or after the entries of an
        // animation graph (the resource groups the cache builder culled) - or before a render model - and each tag has the
        // same number of them in every zone, so they are learned from the other zones.
        private readonly Dictionary<int, int> PlaceholdersBefore = new Dictionary<int, int>();
        private readonly Dictionary<int, int> PlaceholdersAfter = new Dictionary<int, int>();
        private ResourceGestalt.TagResourceData PlaceholderTemplate;

        private void LearnPlaceholders()
        {
            foreach (var z in OtherZones())
            {
                var t = z.TagResourceTable;
                int Parent(int i) => i >= 0 && i < t.Count ? t[i].RuntimeData?.ParentTag?.Index ?? -1 : -1;
                bool IsJmad(int tag) => tag >= 0 && Cache.TagCache.GetTag(tag)?.IsInGroup("jmad") == true;
                for (int i = 0; i < t.Count;)
                {
                    if (Parent(i) != -1) { i++; continue; }
                    int j = i;
                    while (j < t.Count && Parent(j) == -1) j++;
                    PlaceholderTemplate ??= t[i];
                    int prev = Parent(i - 1), next = Parent(j);
                    if (IsJmad(next) || (!IsJmad(prev) && next != -1)) PlaceholdersBefore[next] = j - i;
                    else if (prev != -1) PlaceholdersAfter[prev] = j - i;
                    i = j;
                }
            }
        }

        private ManifestVotes CollectVotes()
        {
            var votes = new ManifestVotes();
            // debugging aid: treat every tag as one no other zone contains (to evaluate the unseen tag rules)
            if (Environment.GetEnvironmentVariable("GENERATEZONE_NOVOTES") != null) return votes;
            foreach (var z in OtherZones())
            {
                var manifests = new[] { z.GlobalZoneManifests, z.DefaultBspZoneManifests, z.StaticBspZoneManifests, z.DynamicBspZoneManifests }
                    .Select(l => l?.FirstOrDefault()).ToArray();
                var required = manifests.Select(m => Bits(m?.RequiredResourcesBitVector)).ToArray();
                var optional = manifests.Select(m => Bits(m?.OptionalResourcesBitVector)).ToArray();
                foreach (var (manifest, slot) in new[] { (manifests[0], 0), (manifests[3], 1) })
                    foreach (var o in manifest?.ZonesetObjects ?? new List<ZoneManifest.ZoneResourceZonesetObjects>())
                    {
                        if (o.Object == null) continue;
                        if (!votes.Objects.TryGetValue(o.Object.Index, out var ov)) votes.Objects[o.Object.Index] = ov = new int[2];
                        ov[slot]++;
                    }
                // one vote per tag per zone (a tag's resources are classified together)
                var seen = new HashSet<int>();
                for (int i = 0; i < z.TagResourceTable.Count; i++)
                {
                    var tag = z.TagResourceTable[i].RuntimeData?.ParentTag;
                    if (tag == null || !seen.Add(tag.Index)) continue;
                    if (!votes.Counts.TryGetValue(tag.Index, out var c))
                        votes.Counts[tag.Index] = c = Enumerable.Range(0, ManifestCount).Select(_ => new int[3]).ToArray();
                    for (int m = 0; m < ManifestCount; m++)
                        c[m][required[m].Contains(i) ? 1 : optional[m].Contains(i) ? 2 : 0]++;
                }
            }
            return votes;
        }

        // Reference edges of a tag with the field path they are stored in (debug dump helper reused for the manifests).
        private readonly Dictionary<int, List<(string Path, int Tag)>> EdgeCache = new Dictionary<int, List<(string, int)>>();
        private List<(string Path, int Tag)> Edges(int index)
        {
            if (EdgeCache.TryGetValue(index, out var edges)) return edges;
            edges = new List<(string, int)>();
            var tag = Cache.TagCache.GetTag(index);
            if (tag != null)
            {
                bool ok = false;
                try { var definition = TryDeserialize(tag); if (definition != null) { CollectEdges(definition, "", edges, 0); ok = true; } } catch { }
                if (!ok) { edges.Clear(); foreach (var d in Dependencies(index)) edges.Add(("?", d)); }
            }
            return EdgeCache[index] = edges;
        }

        // Groups whose references lead out of the object/bsp being registered (globals, AI, UI).
        private static readonly HashSet<string> ClosureStopGroups = new HashSet<string>
            { "zone", "scnr", "matg", "mulg", "gfxt", "sus!", "smdt", "sqtm", "char", "aigl", "chdt", "wezr", "cfgt", "unic", "vfsl", "draw" };

        private HashSet<int> Closure(IEnumerable<int> roots)
        {
            var seen = new HashSet<int>();
            var stack = new Stack<int>(roots.Where(r => r >= 0));
            while (stack.Count > 0)
            {
                int t = stack.Pop();
                if (!seen.Add(t)) continue;
                foreach (var (path, child) in Edges(t))
                {
                    if (child < 0 || seen.Contains(child)) continue;
                    // render method parameters are source data the cache builder doesn't follow
                    if (path.StartsWith("Parameters.") || path.Contains(".Parameters.")) continue;
                    var group = Cache.TagCache.GetTag(child)?.Group.Tag.ToString();
                    if (group == null || ClosureStopGroups.Contains(group)) continue;
                    stack.Push(child);
                }
            }
            return seen;
        }

        private HashSet<int> ScenarioFieldClosure(CachedTag scenarioTag, Func<string, bool> field) =>
            Closure(Edges(scenarioTag.Index).Where(e => field(e.Path)).Select(e => e.Tag));

        // Entries of the tags an object reaches (walk order), not passing through the other zoneset objects.
        private List<int> ObjectReach(Layout layout, int root, HashSet<int> stopObjects, Dictionary<int, List<int>> entriesOf, Dictionary<int, int> parents)
        {
            var seen = new HashSet<int>(); var result = new List<int>();
            void Walk(int t, int parent)
            {
                if (t < 0 || !seen.Add(t)) return;
                var g = Cache.TagCache.GetTag(t)?.Group.Tag.ToString();
                // render method definitions (and the option default bitmaps behind them) belong to no object
                if (g == null || g == "zone" || g == "sbsp" || g == "cfgt" || g == "scnr" || g == "rmdf") return;
                if (parents != null) parents[t] = parent;
                if (entriesOf.TryGetValue(t, out var l)) result.AddRange(l);
                foreach (var c in Children(t, layout)) if (!seen.Contains(c) && (stopObjects == null || !stopObjects.Contains(c))) Walk(c, t);
            }
            Walk(root, -1);
            return result;
        }

        // Object tags actually placed in the scenario (placement blocks' palette entries).
        private static readonly (string Instances, string Palette)[] Placements =
        {
            ("Weapons", "WeaponPalette"), ("Vehicles", "VehiclePalette"), ("Equipment", "EquipmentPalette"), ("Scenery", "SceneryPalette"),
            ("Crates", "CratePalette"), ("Bipeds", "BipedPalette"), ("Machines", "MachinePalette"), ("EffectScenery", "EffectSceneryPalette"),
            ("SoundScenery", "SoundSceneryPalette"), ("Creatures", "CreaturePalette"), ("LightVolumes", "LightVolumePalette"),
            ("Terminals", "TerminalPalette"), ("Controls", "ControlPalette"), ("Giants", "GiantPalette"),
        };

        private HashSet<int> PlacedObjects(CachedTag scenarioTag) => new HashSet<int>(Placements_(scenarioTag).Select(p => p.Tag));

        // every placement: (object tag, placement block, whether the instance is named)
        private List<(int Tag, string Block, bool Named)> Placements_(CachedTag scenarioTag)
        {
            var placed = new List<(int, string, bool)>();
            Scenario scenario;
            try { scenario = Cache.Deserialize<Scenario>(CacheStream, scenarioTag); } catch { return placed; }
            foreach (var (instancesName, paletteName) in Placements)
            {
                var instances = typeof(Scenario).GetField(instancesName)?.GetValue(scenario) as IList;
                var palette = typeof(Scenario).GetField(paletteName)?.GetValue(scenario) as IList;
                if (instances == null || palette == null) continue;
                foreach (var instance in instances)
                {
                    var index = instance?.GetType().GetField("PaletteIndex")?.GetValue(instance);
                    if (index is short i && i >= 0 && i < palette.Count)
                    {
                        var entry = palette[i];
                        var obj = entry?.GetType().GetField("Object")?.GetValue(entry) as CachedTag;
                        var name = instance.GetType().GetField("NameIndex")?.GetValue(instance);
                        if (obj != null && obj.Index >= 0) placed.Add((obj.Index, instancesName, name is short n && n != -1));
                    }
                }
            }
            return placed;
        }

        // "unseen learned": the shipped zones classify a zoneset object's content consistently by the object fields it is
        // reached through (animation graphs and material effects required, weapon/vehicle models and attachment sounds
        // optional, ...). A tag's key is its resource group plus the set of "<object group>.<top level field>" paths through
        // which any zoneset object reaches it; each key takes the majority label of the other zones' global manifests.
        private Dictionary<int, SortedSet<string>> ObjectFieldKeys(HashSet<int> objects)
        {
            var keys = new Dictionary<int, SortedSet<string>>();
            bool Follow(string path, int child)
            {
                if (child < 0 || objects.Contains(child) || path.StartsWith("Parameters.") || path.Contains(".Parameters.")) return false;
                var g = Cache.TagCache.GetTag(child)?.Group.Tag.ToString();
                return g != null && !ClosureStopGroups.Contains(g) && g != "sbsp" && g != "rmdf";
            }
            foreach (var o in objects)
            {
                var objectGroup = Cache.TagCache.GetTag(o)?.Group.Tag.ToString();
                foreach (var (path, child) in Edges(o))
                {
                    if (!Follow(path, child)) continue;
                    var label = objectGroup + "." + path.Split('.')[0];
                    var seen = new HashSet<int>();
                    var stack = new Stack<int>();
                    stack.Push(child);
                    while (stack.Count > 0)
                    {
                        int t = stack.Pop();
                        if (!seen.Add(t)) continue;
                        if (!keys.TryGetValue(t, out var set)) keys[t] = set = new SortedSet<string>();
                        set.Add(label);
                        foreach (var (p, c) in Edges(t)) if (!seen.Contains(c) && Follow(p, c)) stack.Push(c);
                    }
                }
            }
            return keys;
        }

        private string ObjectFieldKey(Dictionary<int, SortedSet<string>> keys, int tag) =>
            keys.TryGetValue(tag, out var set) ? Cache.TagCache.GetTag(tag)?.Group.Tag + "|" + string.Join("|", set) : null;

        // key -> {required, optional} tag counts over the other zones' global manifests
        private Dictionary<string, int[]> LearnUnseenRule(Dictionary<int, SortedSet<string>> keys)
        {
            var table = new Dictionary<string, int[]>();
            foreach (var z in OtherZones())
            {
                var manifest = z.GlobalZoneManifests?.FirstOrDefault();
                if (manifest == null) continue;
                var required = Bits(manifest.RequiredResourcesBitVector);
                var optional = Bits(manifest.OptionalResourcesBitVector);
                var seen = new HashSet<int>();
                for (int i = 0; i < z.TagResourceTable.Count; i++)
                {
                    var tag = z.TagResourceTable[i].RuntimeData?.ParentTag;
                    if (tag == null || !seen.Add(tag.Index) || !(required.Contains(i) || optional.Contains(i))) continue;
                    var key = ObjectFieldKey(keys, tag.Index);
                    if (key == null) continue;
                    if (!table.TryGetValue(key, out var c)) table[key] = c = new int[2];
                    c[optional.Contains(i) ? 1 : 0]++;
                }
            }
            return table;
        }

        // What the bsp game systems register for the static (category 0) and dynamic (1, 2) bsp manifests. From the H3EK tool:
        //  - object placement (object_placement_attach): a placement is static if its placement flags have 0x400; otherwise,
        //    with the default bsp policy, bipeds/vehicles/weapons/equipment/crates/creatures/giants are dynamic, and the other
        //    types are dynamic only when named or of a "dynamic" kind (terminal, machine, control, or scenery whose model has
        //    damage info) - so unnamed scenery, sound scenery and effect scenery are static. The "always placed" policy is
        //    dynamic. Objects attached to a non-static placement are dynamic.
        //  - decals: the palette entries the placed decals use are static.
        //  - flocks: the flock and creature palette entries the flocks use are static.
        //  - ai squads: the palette entries the squads use are dynamic.
        private (HashSet<int> Static, HashSet<int> Dynamic) BspRegistrations(CachedTag scenarioTag)
        {
            var statics = new HashSet<int>(); var dynamics = new HashSet<int>();
            Scenario scenario;
            try { scenario = Cache.Deserialize<Scenario>(CacheStream, scenarioTag); } catch { return (statics, dynamics); }
            var alwaysDynamic = new HashSet<string> { "Bipeds", "Vehicles", "Weapons", "Equipment", "Crates", "Creatures", "Giants" };
            var dynamicKinds = new HashSet<string> { "Terminals", "Machines", "Controls" };
            bool ModelHasDamage(CachedTag obj)
            {
                try
                {
                    var def = (GameObject)Cache.Deserialize(CacheStream, obj);
                    return def.Model != null && Cache.Deserialize<Model>(CacheStream, def.Model).NewDamageInfo?.Count > 0;
                }
                catch { return false; }
            }
            var byName = new Dictionary<short, int>();   // object name index -> category
            var instancesByBlock = new List<(string Block, object Instance, CachedTag Object)>();
            foreach (var (instancesName, paletteName) in Placements)
            {
                var instances = typeof(Scenario).GetField(instancesName)?.GetValue(scenario) as IList;
                var palette = typeof(Scenario).GetField(paletteName)?.GetValue(scenario) as IList;
                if (instances == null || palette == null) continue;
                foreach (var instance in instances)
                {
                    var index = instance?.GetType().GetField("PaletteIndex")?.GetValue(instance);
                    if (!(index is short i) || i < 0 || i >= palette.Count) continue;
                    var obj = palette[i]?.GetType().GetField("Object")?.GetValue(palette[i]) as CachedTag;
                    if (obj == null || obj.Index < 0) continue;
                    instancesByBlock.Add((instancesName, instance, obj));
                }
            }
            int Category(string block, object instance, CachedTag obj)
            {
                var t = instance.GetType();
                var flagsStruct = t.GetField("PlacementFlags")?.GetValue(instance);
                var flags = flagsStruct?.GetType().GetField("Flags")?.GetValue(flagsStruct);
                if (flags != null && (Convert.ToInt64(flags) & 0x400) != 0) return 0;
                var policy = t.GetField("BspPolicy")?.GetValue(instance)?.ToString() ?? "Default";
                bool staticCapable = policy == "Default" ? !alwaysDynamic.Contains(block) : policy != "AlwaysPlaced";
                if (!staticCapable) return 2;
                bool named = t.GetField("NameIndex")?.GetValue(instance) is short n && n != -1;
                bool dynamicKind = dynamicKinds.Contains(block) || alwaysDynamic.Contains(block) || (block == "Scenery" && ModelHasDamage(obj));
                return named || dynamicKind ? 1 : 0;
            }
            var categories = new List<int>();
            foreach (var (block, instance, obj) in instancesByBlock)
            {
                int category = Category(block, instance, obj);
                categories.Add(category);
                (category == 0 ? statics : dynamics).Add(obj.Index);
                if (instance.GetType().GetField("NameIndex")?.GetValue(instance) is short n && n != -1) byName[n] = category;
            }
            // objects attached to a named, non-static parent placement
            for (int k = 0; k < instancesByBlock.Count; k++)
            {
                var parent = instancesByBlock[k].Instance.GetType().GetField("ParentId")?.GetValue(instancesByBlock[k].Instance);
                if (parent?.GetType().GetField("NameIndex")?.GetValue(parent) is short pn && pn != -1 && byName.TryGetValue(pn, out var pc) && pc != 0)
                    dynamics.Add(instancesByBlock[k].Object.Index);
            }
            // decals, flocks, squads
            void UsedPalette(string instancesName, string indexField, string paletteName, string objectField, HashSet<int> into)
            {
                var instances = typeof(Scenario).GetField(instancesName)?.GetValue(scenario) as IList;
                var palette = typeof(Scenario).GetField(paletteName)?.GetValue(scenario) as IList;
                if (instances == null || palette == null) return;
                foreach (var instance in instances)
                    if (instance?.GetType().GetField(indexField)?.GetValue(instance) is short i && i >= 0 && i < palette.Count)
                    {
                        var entry = palette[i];
                        var tag = entry?.GetType().GetField(objectField)?.GetValue(entry) as CachedTag;
                        if (tag != null && tag.Index >= 0) into.Add(tag.Index);
                    }
            }
            UsedPalette("Decals", "DecalPaletteIndex", "DecalPalette", "Instance", statics);
            UsedPalette("Flocks", "FlockPaletteIndex", "FlockPalette", "Instance", statics);
            UsedPalette("Flocks", "CreaturePaletteIndex", "CreaturePalette", "Object", statics);
            return (statics, dynamics);
        }

        private void BuildManifests(ResourceGestalt zone, Layout layout, ResourceGestalt template, CachedTag scenarioTag)
        {
            int count = zone.TagResourceTable.Count;
            var votes = CollectVotes();
            var defaultBsp = ScenarioFieldClosure(scenarioTag, p => p.StartsWith("StructureBsps.") || p == "Lightmap" || p.StartsWith("AcousticsPalette."));
            var registrations = BspRegistrations(scenarioTag);
            // the tool's global walk: the globals and the scenario, except the scenario's bsp, sky, decal, character and
            // decorator blocks (those are registered by the bsp manifests' game systems)
            var globalReach = new HashSet<int>();
            {
                var excluded = new[] { "StructureBsps.", "Lightmap", "SkyReferences.", "SkyParameters", "DecalPalette.", "CharacterPalette.", "Decorators.", "ZoneDebugger." };
                var stack = new Stack<int>(layout.Roots.Where(r => r != scenarioTag.Index));
                foreach (var (path, child) in Edges(scenarioTag.Index))
                    if (!excluded.Any(x => path == x.TrimEnd('.') || path.StartsWith(x))) stack.Push(child);
                while (stack.Count > 0)
                {
                    int t = stack.Pop();
                    if (t < 0 || !globalReach.Add(t)) continue;
                    var g = GroupOf(t);
                    if (g == null || g == "zone" || g == "sbsp" || g == "scnr" || g == "cfgt") continue;
                    foreach (var (path, child) in Edges(t))
                        if (!path.StartsWith("Parameters.") && !path.Contains(".Parameters.")) stack.Push(child);
                }
            }
            var staticBsp = Closure(registrations.Static.Concat(Edges(scenarioTag.Index).Where(e => e.Path.StartsWith("SkyReferences.") || e.Path == "SkyParameters").Select(e => e.Tag)));

            // membership[manifest][entry]
            var membership = Enumerable.Range(0, ManifestCount).Select(_ => new Membership[count]).ToArray();
            for (int i = 0; i < count; i++)
            {
                int tag = layout.Entries[i].Tag;
                // a map's own bsp content (lightmaps, cubemaps, sky, decals) is listed by the bsp manifests, not the global one
                var global = votes.Get(tag, 0) ?? (globalReach.Contains(tag) ? Membership.Required : Membership.None);
                // a "none" vote from only a zone or two is usually a leftover entry there (e.g. another map's decals or shader
                // bitmaps); it doesn't outweigh this map's own reach
                if (global == Membership.None && globalReach.Contains(tag) && votes.Counts.TryGetValue(tag, out var gv) && gv[0][0] < FewVotes)
                    global = Membership.Required;
                // content only this map's bsp uses stays out of the global manifest even where other maps use it globally
                if (!globalReach.Contains(tag) && (defaultBsp.Contains(tag) || staticBsp.Contains(tag)))
                    global = Membership.None;
                membership[0][i] = global;
                // a tag the other zones keep out of every manifest stays out of the structural BSP manifests too
                // (a zone or two isn't enough: those are usually leftover entries of this map's content in another map's zone)
                bool neverListed = votes.Counts.TryGetValue(tag, out var c) && c[0].Sum() >= FewVotes && Enumerable.Range(0, ManifestCount).All(m => c[m][0] == c[m].Sum());
                membership[1][i] = defaultBsp.Contains(tag) && !neverListed ? Membership.Required : Membership.None;
                membership[2][i] = staticBsp.Contains(tag) && !neverListed ? Membership.Required : Membership.None;
            }
            // Zoneset objects: the weapons, vehicles and armor of the zone, each listing the optional resources it reaches
            // (loaded when the object is created). Every optional resource must be listed by some object, so optional
            // resources no object reaches become required.
            var objectGroups = new HashSet<string> { "weap", "vehi", "armr" };
            string GroupOf(int t) => Cache.TagCache.GetTag(t)?.Group.Tag.ToString();
            var entriesOf = new Dictionary<int, List<int>>();
            for (int i = 0; i < count; i++)
            {
                if (!entriesOf.TryGetValue(layout.Entries[i].Tag, out var l)) entriesOf[layout.Entries[i].Tag] = l = new List<int>();
                l.Add(i);
            }
            var globalObjects = layout.VisitOrder.Where(t => objectGroups.Contains(GroupOf(t))).ToList();
            var objectSet = new HashSet<int>(globalObjects);
            // dynamic_bsp zoneset objects: objects placed in the map and the objects they carry or spawn, plus those the
            // other zones always list (multiplayer objects such as the flag, ball and bomb)
            var placed = registrations.Dynamic;
            var placedObjects = new HashSet<int>();
            var staticObjects = new HashSet<int>();
            {
                // placed objects and the objects they carry: model variant child objects and unit weapons. Objects that
                // placed scenery carries (e.g. the weapons on a weapon rack) are static_bsp objects instead.
                bool Carries(string path) => path == "Model" || path.StartsWith("Variants.Objects.ChildObject") || path == "Weapons.Weapon" || path == "DetachedWeapon";
                void Collect(IEnumerable<int> roots, HashSet<int> into, bool sceneryChildrenStatic)
                {
                    var stack = new Stack<(int Tag, bool FromScenery)>(roots.Select(r => (r, GroupOf(r) == "scen")));
                    while (stack.Count > 0)
                    {
                        var (t, fromScenery) = stack.Pop();
                        bool isObject = objectGroups.Contains(GroupOf(t));
                        var target = sceneryChildrenStatic && fromScenery && isObject ? staticObjects : into;
                        if (!target.Add(t)) continue;
                        foreach (var (path, child) in Edges(t))
                            if (child >= 0 && Carries(path)) stack.Push((child, fromScenery));
                    }
                }
                Collect(placed, placedObjects, false);
                Collect(registrations.Static, staticObjects, false);
                // multiplayer maps: the game engine's flag, ball and bomb
                var scenarioDefinition = Cache.Deserialize<Scenario>(CacheStream, scenarioTag);
                if (scenarioDefinition.MapType == ScenarioMapType.Multiplayer)
                    foreach (var mulg in Cache.TagCache.NonNull().Where(t => t.IsInGroup("mulg")))
                        foreach (var (path, child) in Edges(mulg.Index))
                            if (path == "Runtime.Flag" || path == "Runtime.Ball" || path == "Runtime.Bomb") placedObjects.Add(child);
            }
            var dynamicObjects = globalObjects.Where(placedObjects.Contains).ToList();
            var staticZonesetCandidates = globalObjects.Where(staticObjects.Contains).ToList();
            var staticZonesetSet = new HashSet<int>(staticZonesetCandidates.Where(o => !placedObjects.Contains(o)));
            List<int> ObjectEntries(int root, bool throughObjects) => ObjectReach(layout, root, throughObjects ? null : objectSet, entriesOf, null);
            List<ZoneManifest.ZoneResourceZonesetObjects> ZonesetObjects(List<int> objects, Membership[] members, bool throughObjects)
            {
                var objectReach = objects.ToDictionary(o => o, o => ObjectEntries(o, throughObjects));
                var list = objects.Select(o => new ZoneManifest.ZoneResourceZonesetObjects
                {
                    Object = Cache.TagCache.GetTag(o),
                    Dependencies = objectReach[o].Where(i => members[i] == Membership.Optional)
                        .Select(i => new ZoneManifest.ZoneResourceZonesetObjects.ZoneResourceObjectDependency { TagResourceIndex = (short)i }).ToList(),
                }).Where(z => z.Dependencies.Count > 0).ToList();
                var listed = new HashSet<int>(list.SelectMany(z => z.Dependencies.Select(d => (int)d.TagResourceIndex)));
                for (int i = 0; i < count; i++)
                    if (members[i] == Membership.Optional && !listed.Contains(i)) members[i] = Membership.Required;
                return list;
            }
            // resources reachable without going through a zoneset object are always required (true for all but a handful of
            // resources on the shipped maps)
            {
                var outside = new HashSet<int>();
                var seenOutside = new HashSet<int>();
                void WalkOutside(int t)
                {
                    if (t < 0 || objectSet.Contains(t) || !seenOutside.Add(t)) return;
                    var g = GroupOf(t);
                    if (g == null || g == "zone" || g == "cfgt") return;
                    if (entriesOf.TryGetValue(t, out var l)) outside.UnionWith(l);
                    foreach (var c in Children(t, layout)) WalkOutside(c);
                }
                foreach (var r in layout.Roots) WalkOutside(r);
                foreach (var b in layout.Bsps) WalkOutside(b);
                foreach (var i in outside)
                    if (membership[0][i] == Membership.Optional) membership[0][i] = Membership.Required;

                if (UnseenMode == "learned")
                {
                    // content no other zone contains and only zoneset objects reach: take the learned label of its key
                    var learnObjects = new HashSet<int>(globalObjects);
                    foreach (var z in OtherZones())
                        foreach (var o in z.GlobalZoneManifests?.FirstOrDefault()?.ZonesetObjects ?? new List<ZoneManifest.ZoneResourceZonesetObjects>())
                            if (o.Object != null) learnObjects.Add(o.Object.Index);
                    var keys = ObjectFieldKeys(learnObjects);
                    var table = LearnUnseenRule(keys);
                    int made = 0, kept = 0;
                    for (int i = 0; i < count; i++)
                    {
                        int tag = layout.Entries[i].Tag;
                        if (tag < 0 || votes.Counts.ContainsKey(tag) || outside.Contains(i) || membership[0][i] != Membership.Required) continue;
                        var key = ObjectFieldKey(keys, tag);
                        if (key != null && table.TryGetValue(key, out var c) && c[1] > c[0]) { membership[0][i] = Membership.Optional; made++; }
                        else kept++;
                    }
                    Console.WriteLine($"unseen learned: {made} resources made optional, {kept} kept required");
                }

                var featurePath = Environment.GetEnvironmentVariable("GENERATEZONE_FEATURES");
                if (featurePath != null)
                {
                    // per entry: votes, structural reach, first object path - for working out the required/optional rule
                    var reachCount = new Dictionary<int, int>(); var firstPath = new Dictionary<int, string>();
                    foreach (var ob in globalObjects)
                    {
                        var par = new Dictionary<int, int>();
                        foreach (var i in ObjectReach(layout, ob, objectSet, entriesOf, par).Distinct())
                        {
                            reachCount[i] = reachCount.TryGetValue(i, out var rc) ? rc + 1 : 1;
                            if (firstPath.ContainsKey(i)) continue;
                            var chain = new List<string>();
                            for (int x = layout.Entries[i].Tag, n = 0; x >= 0 && n < 8; x = par.TryGetValue(x, out var px) ? px : -1, n++) chain.Add(GroupOf(x));
                            firstPath[i] = string.Join("<", chain);
                        }
                    }
                    var placedReach = new HashSet<int>();
                    foreach (var o in placed) placedReach.UnionWith(ObjectReach(layout, o, null, entriesOf, null));
                    using (var w = new StreamWriter(featurePath))
                    {
                        w.WriteLine("index,tag,group,voteR,voteO,voteN,objects,outside,placed,path");
                        for (int i = 0; i < count; i++)
                        {
                            int tag = layout.Entries[i].Tag;
                            var c = votes.Counts.TryGetValue(tag, out var cc) ? cc[0] : new int[3];
                            w.WriteLine($"{i},0x{tag:X4},{GroupOf(tag)},{c[1]},{c[2]},{c[0]},{(reachCount.TryGetValue(i, out var rc) ? rc : 0)},{(outside.Contains(i) ? 1 : 0)},{(placedReach.Contains(i) ? 1 : 0)},{(firstPath.TryGetValue(i, out var fp) ? fp : "")}");
                        }
                    }
                }
            }
            var globalZonesetObjects = ZonesetObjects(globalObjects, membership[0], false);
            // dynamic_bsp: everything the placed objects reach (more accurate than the other zones' votes, since placement
            // is map specific), optional where the global manifest has it optional
            for (int i = 0; i < count; i++) membership[3][i] = Membership.None;
            foreach (var o in placed)
                foreach (var i in ObjectReach(layout, o, staticZonesetSet, entriesOf, null))
                    membership[3][i] = membership[0][i] == Membership.Optional ? Membership.Optional : Membership.Required;
            // static_bsp: the static objects' resources, optional where the global manifest has them optional; resources only
            // the other zones' votes put in static_bsp can't be optional (only static objects list optional resources)
            for (int i = 0; i < count; i++) if (membership[2][i] == Membership.Optional) membership[2][i] = Membership.None;
            foreach (var o in staticZonesetCandidates)
                foreach (var i in ObjectEntries(o, true))
                {
                    bool votedOptional = votes.Counts.TryGetValue(layout.Entries[i].Tag, out var sv) && sv[2][2] > sv[2][1];
                    membership[2][i] = membership[0][i] == Membership.Optional || votedOptional ? Membership.Optional : Membership.Required;
                }
            var staticZonesetObjects = ZonesetObjects(staticZonesetCandidates, membership[2], false);
            // placed objects' optional resources are optional in the dynamic manifest too
            foreach (var o in dynamicObjects)
                foreach (var i in ObjectEntries(o, false))
                {
                    if (membership[0][i] == Membership.Optional) membership[3][i] = Membership.Optional;
                    // resources the other zones' dynamic manifests mostly have optional
                    else if (votes.Counts.TryGetValue(layout.Entries[i].Tag, out var dv) && dv[3][2] > dv[3][1]) membership[3][i] = Membership.Optional;
                }
            var dynamicZonesetObjects = ZonesetObjects(dynamicObjects, membership[3], false);

            if (OptionalMode == "none")
            {
                // H3/ODST tool behaviour: s_tag_resource_definition::optional is a stub returning false, so nothing is optional
                // and no zoneset object has optional dependencies to list
                for (int m = 0; m < ManifestCount; m++)
                    for (int i = 0; i < count; i++)
                        if (membership[m][i] == Membership.Optional) membership[m][i] = Membership.Required;
                globalZonesetObjects.Clear();
                staticZonesetObjects.Clear();
                dynamicZonesetObjects.Clear();
            }

            var zonesOnly = new Membership[count];
            for (int i = 0; i < count; i++)
                for (int m = 0; m < ManifestCount; m++)
                    if (membership[m][i] == Membership.Required || (membership[m][i] == Membership.Optional && zonesOnly[i] == Membership.None))
                        zonesOnly[i] = membership[m][i];

            // Owner bitvectors: the bits index the tool's internal tag table and nothing at runtime acts on them, but the runtime
            // ORs every active manifest's ActiveResourceOwners over its owner count (sub_5BEFC0, ~tag count bits) without a length
            // check, so an empty vector is a null read on map load. Write them full length: set where the manifest has resources.
            int ownerBits = Math.Max(Math.Max(Cache.TagCache.Count, 20000), (template.GlobalZoneManifests?.FirstOrDefault()?.ActiveResourceOwners?.Words?.Count ?? 0) * 32);
            TagBlockBitVector Owners(TagBlockBitVector source, bool any)
            {
                int words = Math.Max((ownerBits + 31) / 32, source?.Words?.Count ?? 0);
                return new TagBlockBitVector { Words = Enumerable.Range(0, words).Select(_ => new TagBlockBitVector.Word { Value = any ? uint.MaxValue : 0 }).ToList() };
            }

            ZoneManifest Manifest(ZoneManifest source, Membership[] members, List<ZoneManifest.ZoneResourceZonesetObjects> objects = null)
            {
                var req = Enumerable.Range(0, count).Where(i => members[i] == Membership.Required).ToList();
                var opt = Enumerable.Range(0, count).Where(i => members[i] == Membership.Optional).ToList();
                var usage = Enumerable.Range(0, 8).Select(_ => new ZoneResourceUsage()).ToList();
                uint Size(int i) => PageRound(zone.TagResourceTable[i].FileLocation.UncompressedBlockSize);
                int Type(int i) => (int)zone.TagResourceTable[i].RuntimeData.ResourceType;
                foreach (var i in req) if (Type(i) >= 0 && Type(i) < 8) { usage[Type(i)].RequiredPageableSize += Size(i); usage[Type(i)].OptionalMemorySize += Size(i); }
                foreach (var i in opt) if (Type(i) >= 0 && Type(i) < 8) { usage[Type(i)].DeferredRequiredSize += Size(i); usage[Type(i)].OptionalMemorySize += Size(i); }
                return new ZoneManifest
                {
                    RequiredResourcesBitVector = BitVector(req, count),
                    OptionalResourcesBitVector = BitVector(opt, count),
                    RequiredPageableSize = (uint)req.Sum(i => (long)Size(i)),
                    OptionalMemorySize = (uint)opt.Sum(i => (long)Size(i)),
                    Name = source?.Name ?? StringId.Invalid,
                    ResourceUsage = req.Count + opt.Count > 0 || (source?.ResourceUsage?.Count ?? 0) > 0 ? usage : new List<ZoneResourceUsage>(),
                    ActiveResourceOwners = Owners(source?.ActiveResourceOwners, req.Count + opt.Count > 0),
                    TopLevelResourceOwners = Owners(source?.TopLevelResourceOwners, req.Count + opt.Count > 0),
                    ZonesetObjects = objects ?? new List<ZoneManifest.ZoneResourceZonesetObjects>(),
                };
            }
            List<ZoneManifest> One(List<ZoneManifest> source, Membership[] members, List<ZoneManifest.ZoneResourceZonesetObjects> objects = null) =>
                new List<ZoneManifest> { Manifest(source?.FirstOrDefault(), members, objects) };
            List<ZoneManifest> None() => new List<ZoneManifest>();
            var empty = new Membership[count];

            var bspName = Cache.StringTable.GetStringId(Path.GetFileName(scenarioTag.Name ?? ""));
            zone.DesignerZoneManifests = None();
            zone.GlobalZoneManifests = One(template.GlobalZoneManifests, membership[0], globalZonesetObjects);
            zone.UnattachedDesignerZoneManifests = One(template.UnattachedDesignerZoneManifests, empty);
            zone.DvdForbiddenZoneManifests = One(template.DvdForbiddenZoneManifests, empty);
            zone.DvdAlwaysStreamingZoneManifests = None();
            zone.DefaultBspZoneManifests = One(template.DefaultBspZoneManifests, membership[1]);
            zone.StaticBspZoneManifests = One(template.StaticBspZoneManifests, membership[2], staticZonesetObjects);
            zone.DynamicBspZoneManifests = One(template.DynamicBspZoneManifests, membership[3], dynamicZonesetObjects);
            zone.CinematicZoneManifests = None();
            zone.ZonesOnlyZoneSetManifests = One(template.ZonesOnlyZoneSetManifests, zonesOnly);
            zone.ExpectedZoneManifests = None();
            zone.FullyPopulatedZoneManifests = None();
            foreach (var m in zone.DefaultBspZoneManifests.Concat(zone.StaticBspZoneManifests).Concat(zone.DynamicBspZoneManifests))
                if (bspName != StringId.Invalid) m.Name = bspName;
            var scenarioName = Cache.StringTable.GetStringId(scenarioTag.Name ?? "");
            if (scenarioName != StringId.Invalid) zone.ZonesOnlyZoneSetManifests[0].Name = scenarioName;
            // the zone set usage is named after the scenario too (the runtime copies this block on map load)
            zone.ZoneSetZoneUsages = zone.ZoneSetZoneUsages.Select(u => u.DeepCloneV2()).ToList();
            if (scenarioName != StringId.Invalid && zone.ZoneSetZoneUsages.Count > 0) zone.ZoneSetZoneUsages[0].Name = scenarioName;

            var zonesOnlyManifest = zone.ZonesOnlyZoneSetManifests[0];
            zone.ResourcesSize = zonesOnlyManifest.RequiredPageableSize;
            zone.ResourcesAvailable = 0;
            zone.GlobalPageableDataSize = zonesOnlyManifest.RequiredPageableSize + zonesOnlyManifest.OptionalMemorySize;
            zone.GlobalResourceUsage = zonesOnlyManifest.ResourceUsage.Select(u => new ZoneResourceUsage { RequiredPageableSize = u.OptionalMemorySize, OptionalMemorySize = u.OptionalMemorySize }).ToList();
            zone.BspGameAttachments = new List<BspGameAttachment>();
            zone.ModelVariantZoneManifests = new List<DebugZoneManifest>();
            zone.CombatDialogueZoneManifests = new List<DebugZoneManifest>();
            zone.TagZoneManifests = new List<DebugZoneManifest>();
            zone.DebugResourceDefinitions = new List<DebugResourceDefinition>();
            zone.ResourceLayouts = new List<ResourceLayout>();
            zone.ResourceProperties = new List<ResourceProperty>();
            zone.Parentages = new List<Parentage>();
        }

        //
        // Adding tags to an existing zone
        //
        // Newly ported tags need entries in the map's existing zone. Rebuilding the whole zone would renumber every entry
        // and replace the shipped classification with the (99%) generated one, so instead the full generation is used only
        // as a reference: the resources of the added tags' closure that the zone doesn't have yet are appended after the
        // existing entries (keeping every existing index valid) with the manifest membership and zoneset object
        // dependencies the generated zone gives them. Closure tags the map doesn't reference (not in the generated zone)
        // are added as required by the global manifest, the safe choice for a tag loaded outside the map's normal walk.

        private static readonly HashSet<string> AddStopGroups = new HashSet<string> { "zone", "cfgt", "scnr", "sbsp", "matg", "mulg" };

        private int AddToZone(ResourceGestalt target, ResourceGestalt generated, Layout layout, List<CachedTag> addTags)
        {
            // the added tags and everything they reference (not following into the map/global level tags)
            var closure = new HashSet<int>();
            var order = new List<int>();
            void Walk(int t, bool root)
            {
                if (t < 0 || closure.Contains(t)) return;
                var group = Cache.TagCache.GetTag(t)?.Group.Tag.ToString();
                if (group == null || (!root && AddStopGroups.Contains(group))) return;
                closure.Add(t);
                order.Add(t);
                foreach (var c in Children(t, layout)) Walk(c, false);
            }
            foreach (var t in addTags) Walk(t.Index, true);

            var existingKeys = new HashSet<string>();
            foreach (var e in target.TagResourceTable)
                if (e.RuntimeData?.ParentTag != null && e.FileLocation != null) existingKeys.Add(Key(e.RuntimeData.ParentTag.Index, e.FileLocation));

            // generated index -> new index
            var map = new Dictionary<int, int>();
            int count = target.TagResourceTable.Count;
            for (int i = 0; i < generated.TagResourceTable.Count; i++)
            {
                int tag = layout.Entries[i].Tag;
                if (tag < 0 || !closure.Contains(tag) || !existingKeys.Add(Key(tag, generated.TagResourceTable[i].FileLocation))) continue;
                map[i] = count + map.Count;
            }
            var entries = map.Keys.Select(i => generated.TagResourceTable[i]).ToList();
            // closure resources the map's walk doesn't reach
            var unreferenced = new List<int>();
            foreach (var t in order)
                foreach (var (resource, _) in Resources(t))
                {
                    var page = resource.Page.DeepCloneV2();
                    page.Index = (int)DatOffset(resource);
                    if (!existingKeys.Add(Key(t, page))) continue;
                    unreferenced.Add(count + map.Count + unreferenced.Count);
                    entries.Add(new ResourceGestalt.TagResourceData { FileLocation = page, RuntimeData = resource.Resource });
                }

            var addedTags = entries.Select(e => e.RuntimeData.ParentTag).Where(t => t != null).GroupBy(t => t.Index).Select(g => g.First()).ToList();
            Console.WriteLine($"add: {closure.Count} tags in the closure, {entries.Count} new resources from {addedTags.Count} tags" +
                (unreferenced.Count > 0 ? $" ({unreferenced.Count} from tags the map doesn't reference, added as global required)" : ""));
            foreach (var t in addedTags) Console.WriteLine($"  0x{t.Index:X4} {t.Name}.{t.Group.Tag}");
            if (entries.Count == 0) return 0;

            target.TagResourceTable.AddRange(entries);
            int newCount = target.TagResourceTable.Count;
            uint Size(int i) => PageRound(target.TagResourceTable[i].FileLocation.UncompressedBlockSize);
            int Type(int i) => (int)target.TagResourceTable[i].RuntimeData.ResourceType;

            // (required, optional) resources added to each manifest
            (uint Required, uint Optional) Extend(ZoneManifest manifest, ZoneManifest reference, bool unreferencedRequired, string label)
            {
                var required = Bits(manifest.RequiredResourcesBitVector);
                var optional = Bits(manifest.OptionalResourcesBitVector);
                var newRequired = new List<int>(); var newOptional = new List<int>();
                if (reference != null)
                {
                    var referenceRequired = Bits(reference.RequiredResourcesBitVector);
                    var referenceOptional = Bits(reference.OptionalResourcesBitVector);
                    foreach (var (g, n) in map)
                    {
                        if (referenceRequired.Contains(g)) newRequired.Add(n);
                        else if (referenceOptional.Contains(g)) newOptional.Add(n);
                    }
                    // the objects listing the new optional resources
                    foreach (var o in reference.ZonesetObjects ?? new List<ZoneManifest.ZoneResourceZonesetObjects>())
                    {
                        var deps = o.Dependencies.Where(d => map.ContainsKey(d.TagResourceIndex)).Select(d => (short)map[d.TagResourceIndex]).ToList();
                        if (o.Object == null || deps.Count == 0) continue;
                        manifest.ZonesetObjects ??= new List<ZoneManifest.ZoneResourceZonesetObjects>();
                        var existingObject = manifest.ZonesetObjects.FirstOrDefault(z => z.Object?.Index == o.Object.Index);
                        if (existingObject == null)
                            manifest.ZonesetObjects.Add(existingObject = new ZoneManifest.ZoneResourceZonesetObjects
                                { Object = o.Object, Dependencies = new List<ZoneManifest.ZoneResourceZonesetObjects.ZoneResourceObjectDependency>() });
                        existingObject.Dependencies.AddRange(deps.Select(d => new ZoneManifest.ZoneResourceZonesetObjects.ZoneResourceObjectDependency { TagResourceIndex = d }));
                    }
                }
                if (unreferencedRequired) newRequired.AddRange(unreferenced);
                if (newRequired.Count + newOptional.Count > 0 && label != null)
                    Console.WriteLine($"  {label}: +{newRequired.Count} required, +{newOptional.Count} optional");

                required.UnionWith(newRequired);
                optional.UnionWith(newOptional);
                if (required.Count > 0 || (manifest.RequiredResourcesBitVector?.Words?.Count ?? 0) > 0)
                    manifest.RequiredResourcesBitVector = BitVector(required, newCount);
                if (optional.Count > 0 || (manifest.OptionalResourcesBitVector?.Words?.Count ?? 0) > 0)
                    manifest.OptionalResourcesBitVector = BitVector(optional, newCount);

                uint addedRequired = 0, addedOptional = 0;
                void Account(int i, bool isRequired)
                {
                    var size = Size(i);
                    if (isRequired) addedRequired += size; else addedOptional += size;
                    int type = Type(i);
                    if (type < 0 || type >= 8) return;
                    manifest.ResourceUsage ??= new List<ZoneResourceUsage>();
                    while (manifest.ResourceUsage.Count < 8) manifest.ResourceUsage.Add(new ZoneResourceUsage());
                    if (isRequired) manifest.ResourceUsage[type].RequiredPageableSize += size;
                    else manifest.ResourceUsage[type].DeferredRequiredSize += size;
                    manifest.ResourceUsage[type].OptionalMemorySize += size;
                }
                foreach (var i in newRequired) Account(i, true);
                foreach (var i in newOptional) Account(i, false);
                manifest.RequiredPageableSize += addedRequired;
                manifest.OptionalMemorySize += addedOptional;
                return (addedRequired, addedOptional);
            }

            var lists = new (List<ZoneManifest> Target, List<ZoneManifest> Reference, bool Unreferenced, string Label)[]
            {
                (target.DesignerZoneManifests, null, false, null),
                (target.GlobalZoneManifests, generated.GlobalZoneManifests, true, "global"),
                (target.UnattachedDesignerZoneManifests, null, false, null),
                (target.DvdForbiddenZoneManifests, null, false, null),
                (target.DvdAlwaysStreamingZoneManifests, null, false, null),
                (target.DefaultBspZoneManifests, generated.DefaultBspZoneManifests, false, "default_bsp"),
                (target.StaticBspZoneManifests, generated.StaticBspZoneManifests, false, "static_bsp"),
                (target.DynamicBspZoneManifests, generated.DynamicBspZoneManifests, false, "dynamic_bsp"),
                (target.CinematicZoneManifests, null, false, null),
                (target.ExpectedZoneManifests, null, false, null),
                (target.FullyPopulatedZoneManifests, null, false, null),
            };
            foreach (var (list, reference, unreferencedRequired, label) in lists)
                if (list != null)
                    for (int m = 0; m < list.Count; m++)
                        Extend(list[m], m == 0 ? reference?.FirstOrDefault() : null, m == 0 && unreferencedRequired, m == 0 ? label : null);

            // zones only: the union of the manifests (required wins); the gestalt totals follow from it
            var zonesOnly = target.ZonesOnlyZoneSetManifests?.FirstOrDefault();
            if (zonesOnly != null)
            {
                var required = new HashSet<int>(); var optional = new HashSet<int>();
                foreach (var list in new[] { target.GlobalZoneManifests, target.DefaultBspZoneManifests, target.StaticBspZoneManifests, target.DynamicBspZoneManifests })
                {
                    var manifest = list?.FirstOrDefault();
                    if (manifest == null) continue;
                    required.UnionWith(Bits(manifest.RequiredResourcesBitVector).Where(i => i >= count));
                    optional.UnionWith(Bits(manifest.OptionalResourcesBitVector).Where(i => i >= count));
                }
                optional.ExceptWith(required);
                // a zones-only manifest as reference: bits are generated indices, so map the new indices back
                var back = map.ToDictionary(kv => kv.Value, kv => kv.Key);
                int Fake(int n) => back.TryGetValue(n, out var g) ? g : -1;
                var reference = new ZoneManifest
                {
                    RequiredResourcesBitVector = BitVector(required.Select(Fake).Where(g => g >= 0), generated.TagResourceTable.Count),
                    OptionalResourcesBitVector = BitVector(optional.Select(Fake).Where(g => g >= 0), generated.TagResourceTable.Count),
                };
                var (addedRequired, addedOptional) = Extend(zonesOnly, reference, true, "zones_only");
                target.ResourcesSize += addedRequired;
                target.GlobalPageableDataSize += addedRequired + addedOptional;
                foreach (var i in required.Concat(optional))
                {
                    int type = Type(i);
                    if (type < 0 || type >= 8) continue;
                    target.GlobalResourceUsage ??= new List<ZoneResourceUsage>();
                    while (target.GlobalResourceUsage.Count < 8) target.GlobalResourceUsage.Add(new ZoneResourceUsage());
                    target.GlobalResourceUsage[type].RequiredPageableSize += Size(i);
                    target.GlobalResourceUsage[type].OptionalMemorySize += Size(i);
                }
            }
            for (int m = 1; m < (target.ZonesOnlyZoneSetManifests?.Count ?? 0); m++)
                Extend(target.ZonesOnlyZoneSetManifests[m], null, false, null);
            return entries.Count;
        }

        //
        // Comparison with an existing zone
        //

        private static string Key(int tag, ResourcePage page) =>
            $"{tag}|{page.NewFlags}|{(uint)page.Index}|{page.CompressedBlockSize}|{page.UncompressedBlockSize}|{page.CrcChecksum}";

        private static HashSet<int> Bits(TagBlockBitVector vector)
        {
            var set = new HashSet<int>();
            if (vector?.Words == null) return set;
            for (int w = 0; w < vector.Words.Count; w++)
                for (int b = 0; b < 32; b++)
                    if ((vector.Words[w].Value & (1u << b)) != 0) set.Add(w * 32 + b);
            return set;
        }

        private void Compare(ResourceGestalt generated, ResourceGestalt existing, Layout layout)
        {
            string KeyOf(ResourceGestalt.TagResourceData e) => e.RuntimeData?.ParentTag == null ? null : Key(e.RuntimeData.ParentTag.Index, e.FileLocation);
            var genKeys = generated.TagResourceTable.Select(KeyOf).ToList();
            var oldKeys = existing.TagResourceTable.Select(KeyOf).ToList();
            var genSet = new HashSet<string>(genKeys);
            var oldIndex = new Dictionary<string, int>();
            for (int i = 0; i < oldKeys.Count; i++) if (oldKeys[i] != null && !oldIndex.ContainsKey(oldKeys[i])) oldIndex[oldKeys[i]] = i;
            var oldSet = new HashSet<string>(oldKeys.Where(k => k != null));
            int nulls = oldKeys.Count(k => k == null);
            int prefix = 0;
            while (prefix < Math.Min(genKeys.Count, oldKeys.Count) && genKeys[prefix] == oldKeys[prefix]) prefix++;
            int samePos = Enumerable.Range(0, Math.Min(genKeys.Count, oldKeys.Count)).Count(i => genKeys[i] == oldKeys[i]);

            Console.WriteLine($"Resource table: generated {generated.TagResourceTable.Count}, existing {existing.TagResourceTable.Count} ({nulls} empty placeholder entries)");
            Console.WriteLine($"  existing entries reproduced: {oldSet.Count(genSet.Contains)} / {oldSet.Count}; generated entries not in existing: {genSet.Count(k => !oldSet.Contains(k))}");
            // longest run of existing entries that the generated table keeps in the same relative order
            var genPos = new Dictionary<string, int>();
            for (int i = 0; i < genKeys.Count; i++) if (genKeys[i] != null && !genPos.ContainsKey(genKeys[i])) genPos[genKeys[i]] = i;
            var tails = new List<int>();
            foreach (var k in oldKeys)
            {
                if (k == null || !genPos.TryGetValue(k, out var gp)) continue;
                int lo = 0, hi = tails.Count;
                while (lo < hi) { int mid = (lo + hi) / 2; if (tails[mid] < gp) lo = mid + 1; else hi = mid; }
                if (lo == tails.Count) tails.Add(gp); else tails[lo] = gp;
            }
            int commonCount = oldKeys.Count(k => k != null && genPos.ContainsKey(k));
            Console.WriteLine($"  order: identical prefix {prefix}, entries at the same position {samePos}, in the same relative order {tails.Count}/{commonCount} ({100.0 * tails.Count / Math.Max(1, commonCount):F1}%)");
            Console.WriteLine($"  definition data identical: {Enumerable.Range(0, existing.TagResourceTable.Count).Count(i => { var k = oldKeys[i]; if (k == null) return false; int g = genKeys.IndexOf(k); return g >= 0 && (existing.TagResourceTable[i].RuntimeData.DefinitionData ?? new byte[0]).SequenceEqual(generated.TagResourceTable[g].RuntimeData.DefinitionData ?? new byte[0]); })}");
            Console.WriteLine($"  tags whose definition failed to deserialize (walked by stored dependencies): {layout.FailedTags}");
            if (Environment.GetEnvironmentVariable("GENERATEZONE_DEBUG") != null && prefix < oldKeys.Count)
            {
                string Tag(int t) => t < 0 ? "null" : $"0x{t:X4}.{Cache.TagCache.GetTag(t)?.Group.Tag}";
                string Chain(int t) { var parts = new List<string>(); for (int k = 0; k < 7 && t >= 0; k++) { parts.Add(Tag(t)); t = layout.Referrer.TryGetValue(t, out var r) ? r : -1; } return string.Join(" <- ", parts); }
                int P(List<string> keys, int i) => i < keys.Count && keys[i] != null ? int.Parse(keys[i].Split('|')[0]) : -1;
                int window = int.TryParse(Environment.GetEnvironmentVariable("GENERATEZONE_DEBUG"), out var wv) ? wv : 20;
                for (int i = Math.Max(0, prefix - 4); i < Math.Min(oldKeys.Count, prefix + window); i++)
                    Console.WriteLine($"   [{i}] existing {Tag(P(oldKeys, i)),-12} generated {Tag(P(genKeys, i)),-12} | existing via {Chain(P(oldKeys, i))}");
            }
            var missing = oldKeys.Where(k => k != null && !genSet.Contains(k)).Select(k => int.Parse(k.Split('|')[0])).Distinct().ToList();
            if (missing.Count > 0)
                Console.WriteLine("  missing parent tags: " + string.Join(" ", missing.Take(20).Select(t => $"0x{t:X4}.{Cache.TagCache.GetTag(t)?.Group.Tag}")));

            void CompareManifests(string label, List<ZoneManifest> gen, List<ZoneManifest> old)
            {
                var g = gen?.FirstOrDefault(); var o = old?.FirstOrDefault();
                if (g == null && o == null) return;
                HashSet<string> Resolve(List<string> keys, TagBlockBitVector bv) => new HashSet<string>(Bits(bv).Where(i => i < keys.Count && keys[i] != null).Select(i => keys[i]));
                var gr = Resolve(genKeys, g?.RequiredResourcesBitVector); var go = Resolve(genKeys, g?.OptionalResourcesBitVector);
                var orq = Resolve(oldKeys, o?.RequiredResourcesBitVector); var oo = Resolve(oldKeys, o?.OptionalResourcesBitVector);
                // classification agreement over the entries both zones contain
                var common = oldSet.Where(genSet.Contains).ToList();
                int agree = common.Count(k => (gr.Contains(k) ? 1 : go.Contains(k) ? 2 : 0) == (orq.Contains(k) ? 1 : oo.Contains(k) ? 2 : 0));
                Console.WriteLine($"{label}: existing required {orq.Count} optional {oo.Count} (size {o?.RequiredPageableSize}/{o?.OptionalMemorySize}); " +
                    $"generated required {gr.Count} optional {go.Count} (size {g?.RequiredPageableSize}/{g?.OptionalMemorySize}); " +
                    $"same classification {agree}/{common.Count} ({100.0 * agree / Math.Max(1, common.Count):F1}%)");
            }
            var labelsPath = Environment.GetEnvironmentVariable("GENERATEZONE_LABELS");
            if (labelsPath != null)
            {
                // existing manifest membership of each generated entry (joins with GENERATEZONE_FEATURES)
                var m = new[] { existing.GlobalZoneManifests, existing.DefaultBspZoneManifests, existing.StaticBspZoneManifests, existing.DynamicBspZoneManifests }
                    .Select(l => l?.FirstOrDefault()).Select(f => (R: Bits(f?.RequiredResourcesBitVector), O: Bits(f?.OptionalResourcesBitVector))).ToArray();
                using (var w = new StreamWriter(labelsPath))
                {
                    var gm = new[] { generated.GlobalZoneManifests, generated.DefaultBspZoneManifests, generated.StaticBspZoneManifests, generated.DynamicBspZoneManifests }
                        .Select(l => l?.FirstOrDefault()).Select(f => (R: Bits(f?.RequiredResourcesBitVector), O: Bits(f?.OptionalResourcesBitVector))).ToArray();
                    string Gen(int i) => string.Join(",", gm.Select(x => x.R.Contains(i) ? "R" : x.O.Contains(i) ? "O" : ""));
                    w.WriteLine("index,global,default_bsp,static_bsp,dynamic_bsp,gen_global,gen_default_bsp,gen_static_bsp,gen_dynamic_bsp");
                    for (int i = 0; i < genKeys.Count; i++)
                    {
                        if (genKeys[i] == null || !oldIndex.TryGetValue(genKeys[i], out var oi)) { w.WriteLine($"{i},?,?,?,?,{Gen(i)}"); continue; }
                        w.WriteLine($"{i}," + string.Join(",", m.Select(x => x.R.Contains(oi) ? "R" : x.O.Contains(oi) ? "O" : "")) + "," + Gen(i));
                    }
                }
            }
            var dumpPath = Environment.GetEnvironmentVariable("GENERATEZONE_DUMP");
            if (dumpPath != null)
            {
                // per existing entry: manifest membership (R = required, O = optional), for working out the manifest rules
                string Member(List<ZoneManifest> m, int i)
                {
                    var f = m?.FirstOrDefault();
                    if (f == null) return "";
                    return (Bits(f.RequiredResourcesBitVector).Contains(i) ? "R" : "") + (Bits(f.OptionalResourcesBitVector).Contains(i) ? "O" : "");
                }
                var genIndex = new Dictionary<string, int>();
                for (int i = 0; i < genKeys.Count; i++) if (genKeys[i] != null && !genIndex.ContainsKey(genKeys[i])) genIndex[genKeys[i]] = i;
                using (var w = new StreamWriter(dumpPath))
                {
                    w.WriteLine("index,tag,group,name,type,flags,dataflags,size,global,default_bsp,static_bsp,dynamic_bsp,zones_only,unattached,dvd_forbidden,gen_pass,quanta");
                    var quanta = new Dictionary<int, int>();
                    foreach (var q in existing.PredictionTable?.PredictionQuanta ?? new List<PredictionQuantum>()) { int qi = (int)(q.ResourceHandle.Value & 0xFFFF); quanta[qi] = quanta.TryGetValue(qi, out var qc) ? qc + 1 : 1; }
                    for (int i = 0; i < existing.TagResourceTable.Count; i++)
                    {
                        var e = existing.TagResourceTable[i];
                        var t = e.RuntimeData?.ParentTag;
                        string pass = "";
                        if (oldKeys[i] != null && genIndex.TryGetValue(oldKeys[i], out var gi))
                            pass = layout.BspEntries.Contains(gi) ? "bsp" : layout.GlobalEntries.Contains(gi) ? "global" : "other";
                        w.WriteLine($"{i},{(t == null ? "" : $"0x{t.Index:X4}")},{t?.Group.Tag},{t?.Name},{e.RuntimeData?.ResourceType},{e.FileLocation?.NewFlags.ToString().Replace(", ", "|")},{e.RuntimeData?.Unknown2},{e.FileLocation?.UncompressedBlockSize}," +
                            $"{Member(existing.GlobalZoneManifests, i)},{Member(existing.DefaultBspZoneManifests, i)},{Member(existing.StaticBspZoneManifests, i)},{Member(existing.DynamicBspZoneManifests, i)}," +
                            $"{Member(existing.ZonesOnlyZoneSetManifests, i)},{Member(existing.UnattachedDesignerZoneManifests, i)},{Member(existing.DvdForbiddenZoneManifests, i)},{pass},{(quanta.TryGetValue(i, out var qn) ? qn : 0)}");
                    }
                }
            }
            CompareManifests("global", generated.GlobalZoneManifests, existing.GlobalZoneManifests);
            CompareManifests("default_bsp", generated.DefaultBspZoneManifests, existing.DefaultBspZoneManifests);
            CompareManifests("static_bsp", generated.StaticBspZoneManifests, existing.StaticBspZoneManifests);
            CompareManifests("dynamic_bsp", generated.DynamicBspZoneManifests, existing.DynamicBspZoneManifests);
            CompareManifests("zones_only", generated.ZonesOnlyZoneSetManifests, existing.ZonesOnlyZoneSetManifests);
            void CompareObjects(string label, List<ZoneManifest> gen, List<ZoneManifest> old)
            {
                Dictionary<int, HashSet<string>> Read(List<ZoneManifest> l, List<string> keys) => (l?.FirstOrDefault()?.ZonesetObjects ?? new List<ZoneManifest.ZoneResourceZonesetObjects>())
                    .Where(o => o.Object != null).GroupBy(o => o.Object.Index).ToDictionary(g => g.Key,
                        g => new HashSet<string>(g.First().Dependencies.Where(d => d.TagResourceIndex >= 0 && d.TagResourceIndex < keys.Count && keys[d.TagResourceIndex] != null).Select(d => keys[d.TagResourceIndex])));
                var g = Read(gen, genKeys); var o = Read(old, oldKeys);
                var common = g.Keys.Where(o.ContainsKey).ToList();
                Console.WriteLine($"{label} zoneset objects: existing {o.Count}, generated {g.Count}, common {common.Count}; identical dependency lists {common.Count(k => g[k].SetEquals(o[k]))}; " +
                    $"dependencies missing {common.Sum(k => o[k].Count(x => !g[k].Contains(x)))}, extra {common.Sum(k => g[k].Count(x => !o[k].Contains(x)))}");
                string Name(int t) => $"0x{t:X4}:{Path.GetFileName(Cache.TagCache.GetTag(t)?.Name ?? "")}";
                if (g.Keys.Any(k => !o.ContainsKey(k)) || o.Keys.Any(k => !g.ContainsKey(k)))
                    Console.WriteLine($"  {label} objects generated only: {string.Join(" ", g.Keys.Where(k => !o.ContainsKey(k)).Select(Name))}; existing only: {string.Join(" ", o.Keys.Where(k => !g.ContainsKey(k)).Select(Name))}");
            }
            CompareObjects("global", generated.GlobalZoneManifests, existing.GlobalZoneManifests);
            CompareObjects("static_bsp", generated.StaticBspZoneManifests, existing.StaticBspZoneManifests);
            CompareObjects("dynamic_bsp", generated.DynamicBspZoneManifests, existing.DynamicBspZoneManifests);
            Console.WriteLine($"sizes: ResourcesSize {generated.ResourcesSize} vs {existing.ResourcesSize}; GlobalPageableDataSize {generated.GlobalPageableDataSize} vs {existing.GlobalPageableDataSize}");
            int OwnerWords(List<ZoneManifest> m) => m?.FirstOrDefault()?.ActiveResourceOwners?.Words?.Count ?? 0;
            Console.WriteLine($"owner bitvector words (global): generated {OwnerWords(generated.GlobalZoneManifests)}, existing {OwnerWords(existing.GlobalZoneManifests)}; unattached: generated {OwnerWords(generated.UnattachedDesignerZoneManifests)}, existing {OwnerWords(existing.UnattachedDesignerZoneManifests)}");
            Console.WriteLine($"not generated (unused at runtime): resource owner bitvectors, prediction table ({existing.PredictionTable?.PredictionQuanta?.Count} quanta)");
        }
    }
}
