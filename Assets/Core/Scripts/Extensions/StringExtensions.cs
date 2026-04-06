public static class TypeExtensions
{
    public static int ComputeFNV1Hash(this string str)
    {
        uint hash = 2166136261;
        foreach (char c in str)
        {
            hash = (hash ^ c) * 16777619;
        }
        return unchecked((int)hash);
    }
}