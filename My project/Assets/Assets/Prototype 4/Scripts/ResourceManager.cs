using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
	[Header("Resources")]
	[SerializeField] private int scrap = 0;

	[Header("UI")]
	[SerializeField] private TMP_Text scrapText;

	public int Scrap => scrap;

	private void Start()
	{
		UpdateUI();
	}

	public void AddScrap(int amount)
	{
		scrap += amount;

		UpdateUI();

		Debug.Log("Scrap: " + scrap);
	}

	public bool SpendScrap(int amount)
	{
		if (scrap < amount)
			return false;

		scrap -= amount;

		UpdateUI();

		return true;
	}

	private void UpdateUI()
	{
		if (scrapText != null)
		{
			scrapText.text = "Scrap: " + scrap;
		}
	}
}