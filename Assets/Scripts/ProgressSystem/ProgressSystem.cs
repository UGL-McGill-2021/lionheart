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
        BinaryFormatter fileFormatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/BesunderPlayerData.data";

        // create a file, it will overwrite existing file if existed
        FileStream stream = new FileStream(filePath, FileMode.Create); 

        fileFormatter.Serialize(stream, currentPlayerData);  // convert the PlayerData to binary stream
        stream.Close();
    } 


    
}
