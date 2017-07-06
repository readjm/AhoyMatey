using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public float autoLoadNextLevelAfter;
	public bool sceneSkippable = false;
	
	public AudioClip restartSound;
	
	private bool pause = false;
	private OptionsController optionsController;
    private Canvas uiCanvas; 
	
	void Start()
	{
        //Screen.lockCursor = true;

        //Screen.fullScreen = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
		
		if (GameObject.FindObjectOfType<OptionsController>())
		{
			optionsController = GameObject.FindObjectOfType<OptionsController>();
			optionsController.gameObject.SetActive(false);

            uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
		}
		
		if (autoLoadNextLevelAfter <= 0f)
		{
			Debug.Log("Level auto load disabled");
		}
		else
		{
            Debug.Log ("Autoloading level " + SceneManager.GetActiveScene().buildIndex + 1);
			Invoke("LoadNextLevel", autoLoadNextLevelAfter);
		}
	}
	
	void Update()
	{
		if (sceneSkippable && Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log ("Skipping Scene");
			LoadNextLevel();
		}
		
		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}
	}

	public void CatStarDestroyed()
	{
		StartCoroutine(WaitOnDeath());
	}
	
	public void LoadLevel(string name)
	{
		Debug.Log("Level load requested for : " + name);
		SceneManager.LoadScene(name);
	}
	
	public void LoadLevel(int level)
	{
		Debug.Log("Level load requested for: " + level);
		
		if (level >= SceneManager.sceneCount)
		{
			Cursor.visible = true;
		}
		SceneManager.LoadScene(level);		
	}
	
	public void LoadNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

	}
	

    public void OptionsOverlay()
    {
        pause = !pause;
        if (pause)
        {
            if (GameObject.FindObjectOfType<MusicPlayer>())
            {
                GameObject.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().Pause();
            }

            optionsController.gameObject.SetActive(true);
            uiCanvas.gameObject.SetActive(false);
        }
        else
        {
            if (GameObject.FindObjectOfType<MusicPlayer>())
            {
                GameObject.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().Play();
            }

            optionsController.gameObject.SetActive(false);
            uiCanvas.gameObject.SetActive(true);
        }
    }


	public void Pause()
	{
		pause = !pause;
		if (pause)
		{
			Time.timeScale = 0f;
			if (GameObject.FindObjectOfType<MusicPlayer>())
			{
				GameObject.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().Pause();
			}
						
			optionsController.gameObject.SetActive(true);
		}
		else
		{
			Time.timeScale = 1f;
			if (GameObject.FindObjectOfType<MusicPlayer>())
			{
				GameObject.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().Play();
			}
						
			optionsController.gameObject.SetActive(false);
		}
	}
	
	public void QuitRequest()
	{
		Debug.Log ("Quit requested.");
		Application.Quit();
	}
	
	public void Restart()
	{
		StartCoroutine (RestartRoutine());
	}
	
	IEnumerator RestartRoutine()
	{
		
		if (GameObject.FindObjectOfType<MusicPlayer>())
		{
			GameObject.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().Stop();
		}
		//AudioSource.PlayClipAtPoint(restartSound, Vector3.zero);
		yield return new WaitForSeconds(5);
		LoadLevel(1);
	}
	
	IEnumerator WaitOnDeath()
	{
		yield return new WaitForSeconds(5);
		GameObject.FindObjectOfType<LevelManager>().LoadNextLevel();
	}
}