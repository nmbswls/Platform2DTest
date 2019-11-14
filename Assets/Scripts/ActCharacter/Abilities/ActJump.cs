using UnityEngine;
using System.Collections;

public class ActJump : ActAbility
{
    public bool JumpHappenedThisFrame { get; set; }


    //一般来说 都是可以跳的
    public enum JumpBehavior
    {
        //CanJumpOnGround,
        //CanJumpOnGroundAndFromLadders,
        CanJumpAnywhere,
        CantJump,
        //CanJumpAnywhereAnyNumberOfTimes
    }

    public bool DownJump;


    public JumpBehavior JumpRestrictions = JumpBehavior.CanJumpAnywhere;


    public float JumpHeight = 2.025f;
    public int NumberOfJumps = 2;


    public int NumberOfJumpsLeft;

    public bool CanJumpStop { get; set; }

    /// a timeframe during which, after leaving the ground, the character can still trigger a jump
    /// 掉下悬崖可以补救回来
    public float JumpTimeWindow = 0f;
    protected float _lastTimeGrounded = 0f;


    protected bool _doubleJumping = false;
    protected float _jumpButtonPressTime = 0;
    protected bool _jumpButtonPressed = false;
    protected bool _jumpButtonReleased = false;

    //复活时初始化
    protected int _initialNumberOfJumps;

    //处理特殊边界条件 会引起问题 比如判断失败
    //不好意思 误会你了
    public float MovingPlatformsJumpCollisionOffDuration = 0.05f;
    public float OneWayPlatformsJumpCollisionOffDuration = 0.3f;


    public override void Initialization(ActCharacterNew character)
    {
        base.Initialization(character);
        ResetNumberOfJumps();
        _initialNumberOfJumps = NumberOfJumps;
        CanJumpStop = true;
    }



    public bool JumpAuthorized
    {
        get
        {
            if (EvaluateJumpTimeWindow())
            {
                return true;
            }

            if (_movement.CurrentState == CharacterStates.MovementStates.SwimmingIdle)
            {
                return false;
            }

            if ((JumpRestrictions == JumpBehavior.CanJumpAnywhere))
            {
                return true;
            }
            return false;
        }
    }

    protected override void HandleInput()
    {
        if(_character.controller == null)
        {
            return;
        }
        DownJump = false;
        if (_character.controller.WantJump && _verticalInput < -0.1f)
        {
            DownJump = true;
        }

        if (_character.controller.WantJump)
        {
            JumpStart();
        }

    }

    public virtual void JumpStart()
    {
        if (!EvaluateJumpConditions())
        {
            return;
        }
        // we reset our walking speed
        if ((_movement.CurrentState == CharacterStates.MovementStates.Crawling)
            || (_movement.CurrentState == CharacterStates.MovementStates.Crouching)
            || (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing))
        {
            _character.ResetHorizontalSpeed();
        }

        if (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing)
        {
            _character.GetOffTheLadder();
        }

        _platformer.ResetColliderSize();

        // if we're still here, the jump will happen
        // we set our current state to Jumping
        _movement.ChangeState(CharacterStates.MovementStates.Jumping);


        if (NumberOfJumpsLeft != NumberOfJumps)
        {
            _doubleJumping = true;
        }

        // we decrease the number of jumps left
        NumberOfJumpsLeft = NumberOfJumpsLeft - 1;

        // we reset our current condition and gravity
        _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
        _platformer.GravityActive(true);
        _platformer.CollisionsOn();

        // we set our various jump flags and counters
        SetJumpFlags();
        CanJumpStop = true;

        // we make the character jump
        _platformer.SetVerticalForce(Mathf.Sqrt(2f * JumpHeight * Mathf.Abs(_platformer.Parameters.Gravity)));
        JumpHappenedThisFrame = true;

    }

    public virtual void SetJumpFlags()
    {
        _jumpButtonPressTime = Time.time;
        _jumpButtonPressed = true;
        _jumpButtonReleased = false;
    }



