namespace TagTool.Scripting.Compiler
{
    public class ScriptInteger : IScriptSyntax
    {
        public long Value;

        public int Line { get; set; }

        public override string ToString() =>
            $"ScriptInteger {{ Line: {Line}, Value: {Value} }}";
    }
}