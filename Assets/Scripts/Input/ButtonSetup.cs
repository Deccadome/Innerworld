using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dome {
    public class ButtonSetup : MonoBehaviour
    {
        Button button;
        GameManager gm;
        public string function;

        void Start()
        {
            button = GetComponent<Button>();
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            if (function == "NewGame") button.onClick.AddListener(gm.NewGame);
            else if (function == "ReturnToMenu") button.onClick.AddListener(gm.ReturnToMenu);
            else if (function == "Unpause") button.onClick.AddListener(gm.Unpause);
        }
    }
}