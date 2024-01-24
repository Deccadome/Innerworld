using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Dome
{
    public class OrbLeaf : Interactable
    {
        public enum PriorityDir
        {
            Opposite,
            Right,
            Left,
        }

        private enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        PlayableDirector cutscene;
        GameManager gm;
        SaveManager sm;
        OrbController orb;
        public List<AudioSource> chirps;
        public List<AudioSource> runSounds;

        public string puzzleName;

        private Dictionary<Direction, Vector2> dirVectors;
        private float rayLength = 1f;
        public float moveSpeed;
        public float cutsceneLength = 3f;
        public PriorityDir dirPriority;
        
        [SerializeField] private List<Direction> availableDirs;
        [SerializeField] private Vector2 moveDir;

        [SerializeField] Direction playerDir;
        [SerializeField] bool canMove = false;
        bool dirAvailable;
        Vector2 target;

        private void Awake()
        {
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            sm = gm.GetComponent<SaveManager>();

            if(sm.milestones.Contains(puzzleName)) Destroy(gameObject);
        }

        protected override void Start()
        {
            base.Start();
            cutscene = GetComponentInChildren<PlayableDirector>();
            orb = GetComponentInChildren<OrbController>();

            requiresObject = false;
            dirVectors = new Dictionary<Direction, Vector2>
            {
                { Direction.Up, transform.up },
                { Direction.Right, transform.right },
                { Direction.Down, -transform.up },
                { Direction.Left, -transform.right },
            };
            availableDirs = new List<Direction>();
        }

        private void Update()
        {
            if((Vector2)transform.position == target && canMove)
            {
                canMove = false;
            }
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);
            }
        }

        public override void Interact()
        {
            //Debug.Log("Interact called");
            if (interactor.emptyHands)
            {
                // if player is L/R/U/D
                CheckPlayerDir();

                // move in the opposite direction if possible
                GetMoveDir();

                if(dirAvailable) GetMoveTarget();
            }
        }

        private void CheckPlayerDir()
        {
            // get x diff
            float xDiff = player.position.x - transform.position.x;
            // get y diff
            float yDiff = player.position.y - transform.position.y - 0.5f; // collider offset
            // whichever is further from zero, set as direction
            if(Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
            {
                if (xDiff > 0) playerDir = Direction.Right;
                else playerDir = Direction.Left;
            }
            else
            {
                if(yDiff > 0) playerDir = Direction.Up;
                else playerDir = Direction.Down;
            }
        }

        private void CheckAvailableDirs()
        {
            availableDirs.Clear();
            foreach(Direction dir in dirVectors.Keys)
            {
                bool freeDir = true;
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dirVectors[dir], rayLength);
                foreach(RaycastHit2D hit in hits)
                {
                    if(hit.collider.gameObject != gameObject &&
                        hit.collider.gameObject != interactor.gameObject)
                        freeDir = false;
                }
                if (freeDir) { availableDirs.Add(dir); }
            }
        }

        private void GetMoveDir()
        {
            CheckAvailableDirs();
            Direction firstDir;
            if (dirPriority == PriorityDir.Opposite) firstDir = (Direction)(((int)playerDir + 2) % 4);
            else if (dirPriority == PriorityDir.Right) firstDir = (Direction)(((int)playerDir + 3) % 4);
            else firstDir = (Direction)(((int)playerDir + 1) % 4);
            //Debug.Log(firstDir);
            if (availableDirs.Contains(firstDir)) 
            { 
                moveDir = dirVectors[firstDir];
                dirAvailable = true;
                chirps[Random.Range(0, chirps.Count)].Play();
            }
            else if (availableDirs.Count > 0)
            {
                foreach(Direction dir in availableDirs)
                {
                    if(dir != playerDir)
                    {
                        moveDir = dirVectors[dir];
                        dirAvailable = true;
                        chirps[Random.Range(0, chirps.Count)].Play();
                    }
                }
            }
            else { 
                dirAvailable = false; 
                StartCoroutine(Caught());
            }
        }

        private void GetMoveTarget()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, moveDir, 8f);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject != gameObject &&
                    hit.collider.gameObject != interactor.gameObject)
                {
                    float distanceToObstacle = Vector2.Distance(transform.position, hit.point) - 0.5f;
                    target = (Vector2)(transform.position) + moveDir * distanceToObstacle;
                    canMove = true;
                    break;
                }
            }
        }

        IEnumerator Caught()
        {
            sm.SetMilestone(puzzleName);

            // Player pick up orb
            orb.pickedUp = false;
            interactor.Pickup(orb);

            // Play cutscene
            Time.timeScale = 0f;
            cutscene.Play();
            yield return new WaitForSecondsRealtime(cutsceneLength);

            // Resume
            Time.timeScale = 1f;
            Destroy(gameObject);

        }

        public void RunAwaySounds()
        {
            StartCoroutine(RunAwayCoroutine());
        }

        private IEnumerator RunAwayCoroutine()
        {
            chirps[2].Play();
            yield return new WaitForSecondsRealtime(1.15f);
            runSounds[0].Play();
            yield return new WaitForSecondsRealtime(0.37f);
            runSounds[1].Play();
        }
    }
}