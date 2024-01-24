using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dome
{
    public class OrbController : ItemPickup
    {
        Light2D orbLight;
        Animator animController;
        public float rotateAnimationTime = 1.5f;
        public float popOffTime = 1f;
        [SerializeField] private bool poppingOff = false;
        [SerializeField] private Vector3 targetPos;


        [SerializeField]
        float distance;

        private bool placed = false;

        protected override void Start()
        {
            base.Start();
            orbLight = GetComponent<Light2D>();
            animController = GetComponent<Animator>();
            animController.enabled = placed;
        }

        protected override void Update()
        {
            base.Update();
            // Increase intensity and radius of light the closer the player is
            if (!placed)
            {
                orbLight.intensity = Mathf.Clamp(1 / (distance + 1) * 10f, 0.25f, 4f);
                orbLight.pointLightOuterRadius = Mathf.Clamp(1f / (distance + 1f), 0.28f, 0.5f);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if(!placed) GetDistanceToPlayer();
            if (poppingOff)
            {
                Debug.Log("Popping off");
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 5 * Time.fixedDeltaTime);
                if(transform.position == targetPos || pickedUp) poppingOff = false;
            }
        }

        public override void Pickup()
        {
            base.Pickup();
            placed = false;
            animController.enabled = placed;
        }

        private void GetDistanceToPlayer()
        {
            distance = Vector3.Distance(player.position, transform.position);
        }

        public void PlaceOrb(Transform pylon, Vector3 offset)
        {
            col.enabled = false;
            pickedUp = false;
            transform.position = pylon.position + offset;
            Debug.Log(offset);
            gameObject.transform.SetParent(pylon);
            placed = true;
            animController.enabled = placed;
        }

        //public IEnumerator PopOffOrb()
        //{
        //    col.enabled = true;
        //    pickedUp = false;
        //    //play animation
        //    placed = false;
        //    animController.enabled = false;
        //    targetPos = transform.position + new Vector3(3f, 3f, 0);
        //    yield return new WaitForSeconds(popOffTime);
        //    gameObject.transform.SetParent(originParent);
        //}
        public void PopOffOrb()
        {
            col.enabled = true;
            pickedUp = false;
            //play animation
            placed = false;
            animController.enabled = false;
            gameObject.transform.SetParent(originParent);
            targetPos = transform.position + new Vector3(2f, 2f, 0);
            animController.SetBool("isRotating", false);
            poppingOff = true;
        }

        public IEnumerator RotateOrb(Rotator rotator)
        {
            animController.SetBool("isRotating", true);
            float newRotation = 0f;

            if (label == "MoveUp")
            {
                animController.SetInteger("curDir", 0);
                newRotation = 270f;
            }
            else if (label == "MoveRight")
            {
                animController.SetInteger("curDir", 1);
                newRotation = 180f;
            }
            else if (label == "MoveDown")
            {
                animController.SetInteger("curDir", 2);
                newRotation = 90f;
            }
            else if (label == "MoveLeft")
            {
                animController.SetInteger("curDir", 3);
                newRotation = 0f;
            }
            else Debug.Log("Invalid label on rotating orb");

            animController.SetTrigger("rotateOrb");
            yield return new WaitForSeconds(rotateAnimationTime);

            transform.eulerAngles = new Vector3(0, 0, newRotation);
            rotator.isRotating = false;
        }
    }
}