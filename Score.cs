using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreGathererV2
{
    class Score
    {
        private int beatmapset;
        private int beatmap;
        private ulong score_id;
        private ulong score;
        private string username;
        private ulong count300;
        private ulong count100;
        private ulong count50;
        private ulong countmiss;
        private int maxcombo;
        private int countkatu;
        private int countgeki;
        private bool perfect;
        private int user_id;
        private DateTime date;
        private string rank;
        private float pp;
        private bool replay_available;

        public int Beatmapset { get => beatmapset; set => beatmapset = value; }
        public int Beatmap { get => beatmap; set => beatmap = value; }
        public ulong ScoreID { get => score_id; set => score_id = value; }
        public ulong RankedScore { get => score; set => score = value; }
        public string Username { get => username; set => username = value; }
        public ulong Count300 { get => count300; set => count300 = value; }
        public ulong Count100 { get => count100; set => count100 = value; }
        public ulong Count50 { get => count50; set => count50 = value; }
        public ulong Countmiss { get => countmiss; set => countmiss = value; }
        public int Maxcombo { get => maxcombo; set => maxcombo = value; }
        public int Countkatu { get => countkatu; set => countkatu = value; }
        public int Countgeki { get => countgeki; set => countgeki = value; }
        public bool Perfect { get => perfect; set => perfect = value; }
        public int UserID { get => user_id; set => user_id = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Rank { get => rank; set => rank = value; }
        public float PP { get => pp; set => pp = value; }
        public bool ReplayAvailable { get => replay_available; set => replay_available = value; }
    }
}
