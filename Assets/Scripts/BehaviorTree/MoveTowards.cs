using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom")]
public class MoveTowards : BehaviorDesigner.Runtime.Tasks.Action
{
    public float speed = 0;
    public SharedTransform target;


    public override void OnAwake()
    {

    }

    public WithinSight referencedTask;

    public override TaskStatus OnUpdate() {
        return TaskStatus.Running;
    }
}
