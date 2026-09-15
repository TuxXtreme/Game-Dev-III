
using UnityEngine;

namespace Prototype5
{
    public class P5Player : MonoBehaviour
    {
        public float speed = 7f;
        public float jumpForce = 11f;
        Rigidbody2D rb;
        float coyote;

        void Awake() { rb = GetComponent<Rigidbody2D>(); }

        void Update()
        {
            bool grounded = Physics2D.Raycast(transform.position, Vector2.down, 0.75f);
            coyote = grounded ? 0.12f : coyote - Time.deltaTime;

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && coyote > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyote = 0f;
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.LeftShift))
                P5Game.Instance.TogglePhase();

            if (transform.position.y < -8f) P5Game.Instance.Restart();
        }

        void FixedUpdate()
        {
            float x = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x = -1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x = 1f;
            rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);
        }
    }
}
