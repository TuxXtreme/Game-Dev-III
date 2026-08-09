using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
	[Header("Resources")]
	[SerializeField] private int water = 0;
	[SerializeField] private int sunlight = 0;

	[Header("UI")]
	[SerializeField] private TMP_Text waterText;
	[SerializeField] private TMP_Text sunlightText;

	public int Water => water;
	public int Sunlight => sunlight;

	private void Start()
	{
		UpdateUI();
	}

	public void AddWater(int amount)
	{
		water += amount;
		UpdateUI();
	}

	public void AddSunlight(int amount)
	{
		sunlight += amount;
		UpdateUI();
	}

	public bool UseResources(int waterAmount, int sunlightAmount)
	{
		if (water < waterAmount || sunlight < sunlightAmount)
			return false;

		water -= waterAmount;
		sunlight -= sunlightAmount;

		UpdateUI();

		return true;
	}

	private void UpdateUI()
	{
		if (waterText != null)
			waterText.text = "Water: " + water;

		if (sunlightText != null)
			sunlightText.text = "Sunlight: " + sunlight;
	}
}