using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableObject : MonoBehaviour {

	public Sprite buttonPressed;
	private Sprite buttonUp;
	public GameObject target;

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
		target.SetActive(true);
	}
}
