using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance = null;
	
	public AudioClip[] trackList;
	private AudioSource audioSource;

    //delegate void OnSceneWasLoaded(int level);
    //OnSceneWasLoaded onLevelWasLoaded;

	void Awake ()
	{
		Debug.Log("Music Player Awake " + GetInstanceID());
		
		if (instance != null && instance != this)
		{
			
			GameObject.Destroy(gameObject);
			Debug.Log ("Destroyed duplicate Music Player");
		}
		else
		{
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
			audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneWasLoaded;
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		Debug.Log("Music Player Start " + GetInstanceID());
	}

	public void PlayTrack(int track)
	{
		Debug.Log ("Level/track #: " + trackList[track]);
		if (this.GetComponent<AudioSource>().clip != trackList[track])
		{
			this.GetComponent<AudioSource>().clip = trackList[track];
			this.GetComponent<AudioSource>().Play();
		}
	}
	
	void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
	{
		if (this.GetComponent<AudioSource>().clip != trackList[scene.buildIndex])
		{
			audioSource.clip = trackList[scene.buildIndex];
			audioSource.loop = true;
			audioSource.Play();
		}
	}
	
	public void SetVolume(float volume)
	{
		audioSource.volume = volume;
	}
	
}
