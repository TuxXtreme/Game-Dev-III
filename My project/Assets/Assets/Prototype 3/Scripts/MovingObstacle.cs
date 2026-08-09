using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private TimeRewind playerTime;

	[Header("Movement")]
	[SerializeField] private Vector2 moveDirection = Vector2.up;
	[SerializeField] private float moveDistance = 300f;
	[SerializeField] private float moveSpeed = 150f;

	[Header("Recording")]
	[SerializeField] private float recordInterval = 0.05f;
	[SerializeField] private float maxRecordTime = 5f;

	private RectTransform rectTransform;

	private Vector2 startPosition;

	private List<Vector2> recordedPositions =
		new List<Vector2>();

	private float movementTime;
	private float recordTimer;
	private float rewindTimer;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private void Start()
	{
		startPosition = rectTransform.anchoredPosition;
	}

	private void Update()
	{
		if (playerTime != null && playerTime.IsRewinding)
		{
			Rewind();
		}
		else
		{
			Move();
			RecordPosition();
		}
	}

	private void Move()
	{
		movementTime += Time.deltaTime;

		float movement =
			Mathf.PingPong(
				movementTime * moveSpeed,
				moveDistance
			);

		rectTransform.anchoredPosition =
			startPosition +
			moveDirection.normalized * movement;
	}

	private void RecordPosition()
	{
		recordTimer += Time.deltaTime;

		if (recordTimer >= recordInterval)
		{
			recordedPositions.Add(
				rectTransform.anchoredPosition
			);

			recordTimer = 0f;

			int maxPositions =
				Mathf.RoundToInt(
					maxRecordTime / recordInterval
				);

			if (recordedPositions.Count > maxPositions)
			{
				recordedPositions.RemoveAt(0);
			}
		}
	}

	private void Rewind()
	{
		if (recordedPositions.Count == 0)
			return;

		rewindTimer += Time.deltaTime;

		if (rewindTimer >= recordInterval)
		{
			int lastPosition =
				recordedPositions.Count - 1;

			rectTransform.anchoredPosition =
				recordedPositions[lastPosition];

			recordedPositions.RemoveAt(lastPosition);

			rewindTimer = 0f;
		}
	}
}