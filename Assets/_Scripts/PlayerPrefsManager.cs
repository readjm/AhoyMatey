using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	const string MASTER_VOLUME_KEY = "master_volume";
    const string MUSIC_VOLUME_KEY = "music_volume";
    const string DIFFICULTY_KEY = "difficulty";
	const string LEVEL_KEY = "level_unlocked_";
    const string PLAYER_FLAG_KEY = "player_flag";
	
	public static void SetMasterVolume(float volume)
	{
		if (volume >= 0f && volume <= 1f)
		{
			PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
		}
		else
		{
			Debug.LogError("Master volume out of range");
		}
	}

    public static void SetMusicVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Music volume out of range");
        }
    }
    
    public static void SetPLayerFlag(int playerFlag)
    {
        if (playerFlag >= 0)
        {
            PlayerPrefs.SetInt(PLAYER_FLAG_KEY, playerFlag);
        }
        else
        {
            Debug.LogError("Player flag out of range");
        }
    }


    public static float GetMasterVolume()
	{
		return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
	}

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }
    public static int GetPlayerFlag()
    {
        return PlayerPrefs.GetInt(PLAYER_FLAG_KEY);
    }

    public static void UnlockLevel(int level)
	{
		if (level <= SceneManager.sceneCount-1)
		{
			PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), 1); //Use 1 for true
		}
		else
		{
			Debug.LogError("Level index out of range");
		}
	}
	
	public static bool IsLevelUnlocked(int level)
	{
		bool isLevelUnlocked = (PlayerPrefs.GetInt(LEVEL_KEY+level.ToString()) == 1);
		
		if (level <= SceneManager.sceneCount-1)
		{
			return isLevelUnlocked;
		}
		else
		{
			Debug.LogError("Level index out of range");
			return false;
		}
	}
	
	public static void SetDifficulty(float difficulty)
	{
		if (difficulty >= 1f && difficulty <= 3f)
		{
			PlayerPrefs.SetFloat(DIFFICULTY_KEY, difficulty);
		}
		else
		{
			Debug.LogError("Difficulty our of range");
		}
	}
	
	public static float GetDifficulty()
	{
		return PlayerPrefs.GetFloat(DIFFICULTY_KEY);
	}	
}
