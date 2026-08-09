using UnityEngine;

public class ScrapSpawner : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject scrapPrefab;

	[Header("Spawning")]
	[SerializeField] private float spawnInterval = 2f;
	[SerializeField] private int maxScrapOnScreen = 10;

	private RectTransform spawnArea;
	private float spawnTimer;

	private void Awake()
	{
		spawnArea = GetComponent<RectTransform>();
	}

	private void Update()
	{
		spawnTimer += Time.deltaTime;

		if (spawnTimer >= spawnInterval)
		{
			SpawnScrap();
			spawnTimer = 0f;
		}
	}

	private void SpawnScrap()
	{
		int currentScrap =
			GameObject.FindGameObjectsWithTag("Scrap").Length;

		if (currentScrap >= maxScrapOnScreen)
			return;

		GameObject newScrap =
			Instantiate(scrapPrefab, spawnArea);

		RectTransform scrapRect =
			newScrap.GetComponent<RectTransform>();

		RectTransform prefabRect =
			scrapPrefab.GetComponent<RectTransform>();

		scrapRect.sizeDelta = prefabRect.sizeDelta;
		scrapRect.localScale = Vector3.one;
		scrapRect.localRotation = Quaternion.identity;

		Rect rect = spawnArea.rect;

		float halfWidth = scrapRect.rect.width / 2f;
		float halfHeight = scrapRect.rect.height / 2f;

		float randomX = Random.Range(
			rect.xMin + halfWidth,
			rect.xMax - halfWidth
		);

		float randomY = Random.Range(
			rect.yMin + halfHeight,
			rect.yMax - halfHeight
		);

		scrapRect.anchoredPosition =
			new Vector2(randomX, randomY);
	}

	public void SetSpawnInterval(float newInterval)
	{
		spawnInterval = newInterval;

		Debug.Log("New Spawn Interval: " + spawnInterval);
	}
}