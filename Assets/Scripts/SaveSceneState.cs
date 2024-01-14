using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSceneState : MonoBehaviour
{
    private void Awake()
    {
        // If there is already a Scene Wrapper of this type, destroy it
        foreach (SaveSceneState script in GameObject.FindObjectsOfType<SaveSceneState>())
        {
            if ((this.gameObject.name == "IWSceneWrapper" && script.gameObject.name == "IWSceneWrapper" && script.gameObject != this.gameObject) ||
               (this.gameObject.name == "OWSceneWrapper" && script.gameObject.name == "OWSceneWrapper" && script.gameObject != this.gameObject))
            {
                Destroy(script.gameObject);
            }
        };
    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
