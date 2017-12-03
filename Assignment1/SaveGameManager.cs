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
    public struct SaveGameData
    {
        public int currLevel;
        public int currScore;
        public int currLives;
        public int currGraphics;
        public float currVolume;

    }

    /// <summary>
    /// Note, major spaghetti coding employed in order to save the player hp.
    /// As we use the player to store their hp, we have to ensure that the player is initialized in order
    /// for saving the player hp.
    /// </summary>
    public static class SaveGameManager
    {
        
        public static string saveGameFileName = "savegame.xml";

        /// <summary>
        /// Initialize our save manager and add dummy values if needed
        /// </summary>
        public static void Initialize()
        {
            string fp = Levels.dir + saveGameFileName;

            // Create dummy data
            if (!File.Exists(fp))
            {
                // Five maximum highscores
                SaveGameData data = new SaveGameData();

                data.currLevel = 0;
                data.currScore = 0;
                data.currVolume = 0.1f;
                data.currGraphics = 1;
                if (Game1.spriteManager.player != null)
                    data.currLives = Game1.spriteManager.player.sprite.hitPoints;
                else
                    data.currLives = 5;


                SaveGameState(data, saveGameFileName);

            }
        }

        /// <summary>
        /// Save the current game
        /// </summary>
        public static void SaveGame()
        {
            SaveGameData data = LoadGameState(saveGameFileName);

            // Save our data
            data.currLevel = Resources.currPlayLevel;
            data.currScore = Resources.score;
            data.currVolume = Resources.volume;
            data.currGraphics = Resources.graphicsQuality;
            data.currLives = Game1.spriteManager.player.sprite.hitPoints;

            SaveGameState(data, saveGameFileName);
        }

        /// <summary>
        /// Save data to file
        /// </summary>
        /// <param name="data">data/state to be saved</param>
        /// <param name="filename">save filename</param>
        public static void SaveGameState(SaveGameData data, string filename)
        {
            // Full path of file
            string fp = Levels.dir + filename;

            // Attempt to open file, if it doesn't exist then we'll create it
            FileStream fileStream = File.Create(fp);
            try
            {
                // Convert to xml
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveGameData));
                xmlSerializer.Serialize(fileStream, data);

            }
            catch (Exception e)
            {
                Console.WriteLine("Aborted saving save file with exception: " + e.ToString());
            }
            finally
            {
                fileStream.Close();
            }
        }

        /// <summary>
        /// Load the game state into a savegamedata struct
        /// </summary>
        /// <param name="filename">save filename</param>
        /// <returns></returns>
        public static SaveGameData LoadGameState(string filename)
        {
            SaveGameData data = new SaveGameData();

            // Full path of file
            string fp = Levels.dir + filename;

            FileStream fileStream = File.Open(fp, FileMode.OpenOrCreate, FileAccess.Read);

            try
            {
                // Convert to HighScoreData
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveGameData));
                data = (SaveGameData)xmlSerializer.Deserialize(fileStream);
            }
            catch (Exception e)
            {
                Console.WriteLine("Aborted loading save file with exception, loading default settings. exception e: " + e.ToString());
                data.currLevel = 0;
                data.currScore = 0;
                data.currVolume = 0.1f;
                data.currGraphics = 1;
                data.currLives = 5;
            }
            finally
            {
                fileStream.Close();
            }


            return data;
        }
    }
}
