using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dome
{

    public class Interactable : MonoBehaviour, IInteractable
    {
        protected Interactor interactor;
        Collider2D col;
        protected Transform player;
        protected bool requiresObject;
        protected GameObject heldItem;

        protected virtual void Start()
        {
            col = gameObject.GetComponent<Collider2D>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            interactor = GameObject.FindGameObjectWithTag("Interactor").GetComponent<Interactor>();
        }

        public virtual void Interact()
        {
            if (requiresObject && !interactor.emptyHands)
            {
                heldItem = interactor.heldItem;
            }
        }

        public virtual void Prompt()
        {

        }
    }
}