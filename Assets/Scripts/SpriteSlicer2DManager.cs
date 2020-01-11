using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpriteSlicer2DManager : MonoBehaviour 
{
	static public HashSet<object> allSliced = new HashSet<object>();
	static public bool dragging = false;
	static private SpriteSlicer2DManager instance;
	TrailRenderer m_TrailRenderer;
	List<SpriteSlicer2DSliceInfo> m_SlicedSpriteInfo = new List<SpriteSlicer2DSliceInfo>();
	private int numberSliced = 0;
	// win
	public int currentLevel = 0;
	public int star1score = 2;
	public SpriteRenderer star1;
	public int star2score = 1;
	public SpriteRenderer star2;
	public GameObject winScreen;
	public Text winScore;
	public loadLevelEnding endingObj;
	public GameObject levelEndingWinText;
	public GameObject tutorialText;

	public Text score;
	public GameObject showOnWin;
	public GameObject hideOnWin;
	public bool disabled = false;

	private void win()
	{
		winScreen.SetActive (true);
		if (currentLevel >= 0) {
			PlayerPrefs.SetInt("levelScore"+currentLevel,1);
			levelsManager.levelScore [currentLevel] = 1;
			if (numberSliced <= star1score) {
				star1.color = Color.white;
				if (currentLevel >= 0)
				{
					PlayerPrefs.SetInt("levelScore"+currentLevel,2);
					levelsManager.levelScore [currentLevel] = 2;
				}
			}
			if (numberSliced <= star2score) {
				if (endingObj != null)
					endingObj.alt = true;
				if (levelEndingWinText != null)
					levelEndingWinText.SetActive(true);
				star2.color = Color.white;
				if (currentLevel >= 0) {
					PlayerPrefs.SetInt("levelScore"+currentLevel,3);
					levelsManager.levelScore [currentLevel] = 3;
				}
			}
			winScore.text = "CUTS: " + numberSliced;
		}
	}

	static public void removeAndTest(object removed)
	{
		if (!dragging) {
			//TODO: check if sometimes this misses
			allSliced.Remove (removed);
			if (allSliced.Count == 0) {
				MusicManager.Instance.play (1);
				instance.showOnWin.SetActive (true);
				instance.hideOnWin.SetActive (false);
				dragging = true;
				instance.Invoke(nameof(win),1);
			}
		}
	}

	struct MousePosition
	{
		public Vector3 m_WorldPosition;
		public float m_Time;
	}

	List<MousePosition> m_MousePositions = new List<MousePosition>();
	float m_MouseRecordTimer = 0.0f;
	float m_MouseRecordInterval = 0.05f;
	int m_MaxMousePositions = 5;
    bool m_FadeFragments = false;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start ()
	{
		dragging = false;
		instance = this;
		allSliced.Clear ();
		m_TrailRenderer = GetComponentInChildren<TrailRenderer>();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	private void Update () 
	{
		// Left mouse button - hold and swipe to cut objects
		if(Input.GetMouseButton(0) && !dragging && !disabled)
		{
			bool mousePositionAdded = false;
			m_MouseRecordTimer -= Time.deltaTime;

			// Record the world position of the mouse every x seconds
			if(m_MouseRecordTimer <= 0.0f)
			{
				MousePosition newMousePosition = new MousePosition();
				newMousePosition.m_WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				newMousePosition.m_Time = Time.time;

				m_MousePositions.Add(newMousePosition);
				m_MouseRecordTimer = m_MouseRecordInterval;
				mousePositionAdded = true;

				// Remove the first recorded point if we've recorded too many
				if(m_MousePositions.Count > m_MaxMousePositions)
				{
					m_MousePositions.RemoveAt(0);
				}
			}

			// Forget any positions that are too old to care about
			if(m_MousePositions.Count > 0 && (Time.time - m_MousePositions[0].m_Time) > m_MouseRecordInterval * m_MaxMousePositions)
			{
				m_MousePositions.RemoveAt(0);
			}

			// Go through all our recorded positions and slice any sprites that intersect them
			if(mousePositionAdded)
			{
				for(int loop = 0; loop < m_MousePositions.Count - 1; loop++)
				{
					SpriteSlicer2D.SliceAllSprites(m_MousePositions[loop].m_WorldPosition, m_MousePositions[m_MousePositions.Count - 1].m_WorldPosition, true, ref m_SlicedSpriteInfo);

					if(m_SlicedSpriteInfo.Count > 0)
					{
						if (tutorialText != null)
						{
							tutorialText.SetActive(true);
							disabled = true;
						}

						numberSliced++;
						score.text = "CUTS: " + numberSliced;

						// Add some force in the direction of the swipe so that stuff topples over rather than just being
						// sliced but remaining stationary
						for(int spriteIndex = 0; spriteIndex < m_SlicedSpriteInfo.Count; spriteIndex++)
						{
							for(int childSprite = 0; childSprite < m_SlicedSpriteInfo[spriteIndex].ChildObjects.Count; childSprite++)
							{
								Vector2 sliceDirection = m_MousePositions[m_MousePositions.Count - 1].m_WorldPosition - m_MousePositions[loop].m_WorldPosition;
								sliceDirection.Normalize();
								//m_SlicedSpriteInfo[spriteIndex].ChildObjects[childSprite].GetComponent<Rigidbody2D>().AddForce(sliceDirection * 500.0f);
								m_SlicedSpriteInfo[spriteIndex].ChildObjects[childSprite].AddComponent<DragTransform>();
							}
						}

						m_MousePositions.Clear();
						break;
					}
				}
			}

			if(m_TrailRenderer)
			{
				Vector3 trailPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				trailPosition.z = -9.0f;
				m_TrailRenderer.transform.position = trailPosition;
			}
		}
		else
		{
			m_MousePositions.Clear();
		}

		// Sliced sprites sharing the same layer as standard Unity sprites could increase the draw call count as
		// the engine will have to keep swapping between rendering SlicedSprites and Unity Sprites.To avoid this, 
		// move the newly sliced sprites either forward or back along the z-axis after they are created
		for(int spriteIndex = 0; spriteIndex < m_SlicedSpriteInfo.Count; spriteIndex++)
		{
			for(int childSprite = 0; childSprite < m_SlicedSpriteInfo[spriteIndex].ChildObjects.Count; childSprite++)
			{
				Vector3 spritePosition = m_SlicedSpriteInfo[spriteIndex].ChildObjects[childSprite].transform.position;
				spritePosition.z = -1.0f;
				m_SlicedSpriteInfo[spriteIndex].ChildObjects[childSprite].transform.position = spritePosition;
			}
		}

        if(m_FadeFragments)
        {
            // If we've chosen to fade out fragments once an object is destroyed, add a fade and destroy component
            for (int spriteIndex = 0; spriteIndex < m_SlicedSpriteInfo.Count; spriteIndex++)
            {
                for (int childSprite = 0; childSprite < m_SlicedSpriteInfo[spriteIndex].ChildObjects.Count; childSprite++)
                {                    
                    m_SlicedSpriteInfo[spriteIndex].ChildObjects[childSprite].AddComponent<FadeAndDestroy>();
                }
            }
        }

		m_SlicedSpriteInfo.Clear();
	}

}
