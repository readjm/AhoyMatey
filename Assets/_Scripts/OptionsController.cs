using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsController : MonoBehaviour {

    public Dropdown flagDropdown;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    //public Slider difficultySlider;
    public LevelManager levelManager;
	
	private MusicPlayer musicPlayer;
	
	void Start ()
	{
		musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();
        masterVolumeSlider.value = PlayerPrefsManager.GetMasterVolume();
        musicVolumeSlider.value = PlayerPrefsManager.GetMusicVolume();

        //difficultySlider.value = PlayerPrefsManager.GetDifficulty();

    }
	
	void Update ()
	{
        AudioListener.volume = masterVolumeSlider.value;
        if (musicPlayer)
		{
			musicPlayer.SetVolume(musicVolumeSlider.value);
		}
	}
	
	public void SaveAndExit()
	{
        PlayerPrefsManager.SetMasterVolume(masterVolumeSlider.value);
        PlayerPrefsManager.SetMusicVolume(musicVolumeSlider.value);

        AudioListener.volume = masterVolumeSlider.value;

        if (musicPlayer)
		{
			musicPlayer.SetVolume(musicVolumeSlider.value);
		}

        //PlayerPrefsManager.SetDifficulty(difficultySlider.value);
        //levelManager.LoadLevel("Menu");
	}
	
    public void SetPlayerFlag()
    {
        PlayerPrefsManager.SetPLayerFlag(flagDropdown.value);
        GameObject.Find("Flag").GetComponent<FlagController>().SetFlag(flagDropdown.value);
    }

	public void SetDefaults()
	{
        masterVolumeSlider.value = 1f;
        musicVolumeSlider.value = .8f;
        //difficultySlider.value = 2f;
    }
}
