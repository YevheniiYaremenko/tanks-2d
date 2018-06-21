using Unity.Mathematics;
using UnityEngine;

namespace Game.Model
{
	public class Session
	{
		public int score = 0;
		public int kills = 0;
	}

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Data", order = 0)]
    public class Data : ScriptableObject
    {	
        PlayerPrefsInt sessions = new PlayerPrefsInt("Data_Sessions", 0);
		public int Sessions
		{
			get { return sessions; }
		}

        PlayerPrefsInt bestScore = new PlayerPrefsInt("Data_BestScore", 0);
		public int BestScore
		{
            get { return bestScore; }
		}

        PlayerPrefsInt maxkills = new PlayerPrefsInt("Data_MaxKills", 0);
        public int MaxKills
        {
            get { return maxkills; }
        }

		public void RegisterSession(Session session)
		{
			sessions.Value++;
			bestScore.Value = math.max(bestScore, session.score);
			maxkills.Value = math.max(maxkills, session.kills);
		}
    }
}
