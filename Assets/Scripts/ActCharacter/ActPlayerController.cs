using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActPlayerController : Controller
{
    public PlayerInput InputMdl;

    public ActCharacterNew Pawn;
    public List<MapReactor> ActivateReactors = new List<MapReactor>();

    // Use this for initialization
    void Start()
    {
        InputMdl = new PlayerInput();
        InputMdl.controller = this;
        Pawn = GetComponent<ActCharacterNew>();
        Pawn.controller = this;
    }

    // Update is called once per frame
    void Update()
    {
        InputMdl.Tick(Time.deltaTime);

        if (AllowInteract() && InputMdl.Interact)
        {
            if (ActivateReactors.Count > 0)
            {
                string actionString = ActivateReactors[0].actionString;
                if (actionString == "map")
                {
                    GameMain.GetInstance().SwitchScene("map01");
                }
            }
        }
        //if (InputMdl.Throw &&  Pawn.AllowUseSkill() )
        //{
        //    Pawn.ThrowChain();
        //}

        //if (InputMdl.Jump && Pawn.AllowJump())
        //{
        //    //没有攻击动作
        //     Pawn.Jump();
        //}

        //if (Pawn.isClimbing)
        //{
        //    Pawn.ClimbMove(InputMdl.DVertical);
        //}
        //else
        //{
        //    if (InputMdl.DVertical > 0)
        //    {
        //        if (Pawn.AllowClimb())
        //        {
        //            Collider2D c = Pawn.GetClimbingColliderAbove();
        //            if (c != null)
        //            {
        //                Pawn.StartClimb(c);
        //            }
        //        }
        //    }
        //}

        MoveH = InputMdl.DLateral;
        MoveV = InputMdl.DVertical;


        WantJump = InputMdl.Jump;
        WantDash = InputMdl.Dash;
    }

    private bool AllowInteract()
    {
        return true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Reactor")
        {
            MapReactor reactor = collision.GetComponent<MapReactor>();
            if (!ActivateReactors.Contains(reactor))
            {
                ActivateReactors.Add(reactor);
                GameMain.GetInstance().gameMode.HudRoot.ShowReactHud(reactor);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Reactor")
        {
            MapReactor reactor = collision.GetComponent<MapReactor>();
            if (ActivateReactors.Contains(reactor))
            {
                int idx = ActivateReactors.IndexOf(reactor);
                ActivateReactors.Remove(reactor);
                GameMain.GetInstance().gameMode.HudRoot.HideReactHud(idx);
            }
        }
    }
}
