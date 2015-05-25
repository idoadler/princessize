using UnityEngine;
using System.Collections;

public class levelsManager : MonoBehaviour {

	static public int[] levelScore;
	public GameObject token;
	public GameObject[] instanceLevels;
	public GameObject[] instanceLocks;

	// Use this for initialization
	void Start () {
		if (levelScore == null) 
		{
			levelScore = new int[instanceLevels.Length];
			for (int i = 0; i < instanceLevels.Length; i++) 
			{
				levelScore[i] = -1;
			}
		} else 
		{
			for (int i = 0; i < levelScore.Length-1; i++) 
			{
				if(levelScore[i] > -1)
				{
					instanceLevels[i+1].SetActive(true);
					instanceLocks[i+1].SetActive(false);
					token.transform.position = instanceLocks[i+1].transform.localPosition;
				}
			}
			if (levelScore[levelScore.Length-1] > -1)
				token.SetActive(false);
		}
	}
}
