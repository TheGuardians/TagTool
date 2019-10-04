namespace TagTool.Scripting.Compiler
{
    public class ScriptInteger : IScriptSyntax
    {
        public long Value;

        public int Line { get; set; }
    }
}