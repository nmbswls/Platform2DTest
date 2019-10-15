using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActActionInfo
{
    public string ActionId;
    public string TotalFrame;
    public bool HasCombo;
    public string ComboAction = string.Empty;
    public List<string> ComboReq = new List<string>();
    public int AtkBoxId;
}




public class ActCtrl : MonoBehaviour {



    public bool isCombo = false;
    public bool enableCombo = false;
    public bool canInterrupt = false;

    public bool isCasting = false;
    public Animator boxAnimator;
    
   

    public Animator SpriteAnimtor;

    public string NowActionId = string.Empty;

    List<AtkBox> AtkBoxes = new List<AtkBox>();

    Dictionary<string, ActActionInfo> ActInfoDict = new Dictionary<string, ActActionInfo>();

    List<string> OptStack = new List<string>();
    float LastValidOpt = 0;

    int[] SKillMap = new int[5];
    // Use this for initialization
    void Start () {
        boxAnimator = GetComponent<Animator>();
        boxAnimator.GetCurrentAnimatorStateInfo(0);
        BindBoxes();
        InitActionTable();

        OptStack.Clear();

    }

    void BindAnimationEvent()
    {
        //同一个连击动作 用不同动画给替换了
        //AnimatorOverrideController overrideController = new AnimatorOverrideController();
        //overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
        //overrideController["clipName"] = newClip;
        //anim.runtimeAnimatorController = overrideController;


        //AnimationEvent aEvent1 = new AnimationEvent();
        //aEvent1.time = clip.length;
        //aEvent1.functionName = "OnOpenComplete";
        //clip.AddEvent(aEvent1);
    }


    void InitActionTable()
    {
        {
            ActActionInfo action = new ActActionInfo();
            action.ActionId = "normal_1";
            action.HasCombo = true;
            action.ComboAction = "normal_2";
            action.AtkBoxId = 0;
            ActInfoDict.Add(action.ActionId,action); 
        }
        {
            ActActionInfo action = new ActActionInfo();
            action.ActionId = "normal_2";
            action.HasCombo = true;
            action.ComboAction = "normal_3";
            action.AtkBoxId = 0;
            ActInfoDict.Add(action.ActionId, action);
        }
        {
            ActActionInfo action = new ActActionInfo();
            action.ActionId = "normal_3";
            action.HasCombo = false;
            action.AtkBoxId = 0;
            ActInfoDict.Add(action.ActionId, action);
        }
    }

    void BindBoxes()
    {
        foreach(Transform child in transform)
        {
            AtkBox b = child.GetComponent<AtkBox>();
            b.Init(this);
            AtkBoxes.Add(b);
        }
        
    }

   

    void Tick(float dTime)
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(NowActionId == "normal_1" || NowActionId == "normal_2")
            {
                if (enableCombo)
                {
                    isCombo = true;
                }
            }else if (NowActionId == string.Empty)
            {
                StartAction("normal_1");
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            boxAnimator.SetBool("lasting", true);
            SpriteAnimtor.SetBool("lasting", true);
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            boxAnimator.SetBool("lasting", false);
            SpriteAnimtor.SetBool("lasting", false);
        }


    }
	// Update is called once per frame
	void ChangeSpdRate(float rate)
    {
        boxAnimator.speed = rate;
    }


    void Update()
    {
        Tick(Time.deltaTime);
    }


    public void MoveInterupt()
    {
        if (!canInterrupt)
        {
            return;
        }
    }


    public void StartAction(string nextAction)
    {

        boxAnimator.SetBool(nextAction, true);
        SpriteAnimtor.SetBool(nextAction, true);
        NowActionId = nextAction;
        canInterrupt = true;
        isCombo = false;
        enableCombo = false;

    }


    public void StartAtk()
    {
        if (NowActionId == string.Empty)
        {
            return;
        }
        canInterrupt = false;
        ActActionInfo info = ActInfoDict[NowActionId];
        AtkBoxes[info.AtkBoxId].ChangeEnable(true);
    }

    public void EnableCombo()
    {
        enableCombo = true;
    }

    

   

    public void CheckCombo()
    {
        enableCombo = false;
        canInterrupt = true;
       

        ActActionInfo info = ActInfoDict[NowActionId];
        if (!info.HasCombo)
        {
            return;
        }
        if (!isCombo)
        {
            return;
        }
        if (info.ComboAction == string.Empty)
        {
            return;
        }

        FinishAtk();

        string nextAction = info.ComboAction;
        StartAction(nextAction);
    }



    public void FinishAtk()
    {

        if (NowActionId == string.Empty)
        {
            return;
        }

        ActActionInfo info = ActInfoDict[NowActionId];
        AtkBoxes[info.AtkBoxId].ChangeEnable(false);

        if(info.ComboAction != string.Empty)
        {
            boxAnimator.SetBool(info.ComboAction, false);
            SpriteAnimtor.SetBool(info.ComboAction, false);
        }

        boxAnimator.SetBool(info.ActionId, false);
        SpriteAnimtor.SetBool(info.ActionId, false);

        canInterrupt = false;

        enableCombo = false;
        isCombo = false;

        NowActionId = string.Empty;
    }


    


    public void OnBoxHit(AtkBox box, Collider2D other)
    {
        int idx = AtkBoxes.IndexOf(box);
        int skill = SKillMap[idx];
        ApplyAtk();
    }

    public void ApplyAtk()
    {

    }

   

    


}
