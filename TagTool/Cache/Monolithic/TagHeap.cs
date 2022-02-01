namespace TagTool.Cache.Monolithic
{
    public class TagHeap
    {
        public MonolithicTagFileIndex TagFileIndex;
        
        public TagHeap(PersistChunkReader reader)
        {
            foreach (var chunk in reader.ReadChunks())
            {   
                switch (chunk.Header.Signature.ToString())
                {
                    case "mtag":
                        TagFileIndex = new MonolithicTagFileIndex(new PersistChunkReader(chunk.Stream, reader.Format));
                        break;
                }
            }
        }
    }
}