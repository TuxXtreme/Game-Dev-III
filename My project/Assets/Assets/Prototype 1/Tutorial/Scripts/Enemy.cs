using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody enemyRb;
    private GameObject player;

	private void Start()
	{
		enemyRb = GetComponent<Rigidbody>();
		player = GameObject.Find("Player");
	}

	private void Update()
	{
		Vector3 lookDirection = (player.transform.position - transform.position).normalized;
		enemyRb.AddForce( lookDirection * speed);

		if (transform.position.y < -10)
		{
			Destroy(gameObject);
		}
}
}
