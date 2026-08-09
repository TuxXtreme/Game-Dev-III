using UnityEngine;

public class PressurePlate : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private RectTransform player;
	[SerializeField] private GameObject door;
	[SerializeField] private RectTransform echo;

	private RectTransform plateRect;

	private bool isPressed;

	private void Awake()
	{
		plateRect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		bool playerOnPlate =
	RectsOverlap(player, plateRect);

		bool echoOnPlate =
			echo.gameObject.activeSelf &&
			RectsOverlap(echo, plateRect);

		bool overlapping =
			playerOnPlate || echoOnPlate;

		if (overlapping && !isPressed)
		{
			PressPlate();
		}
		else if (!overlapping && isPressed)
		{
			ReleasePlate();
		}
	}

	private void PressPlate()
	{
		isPressed = true;

		door.SetActive(false);

		Debug.Log("Pressure plate activated!");
	}

	private void ReleasePlate()
	{
		isPressed = false;

		door.SetActive(true);

		Debug.Log("Pressure plate released!");
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