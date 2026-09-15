
using UnityEngine;

namespace Prototype5
{
    public class P5Core : MonoBehaviour
    {
        void Update() { transform.Rotate(0,0,90f * Time.deltaTime); }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<P5Player>()) return;
            P5Game.Instance.AddCore();
            Destroy(gameObject);
        }
    }
}
