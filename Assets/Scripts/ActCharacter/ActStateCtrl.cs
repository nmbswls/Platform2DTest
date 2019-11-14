using UnityEngine;
using System.Collections;


public enum eActState
{
    Idle,
    Walking,
    Jumping,
    Falling,
    Climbing,
    ClimbingIdle,
    Dying,
}

public class ActStateCtrl
{
    public Animator ViewAnimator;
    public Animator LogicAnimator;

    private eActState m_state = eActState.Idle;
    private eActState m_nextState = eActState.Idle;

    public delegate void OnStateChangedDelegate(ActStateCtrl source, eActState prevState, eActState newState);

    public OnStateChangedDelegate OnStateChanged;

    public void Init()
    {
        //OnStateChanged += _OnStateChanged;
    }

    public void SetTrigger(string trigger)
    {
        ViewAnimator.ResetTrigger(trigger);
        ViewAnimator.SetTrigger(trigger);
        LogicAnimator.ResetTrigger(trigger);
        LogicAnimator.SetTrigger(trigger);
    }
    public void ChangeAnim()
    {
        //RuntimeAnimatorController runtimeAnimatorController = SpriteAnimtor.runtimeAnimatorController;

        //overrideController = new AnimatorOverrideController();
        //overrideController.runtimeAnimatorController = runtimeAnimatorController;

        //overrideController["1"] = anima1;
        //overrideController["2"] = anima2;
    }



    public void ChangeState(eActState newState)
    {
        m_nextState = newState;
    }


    public void HitFrozenFrame()
    {
        ViewAnimator.speed = 0;
        LogicAnimator.speed = 0;
    }

    public void FinishFrozenFrame()
    {
        ViewAnimator.speed = 1;
        LogicAnimator.speed = 1;
    }


    public void SwitchLayer(int layer)
    {
        for(int i = 0; i < 2; i++)
        {
            ViewAnimator.SetLayerWeight(i, 0);
            LogicAnimator.SetLayerWeight(i, 0);
        }
        ViewAnimator.SetLayerWeight(layer, 1);
        LogicAnimator.SetLayerWeight(layer, 1);
    }
    public void AddLayer(int layer)
    {
        ViewAnimator.SetLayerWeight(layer, 1);
        LogicAnimator.SetLayerWeight(layer, 1);


    }
    public void RemoveLayer(int layer)
    {
        ViewAnimator.SetLayerWeight(layer, 0);
        LogicAnimator.SetLayerWeight(layer, 0);



    }
}
