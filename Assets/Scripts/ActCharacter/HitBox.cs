using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour
{
    public IHittable owner;
    public BoxCollider2D hitCollider;



    public void Init(IHittable owner)
    {
        this.owner = owner;
        hitCollider = GetComponent<BoxCollider2D>();
    }

    public void ChangeEnable(bool enable)
    {
        hitCollider.enabled = enable;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
