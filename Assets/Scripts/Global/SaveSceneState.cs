using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dome {
    public class SaveSceneState : MonoBehaviour
    {
        GameManager gm;
        private void Awake()
        {
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

            string curWrapper = gameObject.name;
            string curScene = SceneManager.GetActiveScene().name;

            // If there is already a Scene Wrapper of this type, destroy it
            foreach (GameObject wrapper in GameObject.FindGameObjectsWithTag("SceneWrapper"))
            {
                if ((curWrapper == "IWSceneWrapper" && wrapper.name == "IWSceneWrapper" && wrapper != gameObject) ||
                   (curWrapper == "OWSceneWrapper" && wrapper.name == "OWSceneWrapper" && wrapper != gameObject))
                {
                    Destroy(wrapper);
                }
            };

            if (curWrapper == "IWSceneWrapper")
            {
                gm.iwWrapper = gameObject;
            }
            else
            {
                gm.owWrapper = gameObject;
            }

        }
        void Start()
        {
            DontDestroyOnLoad(this);
        }
    }
}
