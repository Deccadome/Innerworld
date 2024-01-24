using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dome
{
    public class ItemPickup : MonoBehaviour, IPickup
    {
        protected Transform player;
        public Transform originParent;
        Interactor interactor;
        protected Collider2D col;
        [SerializeField]
        public bool pickedUp = false;
        public GameObject tooltipPrefab;
        private GameObject tooltip;

        public string label;
        public int itemID;
        private Vector3 downTooltipOffset = new(0, -1.5f, 0);
        private Vector3 upTooltipOffset = new(0, 1.5f, 0);
        public Vector3 pickedUpOffset = new(0, 0.75f, 0);

        protected virtual void Awake()
        {
            foreach (ItemPickup item in GameObject.FindObjectsOfType<ItemPickup>(true))
            {
                Debug.Log("GameObject: " + item.gameObject + " Label: " + item.label + " ItemID: " + item.itemID);
                if (item.itemID == itemID && item.gameObject != gameObject)
                {
                    Debug.Log("Item ID: " + itemID + "GameObject: " + item.gameObject.name);
                    Destroy(gameObject);
                }
            }
            col = gameObject.GetComponent<Collider2D>();
            if (pickedUp) col.enabled = false;
        }

        protected virtual void Start()
        {
            originParent = gameObject.transform.parent;

            player = GameObject.FindGameObjectWithTag("Player").transform;
            interactor = GameObject.FindGameObjectWithTag("Interactor").GetComponent<Interactor>();

            //Vector3 tooltipOffset;
            //if (player.position.y > transform.position.y)
            //{
            //    tooltipOffset = downTooltipOffset;
            //}
            //else tooltipOffset = upTooltipOffset;

            //tooltip = Instantiate(tooltipPrefab, transform.position + tooltipOffset, Quaternion.identity, transform);
            //tooltip.GetComponentInChildren<TMP_Text>().text = "E";
            //tooltip.SetActive(false);
        }

        protected virtual void Update()
        {
            //if (tooltip != null)
            //{
            //    if (player.position.y > transform.position.y)
            //    {
            //        tooltip.transform.position = transform.position + downTooltipOffset;
            //    }
            //    else tooltip.transform.position = transform.position + upTooltipOffset;
            //}
        }

        protected virtual void FixedUpdate()
        {
            if (transform.parent == player)
            {
                transform.position = player.position + pickedUpOffset;
            }
            else if (pickedUp) transform.position = transform.parent.position;
        }

        public virtual void Pickup()
        {
            if (!pickedUp)
            {
                // Parent the player to the object
                transform.SetParent(player);

                // Move the object above the player
                transform.position = player.position + pickedUpOffset;

                interactor.heldItem = gameObject;
                col.enabled = false;
                pickedUp = true;
            }
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
            //tooltip.SetActive(true);
        }

        public virtual void DisablePrompt()
        {
            //tooltip.SetActive(false);
        }

        public virtual string GetLabel()
        {
            return label;
        }

        public virtual void SetLabel(string newLabel)
        {
            label = newLabel;
        }
    }
}