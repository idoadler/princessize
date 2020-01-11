using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevelEnding : MonoBehaviour {

	public Sprite buttonPressed;
	private Sprite buttonUp;
	public string level;
	public string altlevel;
	public bool alt = false;

	// Use this for initialization
	private void Start () {
		if (GetComponent<SpriteRenderer>() != null)
			buttonUp = GetComponent<SpriteRenderer> ().sprite;
	}

	private void OnMouseEnter()
	{
		if (buttonPressed != null)
			GetComponent<SpriteRenderer>().sprite = buttonPressed;
	}

	private void OnMouseExit()
	{
		if (buttonUp != null)
			GetComponent<SpriteRenderer>().sprite = buttonUp;
	}

	private void OnMouseDown()
	{
		if (buttonPressed != null)
			GetComponent<SpriteRenderer>().sprite = buttonPressed;
	}

	private void OnMouseUp()
	{
		if (buttonUp != null)
			GetComponent<SpriteRenderer>().sprite = buttonUp;
		if (string.IsNullOrEmpty(level))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		else if (alt)
			SceneManager.LoadScene(altlevel);
		else
			SceneManager.LoadScene(level);
	}
}
