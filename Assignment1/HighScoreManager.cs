using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assignment1
{
    [Serializable]
    public struct HighScoreData
    {
        public int[] score;
        public int[] level;
        public int count;

        public HighScoreData(int count)
        {
            score = new int[count];
            level = new int[count];
            this.count = count;
        }
    }

    /// <summary>
    /// Based off of http://xnaessentials.com/tutorials/highscores.aspx
    /// </summary>
    public static class HighScoreManager
    {
        // High scores filename
        public static string highScoresFileName = "highscores.xml";

        // Amonut of scores to be added
        public static int count = 10;

        /// <summary>
        /// Create our high score manager and add any dummy data if needed
        /// </summary>
        public static void Initialize()
        {
            string fp = Levels.dir + highScoresFileName;

            // Create dummy data
            if (!File.Exists(fp))
            {
                // Five maximum highscores
                HighScoreData data = new HighScoreData(count);

                data.score[0] = 952455;
                data.level[0] = 5;

                data.score[1] = 55000;
                data.level[1] = 5;

                data.score[2] = 53000;
                data.level[2] = 5;

                data.score[3] = 39000;
                data.level[3] = 4;

                data.score[4] = 29000;
                data.level[4] = 3;

                data.score[5] = 25000;
                data.level[5] = 3;

                data.score[6] = 24500;
                data.level[6] = 3;

                data.score[7] = 21000;
                data.level[7] = 3;

                data.score[8] = 19000;
                data.level[8] = 2;

                data.score[9] = 9000;
                data.level[9] = 1;
                
                SaveHighScores(data, highScoresFileName);

            }
        }

        /// <summary>
        /// Add a highscore and save it
        /// </summary>
        /// <param name="score">score to be saved</param>
        /// <param name="currLevel">current play level</param>
        public static void AddHighScore(int score, int currLevel)
        {
            HighScoreData data = LoadHighScores(highScoresFileName);

            int scoreIndex = -1;

            for (int i = 0; i < data.count; i++)
            {
                if (score > data.score[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex > -1)
            {
                //New high score found ... do swaps
                for (int i = data.count - 1; i > scoreIndex; i--)
                {
                    data.score[i] = data.score[i - 1];
                    data.level[i] = data.level[i - 1];
                }

                data.score[scoreIndex] = score;
                data.level[scoreIndex] = currLevel + 1;

                SaveHighScores(data, highScoresFileName);
            }
        }

        /// <summary>
        /// Actually save data to disk/file
        /// </summary>
        /// <param name="data">highscore data to be saved</param>
        /// <param name="filename">highscore filename</param>
        public static void SaveHighScores(HighScoreData data, string filename)
        {
            // Full path of file
            string fp = Levels.dir + filename;

            // Attempt to open file, if it doesn't exist then we'll create it
            FileStream fileStream = File.Create(fp);
            try
            {
                // Convert to xml
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(HighScoreData));
                xmlSerializer.Serialize(fileStream, data);

            } catch (Exception e)
            {
                Console.WriteLine("Aborted saving hs file with exception: " + e.ToString());
            } finally
            {
                fileStream.Close();
            }
        }

        /// <summary>
        /// Load highscores in highscoredata struct
        /// </summary>
        /// <param name="filename">highscore filename</param>
        /// <returns></returns>
        public static HighScoreData LoadHighScores(string filename)
        {
            HighScoreData data = new HighScoreData(count);

            // Full path of file
            string fp = Levels.dir + filename;

            FileStream fileStream = File.Open(fp, FileMode.OpenOrCreate, FileAccess.Read);

            try
            {     
                // Convert to HighScoreData
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(HighScoreData));
                data = (HighScoreData)xmlSerializer.Deserialize(fileStream);
            } catch (Exception e)
            {
                Console.WriteLine("Aborted loading hs file with exception: " + e.ToString());
            }
            finally
            {
                fileStream.Close();
            }


            return data;
        }

    }
}
