using UnityEngine;
using System.Collections;
using System;

public interface IStateMachine {
    bool TriggerEvents { get; set; }
}

public class StateMachine<T> : IStateMachine where T : struct , IComparable, IConvertible, IFormattable 
{
    public bool TriggerEvents { get; set; }

    public GameObject Target;

    public T CurrentState { get; protected set; }

    public T PrevState { get; protected set; }

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }


    public StateMachine(GameObject target, bool triggerEvents) {
        this.Target = target;
        this.TriggerEvents = triggerEvents;
    }

    public virtual void ChangeState(T newState) {
        if (newState.Equals(CurrentState)) {
            return;
        }
        PrevState = CurrentState;
        CurrentState = newState;

        if (TriggerEvents) {

        }
    }

    public virtual void RestorePrevState() {
        CurrentState = PrevState;
        if (TriggerEvents)
        {

        }
    }

}
