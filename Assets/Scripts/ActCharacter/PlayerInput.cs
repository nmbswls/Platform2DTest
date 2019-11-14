using UnityEngine;
using System.Collections;

public class PlayerInput
{
    public ActPlayerController controller;

    public string keyLeftStrig = "a";
    public string keyRightStrig = "d";
    public bool Interact;
    public bool Jump;
    public bool Dash;

    public float DVertical;
    public float DLateral;
    public float DTargetLateral;


    float velocityDLateral;
    public bool Throw;


    public void Tick(float dTime)
    {
        controller.attackL = false;
        controller.attackR = false;

        Jump = false;
        Dash = false;

        Throw = false;

        Interact = false;

        DTargetLateral = (Input.GetKey(keyRightStrig) ? 1.0f : 0.0f) - (Input.GetKey(keyLeftStrig) ? 1.0f : 0.0f);

        DVertical = (Input.GetKey(KeyCode.W) ? 1.0f : 0.0f) - (Input.GetKey(KeyCode.S) ? 1.0f : 0.0f);

        if (!controller.inputEnable)
        {
            DTargetLateral = 0;

            DVertical = 0;
        }


        DLateral = Mathf.SmoothDamp(DLateral, DTargetLateral, ref velocityDLateral, 0.1f);

        controller.Dmag = DLateral;
        controller.Dvec = Vector2.right;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump = controller.inputEnable;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash = controller.inputEnable;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            controller.attackL = controller.inputEnable;
        }
        if (Input.GetKey(KeyCode.K))
        {
            controller.attackR = controller.inputEnable;
        }


        if (Input.GetKeyDown(KeyCode.G))
        {
            Throw = controller.inputEnable;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact = controller.inputEnable;
        }
    }
}
