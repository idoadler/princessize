using UnityEngine;
using System.Collections;

public class backButton : MonoBehaviour {

	public string level = "main";
	public int song;

	void Start () 
	{
		MusicManager.Instance.play (song);
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (string.IsNullOrEmpty(level))
				Application.Quit();
			else  
				Application.LoadLevel(level);
		}
	}
}
