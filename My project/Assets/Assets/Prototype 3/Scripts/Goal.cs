using UnityEngine;

public class Goal : MonoBehaviour
{
	[SerializeField] private RectTransform player;
	[SerializeField] private GameObject winText;

	private RectTransform goalRect;
	private bool completed;

	private void Awake()
	{
		goalRect = GetComponent<RectTransform>();
	}

	private void Start()
	{
		if (winText != null)
		{
			winText.SetActive(false);
		}
	}

	private void Update()
	{
		if (completed)
			return;

		if (RectsOverlap(player, goalRect))
		{
			CompleteLevel();
		}
	}

	private void CompleteLevel()
	{
		completed = true;

		Debug.Log("Level Complete!");

		if (winText != null)
		{
			winText.SetActive(true);
		}
	}

	private bool RectsOverlap(RectTransform a, RectTransform b)
	{
		Vector3[] aCorners = new Vector3[4];
		Vector3[] bCorners = new Vector3[4];

		a.GetWorldCorners(aCorners);
		b.GetWorldCorners(bCorners);

		Rect aRect = new Rect(
			aCorners[0].x,
			aCorners[0].y,
			aCorners[2].x - aCorners[0].x,
			aCorners[2].y - aCorners[0].y
		);

		Rect bRect = new Rect(
			bCorners[0].x,
			bCorners[0].y,
			bCorners[2].x - bCorners[0].x,
			bCorners[2].y - bCorners[0].y
		);

		return aRect.Overlaps(bRect);
	}
}