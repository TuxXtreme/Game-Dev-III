using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRewind : MonoBehaviour
{
	[Header("Recording")]
	[SerializeField] private float recordInterval = 0.05f;
	[SerializeField] private float maxRecordTime = 5f;

	[Header("Rewind")]
	[SerializeField] private KeyCode rewindKey = KeyCode.Space;

	[Header("Time Energy")]
	[SerializeField] private float maxEnergy = 100f;
	[SerializeField] private float rewindDrainRate = 25f;
	[SerializeField] private float rechargeRate = 15f;
	[SerializeField] private Slider energyBar;

	private float currentEnergy;

	private RectTransform rectTransform;
	private PlayerMovement playerMovement;

	private List<Vector2> recordedPositions = new List<Vector2>();

	private float recordTimer;
	private float rewindTimer;

	private bool isRewinding;
	public bool IsRewinding => isRewinding;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		playerMovement = GetComponent<PlayerMovement>();

		currentEnergy = maxEnergy;
	}

	private void Start()
	{
		UpdateEnergyUI();
	}

	private void Update()
	{
		if (Input.GetKey(rewindKey) &&
			recordedPositions.Count > 0 &&
			currentEnergy > 0)
		{
			StartRewind();
			Rewind();

			DrainEnergy();
		}
		else
		{
			StopRewind();
			RecordPosition();

			RechargeEnergy();
		}

		UpdateEnergyUI();
	}

	private void RecordPosition()
	{
		recordTimer += Time.deltaTime;

		if (recordTimer >= recordInterval)
		{
			recordedPositions.Add(rectTransform.anchoredPosition);

			recordTimer = 0f;

			int maxPositions =
				Mathf.RoundToInt(maxRecordTime / recordInterval);

			if (recordedPositions.Count > maxPositions)
			{
				recordedPositions.RemoveAt(0);
			}
		}
	}

	private void StartRewind()
	{
		if (isRewinding)
			return;

		isRewinding = true;

		if (playerMovement != null)
			playerMovement.enabled = false;
	}

	private void Rewind()
	{
		rewindTimer += Time.deltaTime;

		if (rewindTimer >= recordInterval)
		{
			int lastPosition = recordedPositions.Count - 1;

			rectTransform.anchoredPosition =
				recordedPositions[lastPosition];

			recordedPositions.RemoveAt(lastPosition);

			rewindTimer = 0f;
		}
	}

	private void StopRewind()
	{
		if (!isRewinding)
			return;

		isRewinding = false;

		if (playerMovement != null)
			playerMovement.enabled = true;
	}

	private void DrainEnergy()
	{
		currentEnergy -= rewindDrainRate * Time.deltaTime;

		currentEnergy = Mathf.Clamp(
			currentEnergy,
			0,
			maxEnergy
		);
	}

	private void RechargeEnergy()
	{
		currentEnergy += rechargeRate * Time.deltaTime;

		currentEnergy = Mathf.Clamp(
			currentEnergy,
			0,
			maxEnergy
		);
	}

	private void UpdateEnergyUI()
	{
		if (energyBar != null)
		{
			energyBar.value = currentEnergy / maxEnergy;
		}
	}

	public void ClearHistory()
	{
		recordedPositions.Clear();

		recordTimer = 0f;
		rewindTimer = 0f;
	}
}