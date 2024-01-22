using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private Animator animController;

    private void Awake()
    {
        animController = GetComponent<Animator>();
    }

    void Start()
    {
        animController.SetTrigger("enterMenu");
    }

    
}
