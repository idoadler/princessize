using UnityEngine;
using System.Collections;


class DragTransform : MonoBehaviour
{

	private Vector3 screenPoint;
	private Vector3 offset;

	private Color mouseOverColor = new Color(1,0.96f,0.6f);
	private Color originalColor = Color.white;
	private Color wrongLocationColor = new Color(1,0.5f,0.5f);
	private bool outside = false;
	private bool dragging = false;
	private float distance;

	void Start()
	{
		OnMouseExit ();
	}
	
	void OnMouseEnter()
	{
		if (!SpriteSlicer2DManager.dragging)
			GetComponent<Renderer>().material.color = mouseOverColor;
	}
	
	void OnMouseExit()
	{
		if (outside) {
			if (!SpriteSlicer2DManager.allSliced.Contains(this.gameObject))
				SpriteSlicer2DManager.allSliced.Add (this.gameObject);
			GetComponent<Renderer> ().material.color = wrongLocationColor;
		} else {
			if (SpriteSlicer2DManager.allSliced.Contains(this.gameObject))
				SpriteSlicer2DManager.removeAndTest(this.gameObject);
			GetComponent<Renderer> ().material.color = originalColor;
		}
	}
	
	void OnMouseDown()
	{
		if (!SpriteSlicer2DManager.dragging) {
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
			dragging = true;
			SpriteSlicer2DManager.dragging = true;
		}
	}
	
	void OnMouseUp()
	{
		dragging = false;
		SpriteSlicer2DManager.dragging = false;
		OnMouseExit ();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "limits") {
			outside = true;
		}
		OnMouseExit ();
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "limits") {
			outside = false;
		}
		OnMouseExit ();
	}

	void OnDestroy() {
		SpriteSlicer2DManager.allSliced.Remove (this.gameObject);
	}
	
	void Update()
	{
		if (dragging)
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
		}
	}
}