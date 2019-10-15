using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActCharacter : MonoBehaviour {

    public ActCtrl mActCtrl;
    public Animator SpriteAnimator;
    public Rigidbody2D rigidbody2D;

    public bool isJump;
    public bool isFalling;

    // Use this for initialization
    void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
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
	// Update is called once per frame
	void Update () {


        float vy = 0;
        float vx = 0;
        if (!isJump || isFalling)
        {
            if (Input.GetKey(KeyCode.A))
            {
                vx += -3;
            }
            if (Input.GetKey(KeyCode.D))
            {
                vx += 3;
            }
        }
        else if(isJump)
        {
            vx = prevx;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump)
            {
                vy += 5;
                isJump = true;
            }
        }
        if (rigidbody2D.velocity.y < 0)
        {
            isFalling = true;
        }

        if (isFalling)
        {
            if (rigidbody2D.velocity.y == 0)
            {
                isJump = false;
                isFalling = false;
            }
        }



        if (mActCtrl.NowActionId==string.Empty
             || mActCtrl.canInterrupt)
        {
            if(!isJump && vx != 0 && vy == 0)
            {
                SpriteAnimator.SetBool("walk",true);
                mActCtrl.boxAnimator.SetBool("walk", true);
            }
            else if(vx == 0)
            {
                SpriteAnimator.SetBool("walk", false);
                mActCtrl.boxAnimator.SetBool("walk", false);
            }
            else
            {
                SpriteAnimator.SetBool("walk", false);
                mActCtrl.boxAnimator.SetBool("walk", false);
            }
            rigidbody2D.velocity = new Vector2(vx, vy + rigidbody2D.velocity.y);
        }
        else
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }

        prevx = vx;


    }
}
