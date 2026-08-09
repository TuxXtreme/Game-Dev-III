using UnityEngine;
using UnityEngine.EventSystems;

public class ScrapPickup : MonoBehaviour, IPointerEnterHandler
{
	private ResourceManager resourceManager;
	private UpgradeManager upgradeManager;

	private bool collected;

	private void Start()
	{
		resourceManager =
			FindFirstObjectByType<ResourceManager>();

		upgradeManager =
			FindFirstObjectByType<UpgradeManager>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (collected)
			return;

		if (resourceManager == null ||
			upgradeManager == null)
			return;

		collected = true;

		int materialMultiplier = 1;

		switch (upgradeManager.MaterialTier)
		{
			case 1:
				materialMultiplier = 1;
				break;

			case 2:
				materialMultiplier = 2;
				break;

			case 3:
				materialMultiplier = 3;
				break;

			case 4:
				materialMultiplier = 5;
				break;
		}

		int amount =
			upgradeManager.ScrapValue *
			materialMultiplier;

		resourceManager.AddScrap(amount);

		Destroy(gameObject);
	}
}