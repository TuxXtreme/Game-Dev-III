using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControllerX : MonoBehaviour
{
	private Rigidbody playerRb;
	private GameObject focalPoint;
	private InputSystem_Actions controls;

	[Header("Movement")]
	public float speed = 1000f;

	[Header("Powerup")]
	public bool hasPowerup;
	public GameObject powerupIndicator;
	public int powerUpDuration = 5;

	private float normalStrength = 10f;
	private float powerupStrength = 25f;

	[Header("Boost")]
	public float boostForce = 25f;
	public float boostCooldown = 2f;
	private bool canBoost = true;
	private float boostTimer = 0f;

	[Header("Boost UI")]
	public Image boostIndicator;
	public Color readyColor = Color.white;
	public Color cooldownColor = Color.gray;

	[Header("Boost Shockwave")]
	public float shockwaveRadius = 5f;
	public float shockwaveForce = 15f;
	public LayerMask enemyLayer;

	void Awake()
	{
		controls = new InputSystem_Actions();
	}

	void OnEnable()
	{
		controls.Player.Enable();
	}

	void OnDisable()
	{
		controls.Player.Disable();
	}

	void Start()
	{
		playerRb = GetComponent<Rigidbody>();
		focalPoint = GameObject.Find("Focal Point");

		if (boostIndicator != null)
		{
			boostIndicator.fillAmount = 1f;
			boostIndicator.color = readyColor;
		}
	}

	void Update()
	{
		Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
		float forwardInput = moveInput.y;

		playerRb.AddForce(
			focalPoint.transform.forward * forwardInput * speed * Time.deltaTime
		);

		if (Keyboard.current != null &&
			Keyboard.current.spaceKey.wasPressedThisFrame &&
			canBoost)
		{
			StartCoroutine(BoostRoutine());
		}

		UpdateBoostUI();

		if (powerupIndicator != null)
		{
			powerupIndicator.transform.position =
				transform.position + new Vector3(0, -0.6f, 0);
		}
	}

	void UpdateBoostUI()
	{
		if (boostIndicator == null) return;

		if (!canBoost)
		{
			boostTimer -= Time.deltaTime;

			boostIndicator.fillAmount =
				Mathf.Clamp01(1 - (boostTimer / boostCooldown));

			boostIndicator.color = cooldownColor;
		}
		else
		{
			boostIndicator.fillAmount = 1f;
			boostIndicator.color = readyColor;
		}
	}

	IEnumerator BoostRoutine()
	{
		canBoost = false;
		boostTimer = boostCooldown;

		playerRb.AddForce(
			focalPoint.transform.forward * boostForce,
			ForceMode.Impulse
		);

		DoBoostShockwave();

		yield return new WaitForSeconds(boostCooldown);

		canBoost = true;
	}

	void DoBoostShockwave()
	{
		Collider[] enemiesHit = Physics.OverlapSphere(
			transform.position,
			shockwaveRadius,
			enemyLayer
		);

		foreach (Collider enemy in enemiesHit)
		{
			Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();

			if (enemyRb != null)
			{
				Vector3 direction =
					enemy.transform.position - transform.position;

				direction.Normalize();

				enemyRb.AddForce(
					direction * shockwaveForce,
					ForceMode.Impulse
				);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Powerup"))
		{
			Destroy(other.gameObject);

			hasPowerup = true;

			if (powerupIndicator != null)
			{
				powerupIndicator.SetActive(true);
			}

			StartCoroutine(PowerupCountdownRoutine());
		}
	}

	IEnumerator PowerupCountdownRoutine()
	{
		yield return new WaitForSeconds(powerUpDuration);

		hasPowerup = false;

		if (powerupIndicator != null)
		{
			powerupIndicator.SetActive(false);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Rigidbody enemyRigidbody =
				collision.gameObject.GetComponent<Rigidbody>();

			if (enemyRigidbody == null) return;

			Vector3 awayFromPlayer =
				collision.gameObject.transform.position - transform.position;

			awayFromPlayer.Normalize();

			float strength = hasPowerup ? powerupStrength : normalStrength;

			enemyRigidbody.AddForce(
				awayFromPlayer * strength,
				ForceMode.Impulse
			);
		}
	}
}