using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom")]
public class ThrowChain : BehaviorDesigner.Runtime.Tasks.Action
{

    public SharedGameObject ownerGO;

    private ActCharacter Player;
    private AIAgent agent;

    bool hasAttack;

    public override void OnAwake()
    {
        {
            //Player = GameMain.GetInstance().gameMode.playerController.Pawn;
        }

        agent = ((GameObject)ownerGO.GetValue()).GetComponent<AIAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        if (!hasAttack) {
            agent.Attack();
            hasAttack = true;
        }
        if (agent.Pawn.isAttacking())
        {
            return TaskStatus.Running;
        }
        else
        {
            hasAttack = false;
            return TaskStatus.Success;
        }

    }
}
