using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActWallCling : ActAbility
{

    public float WallClingingToTolerence = 0.3f;

    public float WallJumpLockTime = 0.3f;

    private float clingCoolDown;

    public ActJump _actJump;

    public void SetCoolDown()
    {
        clingCoolDown = WallJumpLockTime;
    }

    public override void Initialization(ActCharacterNew character)
    {
        base.Initialization(character);

        _actJump = character.GetAbility(typeof(ActJump)) as ActJump;
    }

    protected override void HandleInput()
    {
        if (_horizontalInput < -0.1f || _horizontalInput > 0.1f)
        {
            StartWallClinging();
        }
    }

    public override void Tick(float dTime)
    {
        base.Tick(dTime);

        WallClinging();
        Detach();
        //ExitWallClinging();
        WallClingingLastFrame();
        if(clingCoolDown > 0)
        {
            clingCoolDown -= dTime;
        }
    }

    private void WallClinging()
    {
        if (_movement.CurrentState == CharacterStates.MovementStates.WallClinging)
        {
            _platformer.SetForce(Vector2.zero);
            _platformer.GravityActive(false);
            if (_actJump != null)
            {
                _actJump.ResetNumberOfJumps();
            }
            //_platformer.transform.position = _gripTarget.transform.position + _gripTarget.GripOffset;
        }
    }

    protected virtual void StartWallClinging()
    {
        if (!AbilityPermitted || clingCoolDown>0)
        {
            return;
        }

        if ((_platformer.NormalState.IsCollidingLeft && _horizontalInput <= -0.1f)
               || (_platformer.NormalState.IsCollidingRight && _horizontalInput >= 0.1f))
        {
            WallClingOverride wallClinging = _platformer.CurrentWallCollider.GetComponent<WallClingOverride>();

            //if(wallClinging != null)
            //{
            //    if (!wallClinging.CanWallClingToThis)
            //    {
            //        return;
            //    }
            //    _platformer.SlowFall(wallClinging.WallClingSlowFactor);
            //}
            //else
            //{
            //    _platformer.SlowFall(0.01f);
            //}
            //if (_movement.CurrentState != CharacterStates.MovementStates.WallClinging)
            //{

            //}
            _movement.ChangeState(CharacterStates.MovementStates.WallClinging);

        }
    }

    protected virtual void Detach()
    {
        if ((_movement.CurrentState != CharacterStates.MovementStates.Gripping) && (_movement.PrevState == CharacterStates.MovementStates.Gripping))
        {
            SetCoolDown();
        }
    }
    //protected virtual void ExitWallClinging()
    //{
    //    if(_movement.CurrentState == CharacterStates.MovementStates.WallClinging)
    //    {
    //        bool shouldExit = false;
    //        if((_platformer.NormalState.IsGrounded)
    //            || (_platformer.Speed.y >= 0))
    //        {
    //            shouldExit = true;

    //        }

    //        Vector3 raycastOrigin = _character.transform.position;
    //        Vector3 raycastDir;
    //        if (_character.IsFacingRight)
    //        {
    //            raycastOrigin = raycastOrigin + _character.transform.right * _platformer.Width() / 2;
    //            raycastDir = _character.transform.right - _character.transform.up;
    //        }
    //        else
    //        {
    //            raycastOrigin = raycastOrigin - _character.transform.right * _platformer.Width() / 2;
    //            raycastDir = - _character.transform.right - _character.transform.up;
    //        }

    //        RaycastHit2D hit = PhysicsDebugger.RayCast(raycastOrigin,raycastDir,WallClingingToTolerence,_platformer.PlatformMask | _platformer.OneWayPlatformMask | _platformer.MovingOneWayPlatformMask,Color.black,_platformer.Parameters.DrawRaycastsGizmos);
    //        if (_character.IsFacingRight)
    //        {
    //            if((!hit)||(_horizontalInput <= 0.1f))
    //            {
    //                shouldExit = true;
    //            }
    //        }
    //        else
    //        {
    //            if ((!hit) || (_horizontalInput >= -0.1f))
    //            {
    //                shouldExit = true;
    //            }
    //        }

    //        if (shouldExit)
    //        {
    //            _platformer.SlowFall(0f);
    //            _movement.ChangeState(CharacterStates.MovementStates.Idle);

    //        }
    //    }
    //}

    protected virtual void WallClingingLastFrame()
    {
        if((_movement.PrevState == CharacterStates.MovementStates.WallClinging) && (_movement.CurrentState != CharacterStates.MovementStates.WallClinging))
        {
            _platformer.SlowFall(0f);
        }
    }
}
