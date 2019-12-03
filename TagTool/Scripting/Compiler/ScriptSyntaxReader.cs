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

        public int Line = 1;

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
                return "eof";

            var c = '\0';
            while ((Reader.BaseStream.Position < Reader.BaseStream.Length) && char.IsWhiteSpace(c = Reader.ReadChar()))
            {
                if (c == '\r' || c == '\n')
                    Line++;
            }

            if (Reader.BaseStream.Position >= Reader.BaseStream.Length)
                return "eof";

            if (Delimiters.Contains(c))
                return c.ToString();

            result += c;

            while (Reader.BaseStream.Position < Reader.BaseStream.Length)
            {
                c = Reader.ReadChar();

                if (c == '\r' || c == '\n')
                    Line++;

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
                    return new ScriptInvalid { Line = Line };

                case "eof":
                    return new EndOfFile { Line = Line };

                case ".":
                    {
                        var node = Read();

                        if ((token = ReadToken()) == "eof" || token != ")")
                            throw new Exception(token != "eof" ? $"Syntax error: {token}" : "End of file");

                        return node;
                    }

                default:
                    break;
            }

            SetLastToken(token);

            return new ScriptGroup
            {
                Head = Read(),
                Tail = ReadGroup(),
                Line = Line
            };
        }

        public ScriptString ReadString()
        {
            var result = "";

            for (char c; (c = Reader.ReadChar()) != '"'; result += c)
            {
                if (c == '\r' || c == '\n')
                    Line++;
            }

            return new ScriptString
            {
                Value = result,
                Line = Line
            };
        }

        public IScriptSyntax ReadQuote()
        {
            return new ScriptGroup
            {
                Head = new ScriptSymbol { Value = "quote", Line = Line },
                Tail = new ScriptGroup
                {
                    Head = Read(),
                    Tail = new ScriptInvalid(),
                    Line = Line
                }
            };
        }

        public IScriptSyntax ReadQuasiQuote()
        {
            return new ScriptGroup
            {
                Head = new ScriptSymbol { Value = "quasiquote", Line = Line },
                Tail = new ScriptGroup
                {
                    Head = Read(),
                    Tail = new ScriptInvalid(),
                    Line = Line
                }
            };
        }

        public IScriptSyntax ReadUnquote()
        {
            return new ScriptGroup
            {
                Head = new ScriptSymbol { Value = "unquote", Line = Line },
                Tail = new ScriptGroup
                {
                    Head = Read(),
                    Tail = new ScriptInvalid(),
                    Line = Line
                }
            };
        }

        public IScriptSyntax Read()
        {
        begin:
            if (Reader.BaseStream.Position >= Reader.BaseStream.Length)
                return new EndOfFile { Line = Line };

            var token = ReadToken();

            switch (token)
            {
                case "eof":
                    return new EndOfFile { Line = Line }; //handle this in a way that does not suck. Ew.

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
                    for (char c; (Reader.BaseStream.Position < Reader.BaseStream.Length) && ((c = Reader.ReadChar()) != '\r' && c != '\n');)
                    {
                        if (c == '\r' || c == '\n')
                            Line++;
                    }
                    goto begin;

                default:
                    if (bool.TryParse(token, out var boolean))
                        return new ScriptBoolean { Value = boolean, Line = Line };
                    else if (long.TryParse(token, out var integer))
                        return new ScriptInteger { Value = integer, Line = Line };
                    else if (double.TryParse(token, out var real))
                        return new ScriptReal { Value = real, Line = Line };
                    else
                        return new ScriptSymbol { Value = token, Line = Line };
            }
        }

        public List<IScriptSyntax> ReadToEnd()
        {
            var nodes = new List<IScriptSyntax>();

            while (Reader.BaseStream.Position < Reader.BaseStream.Length)
            {
                var node = Read();

                if (node is EndOfFile)
                    break;

                nodes.Add(node);
            }

            return nodes;
        }
    }
}