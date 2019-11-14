using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Custom")]
public class WithinSight : Conditional
{
    public float fieldOfViewAngle; 
    public string targetTag; 
    public SharedTransform target;
    public SharedGameObject ownerGO;

    private ActCharacter Player;
    private AIAgent agent;


    public override void OnAwake()
    {
        {
            //Player = GameMain.GetInstance().gameMode.playerController.Pawn;
        }
        agent = ((GameObject)ownerGO.GetValue()).GetComponent<AIAgent>();
    }

    public override TaskStatus OnUpdate()
    {

        if (withinSight(Player.transform, 30))
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    public bool withinSight(Transform targetTransform, float fieldOfViewAngle)
    {
        Vector3 direction = targetTransform.position;
        return Vector3.Angle(direction, transform.forward) < fieldOfViewAngle;
    }



}
