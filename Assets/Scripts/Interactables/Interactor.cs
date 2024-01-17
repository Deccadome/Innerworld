using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dome
{
    public class Interactor : MonoBehaviour
    {
        PlayerController pc;
        Transform player;
        public Vector3 playerMoveDir;
        public bool emptyHands = true;
        public GameObject heldItem;
        public InputReader inputReader;
        [SerializeField]
        List<Collider2D> colsInRange;
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            pc = player.GetComponent<PlayerController>();
            colsInRange = new List<Collider2D>();
            inputReader.InteractEvent += Interact;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = player.position + GetOffset();
        }

        void DropItem()
        {
            heldItem.GetComponent<IPickup>().Drop();
            emptyHands = true;
            heldItem = null;
        }

        Vector3 GetOffset()
        {
            return pc.lastMoveVector.normalized / 2;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            colsInRange.Add(col);
            if (col.TryGetComponent(out IPickup obj)) obj.EnablePrompt();
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            colsInRange.Remove(col);
            if (col.TryGetComponent(out IPickup obj)) obj.DisablePrompt();
        }

        public void Interact()
        {
            if (!emptyHands)
            {
                bool ableToInteract = false;
                foreach (Collider2D col in colsInRange)
                {
                    if (col.gameObject.TryGetComponent(out IInteractable obj))
                    {
                        ableToInteract = true;
                        obj.Interact();
                        break;
                    }
                }
                if (!ableToInteract) DropItem();
            }
            else if (colsInRange.Count > 0)
            {
                Collider2D col = colsInRange[0];
                if (col.gameObject.TryGetComponent(out IInteractable obj))
                {
                    obj.Interact();
                }
                else if (col.gameObject.TryGetComponent(out IPickup item) && emptyHands)
                {
                    Pickup(item);
                }
            }
        }

        public void Pickup(IPickup item)
        {
            item.Pickup();
            emptyHands = false;
        }
    }
}