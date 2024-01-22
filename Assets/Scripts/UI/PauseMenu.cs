using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dome
{
    public class PauseMenu : MonoBehaviour
    {
        public GameManager gm;

        private void Start()
        {
            gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        }

        void ResumeGame()
        {
            gm.Unpause();
        }

        void ReturnToMenu()
        {
            gm.ReturnToMenu();
        }
    }
}