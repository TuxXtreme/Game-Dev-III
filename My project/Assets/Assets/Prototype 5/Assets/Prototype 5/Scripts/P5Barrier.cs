
using UnityEngine;

namespace Prototype5
{
    public class P5Barrier : MonoBehaviour
    {
        public bool solidInCyan;
        BoxCollider2D box;
        SpriteRenderer sr;

        void Awake()
        {
            box = GetComponent<BoxCollider2D>();
            sr = GetComponent<SpriteRenderer>();
            Refresh();
        }

        public void Refresh()
        {
            if (P5Game.Instance == null) return;
            bool solid = P5Game.Instance.cyanPhase == solidInCyan;
            box.enabled = solid;
            Color c = solidInCyan ? new Color(0.2f,0.8f,1f,1f) : new Color(0.85f,0.25f,1f,1f);
            c.a = solid ? 0.9f : 0.18f;
            sr.color = c;
        }
    }
}
