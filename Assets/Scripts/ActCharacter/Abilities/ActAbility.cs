using UnityEngine;
using System.Collections;

public class ActAbility
{
    public bool AbilityPermitted = true;
    public bool AbilityInitialized { get { return _abilityInitialized; } }

    protected ActCharacterNew _character;
    protected PlatformController _platformer;
    protected Animator _animator;
    //protected CameraController _sceneCamera;

    protected StateMachine<CharacterStates.MovementStates> _movement;
    protected StateMachine<CharacterStates.CharacterConditions> _condition;

    protected bool _abilityInitialized = false;

    protected float _verticalInput;
    protected float _horizontalInput;


    public virtual void Initialization(ActCharacterNew character)
    {
        _character = character;
        _platformer = character.platformer;

        _movement = _character.MovementState;
        _condition = _character.ConditionState;
        _abilityInitialized = true;
    }

    public virtual void Reset()
    {
    }

    public virtual void EarlyTick()
    {
        InternalHandleInput();
    }


    public virtual void Tick(float dTime)
    {

    }

    public virtual void LateTick()
    {

    }

    private void InternalHandleInput()
    {
        if(_character.controller == null)
        {
            return;
        }
        _verticalInput = _character.controller.MoveV;
        _horizontalInput = _character.controller.MoveH;

        HandleInput();
    }

    protected virtual void HandleInput()
    {

    }

    protected virtual void OnDeath()
    {
    }

}
