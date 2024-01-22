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
        public GameManager gm;
        public Vector3 orbOffset = new (0, 0, 0);


        private List<string> validOrbs;

        protected override void Start()
        {
            base.Start();
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
            if(!hasOrb && heldItem)
            {
                if(heldItem.TryGetComponent<OrbController>(out OrbController orb))
                {
                    interactor.emptyHands = true;
                    if (!validOrbs.Contains(orb.GetLabel()))
                    {
                        orb.PlaceOrb(transform, orbOffset);
                        orb.PopOffOrb();
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

            yield return curOrb.RotateOrb();

            int targetIndex = (validOrbs.IndexOf(curOrb.GetLabel()) + 1) % 4;
            curOrb.SetLabel(validOrbs[targetIndex]);

            curOrb.PopOffOrb();
            gm.worldSwitchEnabled = true;
        }
    }
}