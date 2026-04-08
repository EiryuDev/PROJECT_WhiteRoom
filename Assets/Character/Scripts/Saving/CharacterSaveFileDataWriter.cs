using System;
using System.IO;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterSaveFileDataWriter
    {
        public string saveDataDirectoryPath = ""; // Location for save data directory
        public string saveFileName = ""; // Name of the save file 

        // LOGIC: Before we save the file, we must check to see if one of this character slot already exists (max 10 character slots)
        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // BELOW CODE: Used to delete save files
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        // BELOW CODE: Used to create a save file upon starting a new game
        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            // BELOW CODE: Make a path to save file (a location on the machine)
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                // BELOW CODE: Create the directory the file will be written to, if it does not already exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

                // BELOW CODE: Serialize the c# game data object into json
                string dataToStore = JsonUtility.ToJson(characterData, true);

                // BELOW CODE: Write the file to our system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex) 
            {
                Debug.LogError("ERROR WHILIST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + ex);
            }
        }

        // BELOW CODE: Used to load a save file upon loading a previous game
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;
            // BELOW CODE: Make a path to load file (a location on the machine)
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if(File.Exists(loadPath))
            {
                try 
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // BELOW CODE: Deserialised the data from json back to unity
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
            }

            return characterData;
        }
    }
}