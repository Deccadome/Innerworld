using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dome
{
    public class Rotator : Interactable
    {
        public bool hasOrb;
        private OrbController curOrb;
        private AudioSource activateSound;
        public GameManager gm;
        public Vector3 orbOffset = new (0, 0, 0);
        public bool isRotating = false;


        private List<string> validOrbs;

        protected override void Start()
        {
            base.Start();
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            activateSound = GetComponent<AudioSource>();
            requiresObject = true;
            validOrbs = new List<string>
            {
                "MoveUp",
                "MoveRight",
                "MoveDown",
                "MoveLeft",
            };
        }

        public override void Interact()
        {
            base.Interact();
            if(!hasOrb && heldItem && !isRotating)
            {
                if(heldItem.TryGetComponent<OrbController>(out OrbController orb))
                {
                    isRotating = true;
                    interactor.emptyHands = true;
                    if (!validOrbs.Contains(orb.GetLabel()))
                    {
                        orb.PlaceOrb(transform, orbOffset);
                        orb.PopOffOrb();
                        isRotating=false;
                    }
                    else
                    {
                        StartCoroutine(RotateOrb(orb));
                    }
                }
            }
        }

        private IEnumerator RotateOrb(OrbController orb)
        {
            gm.worldSwitchEnabled = false;
            curOrb = orb;
            curOrb.PlaceOrb(transform, orbOffset);
            activateSound.Play();
            yield return curOrb.RotateOrb(this);

            int targetIndex = (validOrbs.IndexOf(curOrb.GetLabel()) + 1) % 4;
            curOrb.SetLabel(validOrbs[targetIndex]);

            curOrb.PopOffOrb();
            gm.worldSwitchEnabled = true;
        }
    }
}