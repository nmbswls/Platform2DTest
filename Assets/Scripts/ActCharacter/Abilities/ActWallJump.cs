using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActWallJump : ActAbility
{
    private bool AntiWallJump;

    public Vector2 WallJumpForce = new Vector2(6,3);

    public bool WallJumpThisFrame { get; set; }

    protected ActJump _actJump;
    protected ActWallCling _actWallCling;

    public override void Initialization(ActCharacterNew character)
    {
        base.Initialization(character);

        _actJump = character.GetAbility(typeof(ActJump)) as ActJump;
        _actWallCling = character.GetAbility(typeof(ActWallCling)) as ActWallCling;
    }

    protected override void HandleInput()
    {
        if (_character.controller == null)
        {
            return;
        }
        AntiWallJump = false;
        if (_character.controller.WantJump && _verticalInput > -0.1f)
        {
            AntiWallJump = true;
        }

        if (_character.controller.WantJump)
        {
            WallJump();
        }

    }

    public override void Tick(float dTime)
    {
        base.Tick(dTime);
    }

    protected virtual void WallJump()
    {
        if(!AbilityPermitted
            || _condition.CurrentState != CharacterStates.CharacterConditions.Normal)
        {
            return;
        }

        float wallJumpDirection;
        if(_movement.CurrentState == CharacterStates.MovementStates.WallClinging)
        {
            _movement.ChangeState(CharacterStates.MovementStates.WallJumping);

            if(_actJump != null)
            {
                _actJump.SetNumberOfJumpsLeft(_actJump.NumberOfJumpsLeft - 1);
                _actJump.SetJumpFlags();

            }

            _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
            _platformer.GravityActive(true);
            _platformer.SlowFall(0f);

            if (_platformer.NormalState.IsCollidingRight)
            {
                wallJumpDirection = -1f;
            }
            else
            {
                wallJumpDirection = 1f;
            }
            Vector2 wallJumpVector = new Vector2(wallJumpDirection * WallJumpForce.x, Mathf.Sqrt(2f * WallJumpForce.y * Mathf.Abs(_platformer.Parameters.Gravity)));
            _platformer.AddForce(wallJumpVector);
            WallJumpThisFrame = true;
            _actWallCling.SetCoolDown();
        }
    }
}
