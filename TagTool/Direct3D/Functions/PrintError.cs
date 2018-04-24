using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Direct3D.Functions
{
	public class PrintError
	{
		public PrintError(string error, out bool isError)
		{
			if (string.IsNullOrEmpty(error))
				isError = false;
			else
			{
				using (StringReader reader = new StringReader(error))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
						Console.WriteLine(line.Substring(line.LastIndexOf('\\') + 1));
				}
				isError = true;
			}
		}
	}
}
