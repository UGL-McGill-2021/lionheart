using System;
using System.Collections.Generic;

/// <summary>
/// Author: Ziqi Li
/// A class represent the player data using for ProgressSystem
/// </summary>
public class PlayerData
{
    private HashSet<string> unlockedLevels;

    public PlayerData()
    {
        unlockedLevels = new HashSet<string>();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to unlock level in the current PlayerData
    /// </summary>
    /// <param name="levelName"></param>
    public void UnlockLevel(string levelName)
    {
        unlockedLevels.Add(levelName);
    }


}
