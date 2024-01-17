using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dome
{
    public class Pylon : Interactable
    {
        public InputMapper im;
        public string control;
        private bool hasOrb;
        private OrbController curOrb;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            hasOrb = false;
            requiresObject = true;
            im = GameObject.Find("InputMapper").GetComponent<InputMapper>();
        }

        public override void Interact()
        {
            Debug.Log("Interactor called");
            base.Interact();
            if (!hasOrb)
            {
                if (heldItem.TryGetComponent<OrbController>(out OrbController orb))
                {
                    curOrb = orb;
                    im.SetBinding(orb.GetLabel(), control);

                    interactor.emptyHands = true;
                    interactor.heldItem = null;
                    hasOrb = true;

                    curOrb.PlaceOrb(transform);
                }
                else
                {
                    Debug.Log("Wrong item");
                }
            }
            else if (interactor.emptyHands)
            {
                Debug.Log("Picked back up");
                interactor.Pickup(curOrb);
            }
        }
    }
}