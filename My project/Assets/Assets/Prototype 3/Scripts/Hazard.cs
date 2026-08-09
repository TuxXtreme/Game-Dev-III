using UnityEngine;

public class Hazard : MonoBehaviour
{
	[SerializeField] private RectTransform player;

	private RectTransform hazardRect;
	private Vector2 playerStartPosition;

	private bool playerInside;
	private TimeRewind timeRewind;

	private void Awake()
	{
		hazardRect = GetComponent<RectTransform>();
	}

	private void Start()
	{
		playerStartPosition = player.anchoredPosition;

		timeRewind = player.GetComponent<TimeRewind>();
	}
	private void Update()
	{
		bool overlapping = RectsOverlap(player, hazardRect);

		if (overlapping && !playerInside)
		{
			HitPlayer();
		}

		playerInside = overlapping;
	}

	private void HitPlayer()
	{
		Debug.Log("Player hit the hazard!");

		player.anchoredPosition = playerStartPosition;

		if (timeRewind != null)
		{
			timeRewind.ClearHistory();
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