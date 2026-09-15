
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype5
{
    public class P5Game : MonoBehaviour
    {
        public static P5Game Instance;
        public bool cyanPhase;
        public int cores;
        public int totalCores = 3;

        void Awake() { Instance = this; }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) Restart();
        }

        public void TogglePhase()
        {
            cyanPhase = !cyanPhase;
            foreach (P5Barrier b in FindObjectsByType<P5Barrier>(FindObjectsSortMode.None))
                b.Refresh();
        }

        public void AddCore() { cores++; }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        void OnGUI()
        {
            GUI.Box(new Rect(15,15,410,92), "PROTOTYPE 5 - SIGNAL SHIFT");
            GUI.Label(new Rect(30,45,380,22), "A/D: Move   Space: Jump   E/Shift: Switch Phase");
            GUI.Label(new Rect(30,70,380,22), "Signal Cores: " + cores + "/" + totalCores + "   Phase: " + (cyanPhase ? "CYAN" : "MAGENTA"));
        }
    }
}
