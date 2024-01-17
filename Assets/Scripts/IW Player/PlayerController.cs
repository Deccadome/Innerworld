using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dome
{
    public class PlayerController : MonoBehaviour
    {
        const int UP = 0;
        const int RIGHT = 1;
        const int DOWN = 2;
        const int LEFT = 3;
        const int ON = 1;
        const int OFF = 0;

        private int lastMoveDir;
        public float moveSpeed;
        private bool isMoving;
        Animator animController;
        Rigidbody2D rb;
        SpriteRenderer sr;
        [HideInInspector] public Vector2 lastMoveVector;
        [SerializeField]
        private Vector2 moveDir;
        private int[] dirList = new int[4] { OFF, OFF, OFF, OFF };

        [SerializeField] private InputReader input;

        void Start()
        {
            SetUpEvents();
            rb = GetComponent<Rigidbody2D>();
            animController = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            InputManagement();
            SetAnimations();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void InputManagement()
        {
            moveDir = new Vector2(dirList[RIGHT] - dirList[LEFT], dirList[UP] - dirList[DOWN]);
            if (moveDir.x != 0 || moveDir.y != 0) isMoving = true;
            else isMoving = false;
        }

        private void Move()
        {
            rb.velocity = new Vector2(moveSpeed * moveDir.x, moveSpeed * moveDir.y);
        }

        private void SetAnimations()
        {
            if (moveDir.x < 0 && moveDir.y == 0) sr.flipX = true;
            else sr.flipX=false;
            animController.SetBool("isMoving", isMoving);
            animController.SetFloat("Horizontal", moveDir.x);
            animController.SetFloat("Vertical", moveDir.y);
        }

        private void MoveLeft() { dirList[LEFT] = ON; }
        private void MoveLeftCancelled() { dirList[LEFT] = OFF; }
        private void MoveRight() { dirList[RIGHT] = ON; }
        private void MoveRightCancelled() { dirList[RIGHT] = OFF; }
        private void MoveUp() { dirList[UP] = ON; }
        private void MoveUpCancelled() { dirList[UP] = OFF; }
        private void MoveDown() { dirList[DOWN] = ON; }
        private void MoveDownCancelled() { dirList[DOWN] = OFF; }


        private void SetUpEvents()
        {
            input.MoveLeftEvent += MoveLeft;
            input.MoveLeftCancelledEvent += MoveLeftCancelled;
            input.MoveRightEvent += MoveRight;
            input.MoveRightCancelledEvent += MoveRightCancelled;
            input.MoveUpEvent += MoveUp;
            input.MoveUpCancelledEvent += MoveUpCancelled;
            input.MoveDownEvent += MoveDown;
            input.MoveDownCancelledEvent += MoveDownCancelled;
        }
    }
}
