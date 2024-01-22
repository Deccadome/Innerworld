using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dome {
    public class RoomSwitcher : MonoBehaviour
    {
        public GameManager gm;

        public string destination;

        void Start()
        {
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("Player passed threshold. Switching room.");
                gm.LoadRoom(destination);
            }
        }
    }
}