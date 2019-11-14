using UnityEngine;
using System.Collections;

public class ActClimbing : ActAbility
{
    /// the speed of the character when climbing a ladder
    public float LadderClimbingSpeed = 2f;
    /// the current ladder climbing speed of the character
    public Vector2 CurrentLadderClimbingSpeed { get; set; }
    /// true if the character is colliding with a ladder
    public bool LadderColliding { get; set; }
    /// the ladder the character is currently on
    public Climbable CurrentLadder { get; set; }
    /// force face right when on a ladder (useful for 3D characters)
    public bool ForceRightFacing = false;

    const float _climbingDownInitialYTranslation = 0.1f;
    const float _ladderTopSkinHeight = 0.01f;

    //输入
    public bool MoveUp;

    public override void Initialization(ActCharacterNew character)
    {
        base.Initialization(character);

        CurrentLadderClimbingSpeed = Vector2.zero;
        LadderColliding = false;
        CurrentLadder = null;
    }

    public override void Tick(float dTime)
    {
        base.Tick(dTime);
        HandleLadderClimbing();
    }

    protected override void HandleInput()
    {

        if(_character.controller == null)
        {
            return;
        }

        if(_character.controller.MoveV > 0.1f)
        {
            MoveUp = true;
        }
        else
        {
            MoveUp = false;
        }
    }

    protected virtual void HandleLadderClimbing()
    {
        if (!AbilityPermitted
            || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal && _condition.CurrentState != CharacterStates.CharacterConditions.ControlledMovement))
        {
            return;
        }

