
using UnityEngine;

namespace Prototype5
{
    public class P5Hazard : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<P5Player>()) P5Game.Instance.Restart();
        }
    }
}
