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
        public Vector3 orbOffset = new(0, 0.5f, 0);
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
            if (!hasOrb && heldItem)
            {
                if (heldItem.TryGetComponent<OrbController>(out OrbController orb))
                {
                    Debug.Log("Placing orb");
                    curOrb = orb;
                    im.SetBinding(orb.GetLabel(), control);

                    interactor.emptyHands = true;
                    interactor.heldItem = null;
                    hasOrb = true;

                    curOrb.PlaceOrb(transform, orbOffset);
                }
                else
                {
                    Debug.Log("Wrong item");
                }
            }
            else if (hasOrb && interactor.emptyHands)
            {
                
                Debug.Log("Picked back up");
                hasOrb = false;
                im.UnsetBinding(curOrb.GetLabel(), control);
                interactor.Pickup(curOrb);
            }
        }
    }
}