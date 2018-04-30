using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public static class TimeHelper
    {
		public static string GetTime(int seconds, string format = "{0}:{1}")
		{
			int minutes = seconds / 60;
			seconds -= minutes * 60;
			return string.Format(format, minutes, seconds);
		}
    }
}
