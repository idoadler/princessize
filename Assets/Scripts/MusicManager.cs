﻿using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	private static MusicManager instance = null;
	public static MusicManager Instance {
		get { return instance; }
	}

	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
			return;
		} 
		else 
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public int currentSong = 0;
	public AudioSource[] songs;

	public void play(int song)
	{
		if (currentSong != song) 
		{
			if (currentSong >= 0)
				songs[currentSong].Stop();
			currentSong = song;
			if (currentSong >= 0)
				songs[currentSong].Play();
		}
	}
}
