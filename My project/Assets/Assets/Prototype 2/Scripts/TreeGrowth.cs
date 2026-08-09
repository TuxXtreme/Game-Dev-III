using UnityEngine;
using TMPro;

public class TreeGrowth : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private RectTransform player;
	[SerializeField] private PlayerInventory playerInventory;
	[SerializeField] private RectTransform treeVisual;

	[Header("Growth")]
	[SerializeField] private int currentGrowth = 0;
	[SerializeField] private int maxGrowth = 100;
	[SerializeField] private int growthPerStage = 20;

	[Header("Tree Size")]
	[SerializeField] private float minimumScale = 0.5f;
	[SerializeField] private float maximumScale = 1.5f;

	[Header("UI")]
	[SerializeField] private TMP_Text growthText;

	[Header("Pickups")]
	[SerializeField] private GameObject[] waterPickups;
	[SerializeField] private GameObject[] sunlightPickups;

	private RectTransform treeRect;
	private bool playerInsideTree = false;

	public int CurrentGrowth => currentGrowth;
	public int MaxGrowth => maxGrowth;

	private void Awake()
	{
		treeRect = GetComponent<RectTransform>();
	}

	private void Start()
	{
		UpdateTreeVisual();
		UpdateGrowthUI();
	}

	private void Update()
	{
		bool overlapping = RectsOverlap(player, treeRect);

		// Only give water when the player ENTERS the tree
		if (overlapping && !playerInsideTree)
		{
			TryWaterTree();
		}

		playerInsideTree = overlapping;
	}

	private void UpdateGrowthUI()
	{
		if (growthText != null)
		{
			growthText.text = "Tree: " + currentGrowth + "%";
		}
	}
	private void TryWaterTree()
	{
		if (currentGrowth >= maxGrowth)
			return;

		if (playerInventory.UseResources(1, 1))
		{
			AddGrowth(growthPerStage);

			if (currentGrowth < maxGrowth)
			{
				RespawnPickups();
			}

			Debug.Log("Tree received Water + Sunlight!");
		}
		else
		{
			Debug.Log("You need 1 Water and 1 Sunlight!");
		}
	}

	private void RespawnPickups()
	{
		foreach (GameObject water in waterPickups)
		{
			if (water != null)
				water.SetActive(true);
		}

		foreach (GameObject sunlight in sunlightPickups)
		{
			if (sunlight != null)
				sunlight.SetActive(true);
		}

		Debug.Log("Water and Sunlight respawned!");
	}

	private void AddGrowth(int amount)
	{
		currentGrowth += amount;

		currentGrowth = Mathf.Clamp(
			currentGrowth,
			0,
			maxGrowth
		);

		UpdateTreeVisual();
		UpdateGrowthUI();

		Debug.Log("Tree Growth: " + currentGrowth + "%");

		if (currentGrowth >= maxGrowth)
		{
			TreeFullyGrown();
		}
	}

	private void UpdateTreeVisual()
	{
		float growthPercent = (float)currentGrowth / maxGrowth;

		float scale = Mathf.Lerp(
			minimumScale,
			maximumScale,
			growthPercent
		);

		treeVisual.localScale = new Vector3(scale, scale, 1f);
	}

	private void TreeFullyGrown()
	{
		Debug.Log("THE TREE IS FULLY GROWN!");
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