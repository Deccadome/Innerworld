using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dome
{
    public class Interactor : MonoBehaviour
    {
        PlayerController pc;
        Animator ac;
        Transform player;
        public Vector3 playerMoveDir;
        public bool emptyHands = true;
        public GameObject heldItem;
        public InputReader inputReader;
        public bool ableToInteract;
        [SerializeField]
        List<Collider2D> colsInRange;
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            ac = player.GetComponent<Animator>();
            pc = player.GetComponent<PlayerController>();
            colsInRange = new List<Collider2D>();
            inputReader.InteractEvent += Interact;
            ableToInteract = true;
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
            Vector3 offsetPos = pc.lastMoveVector.normalized / 3;
            if (pc.name == "OW Player") offsetPos.y -= 0.5f;
            return offsetPos;
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
                ableToInteract = false;
                foreach (Collider2D col in colsInRange)
                {
                    if (col.gameObject.TryGetComponent(out IInteractable obj))
                    {
                        obj.Interact();
                        ableToInteract = true;
                        break;
                    }
                }
                if (!ableToInteract) DropItem();
            }
            else if (colsInRange.Count > 0)
            {
                Collider2D col = colsInRange[0];
                Debug.Log(col.gameObject.name);
                if (col.gameObject.TryGetComponent(out IInteractable obj))
                {
                    obj.Interact();
                    ableToInteract = true;
                }
                else if (col.gameObject.TryGetComponent(out IPickup item) && emptyHands)
                {
                    ac.SetTrigger("grab");
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