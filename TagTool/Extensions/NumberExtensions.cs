namespace System
{
    public static class NumberExtensions
    {
        public static int ToMultipleOf(this int x, int multiple)
        {
            var mod = x % multiple;
            var midPoint = multiple / 2.0f;
            return x + ((mod > midPoint) ? (multiple - mod) : -mod);
        }

		public static string FormatMilliseconds(this long milliseconds)
		{
			// days
			var d = milliseconds / 86_400_000;
			milliseconds -= d * 86_400_000;
			
			// hours
			var h = milliseconds / 3_600_000;
			milliseconds -= h * 3_600_000;

			// minutes
			var m = milliseconds / 60_000;
			milliseconds -= m * 60_000;

			// seconds
			var s = milliseconds / 1_000;
			milliseconds -= s * 1_000;

			var output = "";
			output += d > 0 ? $"{d}d " : "";
			output += h > 0 ? $"{h}h " : "";
			output += m > 0 ? $"{m}m " : "";
			output += s > 0 ? $"{s}s " : "";
			output += milliseconds > 0 ? $"{milliseconds}ms" : "";
			output = output != "" ? output : "GOOD GOLLY THAT WAS FAST";
			return output;
		}
    }
}
