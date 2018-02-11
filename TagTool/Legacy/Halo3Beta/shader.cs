using TagTool.IO;

using rmsh = TagTool.Legacy.shader;
namespace TagTool.Legacy.Halo3Beta
{
    public class shader : rmsh
    {
        protected shader() { }

        public shader(Base.CacheFile Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            Reader.SeekTo(Address + 12);
            BaseShaderTagID = Reader.ReadInt32();

            #region ShaderProperties Chunk
            Reader.SeekTo(Address + 40);
            int iCount = Reader.ReadInt32();
            int iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                Properties.Add(new ShaderProperties(Cache, iOffset + 132 * i));
            #endregion
        }

        new public class ShaderProperties : rmsh.ShaderProperties
        {
            public ShaderProperties(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                Reader.SeekTo(Address + 12);
                TemplateTagID = Reader.ReadInt32();

                #region ShaderProperties Chunk
                Reader.SeekTo(Address + 16);
                int iCount = Reader.ReadInt32();
                int iOffset = Reader.ReadInt32() - Cache.Magic;
                for (int i = 0; i < iCount; i++)
                    ShaderMaps.Add(new ShaderMap(Cache, iOffset + 24 * i));
                #endregion
          
                #region Tiling Chunk
                Reader.SeekTo(Address + 28);
                iCount = Reader.ReadInt32();
                iOffset = Reader.ReadInt32() - Cache.Magic;
                for (int i = 0; i < iCount; i++)
                    Tilings.Add(new Tiling(Cache, iOffset + 16 * i));
                #endregion
            }

            new public class ShaderMap : rmsh.ShaderProperties.ShaderMap
            {
                public ShaderMap(Base.CacheFile Cache, int Address)
                {
                    EndianReader Reader = Cache.Reader;
                    Reader.SeekTo(Address);

                    Reader.SeekTo(Address + 12);
                    BitmapTagID = Reader.ReadInt32();
                    Type = Reader.ReadInt32();
                    TilingIndex = Reader.ReadInt16();
                    Reader.ReadInt16();
                }
            }

            new public class Tiling : rmsh.ShaderProperties.Tiling
            {
                public Tiling(Base.CacheFile Cache, int Address)
                {
                    EndianReader Reader = Cache.Reader;
                    Reader.SeekTo(Address);

                    UTiling = Reader.ReadSingle();
                    VTiling = Reader.ReadSingle();
                    Unknown0 = Reader.ReadSingle();
                    Unknown1 = Reader.ReadSingle();
                }
            }
        }
    }
}
