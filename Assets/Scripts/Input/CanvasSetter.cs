using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dome {
    public class CanvasSetter : MonoBehaviour
    {
        public UIInput gm;
        // Start is called before the first frame update
        void OnEnable()
        {
            gm = GameObject.Find("Game Manager").GetComponent<UIInput>();
            if (gameObject.name == "Main Menu") gm.mmGR = gameObject.GetComponent<GraphicRaycaster>();
            else if(gameObject.name == "Pause Menu") gm.pmGR = gameObject.GetComponent<GraphicRaycaster>();
        }
    }
}