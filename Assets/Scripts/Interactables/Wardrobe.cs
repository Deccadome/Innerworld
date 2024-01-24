using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dome
{
    public class Wardrobe : Interactable
    {
        public SaveManager sm;
        public Collider2D interactCol;
        public Light2D indicatorLight;

        protected override void Start()
        {
            base.Start();
            sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveManager>();
            if (sm.milestones.Contains("playerChanged")) interactCol.enabled = false;
        }

        public override void Interact()
        {
            //Play change animation
            player.GetComponent<Animator>().SetTrigger("changing");

            indicatorLight.GetComponent<Animator>().SetTrigger("turnOff");

            sm.milestones.Add("playerChanged");
        }
    }
}