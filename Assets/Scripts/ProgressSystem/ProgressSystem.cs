using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Author: Ziqi Li
/// A static class for the saving&loading system
/// </summary>
public static class ProgressSystem
{
    // the PlayerData instance for the current player
    private static PlayerData currentPlayerData = null;

    /// <summary>
    /// Author: Ziqi Li
    /// Function to update the PlayerData and update the corresponding file
    /// </summary>
    public static void SavePlayerData()
    {
        if(currentPlayerData != null)
        {
            BinaryFormatter fileFormatter = new BinaryFormatter();
            string filePath = Application.persistentDataPath + "/BesunderProgressData.data";

            // create a file, it will overwrite existing file if existed
            FileStream stream = new FileStream(filePath, FileMode.Create);

            fileFormatter.Serialize(stream, currentPlayerData);  // convert the PlayerData to binary stream
            stream.Close();
        }
        else
        {
            throw new System.Exception("The current player data is null, please load the data first");
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to read the PlayerData from the saving file and return it
    /// Also update it to the currentPlayerData field
    /// </summary>
    public static PlayerData LoadPlayerData()
    {
        string filePath = Application.persistentDataPath + "/BesunderProgressData.data";

        // if the saving file exists
        if (File.Exists(filePath))
        {
            BinaryFormatter fileFormatter = new BinaryFormatter();
            // create a file, it will overwrite existing file if existed
            FileStream stream = new FileStream(filePath, FileMode.Open);
            PlayerData data = (PlayerData) fileFormatter.Deserialize(stream);  // convert the PlayerData to binary stream
            stream.Close();

            currentPlayerData = data;
            return currentPlayerData;
        }
        else
        {
            currentPlayerData = new PlayerData();
            return currentPlayerData;
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to get the current player data
    /// </summary>
    public static PlayerData GetCurrentPlayerData()
    {
        if (currentPlayerData != null)
        {
            return currentPlayerData;
        }
        else
        {
            throw new System.Exception("The current player data is null, please load the data first");
        }
    }



}
