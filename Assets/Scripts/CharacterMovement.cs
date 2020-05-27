using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public enum State
    {
        IdleDown,
        IdleUp,
        IdleRight,
        IdleLeft,
        MoveDown,
        MoveUp,
        MoveRight,
        MoveLeft
    }

    public State state;
    public float speed = 4;
    public int size;

    public Vector2 Direction { get; set; }
    public int CaseMoved { get; set; }

    public bool Moving { get { return Position != Destination; } }

    public Vector2 Position { get { return transform.position; } set { transform.position = value; }  }

    private Vector2 destination;
    public Vector2 Destination 
    { 
        get
        {
            return destination;
        }
        protected set
        {
            destination.x = Mathf.Round(value.x);
            destination.y = Mathf.Round(value.y);
        }
    }
    protected SpriteAnimator animator;
    protected Transform colliderChild;


    public bool Idle
    {
        get
        {
            return (state == State.IdleUp || state == State.IdleDown || state == State.IdleRight || state == State.IdleLeft);
        }
    }

    private void Awake()
    {
        animator = transform.Find("Default Sprite").GetComponent<SpriteAnimator>();
        colliderChild = transform.Find("Collider");
    }

    // Start is called before the first frame update
    void Start()
    {
        Destination = Position;
        SetIdleSpriteDirection();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection(Direction);

        if (changeDirectionTimer <= 0)
        {
            if (Position != Destination)
            {
                Position = Vector3.MoveTowards(Position, Destination, speed * Time.deltaTime);
            }
            else if (!Idle)
            {
                if (!Utils.BlockInPosition(Position + Direction, 1))
                {
                    Destination = Position + Direction;
                    CaseMoved++;
                }
            }
        }
        else
            changeDirectionTimer -= Time.deltaTime;

        // The hitbox is always at the destination to avoid it beeing between two squares
        colliderChild.position = Destination;
    }


    private readonly float changeDirectionTime = 0.1f;
    private float changeDirectionTimer;
    private readonly float stopTime = 0.1f;
    private float stopTimer;
    private void ChangeDirection(Vector2 direction)
    {
        if (Position == Destination) 
        {
            // Change state based on the direction wanted for the character. Small delay if the character is idle to allow changing view without moving
            if (direction == Vector2.down) 
            {
                animator.SetAnimation("Move Down");
                if (state == State.IdleDown || !Idle)
                    state = State.MoveDown;
                else 
                {
                    changeDirectionTimer = changeDirectionTime;
                    state = State.IdleDown;
                }
            } 
            else if (direction == Vector2.up) 
            {
                animator.SetAnimation("Move Up");
                if (state == State.IdleUp || !Idle)
                    state = State.MoveUp;
                else
                {
                    changeDirectionTimer = changeDirectionTime;
                    state = State.IdleUp;
                }
            } 
            else if (direction == Vector2.right) 
            {
                animator.SetAnimation("Move Right");
                if (state == State.IdleRight || !Idle)
                    state = State.MoveRight;
                else
                {
                    changeDirectionTimer = changeDirectionTime;
                    state = State.IdleRight;
                }
            } 
            else if (direction == Vector2.left) 
            {
                animator.SetAnimation("Move Left");
                if (state == State.IdleLeft || !Idle)
                    state = State.MoveLeft;
                else
                {
                    changeDirectionTimer = changeDirectionTime;
                    state = State.IdleLeft;
                }
            }

            // Use a timer when the character stop, removing the change direction time if we are moving
            if (direction == Vector2.zero)
            {
                if (stopTimer <= 0)
                    stopTimer = stopTime;
                else
                {
                    stopTimer -= Time.deltaTime;
                    if (stopTimer <= 0)
                        SetIdleSpriteDirection();
                }
            }
            else
                stopTimer = 0;
        }
    }

    private void SetIdleSpriteDirection()
    {
        // Set the animator so the character face the right position
        switch (state) 
        {
            case State.IdleDown:
                animator.SetAnimation("Idle Down");
                break;
            case State.IdleUp:
                animator.SetAnimation("Idle Up");
                break;
            case State.IdleRight:
                animator.SetAnimation("Idle Right");
                break;
            case State.IdleLeft:
                animator.SetAnimation("Idle Left");
                break;
            case State.MoveDown:
                animator.SetAnimation("Idle Down");
                state = State.IdleDown;
                break;
            case State.MoveUp:
                animator.SetAnimation("Idle Up");
                state = State.IdleUp;
                break;
            case State.MoveRight:
                animator.SetAnimation("Idle Right");
                state = State.IdleRight;
                break;
            case State.MoveLeft:
                animator.SetAnimation("Idle Left");
                state = State.IdleLeft;
                break;
        }
    }
}
