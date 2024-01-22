using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dome
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        private Vector3 defaultPos;
        public float damping;
        private Dictionary<string, Vector2> cameraBorders;
        private GameManager gm;

        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            defaultPos = new (0, 0, -5);
        }

        void FixedUpdate()
        {
            if (target != null)
            {
                Vector3 movePosition = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
            }
            else
            {
                transform.position = defaultPos;
            }
        }
        //public void ChangeRoomBorders()
        //{
        //    cameraBorders = GameObject.FindGameObjectWithTag("SceneWrapper").GetComponent<RoomData>().cameraBorders;
        //}

        public void SetTarget(Transform obj)
        {
            target = obj;
        }
    }
}