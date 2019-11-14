using UnityEngine;
using System.Collections;

public class ActGrip : ActAbility
{

    protected ActJump _actJump;

    private float _lastGripTimestamp = 0f;
    private Grip _gripTarget;

    public float BufferDurationAfterGrip = 0.5f;

    public bool CanGrip { get { return (Time.time - _lastGripTimestamp) > BufferDurationAfterGrip; } }

    public override void Initialization(ActCharacterNew character)
    {
        base.Initialization(character);
        _actJump = character.GetAbility(typeof(ActJump)) as ActJump;
    }

    public override void Tick(float dTime)
    {
        base.Tick(dTime);
        Grip();
        Detach();
    }

    protected virtual void Grip() {

        if(_movement.CurrentState == CharacterStates.MovementStates.Gripping) {
            _platformer.SetForce(Vector2.zero);
            _platformer.GravityActive(false);
            if(_actJump != null) {
                _actJump.ResetNumberOfJumps();
            }
            _platformer.transform.position = _gripTarget.transform.position + _gripTarget.GripOffset; 
        }
    }

    protected virtual void Detach() {
        if((_movement.CurrentState != CharacterStates.MovementStates.Gripping) && (_movement.PrevState == CharacterStates.MovementStates.Gripping)) {
            _lastGripTimestamp = Time.time;
        }
    }
    public virtual void StartGripping(Grip gripTarget)
    {
        if (!CanGrip)
        {
            return;
        }
        _gripTarget = gripTarget;
        _movement.ChangeState(CharacterStates.MovementStates.Gripping);

    }
}
