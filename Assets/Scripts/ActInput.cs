using UnityEngine;
using System.Collections;

public class ActInput
{

    public bool moveEnable = true;

    public string keyLeftStrig = "a";
    public string keyRightStrig = "d";


    public float DLateral;

    public float Dmag;
    public Vector2 Dvec;

    public float DTargetLateral;
    float velocityDLateral;

    public bool attackL;
    public bool attackR;
    public bool Jump;

    public void Tick(float dTime)
    {

        attackL = false;
        attackR = false;
        Jump = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump = moveEnable;
        }

        DTargetLateral = (Input.GetKey(keyRightStrig) ? 1.0f : 0.0f) - (Input.GetKey(keyLeftStrig) ? 1.0f : 0.0f);
        if (!moveEnable)
        {
            DTargetLateral = 0;
        }

        DLateral = Mathf.SmoothDamp(DLateral, DTargetLateral, ref velocityDLateral, 0.1f);

        Dmag = DLateral;
        Dvec = Vector2.right;

        if (Input.GetKeyDown(KeyCode.J))
        {
            attackL = true;
        }
        if (Input.GetKey(KeyCode.K))
        {
            attackR = true;
        }
    }


}