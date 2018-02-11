using TagTool.Cache;
using TagTool.TagDefinitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private GlobalPixelShader ConvertGlobalPixelShader(GlobalPixelShader glps)
        {
            //add conversion code when ready
            return glps;
        }

        private GlobalVertexShader ConvertGlobalVertexShader(GlobalVertexShader glvs)
        {
            //add conversion code when ready
            return glvs;
        }

        private PixelShader ConvertPixelShader(PixelShader pixl)
        {
            //add conversion code when ready
            return pixl;
        }
        
        private RasterizerGlobals ConvertRasterizerGlobals(RasterizerGlobals rasg)
        {
            if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                rasg.Unknown6HO = 6;
            }
            else
            {
                rasg.Unknown6HO = rasg.Unknown6;
            }
            return rasg;
        }
    }
}

