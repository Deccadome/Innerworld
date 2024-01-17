using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dome
{
    public class ItemPickup : MonoBehaviour, IPickup
    {
        protected Transform player;
        protected Transform originParent;
        Interactor interactor;
        protected Collider2D col;
        [SerializeField]
        protected bool pickedUp = false;
        public GameObject tooltipPrefab;
        private GameObject tooltip;

        public string label;
        private Vector3 downTooltipOffset = new(0, -1.5f, 0);
        private Vector3 upTooltipOffset = new(0, 1.5f, 0);
        public Vector3 pickedUpOffset = new(0, 0.75f, 0);

        protected virtual void Start()
        {
            col = gameObject.GetComponent<Collider2D>();
            originParent = gameObject.transform.parent;

            player = GameObject.FindGameObjectWithTag("Player").transform;
            interactor = GameObject.FindGameObjectWithTag("Interactor").GetComponent<Interactor>();

            Vector3 tooltipOffset;
            if (player.position.y > transform.position.y)
            {
                tooltipOffset = downTooltipOffset;
            }
            else tooltipOffset = upTooltipOffset;

            tooltip = Instantiate(tooltipPrefab, transform.position + tooltipOffset, Quaternion.identity, transform);
            tooltip.GetComponentInChildren<TMP_Text>().text = "E";
            tooltip.SetActive(false);
        }

        protected virtual void Update()
        {
            if (tooltip != null)
            {
                if (player.position.y > transform.position.y)
                {
                    tooltip.transform.position = transform.position + downTooltipOffset;
                }
                else tooltip.transform.position = transform.position + upTooltipOffset;
            }
        }

        protected virtual void FixedUpdate()
        {
            if (pickedUp) transform.position = player.position + pickedUpOffset;
        }

        public virtual void Pickup()
        {
            // Parent the player to the object
            transform.SetParent(player);

            // Move the object above the player
            transform.position = Vector3.MoveTowards(transform.position, player.position + pickedUpOffset, 5f);

            interactor.heldItem = gameObject;
            col.enabled = false;
            pickedUp = true;
        }

        public virtual void Drop()
        {
            transform.SetParent(originParent);
            pickedUp = false;
            transform.position = Vector3.MoveTowards(transform.position, transform.position - pickedUpOffset, 5f);
            col.enabled = true;
        }

        public virtual void EnablePrompt()
        {
            tooltip.SetActive(true);
        }

        public virtual void DisablePrompt()
        {
            tooltip.SetActive(false);
        }

        public virtual string GetLabel()
        {
            return label;
        }
    }
}