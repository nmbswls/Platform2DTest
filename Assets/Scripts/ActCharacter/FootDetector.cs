using UnityEngine;
using System.Collections;

public class FootDetector : MonoBehaviour
{

    public LayerMask wallLay;
    public bool BelowGrounded;

    public MapFootHold HoldBelow;

    public void Start()
    {
        wallLay = LayerMask.GetMask("Wall");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if((1<<collision.gameObject.layer) == wallLay)
        {
            BelowGrounded = true;
            HoldBelow = collision.GetComponent<MapFootHold>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == wallLay)
        {
            BelowGrounded = false;
            HoldBelow = null;
        }
    }
}
