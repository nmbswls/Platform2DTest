using System.Collections;
using System.Collections.Generic;
using UnityEngine;







public class ActCharacter : MapObject,IHittable {

    [HideInInspector]
    public LayerMask climbLayer;
    [HideInInspector]
    public LayerMask wallLay;

    [HideInInspector]
    public ActCtrl mActCtrl;

    [HideInInspector]
    public FootDetector mFootDetector;

    public Controller controller;

    public ActStateCtrl animCtrl;


    [HideInInspector]
    public HitBox hitBox { get { return mHitBox; } }
    private HitBox mHitBox;

    float moveBoxHeight;

    CapsuleCollider2D moveCollider;

    public int Hp;



    public Rigidbody2D rb2d;

    public PhysicsMaterial2D staticMat;



    public eActState state;

    public static float BaseMoveSpd = 4f;
    public static float RollSpd = 18f;
    public static float BaseJumpVolecity = 6f;
    public static float epi = 1e-1f;

    public bool isClimbing;

    public bool isJump;

    public bool isFalling;
    public bool isOnGround;

    public bool isRolling;
    public bool isKnockingBack;

    public Vector2 LockedSpd;

    public int RollingLastFrame = 6;
    public int KnockingBackFrame = 6;

    public float footHeight = 0.1f;


    void Awake()
    {
        contactFilter.layerMask = LayerMask.GetMask("Wall");
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = false;
    }

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();



        mHitBox = GetComponentInChildren<HitBox>();
        mHitBox.Init(this);

        animCtrl = new ActStateCtrl();
        animCtrl.LogicAnimator = transform.Find("AtkBoxes").GetComponent<Animator>();
        animCtrl.ViewAnimator = transform.Find("Sprite").GetComponent<Animator>();

        mActCtrl = transform.Find("AtkBoxes").GetComponent<ActCtrl>();
        mActCtrl.Init(controller,animCtrl);

        moveCollider = transform.Find("MoveBox").GetComponent<CapsuleCollider2D>();
        moveBoxHeight = moveCollider.size.y * moveCollider.transform.localScale.y;

        mFootDetector = transform.Find("MoveBox").GetChild(0).GetComponent<FootDetector>();

