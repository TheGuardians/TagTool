using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.IO
{
    public static class ConsoleHistory
    {
        private static bool IsNewLine { get; set; } = true;
        private static string CurrentLine { get; set; } = "";
        private static byte[] InputBuffer = new byte[4096];

        private static List<string> Lines { get; } = new List<string>();

        public static void Initialize()
        {
            Console.SetOut(new Writer(Console.Out));

            Stream inputStream = Console.OpenStandardInput(InputBuffer.Length);
            Console.SetIn(new Reader(new StreamReader(inputStream, Console.InputEncoding, false, InputBuffer.Length)));
        }

        public static string Dump(string name)
        {
            var time = DateTime.Now;

            if (name == null)
                name = "hott_*_crash.log";

            name = name.Replace("*", $"{time.ToShortDateString()}-{time.ToShortTimeString()}")
                    .Replace(' ', '_')
                    .Replace('\\', '_')
                    .Replace('/', '_')
                    .Replace(':', '_');

            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            using (var writer = new StreamWriter(File.Create($"logs/{name}")))
            {
                foreach (var line in Lines)
                    writer.WriteLine(line);
                if (!IsNewLine)
                    writer.Write(CurrentLine);
            }

            return name;
        }

        public class Writer : TextWriter
        {
            private TextWriter BaseWriter { get; }

            public Writer(TextWriter baseWriter)
                : base()
            {
                BaseWriter = baseWriter;
            }

            public override Encoding Encoding =>
                BaseWriter.Encoding;

            public override object InitializeLifetimeService() =>
                BaseWriter.InitializeLifetimeService();

            private void WriteBase(string value)
            {
                if (IsNewLine)
                    IsNewLine = false;
                CurrentLine += value.ToString();
            }

            private void WriteLineBase(string line)
            {
                if (!IsNewLine)
                {
                    IsNewLine = true;
                    CurrentLine += line;
                }
                else
                {
                    CurrentLine = line;
                }

                Lines.Add(CurrentLine);
                CurrentLine = "";
            }

            public override void Write(bool value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void Write(char value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void Write(char[] buffer)
            {
                WriteBase(new string(buffer));
                BaseWriter.Write(buffer);
            }

            public override void Write(char[] buffer, int index, int count)
            {
                WriteBase(new string(buffer, index, count));
                BaseWriter.Write(buffer, index, count);
            }

            public override void Write(decimal value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void Write(double value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void Write(float value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void Write(int value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void Write(long value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void Write(object value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void Write(string format, object arg0)
            {
                WriteBase(string.Format(format, arg0));
                BaseWriter.Write(format, arg0);
            }

            public override void Write(string format, object arg0, object arg1)
            {
                WriteBase(string.Format(format, arg0, arg1));
                BaseWriter.Write(format, arg0, arg1);
            }

            public override void Write(string format, object arg0, object arg1, object arg2)
            {
                WriteBase(string.Format(format, arg0, arg1, arg2));
                BaseWriter.Write(format, arg0, arg1, arg2);
            }

            public override void Write(string format, params object[] arg)
            {
                WriteBase(string.Format(format, arg));
                BaseWriter.Write(format, arg);
            }

            public override void Write(string value)
            {
                WriteBase(value);
                BaseWriter.Write(value);
            }

            public override void Write(uint value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void Write(ulong value)
            {
                WriteBase(value.ToString());
                BaseWriter.Write(value);
            }

            public override void WriteLine()
            {
                WriteLineBase("");
                BaseWriter.WriteLine();
            }

            public override void WriteLine(bool value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(char value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(char[] buffer)
            {
                WriteLineBase(new string(buffer));
                BaseWriter.WriteLine(buffer);
            }

            public override void WriteLine(char[] buffer, int index, int count)
            {
                WriteLineBase(new string(buffer, index, count));
                base.WriteLine(buffer, index, count);
            }

            public override void WriteLine(decimal value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(double value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(float value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(int value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(long value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(object value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(string value)
            {
                WriteLineBase(value);
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(uint value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(ulong value)
            {
                WriteLineBase(value.ToString());
                BaseWriter.WriteLine(value);
            }

            public override void WriteLine(string format, object arg0)
            {
                WriteLineBase(string.Format(format, arg0));
                BaseWriter.WriteLine(format, arg0);
            }

            public override void WriteLine(string format, object arg0, object arg1)
            {
                WriteLineBase(string.Format(format, arg0, arg1));
                BaseWriter.WriteLine(format, arg0, arg1);
            }

            public override void WriteLine(string format, object arg0, object arg1, object arg2)
            {
                WriteLineBase(string.Format(format, arg0, arg1, arg2));
                BaseWriter.WriteLine(format, arg0, arg1, arg2);
            }

            public override void WriteLine(string format, params object[] arg)
            {
                WriteLineBase(string.Format(format, arg));
                BaseWriter.WriteLine(format, arg);
            }

            public override void Close()
            {
                BaseWriter.Close();
            }

            public override ObjRef CreateObjRef(Type requestedType)
            {
                return BaseWriter.CreateObjRef(requestedType);
            }

            public override void Flush()
            {
                Lines.Add(CurrentLine);
                CurrentLine = "";
                BaseWriter.Flush();
            }

            public override Task FlushAsync()
            {
                throw new NotImplementedException();
            }

            public override Task WriteAsync(char value)
            {
                throw new NotImplementedException();
            }

            public override Task WriteAsync(char[] buffer, int index, int count)
            {
                throw new NotImplementedException();
            }

            public override Task WriteAsync(string value)
            {
                throw new NotImplementedException();
            }

            public override Task WriteLineAsync()
            {
                throw new NotImplementedException();
            }

            public override Task WriteLineAsync(char value)
            {
                throw new NotImplementedException();
            }

            public override Task WriteLineAsync(char[] buffer, int index, int count)
            {
                throw new NotImplementedException();
            }

            public override Task WriteLineAsync(string value)
            {
                throw new NotImplementedException();
            }
        }

        public class Reader : TextReader
        {
            private TextReader BaseReader { get; }

            public Reader(TextReader baseReader)
                : base()
            {
                BaseReader = baseReader;
            }

            public override void Close()
            {
                BaseReader.Close();
                base.Close();
            }

            public override ObjRef CreateObjRef(Type requestedType) =>
                BaseReader.CreateObjRef(requestedType);
            
            public override int Peek() =>
                BaseReader.Peek();

            public override int Read()
            {
                var c = (char)BaseReader.Read();
                CurrentLine += c;
                return (int)c;
            }

            public override int Read(char[] buffer, int index, int count) =>
                BaseReader.Read(buffer, index, count);
            
            public override string ReadLine()
            {
                var line = BaseReader.ReadLine();

                if (!IsNewLine)
                    CurrentLine += line;
                else
                    CurrentLine = line;

                Lines.Add(CurrentLine);
                IsNewLine = true;
                CurrentLine = "";

                return line;
            }

            public override Task<int> ReadAsync(char[] buffer, int index, int count)
            {
                throw new NotImplementedException();
            }

            public override string ReadToEnd()
            {
                throw new NotImplementedException();
            }

            public override Task<string> ReadLineAsync()
            {
                throw new NotImplementedException();
            }

            public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
            {
                throw new NotImplementedException();
            }

            public override Task<string> ReadToEndAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}
