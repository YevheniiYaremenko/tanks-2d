using UnityEngine;
using System.Linq;

public static class StringHelper
{
    ///<summary>
    /// Split string on separate words
    ///</summary>
    public static string SeparateWords(this string origin, bool extensionOnly = true)
    {
        if (extensionOnly)
        {
            origin = System.IO.Path.GetExtension(origin);
            origin = origin.Remove(0, 1);
        }

        foreach (var ch in origin.Where(x => x >= 'A' && x <= 'Z').Distinct())
        {
            origin = origin.Replace(ch.ToString(), " " + ch);
        }

        return origin;
    }
}
