using UnityEngine;

public class SunlightPickup : MonoBehaviour
{
	[SerializeField] private RectTransform player;
	[SerializeField] private PlayerInventory playerInventory;
	[SerializeField] private int sunlightAmount = 1;

	private RectTransform sunlightRect;
	private bool collected;

	private void Awake()
	{
		sunlightRect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (collected)
			return;

		if (RectsOverlap(player, sunlightRect))
		{
			collected = true;

			playerInventory.AddSunlight(sunlightAmount);

			Debug.Log("Sunlight collected!");

			gameObject.SetActive(false);
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