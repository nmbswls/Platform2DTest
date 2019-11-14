using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;

public class AIAgent : Controller
{
    public ActCharacter Pawn;
    ExternalBehaviorTree externalBehaviorTree;
    BehaviorTree bt;
    ActGameMode gameMode;
    // Use this for initialization
    void Start()
    {

        Pawn = GetComponent<ActCharacter>();
        Pawn.controller = this;
        gameMode = GameMain.GetInstance().gameMode;
        bt = GetComponent<BehaviorTree>();
        //BehaviorManager.instance.UpdateInterval  = UpdateIntervalType.SpecifySeconds;
        //BehaviorManager.instance.UpdateIntervalSeconds = 0.25f;
        //BehaviorManager.instance.ExecutionsPerTick = BehaviorManager.ExecutionsPerTickType.Count;
        //BehaviorManager.instance.task
        //SharedInt i1 = bt.GetVariable("s") as SharedInt;
        //i1.Value = 1;
        //bt.EnableBehavior();
        //bt.RegisterEvent<object>("MyEvent", ReceivedEvent);
        //bt.UnregisterEvent<object>("MyEvent", ReceivedEvent);
    }
    public void ReceivedEvent(object arg1)
    {

    }


    float JumpInteval;

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack()
    {
        Pawn.ThrowChain();
    }

    public void Jump()
    {
        Pawn.Jump();
    }
    //public void IsActioning()
    //{

    //}

}
