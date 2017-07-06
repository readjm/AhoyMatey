using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour {

	public float fadeInTime;
	
	private Image fadePanel;
	private Color currentColor = Color.black;
	
	// Use this for initialization
	void Start ()
	{
		fadePanel = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeSinceLevelLoad < fadeInTime)
		{
			currentColor = new Color(0, 0, 0, 1 - Time.timeSinceLevelLoad/fadeInTime);
			fadePanel.color = currentColor;
		}
		else 
		{
			gameObject.SetActive(false);
		}
	}
	
}
