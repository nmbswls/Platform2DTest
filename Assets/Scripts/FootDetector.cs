using UnityEngine;
using System.Collections;

public class FootDetector : MonoBehaviour
{

    public LayerMask wallLay;
    public bool BelowGrounded;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if((wallLay.value | (1 << collision.gameObject.layer))!= 0)
        {
            BelowGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((wallLay.value | (1 << collision.gameObject.layer)) != 0)
        {
            BelowGrounded = false;
        }
    }
}
