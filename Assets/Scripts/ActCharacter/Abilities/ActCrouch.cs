using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActCrouch : ActAbility
{

    public bool ResizeColliderWhenCrouch;

    public bool CrawlAuthorized;

    public bool InTunnel;

    public float CrawlSpeed = 2f;

    private bool NeedRecalculateRays = false;
    private int CalculateNum = 0;

    public override void Initialization(ActCharacterNew character)
    {
        base.Initialization(character);
        InTunnel = false;
        CrawlAuthorized = true;
    }


    protected override void HandleInput()
    {
        base.HandleInput();

        if(_verticalInput < -0.1f)
        {
            Crouch();
        }

    }

    public override void Tick(float dTime)
    {
        base.Tick(dTime);
        DetermineState();
        CheckExitCrouch();

        if (NeedRecalculateRays)
        {
            if(CalculateNum <= 0)
            {
                NeedRecalculateRays = false;
                RecalculateRays();
            }
            else
            {
                CalculateNum -= 1;
            }
        }
    }


    private void DetermineState()
    {
        if((_movement.CurrentState == CharacterStates.MovementStates.Crouching) || (_movement.CurrentState == CharacterStates.MovementStates.Crawling))
        {
            if((Mathf.Abs(_horizontalInput) > 0) && CrawlAuthorized)
            {
                _movement.ChangeState(CharacterStates.MovementStates.Crawling);
            }
            else
            {
                _movement.ChangeState(CharacterStates.MovementStates.Crouching);
            }
        }
    }

    protected virtual void Crouch()
    {
        if(!AbilityInitialized 
            || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
            || (!_platformer.NormalState.IsGrounded)
            || (_movement.CurrentState == CharacterStates.MovementStates.Gripping))
        {
            return;
        }

        if((_movement.CurrentState != CharacterStates.MovementStates.Crouching) && (_movement.CurrentState != CharacterStates.MovementStates.Crawling))
        {

        }

        _movement.ChangeState(CharacterStates.MovementStates.Crouching);
        if((Mathf.Abs(_horizontalInput) > 0) && (CrawlAuthorized))
        {
            _movement.ChangeState(CharacterStates.MovementStates.Crawling);
        }

        if (ResizeColliderWhenCrouch)
        {
            //resize
        }

        if (_character.GetAbility(typeof(ActMoveH)) != null)
        {
            ActMoveH a = _character.GetAbility(typeof(ActMoveH)) as ActMoveH;
            a.MovementSpeed = CrawlSpeed;
        }
        if (!CrawlAuthorized)
        {
            ActMoveH a = _character.GetAbility(typeof(ActMoveH)) as ActMoveH;
            a.MovementSpeed = 0f;
        }

        //if(_sceneCamera != null)
        //{
        //    _sceneCamera.LookDown();
        //}

    }

    protected virtual void CheckExitCrouch()
    {

        if (_character == null)
        {
            //ExitCrouch();
        }
        if((_movement.CurrentState == CharacterStates.MovementStates.Crouching)
            || (_movement.CurrentState == CharacterStates.MovementStates.Crawling))
        {

            if((!_platformer.NormalState.IsGrounded) || (_verticalInput >= -0.4f))
            {
                ExitCrouch();
            }
        }
    }


    protected virtual void ExitCrouch()
    {
        InTunnel = !_platformer.CanGoBackToOriginalSize();

        if (!InTunnel)
        {
            if (_character.GetAbility(typeof(ActMoveH)) != null)
            {
                ActMoveH a = _character.GetAbility(typeof(ActMoveH)) as ActMoveH;
                a.ResetHorizontalSpeed();
            }

            //if()

            _movement.ChangeState(CharacterStates.MovementStates.Idle);
            _platformer.ResetColliderSize();

            NeedRecalculateRays = true;
            CalculateNum = 10;
            //Invoke("RecalculateRays",0.1f);    
        }
    }

    protected virtual void RecalculateRays()
    {
        _character.RecalculateRays();
    }

}
