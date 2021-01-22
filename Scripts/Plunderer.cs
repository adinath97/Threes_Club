using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunderer : MonoBehaviour
{
    public Vector2 velocity;
    public float bounceVelocity;
    public bool walk, walk_left, walk_right, jump;
    public LayerMask wallMask;
    public float jumpVelocity;
    public float gravity;
    public LayerMask floorMask;
    private RaycastHit2D hitRay;
    private RaycastHit2D hitRay1;
    public float liftAmount;
    private Animator animator;
    private Vector3 position;
    private Vector3 scale;
    private float bulletSpeed = 20f;
    public float valueChosen;
    //private bool bounce = false;

    //use enum to track player state
    public enum PlayerState
    {
        jumping,
        idle,
        walking,
        //bouncing
    }

    private PlayerState playerState = PlayerState.idle;
    public static bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.gameOver && !LevelManager.playerOut && !PlayerRotator.playerOutOfRotation)
        {
            CheckPlayerInput();
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
            {
                UpdatePlayerPosition();
            }
        }
        else
        {
            animator.enabled = false;
        }
    }

    void UpdatePlayerPosition()
    {

        position = transform.localPosition;
        scale = transform.localScale;

        if (walk)
        {
            Walking();
        }

        if (!walk && !jump)
        {
            Idle();
        }

        if (jump && playerState != PlayerState.jumping)
        {
            PlayerStateJumping();
        }

        JumpingStatus();
        /*
        if(bounce && playerState != PlayerState.bouncing)
        {
            playerState = PlayerState.bouncing;
            velocity = new Vector2(velocity.x, bounceVelocity);
        }

        if(playerState == PlayerState.bouncing)
        {
            position.y += velocity.y * Time.deltaTime;
            velocity.y -= gravity * Time.deltaTime;
        }
        */
        CheckCeilingAndFloorRays();



        transform.localPosition = position;
        transform.localScale = scale;

        /*
        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;

        if (walk)
        {
            if (walk_left)
            {
                pos.x -= velocity.x * Time.deltaTime;
                scale.x = -1;
                animator.SetBool("isRunning", true);
            }
            if (walk_right)
            {
                pos.x += velocity.x * Time.deltaTime;
                scale.x = 1;
                animator.SetBool("isRunning", true);
            }
            pos = CheckWallRays(pos, scale.x);
        }

        if (!walk && !jump)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }

        if (jump && playerState != PlayerState.jumping)
        {
            playerState = PlayerState.jumping;
            velocity = new Vector2(velocity.x, jumpVelocity);
        }

        if (playerState == PlayerState.jumping)
        {
            //increase currenty y position by y velocity 
            pos.y += velocity.y * Time.deltaTime;
            //as gravity acts on the y-velocity, decrease it
            velocity.y -= gravity * Time.deltaTime;
            animator.SetBool("isJumping", true);
            //once velocity is negative, player falls downwards
        }
        if (playerState != PlayerState.jumping)
        {
            animator.SetBool("isJumping", false);
        }

        if (velocity.y <= 0)
        {
            //falling due to gravity
            pos = CheckFloorRays(pos);
        }

        if(velocity.y >= 0)
        {
            pos = CheckCeilingRays(pos);
        }

        transform.localPosition = pos;
        transform.localScale = scale;
        */
    }

    private void Idle()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
    }

    private void PlayerStateJumping()
    {
        playerState = PlayerState.jumping;
        velocity = new Vector2(velocity.x, jumpVelocity);
    }

    private void JumpingStatus()
    {
        if (playerState == PlayerState.jumping)
        {
            //increase currenty y position by y velocity 
            position.y += velocity.y * Time.deltaTime;
            //as gravity acts on the y-velocity, decrease it
            velocity.y -= gravity * Time.deltaTime;
            animator.SetBool("isJumping", true);
            //once velocity is negative, player falls downwards
        }
        if (playerState != PlayerState.jumping)
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void CheckCeilingAndFloorRays()
    {
        if (velocity.y <= 0)
        {
            //falling due to gravity
            position = CheckFloorRays(position);
        }

        if (velocity.y >= 0)
        {
            position = CheckCeilingRays(position);
        }
    }

    private void Walking()
    {
        if (walk_left)
        {
            position.x -= velocity.x * Time.deltaTime;
            scale.x = -1;
            animator.SetBool("isRunning", true);
        }
        if (walk_right)
        {
            position.x += velocity.x * Time.deltaTime;
            scale.x = 1;
            animator.SetBool("isRunning", true);
        }
        position = CheckWallRays(position, scale.x);
    }

    void CheckPlayerInput()
    {
        bool input_Left = Input.GetKey(KeyCode.LeftArrow);
        bool input_Right = Input.GetKey(KeyCode.RightArrow);
        bool input_Up = Input.GetKeyDown(KeyCode.UpArrow);

        walk = input_Left || input_Right;
        walk_right = input_Right && !(input_Left);
        walk_left = input_Left && !(input_Right);
        jump = input_Up;
    }

    Vector3 CheckWallRays(Vector3 pos, float direction)
    {
        //create 3 rays from player (head, body, feet)
        //check if colliding with anything while walking
        Vector2 originTop = new Vector2(pos.x + direction * .4f, pos.y + 1f - .2f);
        Vector2 originMiddle = new Vector2(pos.x + direction * .4f, pos.y);
        Vector2 originBottom = new Vector2(pos.x + direction * .4f, pos.y - 1f + .2f);

        //use physics2d for raycast (2d). returns
        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);

        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
        {
            //if all three return a collider, we know the ray collided with something
            //no moving in direction of object you collided with
            pos.x -= velocity.x * Time.deltaTime * direction;
        }

        return pos;
    }

    Vector3 CheckFloorRays(Vector3 pos)
    {
        //create origin for rays then create raycasts. left, middle, and right because they'll be casting downwards
        Vector2 originLeft = new Vector2(pos.x - .5f + .2f, pos.y - liftAmount);
        Vector2 originMiddle = new Vector2(pos.x, pos.y - liftAmount);
        Vector2 originRight = new Vector2(pos.x + .5f - .2f, pos.y - liftAmount);

        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null)
        {

            //figure out which of the 3 rays actually had the collision, and where (for pos.y)
            if (floorLeft)
            {
                hitRay = floorLeft;
            }
            else if (floorMiddle)
            {
                hitRay = floorMiddle;
            }
            else if (floorRight)
            {
                hitRay = floorRight;
            }

            if (hitRay.collider.tag == "Enemy")
            {
                //bounce = true;
                hitRay.collider.GetComponent<EnemyAI>().Crush();
            }
            //we have a collision
            playerState = PlayerState.idle; //no longer falling because youhit something
            grounded = true;
            velocity.y = 0;

            pos.y = hitRay.collider.bounds.center.y + liftAmount + hitRay.collider.bounds.size.y / 2; //land atop the object you collided with 
        }
        else
        {
            //if player is not jumping, ensure that they are on a solid surface. ie. implement the fall method
            if (playerState != PlayerState.jumping)
            {
                Fall();
            }
        }

        return pos;
    }

    Vector3 CheckCeilingRays(Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - .5f + .2f, pos.y + liftAmount);
        Vector2 originMiddle = new Vector2(pos.x, pos.y + liftAmount);
        Vector2 originRight = new Vector2(pos.x + .5f - .2f, pos.y + liftAmount);

        RaycastHit2D ceilLeft = Physics2D.Raycast(originLeft, Vector2.up, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D ceilMiddle = Physics2D.Raycast(originMiddle, Vector2.up, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D ceilRight = Physics2D.Raycast(originRight, Vector2.up, velocity.y * Time.deltaTime, floorMask);

        if (ceilLeft.collider != null || ceilMiddle.collider != null || ceilRight.collider != null)
        {
            if (ceilLeft)
            {
                hitRay1 = ceilLeft;
            }
            else if (ceilRight)
            {
                hitRay1 = ceilRight;
            }
            else if (ceilMiddle)
            {
                hitRay1 = ceilMiddle;
            }

            pos.y = hitRay1.collider.bounds.center.y - hitRay1.collider.bounds.size.y / 2 - 1f;
            Fall();
        }

        return pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //fail mission/end level
            LevelManager.playerOut = true;
            LevelManager.gameOver = true;
        }
    }

    void Fall()
    {
        //pull player down by setting y-velocity to 0
        velocity.y = 0;
        //bounce = false;
        playerState = PlayerState.jumping;
        grounded = false;
    }

    //    public Vector2 velocity;
    //    private Vector2 prevVelocity;
    //    public float bounceVelocity;
    //    public bool walk, walk_left, walk_right, jump, attack;
    //    private bool input_Left, input_Right, attacking, input_Up;
    //    public LayerMask wallMask;
    //    public float jumpVelocity;
    //    public float gravity;
    //    public LayerMask floorMask;
    //    private RaycastHit2D hitRay;
    //    private RaycastHit2D hitRay1;
    //    public float liftAmount;
    //    private Animator animator;
    //    private Vector3 position;
    //    private Vector3 scale;
    //    private Rigidbody2D myRigidbody;
    //    //private bool bounce = false;

    //    //use enum to track player state
    //    public enum PlayerState
    //    {
    //        jumping,
    //        idle,
    //        walking,
    //        attacking,
    //    }

    //    private PlayerState playerState = PlayerState.idle;
    //    public static bool grounded = false;

    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        animator = this.GetComponent<Animator>();
    //        myRigidbody = this.GetComponent<Rigidbody2D>();
    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {
    //        CheckPlayerInput();
    //        UpdatePlayerPosition();
    //    }

    //    void UpdatePlayerPosition()
    //    {
    //        position = transform.localPosition;
    //        scale = transform.localScale;

    //        if (walk)
    //        {
    //            Walking();
    //        }

    //        if (!walk && !jump)
    //        {
    //            Idle();
    //        }

    //        if (jump && playerState != PlayerState.jumping)
    //        {
    //            PlayerStateJumping();
    //        }

    //        JumpingStatus();

    //        if (attack && playerState != PlayerState.attacking)
    //        {
    //            playerState = PlayerState.attacking;
    //            //Debug.Log("ATTACK MODE");
    //        }

    //        HandleAttack();

    //        /*
    //        if(bounce && playerState != PlayerState.bouncing)
    //        {
    //            playerState = PlayerState.bouncing;
    //            velocity = new Vector2(velocity.x, bounceVelocity);
    //        }

    //        if(playerState == PlayerState.bouncing)
    //        {
    //            position.y += velocity.y * Time.deltaTime;
    //            velocity.y -= gravity * Time.deltaTime;
    //        }
    //        */
    //        CheckCeilingAndFloorRays();

    //        transform.localPosition = position;
    //        transform.localScale = scale;

    //        /*
    //        Vector3 pos = transform.localPosition;
    //        Vector3 scale = transform.localScale;

    //        if (walk)
    //        {
    //            if (walk_left)
    //            {
    //                pos.x -= velocity.x * Time.deltaTime;
    //                scale.x = -1;
    //                animator.SetBool("isRunning", true);
    //            }
    //            if (walk_right)
    //            {
    //                pos.x += velocity.x * Time.deltaTime;
    //                scale.x = 1;
    //                animator.SetBool("isRunning", true);
    //            }
    //            pos = CheckWallRays(pos, scale.x);
    //        }

    //        if (!walk && !jump)
    //        {
    //            animator.SetBool("isRunning", false);
    //            animator.SetBool("isJumping", false);
    //        }

    //        if (jump && playerState != PlayerState.jumping)
    //        {
    //            playerState = PlayerState.jumping;
    //            velocity = new Vector2(velocity.x, jumpVelocity);
    //        }

    //        if (playerState == PlayerState.jumping)
    //        {
    //            //increase currenty y position by y velocity 
    //            pos.y += velocity.y * Time.deltaTime;
    //            //as gravity acts on the y-velocity, decrease it
    //            velocity.y -= gravity * Time.deltaTime;
    //            animator.SetBool("isJumping", true);
    //            //once velocity is negative, player falls downwards
    //        }
    //        if (playerState != PlayerState.jumping)
    //        {
    //            animator.SetBool("isJumping", false);
    //        }

    //        if (velocity.y <= 0)
    //        {
    //            //falling due to gravity
    //            pos = CheckFloorRays(pos);
    //        }

    //        if(velocity.y >= 0)
    //        {
    //            pos = CheckCeilingRays(pos);
    //        }

    //        transform.localPosition = pos;
    //        transform.localScale = scale;
    //        */
    //    }

    //    private void HandleAttack()
    //    {
    //        if(attack && !this.animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
    //        {
    //            animator.SetTrigger("attack");
    //            prevVelocity = Vector2.zero;
    //;           velocity = prevVelocity;
    //        }
    //    }

    //    private void Idle()
    //    {
    //        animator.SetBool("isRunning", false);
    //        animator.SetBool("isJumping", false);
    //        //animator.SetBool("isAttacking", false);
    //    }

    //    private void PlayerStateJumping()
    //    {
    //        playerState = PlayerState.jumping;
    //        velocity = new Vector2(velocity.x, jumpVelocity);
    //    }

    //    private void JumpingStatus()
    //    {
    //        if (playerState == PlayerState.jumping)
    //        {
    //            //increase currenty y position by y velocity 
    //            position.y += velocity.y * Time.deltaTime;
    //            //as gravity acts on the y-velocity, decrease it
    //            velocity.y -= gravity * Time.deltaTime;
    //            animator.SetBool("isJumping", true);
    //            //once velocity is negative, player falls downwards
    //        }
    //        if (playerState != PlayerState.jumping)
    //        {
    //            animator.SetBool("isJumping", false);
    //        }
    //    }

    //    private void CheckCeilingAndFloorRays()
    //    {
    //        if (velocity.y <= 0)
    //        {
    //            //falling due to gravity
    //            position = CheckFloorRays(position);
    //        }

    //        if (velocity.y >= 0)
    //        {
    //            position = CheckCeilingRays(position);
    //        }
    //    }

    //    private void Walking()
    //    {
    //        if (walk_left)
    //        {
    //            position.x -= velocity.x * Time.deltaTime;
    //            scale.x = -1;
    //            animator.SetBool("isRunning", true);
    //        }
    //        if (walk_right)
    //        {
    //            position.x += velocity.x * Time.deltaTime;
    //            scale.x = 1;
    //            animator.SetBool("isRunning", true);
    //        }
    //        position = CheckWallRays(position, scale.x);
    //    }

    //    void CheckPlayerInput()
    //    {
    //        if(!this.animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
    //        {
    //            velocity = new Vector2(5f, 0f);
    //            attacking = Input.GetKeyDown(KeyCode.A);
    //            input_Left = Input.GetKey(KeyCode.LeftArrow);
    //            input_Right = Input.GetKey(KeyCode.RightArrow);
    //            //input_Up = Input.GetKeyDown(KeyCode.UpArrow);

    //            walk = input_Left || input_Right;
    //            walk_right = input_Right && !(input_Left);
    //            walk_left = input_Left && !(input_Right);
    //            //jump = input_Up;
    //            attack = attacking;
    //        }
    //    }

    //    Vector3 CheckWallRays(Vector3 pos, float direction)
    //    {
    //        //create 3 rays from player (head, body, feet)
    //        //check if colliding with anything while walking
    //        Vector2 originTop = new Vector2(pos.x + direction * .4f, pos.y + 1f - .2f);
    //        Vector2 originMiddle = new Vector2(pos.x + direction * .4f, pos.y);
    //        Vector2 originBottom = new Vector2(pos.x + direction * .4f, pos.y - 1f + .2f);

    //        //use physics2d for raycast (2d). returns
    //        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
    //        RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
    //        RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);

    //        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
    //        {
    //            //if all three return a collider, we know the ray collided with something
    //            //no moving in direction of object you collided with
    //            pos.x -= velocity.x * Time.deltaTime * direction;

    //            RaycastHit2D hitRay1 = wallTop;
    //            if (wallTop)
    //            {
    //                hitRay1 = wallTop;
    //            }
    //            else if (wallMiddle)
    //            {
    //                hitRay1 = wallMiddle;
    //            }
    //            else if (wallBottom)
    //            {
    //                hitRay1 = wallBottom;
    //            }
    //            if (hitRay.collider.tag == "Enemy")
    //            {
    //                //bounce = true;
    //                Debug.Log("ENEMY HIT");
    //            }
    //        }

    //        return pos;
    //    }

    //    Vector3 CheckFloorRays(Vector3 pos)
    //    {
    //        //create origin for rays then create raycasts. left, middle, and right because they'll be casting downwards
    //        Vector2 originLeft = new Vector2(pos.x - .5f + .2f, pos.y - liftAmount);
    //        Vector2 originMiddle = new Vector2(pos.x, pos.y - liftAmount);
    //        Vector2 originRight = new Vector2(pos.x + .5f - .2f, pos.y - liftAmount);

    //        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
    //        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
    //        RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

    //        if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null)
    //        {

    //            //figure out which of the 3 rays actually had the collision, and where (for pos.y)
    //            if (floorLeft)
    //            {
    //                hitRay = floorLeft;
    //            }
    //            else if (floorMiddle)
    //            {
    //                hitRay = floorMiddle;
    //            }
    //            else if (floorRight)
    //            {
    //                hitRay = floorRight;
    //            }

    //            if (hitRay.collider.tag == "Enemy")
    //            {
    //                //bounce = true;
    //                hitRay.collider.GetComponent<EnemyAI>().Crush();
    //            }
    //            //we have a collision
    //            playerState = PlayerState.idle; //no longer falling because youhit something
    //            grounded = true;
    //            velocity.y = 0;

    //            pos.y = hitRay.collider.bounds.center.y + liftAmount + hitRay.collider.bounds.size.y / 2; //land atop the object you collided with 
    //        }
    //        else
    //        {
    //            //if player is not jumping, ensure that they are on a solid surface. ie. implement the fall method
    //            if (playerState != PlayerState.jumping)
    //            {
    //                Fall();
    //            }
    //        }

    //        return pos;
    //    }

    //    Vector3 CheckCeilingRays(Vector3 pos)
    //    {
    //        Vector2 originLeft = new Vector2(pos.x - .5f + .2f, pos.y + liftAmount);
    //        Vector2 originMiddle = new Vector2(pos.x, pos.y + liftAmount);
    //        Vector2 originRight = new Vector2(pos.x + .5f - .2f, pos.y + liftAmount);

    //        RaycastHit2D ceilLeft = Physics2D.Raycast(originLeft, Vector2.up, velocity.y * Time.deltaTime, floorMask);
    //        RaycastHit2D ceilMiddle = Physics2D.Raycast(originMiddle, Vector2.up, velocity.y * Time.deltaTime, floorMask);
    //        RaycastHit2D ceilRight = Physics2D.Raycast(originRight, Vector2.up, velocity.y * Time.deltaTime, floorMask);

    //        if (ceilLeft.collider != null || ceilMiddle.collider != null || ceilRight.collider != null)
    //        {
    //            if (ceilLeft)
    //            {
    //                hitRay1 = ceilLeft;
    //            }
    //            else if (ceilRight)
    //            {
    //                hitRay1 = ceilRight;
    //            }
    //            else if (ceilMiddle)
    //            {
    //                hitRay1 = ceilMiddle;
    //            }

    //            pos.y = hitRay1.collider.bounds.center.y - hitRay1.collider.bounds.size.y / 2 - 1f;
    //            Fall();
    //        }

    //        return pos;
    //    }

    //    void Fall()
    //    {
    //        //pull player down by setting y-velocity to 0
    //        velocity.y = 0;
    //        //bounce = false;
    //        playerState = PlayerState.jumping;
    //        grounded = false;
    //    }
}
