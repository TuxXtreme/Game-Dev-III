using System.Collections.Generic;
using UnityEngine;

public class TimeEcho : MonoBehaviour
{
	[Header("Echo")]
	[SerializeField] private RectTransform echo;
	[SerializeField] private KeyCode echoKey = KeyCode.E;

	[Header("Recording")]
	[SerializeField] private float recordInterval = 0.05f;
	[SerializeField] private float maxRecordTime = 5f;

	private RectTransform player;

	private List<Vector2> recordedPositions = new List<Vector2>();

	private float recordTimer;
	private float playbackTimer;

	private int playbackIndex;

	private bool echoPlaying;

	public RectTransform Echo => echo;
	public bool EchoPlaying => echoPlaying;

	private void Awake()
	{
		player = GetComponent<RectTransform>();
	}

	private void Start()
	{
		echo.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (!echoPlaying)
		{
			RecordPlayer();

			if (Input.GetKeyDown(echoKey))
			{
				StartEcho();
			}
		}
		else
		{
			PlayEcho();
		}
	}

	private void RecordPlayer()
	{
		recordTimer += Time.deltaTime;

		if (recordTimer >= recordInterval)
		{
			recordedPositions.Add(player.anchoredPosition);

			recordTimer = 0f;

			int maxPositions =
				Mathf.RoundToInt(maxRecordTime / recordInterval);

			if (recordedPositions.Count > maxPositions)
			{
				recordedPositions.RemoveAt(0);
			}
		}
	}

	private void StartEcho()
	{
		if (recordedPositions.Count < 2)
			return;

		echo.gameObject.SetActive(true);

		echo.anchoredPosition = recordedPositions[0];

		playbackIndex = 0;
		playbackTimer = 0f;
		echoPlaying = true;

		Debug.Log("Time Echo created!");
	}

	private void PlayEcho()
	{
		playbackTimer += Time.deltaTime;

		if (playbackTimer < recordInterval)
			return;

		playbackTimer = 0f;

		if (playbackIndex < recordedPositions.Count)
		{
			echo.anchoredPosition =
				recordedPositions[playbackIndex];

			playbackIndex++;
		}
		else
		{
			FinishEcho();
		}
	}

	private void FinishEcho()
	{
		echoPlaying = false;

		echo.gameObject.SetActive(false);

		recordedPositions.Clear();

		Debug.Log("Time Echo finished!");
	}
}