using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ScoreGathererV2
{
    class Program
    {
        static string clientId = "";
        static string clientSecret = "";

        static string apiUrl = "https://osu.ppy.sh/api/v2";
        static string tokenUrl = "https://osu.ppy.sh/oauth/token";
        static string accessToken = null;


        static async Task Main(string[] args)
        {
            Console.WriteLine("Loading...");

            accessToken = await GetToken();
            Console.WriteLine(accessToken != null ? "Got Token!!!" : "No token found!" );
            

            bool dateNotFalse = false;
            List<Score> Scores = new List<Score>();
            string mode = "taiko";
            ulong scoreID = 1204396;
            DateTime lastTime = new DateTime(2007, 1, 1);

            while (lastTime < new DateTime(2009, 9, 13))
            {
                Console.WriteLine("Currently Checking: " + scoreID + "\tScores found: " + Scores.Count + "\tCurrent Time: " + lastTime);
                var score = await GetScore(mode, scoreID);

                if (score != null)
                {
                    string dateString = ((dynamic)score).created_at;
                    lastTime = DateTime.Parse(dateString, CultureInfo.InvariantCulture);
                    if (DateTime.Parse(dateString, CultureInfo.InvariantCulture) >= new DateTime(2009,9,13))
                    {
                        continue;
                    }

                    if (((dynamic)score).beatmap.status != "ranked")
                    {
                        scoreID++;
                        continue; 
                    }
                        

                    var newScore = new Score();

                    newScore.Beatmapset = ((dynamic)score).beatmap.beatmapset_id;
                    newScore.Beatmap = ((dynamic)score).beatmap.id;
                    newScore.ScoreID = scoreID;
                    newScore.RankedScore = ((dynamic)score).score;
                    newScore.Username = ((dynamic)score).user.username;
                    newScore.Count300 = ((dynamic)score).statistics.count_300;
                    newScore.Count100 = ((dynamic)score).statistics.count_100;
                    newScore.Count50 = ((dynamic)score).statistics.count_50;
                    newScore.Countmiss = ((dynamic)score).statistics.count_miss;
                    newScore.Maxcombo = ((dynamic)score).max_combo;
                    newScore.Countgeki = ((dynamic)score).statistics.count_geki;
                    newScore.Countkatu = ((dynamic)score).statistics.count_katu;
                    newScore.Perfect = ((dynamic)score).perfect;
                    newScore.UserID = ((dynamic)score).user.id;
                    newScore.Date = DateTime.Parse(dateString, CultureInfo.InvariantCulture);
                    newScore.Rank = ((dynamic)score).rank;
                    newScore.ReplayAvailable = ((dynamic)score).replay;

                    Scores.Add(newScore);
                }
                scoreID++;
            }

            TextWriter textWriter = new StreamWriter("7thday.csv");
            var ucsv = new CsvWriter(textWriter, CultureInfo.InvariantCulture);

            ucsv.WriteRecords(Scores);
            ucsv.Flush();
            textWriter.Close();

        }

        private static async Task<string> GetToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(tokenUrl);

                // Set the intended response to be JSON.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                // Build the data to POST.
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("client_id", clientId));
                postData.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
                postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                postData.Add(new KeyValuePair<string, string>("scope", "public"));

                FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                // Post the server and wait for the resonse
                HttpResponseMessage responseMessage = await client.PostAsync("", content);
                string jsonString = await responseMessage.Content.ReadAsStringAsync();
                object responseData = JsonConvert.DeserializeObject(jsonString);

                return ((dynamic)responseData).access_token;
            }
        }

        private static async Task<object> GetScore(string mode, ulong ScoreID)
        {
            object responseData;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://osu.ppy.sh/api/v2/scores/" + mode + "/" + ScoreID.ToString());

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                HttpResponseMessage responseMessage = await client.GetAsync(client.BaseAddress);

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                    return null;

                string jsonString = await responseMessage.Content.ReadAsStringAsync();
                responseData = JsonConvert.DeserializeObject(jsonString);
            }
            return responseData;
        }
    }
}
