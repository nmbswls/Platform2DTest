using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActDash : ActAbility
{

    //控制信息
    public bool WantDash;
    public bool WantDown;

    public float DashCooldown = 0.3f;
    protected float _cooldown = 0;

    public bool isDashing = false;
    public float DashDistance = 1f;
    public float DashForce = 40f;
    //dash 信息
    protected bool _dashEndedNatually = false;
    protected float _distanceDashed = 0;
    protected bool _shouldKeepDashing = true;
    protected Vector2 _initialPosition;

    protected float _computedDashForce;
    protected float _dashDirection;
    //protected float _slopeAngleSave



    public override void Initialization(ActCharacterNew character)
    {
        base.Initialization(character);

    }

    public override void Tick(float dTime)
    {
        base.Tick(dTime);

        if(_cooldown > 0)
        {
            _cooldown -= dTime;
        }

        if (isDashing)
        {
            HandleDash();
        }

        if(_movement.CurrentState == CharacterStates.MovementStates.Dashing)
        {
            //平冲
            _platformer.GravityActive(false);
            _platformer.SetVerticalForce(0);
        }
        if(!(_dashEndedNatually) && (_movement.CurrentState != CharacterStates.MovementStates.Dashing)){
            //强转了
            _dashEndedNatually = true;
        }

    }

    protected override void HandleInput()
    {
        if(_character.controller == null)
        {
            return;
        }

        if (_character.controller.WantDash)
        {
            DashStart();
        }

    }

    public virtual void DashStart()
    {
        if(!AbilityPermitted
            ||(_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
            ||(_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing))
        {
            return;
        }

        if (_verticalInput>-0.1f)
        {
            if(_cooldown <= 0)
            {
                InitiateDash();
            }
        }
    }

    public virtual void InitiateDash()
    {

        _movement.ChangeState(CharacterStates.MovementStates.Dashing);

        _cooldown = DashCooldown;

        _dashEndedNatually = false;
        _shouldKeepDashing = true;
        _distanceDashed = 0;
        _dashDirection = _character.IsFacingRight ? 1f : -1f;
        _computedDashForce = DashForce * _dashDirection;

        isDashing = true;

    }

    protected virtual void HandleDash()
    {
        if(!AbilityPermitted
            ||(_condition.CurrentState != CharacterStates.CharacterConditions.Normal))
        {
            StopDash();
            return;
        }

        if(_distanceDashed >= DashDistance || !_shouldKeepDashing || _movement.CurrentState != CharacterStates.MovementStates.Dashing)
        {
            StopDash();
            return;
        }

        //opt
        _distanceDashed = Vector2.Distance(_initialPosition,_platformer.transform.position);
        if((_platformer.NormalState.IsCollidingLeft) || (_platformer.NormalState.IsCollidingRight))
        {
            _shouldKeepDashing = false;
            _platformer.SetForce(Vector2.zero);
        }
        else
        {
            _platformer.GravityActive(false);
            _platformer.SetVerticalForce(0);
            _platformer.SetHorizontalForce(_computedDashForce);
        }
        //may _shouldKeepDashing = false;
    }

    public virtual void StopDash()
    {
        isDashing = false;
        //冲不会来到更高处
        _platformer.GravityActive(true);
        _dashEndedNatually = true;

        if(_movement.CurrentState == CharacterStates.MovementStates.Dashing)
        {
            _movement.RestorePrevState();
        }
    }
}
