using UnityEngine;
using System.Collections;

public class Grip : MonoBehaviour
{
    public Vector3 GripOffset = Vector3.zero;

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        ActCharacterNew c = collider.gameObject.GetComponent<ActCharacterNew>();
        if(c == null)
        {
            return;
        }
        ActGrip grip = c.GetAbility(typeof(ActGrip)) as ActGrip;
        if(grip == null)
        {
            return;
        }
        grip.StartGripping(this);

    }
}
