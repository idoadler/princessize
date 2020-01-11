using UnityEngine;

class DragTransform : MonoBehaviour
{
	private Vector3 offset;
	public bool destroyed = false;

	private Color mouseOverColor = new Color(1,0.96f,0.6f);
	private Color originalColor = Color.white;
	private Color wrongLocationColor = new Color(1,0.5f,0.5f);
	private bool outside = false;
	private bool dragging = false;
	private float distance;

	private void Start()
	{
		OnMouseExit ();
	}

	private void OnMouseEnter()
	{
		if (!SpriteSlicer2DManager.dragging)
			GetComponent<Renderer>().material.color = mouseOverColor;
	}

	private void OnMouseExit()
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

	private void OnMouseDown()
	{
		if (SpriteSlicer2DManager.dragging) 
			return;
		
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		dragging = true;
		SpriteSlicer2DManager.dragging = true;
	}

	private void OnMouseUp()
	{
		dragging = false;
		SpriteSlicer2DManager.dragging = false;
		OnMouseExit ();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("limits")) {
			outside = true;
		}
		OnMouseExit ();
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (destroyed)
			return;
		if (other.CompareTag("limits")) {
			outside = false;
		}
		OnMouseExit ();
	}

	private void OnDestroy() {
		SpriteSlicer2DManager.allSliced.Remove (this.gameObject);
	}

	private void Update()
	{
		if (!dragging) 
			return;
		
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
	}
}