using UnityEngine;

public static class TimeHelper
{
    ///<summary>
    /// Convert seconds in formatted string of minutes and seconds
    ///</summary>
    public static string GetTime(int seconds, string format = "{0:00}:{1:00}")
    {
        int minutes = seconds / 60;
        seconds -= minutes * 60;
        return string.Format(format, minutes, seconds);
    }
}
