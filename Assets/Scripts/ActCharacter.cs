using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActCharacter : MonoBehaviour {

    public LayerMask climbLayer;
    public LayerMask wallLay;

    public ActCtrl mActCtrl;
    public ActInput InputMdl;
    public Animator SpriteAnimator;

    public float moveBoxHeight;

    CapsuleCollider2D moveCollider;

    public FootDetector footDetector;

    Rigidbody2D rb2d;

    public PhysicsMaterial2D staticMat;

    public bool isClimbing; 




    public static float BaseMoveSpd = 5f;
    public static float BaseJumpVolecity = 6f;
    public static float epi = 1e-1f;
    public bool isJump;
    public bool isFalling;
    public bool isOnGround;

    public float footHeight = 0.1f;

    public List<MapReactor> ActivateReactors = new List<MapReactor>();

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        InputMdl = new ActInput();


        moveCollider = GetComponentInChildren<CapsuleCollider2D>();
        moveBoxHeight = moveCollider.size.y * moveCollider.transform.localScale.y;


    }
    float preVx;
    float preVy;

    public void LockMovement()
    {

    }

    public void ResumeMovement()
    {

    }

    public float prevx = 0;
    public float prevy = 0;

    private Collider2D GetClimbingColliderBelow()
    {
        float dist = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(moveCollider.transform.position, Vector2.down, moveBoxHeight / 2, climbLayer);
        if (hit.collider != null)
        {
            return hit.collider;
        }
        return null;
    }






    public void StartClimb(Collider2D c)
    {
        transform.position = new Vector3(c.transform.position.x, transform.position.y, transform.position.z);

        SpriteAnimator.SetTrigger("Climb");
        mActCtrl.boxAnimator.SetTrigger("Climb");


        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0;
        isClimbing = true;
    }

    public void FinishClimb()
    {

        SpriteAnimator.SetTrigger("Idle");
        mActCtrl.boxAnimator.SetTrigger("Idle");

        rb2d.gravityScale = 1.5f;
        isClimbing = false;
    }

    private Collider2D GetClimbingColliderAbove()
    {
        float dist = 0.1f;

        RaycastHit2D hit = Physics2D.Raycast(moveCollider.transform.position, Vector2.up, moveBoxHeight / 2, climbLayer);
        if (hit.collider != null)
        {
            return hit.collider;
        }
        return null;
    }

    public void LockPosition(bool isLock)
    {
        if (isLock)
        {
            rb2d.sharedMaterial = staticMat;
        }
        else
        {
            rb2d.sharedMaterial = null;
        }
    }

    // Update is called once per frame
    void Update () {

        InputMdl.Tick(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (ActivateReactors.Count > 0)
            {
                string actionString = ActivateReactors[0].actionString;
                if (actionString == "map")
                {
                    GameMain.GetInstance().SwitchScene("map01");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowChain();
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (mActCtrl.NowActionId == string.Empty
             || mActCtrl.canInterrupt)
            {
                Collider2D c = GetClimbingColliderAbove();
                if (c != null)
                {
                    StartClimb(c);
                }
            }
        }



        if (isClimbing)
        {
            Collider2D climbingColliderBelow = GetClimbingColliderBelow();
            Collider2D climbingColliderAbove = GetClimbingColliderAbove();
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.up * Time.deltaTime * 1f;

            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * Time.deltaTime * 1f;

            }
            if (isOnGround || (climbingColliderBelow == null && climbingColliderAbove == null))
            {
                FinishClimb();
            }
            if (InputMdl.Jump)
            {
                FinishClimb();
                rb2d.velocity = new Vector2(0,3f);
            }
            return;
        }


        float jumpVolecity = 0;
        float vx = InputMdl.DLateral * BaseMoveSpd;

        if(vx > 0.5f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }else if (vx < -0.5f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


        //isOnGround = footDetector.BelowGrounded && (rb2d.velocity.y <= epi && rb2d.velocity.y >= -epi);


        if (isJump)
        {
            if (footDetector.BelowGrounded && rb2d.velocity.y <= 0)
            {
                isJump = false;
                isFalling = false;
            }
        }

        isOnGround = footDetector.BelowGrounded && !isJump;

        //斜坡过大 可能还会有傻逼问题
        if (isOnGround && Mathf.Abs(vx)<0.5f)
        {
            LockPosition(true);
        }
        else
        {
            LockPosition(false);
        }


        SpriteAnimator.SetBool("IsGround", isOnGround);
        mActCtrl.boxAnimator.SetBool("IsGround", isOnGround);





        if (mActCtrl.NowActionId == string.Empty
             || mActCtrl.canInterrupt)
        {

            //没有攻击动作
            if (InputMdl.Jump)
            {
                if (!isJump)
                {
                    jumpVolecity += BaseJumpVolecity;
                    isJump = true;
                    SpriteAnimator.SetTrigger("Jump");
                    mActCtrl.boxAnimator.SetTrigger("Jump");
                }
            }
            rb2d.velocity = new Vector2(vx, jumpVolecity + rb2d.velocity.y);
            if(isJump|| Mathf.Abs(vx)<1f)
            {
                SpriteAnimator.SetFloat("GroundSpeed", Mathf.Abs(vx));
                mActCtrl.boxAnimator.SetFloat("GroundSpeed", Mathf.Abs(vx));
            }
            else
            {
                SpriteAnimator.SetFloat("GroundSpeed", Mathf.Abs(vx));
                mActCtrl.boxAnimator.SetFloat("GroundSpeed", Mathf.Abs(vx));
            }

        }
        else
        {
            if (isJump)
            {
                rb2d.velocity = new Vector2(vx, rb2d.velocity.y);
            }
            else
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }

        }
        //if(rb2d.velocity.y<)

        prevx = vx;


    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Reactor")
        {
            MapReactor reactor = collision.GetComponent<MapReactor>();
            if (!ActivateReactors.Contains(reactor))
            {
                ActivateReactors.Add(reactor);
                GameMain.GetInstance().gameMode.HudRoot.ShowReactHud(reactor);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Reactor")
        {
            MapReactor reactor = collision.GetComponent<MapReactor>();
            if (ActivateReactors.Contains(reactor))
            {
                int idx = ActivateReactors.IndexOf(reactor);
                ActivateReactors.Remove(reactor);
                GameMain.GetInstance().gameMode.HudRoot.HideReactHud(idx);
            }
        }
    }



    public void ThrowChain()
    {
        GameObject prefab = Resources.Load("Chain") as GameObject;

        GameObject chainGO = GameObject.Instantiate(prefab,transform.position,Quaternion.identity,transform.parent);
        ActChain chain = chainGO.GetComponent<ActChain>();
        chain.Init(Vector2.right);

    }
}
