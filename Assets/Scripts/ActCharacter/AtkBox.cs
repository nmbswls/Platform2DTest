using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AtkBox : MonoBehaviour {

    ActCtrl parent;
    public BoxCollider2D atkCollider;
    public void Init(ActCtrl parent)
    {
        this.parent = parent;
        atkCollider = GetComponent<BoxCollider2D>();
        atkCollider.enabled = false;
    }

    public void ChangeEnable(bool enable)
    {
        atkCollider.enabled = enable;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            Debug.Log("hit");
            parent.OnBoxHit(this, collision);
        }
    }
    //public void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("stay");
    //}
}
