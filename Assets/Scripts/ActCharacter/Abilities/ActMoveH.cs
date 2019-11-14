using UnityEngine;
using System.Collections;

public class ActMoveH : ActAbility
{

    protected float _horizontalMovement;
    protected float _horizontalMovementForce;
    protected float _normalizedHorizontalSpeed;

    public float MovementSpeed { get; set; }


    public bool MovementForbidden { get; set; }

    public float InputThreshold = 0.1f;

    public float MovementSpeedMultiplier = 1f;
    public float PushSpeedMultiplier = 1f;

    public float BaseSpeed = 6f;

    public override void Initialization(ActCharacterNew character)
    {
        base.Initialization(character);
        MovementSpeed = BaseSpeed;
        MovementSpeedMultiplier = 1f;
        MovementForbidden = false;
    }
        

    public override void Tick(float dTime)
    {
        base.Tick(dTime);

        HandleHorizontalMovement();
    }

    protected override void HandleInput()
    {
        _horizontalMovement = _horizontalInput;
    }

    /// <summary>
    /// Sets the horizontal move value.
    /// </summary>
    /// <param name="value">Horizontal move value, between -1 and 1 - positive : will move to the right, negative : will move left </param>
    public virtual void SetHorizontalMove(float value)
    {
        _horizontalMovement = value;
    }

    public virtual void ResetHorizontalSpeed()
    {
        MovementSpeed = BaseSpeed;
    }

    /// <summary>
    /// Called at Update(), handles horizontal movement
    /// </summary>
    protected virtual void HandleHorizontalMovement()
    {
        // if movement is prevented, or if the character is dead/frozen/can't move, we exit and do nothing
        if (!AbilityPermitted
            || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
            || (_movement.CurrentState == CharacterStates.MovementStates.Gripping))
        {
            return;
        }

        // check if we just got grounded
        CheckJustGotGrounded();


        if (MovementForbidden)
        {
            _horizontalMovement = _platformer.Speed.x * Time.deltaTime;
        }

        // If the value of the horizontal axis is positive, the character must face right.
        if (_horizontalMovement > InputThreshold)
        {
            _normalizedHorizontalSpeed = _horizontalMovement;
            if (!_character.IsFacingRight)
            {
                _character.Flip();
            }
        }
        // If it's negative, then we're facing left
        else if (_horizontalMovement < -InputThreshold)
        {
            _normalizedHorizontalSpeed = _horizontalMovement;
            if (_character.IsFacingRight)
            {
                _character.Flip();
            }
        }
        else
        {
            _normalizedHorizontalSpeed = 0;
        }

        // if we're grounded and moving, and currently Idle, Running or Dangling, we become Walking
        if ((_platformer.NormalState.IsGrounded)
            && (_normalizedHorizontalSpeed != 0)
            && ((_movement.CurrentState == CharacterStates.MovementStates.Idle)
                || (_movement.CurrentState == CharacterStates.MovementStates.Dangling)))
        {
            _movement.ChangeState(CharacterStates.MovementStates.Walking);
        }

        // if we're walking and not moving anymore, we go back to the Idle state
        if ((_movement.CurrentState == CharacterStates.MovementStates.Walking)
            && (_normalizedHorizontalSpeed == 0))
        {
            _movement.ChangeState(CharacterStates.MovementStates.Idle);
        }

        // if the character is not grounded, but currently idle or walking, we change its state to Falling
        if (!_platformer.NormalState.IsGrounded
            && (
                (_movement.CurrentState == CharacterStates.MovementStates.Walking)
                 || (_movement.CurrentState == CharacterStates.MovementStates.Idle)
                ))
        {
            _movement.ChangeState(CharacterStates.MovementStates.Falling);
        }



        // we pass the horizontal force that needs to be applied to the controller.
        float movementFactor = _platformer.NormalState.IsGrounded ? _platformer.Parameters.SpeedAccelerationOnGround : _platformer.Parameters.SpeedAccelerationInAir;
        float movementSpeed = _normalizedHorizontalSpeed * MovementSpeed * _platformer.Parameters.SpeedFactor * MovementSpeedMultiplier * PushSpeedMultiplier;

        _horizontalMovementForce = Mathf.Lerp(_platformer.Speed.x, movementSpeed, Time.deltaTime * movementFactor);

        // we handle friction
        _horizontalMovementForce = HandleFriction(_horizontalMovementForce);

        // we set our newly computed speed to the controller
        _platformer.SetHorizontalForce(_horizontalMovementForce);
    }


    protected virtual void CheckJustGotGrounded()
    {
        // if the character just got grounded
        if (_platformer.NormalState.JustGotGrounded)
        {
            if (_platformer.NormalState.ColliderResized)
            {
                _movement.ChangeState(CharacterStates.MovementStates.Crouching);
            }
            else
            {
                _movement.ChangeState(CharacterStates.MovementStates.Idle);
            }

            _platformer.SlowFall(0f);
            //触地动画
            //if (TouchTheGroundEffect != null)
            //{
            //    Instantiate(TouchTheGroundEffect, _controller.BoundsBottom, transform.rotation);
            //}
        }
    }

    /// <summary>
    /// Handles surface friction.
    /// </summary>
    /// <returns>The modified current force.</returns>
    /// <param name="force">the force we want to apply friction to.</param>
    protected virtual float HandleFriction(float force)
    {
        // if we have a friction above 1 (mud, water, stuff like that), we divide our speed by that friction
        if (_platformer.Friction > 1)
        {
            force = force / _platformer.Friction;
        }

        // if we have a low friction (ice, marbles...) we lerp the speed accordingly
        if (_platformer.Friction < 1 && _platformer.Friction > 0)
        {
            force = Mathf.Lerp(_platformer.Speed.x, force, Time.deltaTime * _platformer.Friction * 10);
        }

        return force;
    }
}
