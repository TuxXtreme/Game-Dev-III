using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private ResourceManager resourceManager;
	[SerializeField] private GameObject upgradePanel;

	[Header("Scrap Value Upgrade")]
	[SerializeField] private int scrapValueLevel = 1;
	[SerializeField] private int scrapValue = 1;
	[SerializeField] private int scrapUpgradeCost = 10;

	[Header("Spawn Rate Upgrade")]
	[SerializeField] private ScrapSpawner scrapSpawner;

	[SerializeField] private int spawnRateLevel = 1;
	[SerializeField] private float spawnInterval = 2f;
	[SerializeField] private float spawnRateImprovement = 0.3f;
	[SerializeField] private float minimumSpawnInterval = 0.5f;
	[SerializeField] private int spawnRateUpgradeCost = 15;

	[Header("Spawn Rate UI")]
	[SerializeField] private TMP_Text spawnRateText;
	[SerializeField] private TMP_Text spawnRateButtonText;
	[SerializeField] private Button spawnRateButton;

	[Header("Material Upgrade")]
	[SerializeField] private int materialTier = 1;
	[SerializeField] private int materialUpgradeCost = 25;

	[Header("Material UI")]
	[SerializeField] private TMP_Text materialText;
	[SerializeField] private TMP_Text materialButtonText;
	[SerializeField] private Button materialButton;

	public int MaterialTier => materialTier;

	[Header("UI")]
	[SerializeField] private TMP_Text scrapValueText;
	[SerializeField] private TMP_Text scrapValueButtonText;
	[SerializeField] private Button scrapValueButton;

	private bool upgradeMenuOpen;

	public int ScrapValue => scrapValue;

	private void Start()
	{
		upgradePanel.SetActive(false);

		UpdateUpgradeUI();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			ToggleUpgradeMenu();
		}
	}

	private void ToggleUpgradeMenu()
	{
		upgradeMenuOpen = !upgradeMenuOpen;

		upgradePanel.SetActive(upgradeMenuOpen);

		UpdateUpgradeUI();
	}

	public void UpgradeScrapValue()
	{
		if (!resourceManager.SpendScrap(scrapUpgradeCost))
		{
			Debug.Log("Not enough Scrap!");
			return;
		}

		scrapValueLevel++;

		scrapValue++;

		scrapUpgradeCost =
			Mathf.RoundToInt(scrapUpgradeCost * 1.5f);

		Debug.Log(
			"Scrap Value upgraded to Level "
			+ scrapValueLevel
		);

		UpdateUpgradeUI();
	}

	private void UpdateUpgradeUI()
	{
		if (scrapValueText != null)
		{
			scrapValueText.text =
				"Scrap Value\nLevel " + scrapValueLevel +
				"\nScrap: +" + scrapValue;
		}

		if (scrapValueButtonText != null)
		{
			scrapValueButtonText.text =
				scrapUpgradeCost + " Scrap";
		}

		if (scrapValueButton != null)
		{
			scrapValueButton.interactable =
				resourceManager.Scrap >= scrapUpgradeCost;
		}

		if (spawnRateText != null)
		{
			spawnRateText.text =
				"Spawn Rate\nLevel " + spawnRateLevel +
				"\nEvery " + spawnInterval.ToString("0.0") + "s";
		}

		if (spawnRateButtonText != null)
		{
			spawnRateButtonText.text =
				spawnRateUpgradeCost + " Scrap";
		}

		if (spawnRateButton != null)
		{
			spawnRateButton.interactable =
				resourceManager.Scrap >= spawnRateUpgradeCost;
		}

		string materialName = GetMaterialName();

		if (materialText != null)
		{
			materialText.text =
				"Material\n" +
				materialName +
				"\nTier " +
				materialTier;
		}

		if (materialButtonText != null)
		{
			if (materialTier >= 4)
			{
				materialButtonText.text = "MAX LEVEL";
			}
			else
			{
				materialButtonText.text =	
					materialUpgradeCost +
					" Scrap";
			}
		}

		if (materialButton != null)
		{
			materialButton.interactable =
				materialTier < 4 &&
				resourceManager.Scrap >= materialUpgradeCost;
		}
	}

	public string GetMaterialName()
	{
		switch (materialTier)
		{
			case 1:
				return "Scrap";

			case 2:
				return "Copper";

			case 3:
				return "Iron";

			case 4:
				return "Gold";

			default:
				return "Scrap";
		}
	}

	private string GetNextMaterialName()
	{
		switch (materialTier + 1)
		{
			case 2:
				return "Copper";

			case 3:
				return "Iron";

			case 4:
				return "Gold";

			default:
				return "MAX";
		}
	}

	public void SetSpawnInterval(float newInterval)
	{
		spawnInterval = newInterval;

		Debug.Log("New Spawn Interval: " + spawnInterval);
	}

	public void UpgradeSpawnRate()
	{
		if (!resourceManager.SpendScrap(spawnRateUpgradeCost))
		{
			Debug.Log("Not enough Scrap!");
			return;
		}

		spawnRateLevel++;

		spawnInterval -= spawnRateImprovement;

		spawnInterval = Mathf.Max(
			spawnInterval,
			minimumSpawnInterval
		);

		scrapSpawner.SetSpawnInterval(spawnInterval);

		spawnRateUpgradeCost =
			Mathf.RoundToInt(spawnRateUpgradeCost * 1.5f);

		Debug.Log(
			"Spawn Rate upgraded to Level "
			+ spawnRateLevel
		);

		UpdateUpgradeUI();
	}

	public void UpgradeMaterial()
	{
		if (materialTier >= 4)
			return;

		if (!resourceManager.SpendScrap(materialUpgradeCost))
		{
			Debug.Log("Not enough Scrap!");
			return;
		}

		materialTier++;

		materialUpgradeCost =
			Mathf.RoundToInt(materialUpgradeCost * 2f);

		Debug.Log("Material upgraded to Tier " + materialTier);

		UpdateUpgradeUI();
	}
}