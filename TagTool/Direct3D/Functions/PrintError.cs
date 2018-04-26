using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TagTool.Direct3D.Functions
{
	public class PrintError
	{
		public PrintError(string code, string error, out bool isError)
		{
			var startForegroundColor = Console.ForegroundColor;

			var errorLocation = new string[] { "", "", "" };

			if (string.IsNullOrEmpty(error))
				isError = false;
			else
			{
				errorLocation = new string(error.SkipWhile(c => c != '(').TakeWhile(c => c != ')').ToArray()).Replace("(", "").Replace(")", "").Split(',');
				error = Regex.Replace(error, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
				isError = true;
			}

			try
			{
				int.TryParse(errorLocation[0], out int errorLine);
				var errorRange = errorLocation[1].Split('-');
				int.TryParse(errorRange[0], out int errorStart);
				int.TryParse(errorRange[1], out int errorEnd);

				using (StringReader sr = new StringReader(code))
				{
					string line;

					int i = 1;
					while ((line = sr.ReadLine()) != null)
					{
						if (isError == true && i == errorLine)
						{
							Console.ForegroundColor = ConsoleColor.White;
							Console.WriteLine();
							Console.WriteLine($"// {error}");
							Console.CursorTop = Console.CursorTop - 1;
							Console.ForegroundColor = ConsoleColor.Magenta;
							Console.Write($"/*{i}*/	");

							for (var j = 0; j < line.Length; j++)
							{
								if (j >= errorStart - 1 && j <= errorEnd - 1)
								{
									Console.ForegroundColor = ConsoleColor.Red;
									Console.Write(line[j]);
								}
								else
								{
									Console.ForegroundColor = ConsoleColor.Yellow;
									Console.Write(line[j]);
								}
							}
							Console.WriteLine();
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.DarkGreen;
							Console.Write($"/*{i}*/	");
							Console.ForegroundColor = ConsoleColor.Gray;
							Console.WriteLine(line);
						}
						i++;
					}
				}
			}
			catch
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine(error);
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(code);
			}

			Console.ForegroundColor = startForegroundColor;
		}
	}
}
