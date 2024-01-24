using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Playables;

namespace Dome
{
    public class Pylon : Interactable
    {
        private InputMapper im;
        private SaveManager sm;
        public string control;
        private Light2D keyLight;
        private Transform originParent;
        private bool hasOrb;
        private OrbController curOrb;
        public Vector3 orbOffset;
        public List<AudioSource> sounds;

        private void Awake()
        {
            foreach(Pylon pylon in GameObject.FindObjectsOfType<Pylon>())
            {
                if(pylon.control == control && pylon.gameObject != gameObject) Destroy(gameObject);
            }
            originParent = transform.parent;
        }

        protected override void Start()
        {
            base.Start();

            hasOrb = false;
            requiresObject = true;
            im = GameObject.Find("InputMapper").GetComponent<InputMapper>();
            sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveManager>();
            keyLight = GetComponentInChildren<Light2D>();
            keyLight.enabled = false;
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

                    sounds[0].Play();
                    keyLight.enabled = true;

                    interactor.emptyHands = true;
                    interactor.heldItem = null;
                    hasOrb = true;

                    curOrb.PlaceOrb(transform, orbOffset);
                    if (sm.orbLocations.ContainsKey(curOrb.gameObject)) sm.orbLocations.Remove(curOrb.gameObject);
                }
                else
                {
                    Debug.Log("Wrong item");
                }
            }
            else if (hasOrb && interactor.emptyHands)
            {
                sounds[1].Play();
                keyLight.enabled = false;
                Debug.Log("Picked back up");
                hasOrb = false;

                transform.parent = originParent;
                if (sm.orbLocations.ContainsKey(gameObject)) sm.orbLocations.Remove(gameObject);

                im.UnsetBinding(curOrb.GetLabel(), control);
                interactor.Pickup(curOrb);
            }
        }

        public bool HasOrb()
        {
            return hasOrb;
        }
    }
}