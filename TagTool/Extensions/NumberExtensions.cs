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
    }
}