    protected virtual bool EvaluateJumpConditions()
    {
        bool onAOneWayPlatform = (_platformer.OneWayPlatformMask.Contains(_platformer.StandingOn.layer)
            || _platformer.MovingOneWayPlatformMask.Contains(_platformer.StandingOn.layer));


        if (!AbilityPermitted  // if the ability is not permitted
            || !JumpAuthorized // if jumps are not authorized right now
            || (!_platformer.CanGoBackToOriginalSize() && !onAOneWayPlatform)
            || ((_condition.CurrentState != CharacterStates.CharacterConditions.Normal) // or if we're not in the normal stance
                && (_condition.CurrentState != CharacterStates.CharacterConditions.ControlledMovement))
            || (_movement.CurrentState == CharacterStates.MovementStates.Jetpacking) // or if we're jetpacking
            || (_movement.CurrentState == CharacterStates.MovementStates.Dashing) // or if we're dashing
            || (_movement.CurrentState == CharacterStates.MovementStates.Pushing) // or if we're pushing                
            || ((_movement.CurrentState == CharacterStates.MovementStates.WallClinging) /*&& (_characterWallJump != null)*/) // or if we're wallclinging and can walljump
            || (_platformer.NormalState.IsCollidingAbove && !onAOneWayPlatform)) // or if we're colliding with the ceiling
        {
            return false;
        }

        // if we're not grounded, not on a ladder, and don't have any jumps left, we do nothing and exit
        if ((!_platformer.NormalState.IsGrounded)
            && !EvaluateJumpTimeWindow()
            && (_movement.CurrentState != CharacterStates.MovementStates.LadderClimbing)
            && (NumberOfJumpsLeft <= 0))
        {
            return false;
        }

        if (_platformer.NormalState.IsGrounded
            && (NumberOfJumpsLeft <= 0))
        {
            return false;
        }



        // if the character is standing on a one way platform and is also pressing the down button,
        if (DownJump && _platformer.NormalState.IsGrounded)
        {
            if (JumpDownFromOneWayPlatform())
{
                return false;
            }
        }

        // if the character is standing on a moving platform and not pressing the down button,
        if (!DownJump && _platformer.NormalState.IsGrounded)
        {
            JumpFromMovingPlatform();
        }

        return true;
    }

    protected virtual bool JumpDownFromOneWayPlatform()
    {
        if (!_platformer.StandingOn.GetComponent<MapFootHold>().CanJumpDown)
        {
            return true;
        }
        if ((_platformer.OneWayPlatformMask.Contains(_platformer.StandingOn.layer)
            || _platformer.MovingOneWayPlatformMask.Contains(_platformer.StandingOn.layer)))
        {
            // we make it fall down below the platform by moving it just below the platform
            //_platformer.StandingOn.BelowDistance();
            _platformer.transform.position = new Vector2(_platformer.transform.position.x, _platformer.transform.position.y /*- 0.03f*/);
            // we turn the boxcollider off for a few milliseconds, so the character doesn't get stuck mid platform
            _platformer.TryDisableCollisionsWithOneWayPlatforms(OneWayPlatformsJumpCollisionOffDuration);
            _platformer.DetachFromMovingPlatform();
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void JumpFromMovingPlatform()
    {
        if (_platformer.MovingPlatformMask.Contains(_platformer.StandingOn.layer)
            || _platformer.MovingOneWayPlatformMask.Contains(_platformer.StandingOn.layer))
        {
            // we turn the boxcollider off for a few milliseconds, so the character doesn't get stuck mid air
            _platformer.TryDisableCollisionsWithMovingPlatforms(MovingPlatformsJumpCollisionOffDuration);
            _platformer.DetachFromMovingPlatform();
        }
    }

    public override void Tick(float dTime)
    {
        base.Tick(dTime);
        JumpHappenedThisFrame = false;

        if (!AbilityPermitted) { return; }

        // if we just got grounded, we reset our number of jumps
        if (_platformer.NormalState.JustGotGrounded)
        {
            Debug.Log("reset");
            NumberOfJumpsLeft = NumberOfJumps;
            _doubleJumping = false;
        }

        // we store the last timestamp at which the character was grounded
        if (_platformer.NormalState.IsGrounded)
        {
            _lastTimeGrounded = Time.time;
        }


        UpdateController();
    }

    protected virtual void UpdateController()
    {
        _platformer.NormalState.IsJumping = (_movement.CurrentState == CharacterStates.MovementStates.Jumping
                || _movement.CurrentState == CharacterStates.MovementStates.DoubleJumping
                || _movement.CurrentState == CharacterStates.MovementStates.WallJumping);
    }


    protected virtual bool EvaluateJumpTimeWindow()
    {
        if (_movement.CurrentState == CharacterStates.MovementStates.Jumping
            || _movement.CurrentState == CharacterStates.MovementStates.DoubleJumping
            || _movement.CurrentState == CharacterStates.MovementStates.WallJumping)
        {
            return false;
        }

        if (Time.time - _lastTimeGrounded <= JumpTimeWindow)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Reset()
    {
        base.Reset();
        NumberOfJumps = _initialNumberOfJumps;
        NumberOfJumpsLeft = _initialNumberOfJumps;
    }
    public virtual void ResetNumberOfJumps()
    {
        NumberOfJumpsLeft = NumberOfJumps;
    }

    public virtual void SetNumberOfJumpsLeft(int newNumberOfJumps)
    {
        NumberOfJumpsLeft = newNumberOfJumps;
    }
}
