namespace TagTool.Scripting.Compiler
{
    public class ScriptBoolean : IScriptSyntax
    {
        public bool Value;

        public int Line { get; set; }

        public override string ToString() =>
            $"ScriptBoolean {{ Line: {Line}, Value: {Value} }}";
    }
}