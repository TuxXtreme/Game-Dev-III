using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 300f;

	[Header("Collision")]
	[SerializeField] private RectTransform[] walls;

	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		Vector2 movement =
			new Vector2(horizontal, vertical).normalized;

		Vector2 oldPosition = rectTransform.anchoredPosition;

		rectTransform.anchoredPosition +=
			movement * moveSpeed * Time.deltaTime;

		// If the new position overlaps a wall,
		// return to the previous position.
		if (IsTouchingWall())
		{
			rectTransform.anchoredPosition = oldPosition;
		}
	}

	private bool IsTouchingWall()
	{
		foreach (RectTransform wall in walls)
		{
			if (wall == null)
				continue;

			// Disabled doors/walls don't block the player.
			if (!wall.gameObject.activeInHierarchy)
				continue;

			if (RectsOverlap(rectTransform, wall))
				return true;
		}

		return false;
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