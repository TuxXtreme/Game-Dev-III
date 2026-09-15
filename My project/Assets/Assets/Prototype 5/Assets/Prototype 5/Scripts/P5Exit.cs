
using UnityEngine;

namespace Prototype5
{
    public class P5Exit : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<P5Player>()) return;
            if (P5Game.Instance.cores >= P5Game.Instance.totalCores)
                Debug.Log("PROTOTYPE 5 COMPLETE!");
        }
    }
}
