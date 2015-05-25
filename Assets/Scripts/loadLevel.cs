using UnityEngine;
using System.Collections;

public class loadLevel : MonoBehaviour {

	public Sprite buttonPressed;
	private Sprite buttonUp;
	public string level;

	// Use this for initialization
	void Start () {
		if (GetComponent<SpriteRenderer>() != null)
			buttonUp = GetComponent<SpriteRenderer> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter()
	{
		if (buttonPressed != null)
			GetComponent<SpriteRenderer>().sprite = buttonPressed;
	}
	
	void OnMouseExit()
	{
		if (buttonUp != null)
			GetComponent<SpriteRenderer>().sprite = buttonUp;
	}
	
	void OnMouseDown()
	{
		if (buttonPressed != null)
			GetComponent<SpriteRenderer>().sprite = buttonPressed;
	}
	
	void OnMouseUp()
	{
		if (buttonUp != null)
			GetComponent<SpriteRenderer>().sprite = buttonUp;
		if (string.IsNullOrEmpty(level))
			Application.LoadLevel(Application.loadedLevel);
		else
			Application.LoadLevel(level);
	}
}
