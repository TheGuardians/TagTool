using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Scripting.Compiler
{
    public class ScriptSyntaxReader
    {
        /// <summary>
        /// The binary reader which reads from the expression data stream.
        /// </summary>
        public BinaryReader Reader { get; }

        public ScriptSyntaxReader(Stream stream)
        {
            Reader = new BinaryReader(stream);
        }

        /// <summary>
        /// Expression syntax delimiters.
        /// </summary>
        public const string Delimiters = "()\"'`,;";

        public bool LastTokenValid = false;
        public string LastToken = "";

        public void SetLastToken(string token)
        {
            LastToken = token;
            LastTokenValid = true;
        }

        public string ReadToken()
        {
            var result = "";

            if (LastTokenValid)
            {
                result = LastToken;

                LastToken = "";
                LastTokenValid = false;

                return result;
            }

            if (Reader.BaseStream.Position >= Reader.BaseStream.Length)
                return null;

            var c = '\0';
            while ((Reader.BaseStream.Position < Reader.BaseStream.Length) && char.IsWhiteSpace(c = Reader.ReadChar())) ;

            if (Reader.BaseStream.Position >= Reader.BaseStream.Length)
                return null;

            if (Delimiters.Contains(c))
                return c.ToString();

            result += c;

            while (Reader.BaseStream.Position < Reader.BaseStream.Length)
            {
                c = Reader.ReadChar();

                if (Delimiters.Contains(c) || char.IsWhiteSpace(c))
                {
                    Reader.BaseStream.Position--;
                    break;
                }
                
                result += c;
            }
            
            return result;
        }

        public IScriptSyntax ReadGroup()
        {
            var token = ReadToken();

            switch (token)
            {
                case ")":
                    return default(ScriptInvalid);

                case ".":
                    {
                        var node = Read();
                        
                        if ((token = ReadToken()) == null || token != ")")
                            throw new Exception(token != null ? $"Syntax error: {token}" : "End of file");

                        return node;
                    }

                default:
                    break;
            }

            SetLastToken(token);

            return new ScriptGroup
            {
                Head = Read() ?? throw new Exception("End of file"),
                Tail = ReadGroup() ?? throw new Exception("End of file")
            };
        }

        public ScriptString ReadString()
        {
            var result = "";

            for (char c; (c = Reader.ReadChar()) != '"'; result += c) ;

            return new ScriptString
            {
                Value = result
            };
        }

        public IScriptSyntax ReadQuote()
        {
            return new ScriptGroup
            {
                Head = new ScriptSymbol { Value = "quote" },
                Tail = new ScriptGroup
                {
                    Head = Read() ?? throw new Exception("End of file"),
                    Tail = new ScriptInvalid()
                }
            };
        }

        public IScriptSyntax ReadQuasiQuote()
        {
            return new ScriptGroup
            {
                Head = new ScriptSymbol { Value = "quasiquote" },
                Tail = new ScriptGroup
                {
                    Head = Read() ?? throw new Exception("End of file"),
                    Tail = new ScriptInvalid()
                }
            };
        }

        public IScriptSyntax ReadUnquote()
        {
            return new ScriptGroup
            {
                Head = new ScriptSymbol { Value = "unquote" },
                Tail = new ScriptGroup
                {
                    Head = Read() ?? throw new Exception("End of file"),
                    Tail = new ScriptInvalid()
                }
            };
        }

        public IScriptSyntax Read()
        {
        begin:
            if (Reader.BaseStream.Position >= Reader.BaseStream.Length)
                return null;

            var token = ReadToken();

            if (token == null)
                return null;

            switch (token)
            {
                case "(":
                    return ReadGroup();

                case "\"":
                    return ReadString();

                case "'":
                    return ReadQuote();

                case "`":
                    return ReadQuasiQuote();

                case ",":
                    return ReadUnquote();

                case ";":
                    for (char c; (Reader.BaseStream.Position < Reader.BaseStream.Length) && ((c = Reader.ReadChar()) != '\r' && c != '\n');) ;
                    goto begin;

                default:
                    if (bool.TryParse(token, out var boolean))
                        return new ScriptBoolean { Value = boolean };
                    else if (long.TryParse(token, out var integer))
                        return new ScriptInteger { Value = integer };
                    else if (double.TryParse(token, out var real))
                        return new ScriptReal { Value = real };
                    else
                        return new ScriptSymbol { Value = token };
            }
        }

        public List<IScriptSyntax> ReadToEnd()
        {
            var nodes = new List<IScriptSyntax>();

            while (Reader.BaseStream.Position < Reader.BaseStream.Length)
            {
                var node = Read();

                if (node == null)
                    break;

                nodes.Add(node);
            }

            return nodes;
        }
    }
}