        // if the character is colliding with a ladder
        if (LadderColliding)
        {
            if ((_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing) // if the character is climbing
                && _platformer.NormalState.IsGrounded) // and is grounded
            {
                // we make it get off the ladder
                GetOffTheLadder();
            }

            if (MoveUp// if the player is pressing up
            && (_movement.CurrentState != CharacterStates.MovementStates.LadderClimbing) // and we're not climbing a ladder already
            && (_movement.CurrentState != CharacterStates.MovementStates.Gliding) // and we're not gliding
            && (_movement.CurrentState != CharacterStates.MovementStates.Jetpacking)) // and we're not jetpacking
            {
                // then the character starts climbing
                StartClimbing();
            }

            // if the character is climbing the ladder (which means it previously connected with it)
            if (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing)
            {
                Climbing();
            }

            // if the current ladder does have a ladder platform associated to it
            if (CurrentLadder.LadderPlatform != null)
            {
                if ((_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing) // if the character is climbing
                && AboveLadderPlatform()) // and is above the ladder platform
                {
                    // we make it get off the ladder
                    GetOffTheLadder();
                }
            }

            if (CurrentLadder.LadderPlatform != null)
            {
                if ((_movement.CurrentState != CharacterStates.MovementStates.LadderClimbing) // if the character is climbing
                    && (_verticalInput<-0.1f) // and is pressing down
                && (AboveLadderPlatform()) // and is above the ladder's platform
                && _platformer.NormalState.IsGrounded) // and is grounded
                {
                    // we make it get off the ladder
                    StartClimbingDown();
                }
            }
        }
        else
        {
            // if we're not colliding with a ladder, but are still in the LadderClimbing state
            if (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing)
            {
                GetOffTheLadder();
            }
        }

    }

    /// <summary>
    /// Puts the character on the ladder
    /// </summary>
    protected virtual void StartClimbing()
    {
        if (CurrentLadder.LadderPlatform != null)
        {
            if (AboveLadderPlatform())
            {
                return;
            }
        }

        // we rotate our character if requested
        if (ForceRightFacing)
        {
            _character.Face(ActCharacterNew.FacingDirections.Right);
        }

        SetClimbingState();

        // we set collisions
        _platformer.CollisionsOn();

        if (CurrentLadder.CenterCharacterOnLadder)
        {
            _platformer.transform.position = new Vector2(CurrentLadder.transform.position.x, _platformer.transform.position.y);
        }
    }


    protected virtual void Climbing()
    {
        // we disable the gravity
        _platformer.GravityActive(false);

        if (CurrentLadder.LadderPlatform != null)
        {
            if (!AboveLadderPlatform())
            {
                _platformer.CollisionsOn();
            }
        }
        else
        {
            _platformer.CollisionsOn();
        }

        // we set the force according to the ladder climbing speed
        if (CurrentLadder.LadderType == Climbable.LadderTypes.Simple)
        {
            _platformer.SetVerticalForce(_verticalInput * LadderClimbingSpeed);
            // we set the climbing speed state.
            CurrentLadderClimbingSpeed = Mathf.Abs(_verticalInput) * _platformer.transform.up;
        }
        if (CurrentLadder.LadderType == Climbable.LadderTypes.BiDirectional)
        {
            _platformer.SetHorizontalForce(_horizontalInput * LadderClimbingSpeed);
            _platformer.SetVerticalForce(_verticalInput * LadderClimbingSpeed);
            CurrentLadderClimbingSpeed = Mathf.Abs(_horizontalInput) * _platformer.transform.right;
            CurrentLadderClimbingSpeed += Mathf.Abs(_verticalInput) * (Vector2)_platformer.transform.up;
        }

    }

    public virtual void GetOffTheLadder()
    {
        // we make it stop climbing, it has reached the ground.
        LadderColliding = false;
        _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
        _movement.ChangeState(CharacterStates.MovementStates.Idle);
        CurrentLadderClimbingSpeed = Vector2.zero;
        _platformer.GravityActive(true);
        _platformer.CollisionsOn();

        _character.ResetHorizontalSpeed();
    }

    /// <summary>
    /// Puts the character on the ladder if it's standing on top of it
    /// </summary>
    protected virtual void StartClimbingDown()
    {
        SetClimbingState();
        _platformer.CollisionsOff();
        _platformer.ResetColliderSize();

        // we rotate our character if requested
        if (ForceRightFacing)
        {
            _character.Face(ActCharacterNew.FacingDirections.Right);
        }


        // we force its position to be a bit lower 
        if (CurrentLadder.CenterCharacterOnLadder)
        {
            _platformer.transform.position = new Vector2(CurrentLadder.transform.position.x, _platformer.transform.position.y - _climbingDownInitialYTranslation);
        }
        else
        {
            _platformer.transform.position = new Vector2(_platformer.transform.position.x, _platformer.transform.position.y - _climbingDownInitialYTranslation);
        }
    }

    protected virtual void SetClimbingState()
    {

        // we set its state to LadderClimbing
        _movement.ChangeState(CharacterStates.MovementStates.LadderClimbing);
        // it can't move freely anymore
        _condition.ChangeState(CharacterStates.CharacterConditions.ControlledMovement);
        // we initialize the ladder climbing speed to zero
        CurrentLadderClimbingSpeed = Vector2.zero;
        // we make sure the controller won't move
        _platformer.SetHorizontalForce(0);
        _platformer.SetVerticalForce(0);
        // we disable the gravity
        _platformer.GravityActive(false);

    }


    protected virtual bool AboveLadderPlatform()
    {
        // we make sure we have a current ladder and that it has a ladder platform associated to it
        if (CurrentLadder == null)
        {
            return false;
        }
        if (CurrentLadder.LadderPlatform == null)
        {
            return false;
        }

        float ladderColliderY = 0;

        if (CurrentLadder.LadderPlatformBoxCollider != null)
        {
            ladderColliderY = CurrentLadder.LadderPlatformBoxCollider.bounds.center.y + CurrentLadder.LadderPlatformBoxCollider.bounds.extents.y;
        }
        //if (CurrentLadder.LadderPlatformEdgeCollider2D != null)
        //{
        //    ladderColliderY = CurrentLadder.LadderPlatform.transform.position.y
        //        + CurrentLadder.LadderPlatformEdgeCollider2D.offset.y;
        //}

        bool conditionAboveLadderPlatform = (ladderColliderY < _platformer.ColliderBottomPosition.y + _ladderTopSkinHeight);



        // if the bottom of the player's collider is above the ladder platform, we return true
        if (conditionAboveLadderPlatform)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        GetOffTheLadder();
    }

}
