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

        [SerializeField]
        float distance;

        public Vector3 placedOffset = new (0, 0.5f, 0);
        private bool placed;

        protected override void Start()
        {
            base.Start();
            orbLight = GetComponent<Light2D>();
            animController = GetComponent<Animator>();
            placed = false;
            animController.enabled = placed;
        }

        protected override void Update()
        {
            base.Update();
            // Increase intensity and radius of light the closer the player is
            if (!placed)
            {
                orbLight.intensity = Mathf.Clamp(1 / (distance + 1) * 25f, 0.25f, 50f);
                orbLight.pointLightOuterRadius = Mathf.Clamp(1f / (distance + 1f), 0.1f, 0.3f);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            GetDistanceToPlayer();
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

        public void PlaceOrb(Transform pylon)
        {
            col.enabled = false;
            pickedUp = false;
            gameObject.transform.SetParent(pylon);
            Debug.Log(transform.parent);
            Debug.Log("Pylon position: " + pylon.position);
            transform.position = pylon.position + placedOffset;
            placed = true;
            animController.enabled = placed;
        }
    }
}