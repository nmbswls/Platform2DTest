using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Custom")]
public class JumpTask : Action
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
        agent.Jump();
        return TaskStatus.Success;
    }
}
