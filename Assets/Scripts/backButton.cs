using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class backButton : MonoBehaviour {

	public string level = "main";
	public int song;

	private void Start () 
	{
		MusicManager.Instance.play (song);
	}

	// Update is called once per frame
	private void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (string.IsNullOrEmpty(level))
				Application.Quit();
			else  
				SceneManager.LoadScene(level);
		}
	}
}