        climbLayer = LayerMask.GetMask("Climbable");
        wallLay = LayerMask.GetMask("Wall");
    }


    public void GetHit()
    {
        Hp -= 2;
    }


    public void LockMovement()
    {

    }

    public void ResumeMovement()
    {

    }

    public Collider2D GetClimbingColliderBelow()
    {
        float dist = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(moveCollider.transform.position, Vector2.down, moveBoxHeight / 2, climbLayer);
        if (hit.collider != null)
        {
            return hit.collider;
        }
        return null;
    }

    public Collider2D GetClimbingColliderAbove()
    {
        float dist = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(moveCollider.transform.position, Vector2.up, moveBoxHeight / 2, climbLayer);
        if (hit.collider != null)
        {
            return hit.collider;
        }
        return null;
    }


    public void StartClimb(Collider2D c)
    {
        transform.position = new Vector3(c.transform.position.x, transform.position.y, transform.position.z);
        animCtrl.ViewAnimator.SetTrigger("Climb");
        animCtrl.LogicAnimator.SetTrigger("Climb");


        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0;
        isRolling = false;
        isFalling = false;
        isJump = false;
        isClimbing = true;
    }

    public void FinishClimb(bool jumpOut)
    {

        animCtrl.ViewAnimator.SetTrigger("Ground");
        animCtrl.LogicAnimator.SetTrigger("Ground");

        rb2d.gravityScale = 1.5f;
        if (jumpOut)
        {
            isJump = true;
        }
        isClimbing = false;
    }



    public void LockPosition(bool isLock)
    {
        if (isLock)
        {
            rb2d.sharedMaterial = staticMat;
            rb2d.gravityScale = 0;
        }
        else
        {
            rb2d.sharedMaterial = null;
            rb2d.gravityScale = 1.5f;
        }
    }

    public void KnockBack()
    {
        if (!isKnockingBack)
        {
            isKnockingBack = true;
            KnockingBackFrame = 6;
            rb2d.velocity += Vector2.up * 2f;
        }
        else
        {
            KnockingBackFrame = 6;
            rb2d.velocity += Vector2.up * 2f;
        }

    }

    public void ClimbMove(float DVerticle)
    {
        if (!isClimbing)
        {
            return;
        }
        transform.position += Vector3.up * DVerticle * Time.deltaTime * 2f;
    }

    float hitFrame = 0;
    // Update is called once per frame
    void Update () {

        if(hitFrame > 0)
        {
            hitFrame -= 1;
            if (hitFrame <= 0)
            {
                animCtrl.RemoveLayer(2);
            }
        }

        if (isClimbing)
        {
            Collider2D climbingColliderBelow = GetClimbingColliderBelow();
            Collider2D climbingColliderAbove = GetClimbingColliderAbove();

            if (isOnGround || (climbingColliderBelow == null && climbingColliderAbove == null))
            {
                FinishClimb(false);
            }
        }
        else
        {



            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    KnockBack();
            //}

            //if (isKnockingBack)
            //{
            //    rb2d.velocity = new Vector2(-15f, rb2d.velocity.y);
            //    KnockingBackFrame -= 1;
            //    if (KnockingBackFrame <= 0)
            //    {
            //        isKnockingBack = false;
            //    }
            //}




            //if (controller.Roll && AllowRoll())
            //{
            //    if (controller.DLateral > 0.5f)
            //    {
            //        isRolling = true;
            //        RollingLastFrame = 6;
            //        LockedSpd = Vector2.right * RollSpd;
            //        animCtrl.SetTrigger("Roll");
            //    }
            //    else if (controller.DLateral < -0.5f)
            //    {
            //        RollingLastFrame = 6;
            //        isRolling = true;
            //        LockedSpd = Vector2.left * RollSpd;
            //        animCtrl.SetTrigger("Roll");
            //    }
            //}

            //if (isRolling)
            //{
            //    rb2d.velocity = LockedSpd;
            //    RollingLastFrame -= 1;
            //    if (RollingLastFrame <= 0)
            //    {
            //        animCtrl.SetTrigger("Ground");
            //        isRolling = false;
            //    }
            //}


            //核心判断


            //判断是否在空中
            if (isJump)
            {
                if (!isFalling && rb2d.velocity.y <= 0)
                {
                    isFalling = true;
                    animCtrl.SetTrigger("Fall");
                }
            }
            if (isFalling)
            {
                if (isGroundedNew)
                {
                    isJump = false;
                    isFalling = false;
                    animCtrl.SetTrigger("Ground");
                }
            }


            if (!isJump && !isFalling && !isGroundedNew)
            {
                isFalling = true;
                animCtrl.SetTrigger("Fall");
            }

            isOnGround = isGroundedNew && !isJump && !isFalling;


            Vector2 extraV = Vector2.zero;
            if (mFootDetector.HoldBelow != null && (mFootDetector.HoldBelow as MovingPlatform) != null)
            {
                //extraV = (mFootDetector.HoldBelow as MovingPlatform).NowVVector;
            }

            float vx = 1 * BaseMoveSpd;
            TurnDirection(vx);
            //斜坡过大 可能还会有傻逼问题
            if (isOnGround && Mathf.Abs(vx) < 0.5f)
            {
                LockPosition(true);
            }
            else
            {
                LockPosition(false);
            }

            //if (isOnGround)
            //{
            //    GroundedVerticalMovement();
            //}
            //else
            //{
            //    AirborneVerticalMovement();
            //}







            if (isJump || Mathf.Abs(vx) < 1f)
            {
                animCtrl.LogicAnimator.SetFloat("GroundSpeed", Mathf.Abs(vx));
                animCtrl.ViewAnimator.SetFloat("GroundSpeed", Mathf.Abs(vx));
            }
            else
            {
                animCtrl.LogicAnimator.SetFloat("GroundSpeed", Mathf.Abs(vx));
                animCtrl.ViewAnimator.SetFloat("GroundSpeed", Mathf.Abs(vx));
            }

            if (AllowMove())
            {
                //NextMovement += Vector2.right * vx * Time.deltaTime;
                rb2d.velocity = new Vector2(vx,rb2d.velocity.y);
                //SetHorizontalMovement(vx); 
                //Vector3 move = Vector2.right * vx * Time.deltaTime;
                //transform.position += move;
            }

            if (isAttacking())
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

            if (AllowUseSkill())
            {
                mActCtrl.CheckInput();
            }

            if (isOnGround)
            {
                rb2d.position += extraV * Time.deltaTime;
            }
        }



    }


    public void Jump()
    {

        if (isClimbing)
        {
            FinishClimb(true);
            rb2d.velocity = new Vector2(0, 3f);
        }
        else
        {
            float jumpVolecity = BaseJumpVolecity;
            isJump = true;
            animCtrl.SetTrigger("Jump");
            rb2d.velocity += new Vector2(0, jumpVolecity);
        }

    }

    private void TurnDirection(float vx)
    {
        if (!mActCtrl.CanChangeDir())
        {
            return;
        }
        if (vx > 0.5f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (vx < -0.5f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    AnimatorOverrideController overrideController;






    public void ThrowChain()
    {
        GameObject prefab = Resources.Load("Chain") as GameObject;

        GameObject chainGO = GameObject.Instantiate(prefab,transform.position,Quaternion.identity,transform.parent);
        ActChain chain = chainGO.GetComponent<ActChain>();
        chain.Init(Vector2.right);

    }



    public bool AllowUseSkill()
    {
        return !isClimbing && ! isRolling;
    }

    public bool AllowClimb()
    {
        return !isRolling && !isClimbing && (mActCtrl.NowActionId == string.Empty
             || mActCtrl.canInterrupt);
    }
    public bool AllowRoll()
    {
        return !isJump && !isRolling && !isClimbing && (mActCtrl.NowActionId == string.Empty
             || mActCtrl.canInterrupt);
    }
    public bool AllowJump()
    {
        return !isKnockingBack &&!isJump && !isRolling && (mActCtrl.NowActionId == string.Empty
             || mActCtrl.canInterrupt);
    }

    private bool AllowMove()
    {
        return !isClimbing && !isKnockingBack&&!isRolling && !isClimbing && (mActCtrl.NowActionId == string.Empty);
    }

    public bool isAttacking()
    {
        return mActCtrl.NowActionId != string.Empty;
    }



    Vector2[] m_RaycastPositions = new Vector2[3];
    ContactFilter2D contactFilter;
    RaycastHit2D[] GroundHitBuffer = new RaycastHit2D[5];
    RaycastHit2D[] FoundHit = new RaycastHit2D[3];
    public bool isGroundedNew;

    public Vector2 Velocity;
    public Vector2 PrevPos;
    public Vector2 CurPos;

    Vector2 NextMovement;
    public float gravity = 10f;
    public float k_GroundedStickingVelocityMultiplier = 0;

    public Vector2 m_MoveVector;


    public void SetHorizontalMovement(float newHorizontalMovement)
    {
        m_MoveVector.x = newHorizontalMovement;
    }

    public void SetVerticalMovement(float newVerticalMovement)
    {
        m_MoveVector.y = newVerticalMovement;
    }
    public void IncrementVerticalMovement(float additionalVerticalMovement)
    {
        m_MoveVector.y += additionalVerticalMovement;
    }
    public void GroundedVerticalMovement()
    {
        m_MoveVector.y -= gravity * Time.deltaTime;

        if (m_MoveVector.y < -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier)
        {
            m_MoveVector.y = -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier;
        }
    }

    public void AirborneVerticalMovement()
    {
        if (Mathf.Approximately(m_MoveVector.y, 0f))
        {
            m_MoveVector.y = 0f;
        }
        m_MoveVector.y -= gravity * Time.deltaTime;
    }

    Vector2 groundNormal;
    public void FixedUpdate()
    {

        //NextMovement = m_MoveVector * Time.deltaTime;

        //PrevPos = rb2d.position;
        //CurPos = PrevPos + NextMovement;


        //Velocity = (CurPos - PrevPos) / Time.deltaTime;
        //rb2d.MovePosition(CurPos);
        //NextMovement = Vector2.zero;
        CheckCapsuleEndCollisions();
        if (isGroundedNew && !isJump && !isFalling)
        {
            rb2d.velocity -= groundNormal * Vector2.Dot(rb2d.velocity, groundNormal);
            Debug.Log(rb2d.velocity);
        }

    }





    public void CheckCapsuleEndCollisions(bool bottom = true)
    {
        Vector2 raycastDirection;
        Vector2 raycastStart;

        float raycastDistance;
        {
            raycastStart = rb2d.position + moveCollider.offset;
            raycastDistance = moveCollider.size.x * 0.05f + 0.2f;

            raycastDirection = Vector2.down;
            Vector2 raycastStartBottomCenter = raycastStart + Vector2.down * (moveCollider.size.y * 0.5f - moveCollider.size.x * 0.5f + moveCollider.size.x * 0.45f);
            m_RaycastPositions[0] = raycastStartBottomCenter + Vector2.left * moveCollider.size.x * 0.5f;
            m_RaycastPositions[1] = raycastStartBottomCenter ;
            m_RaycastPositions[2] = raycastStartBottomCenter + Vector2.right * moveCollider.size.x * 0.5f;

            for(int i=1; i < m_RaycastPositions.Length-1; i++)
            {
                int count = Physics2D.Raycast(m_RaycastPositions[i], raycastDirection, contactFilter, GroundHitBuffer, raycastDistance);
                FoundHit[i] = count>0? GroundHitBuffer[0]:new RaycastHit2D();
            }
            groundNormal = Vector2.zero;
            int hitCount = 0;

            for(int i=0;i< FoundHit.Length; i++)
            {
                if(FoundHit[i].collider != null)
                {
                    groundNormal += FoundHit[i].normal;
                    hitCount++;
                }
            }
            if(hitCount > 0)
            {
                groundNormal.Normalize();
            }
            //移动平台
            //Vector2 relativeV = 
            if(Mathf.Approximately(groundNormal.x,0f) && Mathf.Approximately(groundNormal.y, 0f))
            {
                isGroundedNew = false;
            }
            else
            {
                isGroundedNew = Velocity.y <= 0f;
                if(FoundHit[1].collider != null)
                {
                    float capsuleBottomHeight = rb2d.position.y + moveCollider.offset.y - moveCollider.size.y * 0.5f;
                    float middleHitHeight = FoundHit[1].point.y;

                    isGroundedNew &= middleHitHeight < capsuleBottomHeight + raycastDistance;
                }

            }

            for (int i=0;i< GroundHitBuffer.Length; i++)
            {
                GroundHitBuffer[i] = new RaycastHit2D();
            }
        }
    }

}
