using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dome {
    public class UIInput : MonoBehaviour
    {
        public InputReader inputReader;
        private Vector2 pointerPosition;
        public GraphicRaycaster mmGR;
        public GraphicRaycaster pmGR;
        public GameManager gm;
        
        private void Start()
        {
            inputReader.PointEvent += Point;
            inputReader.ClickEvent += Click;
            gm = GetComponent<GameManager>();
        }

        private void Update()
        {
            
        }

        void Point(Vector2 position) { pointerPosition = position; }
        void Click()
        {
            GraphicRaycaster gr;
            if (gm.curState == GameManager.GameState.Paused) gr = pmGR;
            else gr = mmGR;

            Debug.Log(gameObject.name);
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = pointerPosition;

            int uiLayerMask = LayerMask.GetMask("UI");

            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(eventData, results);

            if (results.Count > 0)
            {
                RaycastResult result = results[0];
                Debug.Log(result.gameObject.name);
                if (result.gameObject.TryGetComponent<Button>(out Button button))
                {
                    Debug.Log("Button there");
                    button.onClick.Invoke();
                }
            }
        }
    }
}