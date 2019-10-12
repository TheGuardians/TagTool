namespace TagTool.Scripting.Compiler
{
    public class ScriptSymbol : IScriptSyntax
    {
        public string Value;

        public int Line { get; set; }

        public override string ToString() =>
            $"ScriptSymbol {{ Line: {Line}, Value: \"{Value}\" }}";
    }
}