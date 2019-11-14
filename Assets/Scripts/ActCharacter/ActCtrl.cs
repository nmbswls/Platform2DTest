using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActActionInfo
{
    public string ActionId;
    public string TotalFrame;

    public bool CanChangeDirection;
    public bool HasCombo;
    public string ComboAction = string.Empty;
    public List<string> ComboReq = new List<string>();
    public int AtkBoxId;
}




public class ActCtrl : MonoBehaviour {

    public Controller controller;
    public ActStateCtrl StateCtrl;

    public bool isCombo = false;
    public bool enableCombo = false;
    public bool canInterrupt = false;

    public bool isCasting = false;
    

    public string NowActionId = string.Empty;

    List<AtkBox> AtkBoxes = new List<AtkBox>();

    Dictionary<string, ActActionInfo> ActInfoDict = new Dictionary<string, ActActionInfo>();

    List<string> OptStack = new List<string>();
    float LastValidOpt = 0;

    int[] SKillMap = new int[5];

    public void Init(Controller controller, ActStateCtrl StateCtrl)
    {

        this.controller = controller;
        this.StateCtrl = StateCtrl;

        BindBoxes();
        InitActionTable();

        OptStack.Clear();
    }

    public void Update()
    {
        if (frozenFrame > 0)
        {
            frozenFrame -= 1;
            if (frozenFrame <= 0)
            {
                StateCtrl.FinishFrozenFrame();
            }
        }
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
            action.CanChangeDirection = true;
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

    public bool CanChangeDir()
    {
        if(NowActionId == string.Empty)
        {
            return true;
        }

        return ActInfoDict[NowActionId].CanChangeDirection;
    }

    private void BindBoxes()
    {
        foreach(Transform child in transform)
        {
            AtkBox b = child.GetComponent<AtkBox>();
            b.Init(this);
            AtkBoxes.Add(b);
        }
        
    }




    public void CheckInput()
    {
        if (controller.attackL)
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
    }

    public void MoveInterupt()
    {
        if (!canInterrupt)
        {
            return;
        }
        canInterrupt = false;

        enableCombo = false;
        isCombo = false;

        NowActionId = string.Empty;
    }


    public void StartAction(string nextAction)
    {
        StateCtrl.SetTrigger(nextAction);
        StateCtrl.SwitchLayer(1);
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

        canInterrupt = false;

        enableCombo = false;
        isCombo = false;

        NowActionId = string.Empty;
        StateCtrl.SwitchLayer(0);
    }



    private int frozenFrame = 0;

    public void HitFrozenFrame()
    {
        frozenFrame = 6;
        StateCtrl.HitFrozenFrame();
    }

    public void OnBoxHit(AtkBox box, Collider2D other)
    {
        int idx = AtkBoxes.IndexOf(box);

        HitBox oppo = other.GetComponent<HitBox>();
        int skill = SKillMap[idx];
        ApplyAtk();
        Vector2 pos = GetHitEffectPosition(box,oppo);
        oppo.owner.GetHit();
        GameObject.Instantiate(Resources.Load<GameObject>("Effect01"),pos,Quaternion.identity,null);
        //GameObject.Instantiate();
    }

    public void ApplyAtk()
    {
        HitFrozenFrame();

    }



    public Vector2 GetHitEffectPosition(AtkBox self, HitBox oppo)
    {
        Vector2 hit = new Vector2(1, 1);

        float self_pos_x = self.transform.position.x + self.atkCollider.offset.x;
        float self_pos_y = self.transform.position.y + self.atkCollider.offset.y;

        float oppo_pos_x = oppo.transform.position.x + oppo.hitCollider.offset.x;
        float oppo_pos_y = oppo.transform.position.y + oppo.hitCollider.offset.y;

        if (self_pos_y + self.atkCollider.size.y / 2 >= oppo_pos_y + oppo.hitCollider.size.y / 2)
        {
            if (self_pos_y - self.atkCollider.size.y / 2 <= oppo_pos_y - oppo.hitCollider.size.y / 2)
            {
                hit = oppo.transform.position;
            }
            else
            {
                hit = new Vector2(oppo.transform.position.x, (self_pos_y - self.atkCollider.size.y / 2) + ((oppo_pos_y + oppo.hitCollider.size.y / 2) - (self_pos_y - self.atkCollider.size.y / 2)) / 2);
            }
        }
        else if (self_pos_y + self.atkCollider.size.y / 2 < oppo_pos_y + oppo.hitCollider.size.y / 2)
        {
            if (self_pos_y - self.atkCollider.size.y / 2 >= oppo_pos_y - oppo.hitCollider.size.y / 2)
            {
                hit = new Vector2(oppo.transform.position.x, self_pos_y);
            }
            else if (self_pos_y - self.atkCollider.size.y / 2 < oppo_pos_y - oppo.hitCollider.size.y / 2)
            {
                hit = new Vector2(oppo.transform.position.x, (self_pos_y + self.atkCollider.size.y / 2) - ((self_pos_y + self.atkCollider.size.y / 2) - (oppo_pos_y - oppo.hitCollider.size.y / 2)) / 2);
            }
        }

        hit += Random.insideUnitCircle * 0.1f;

        return hit;
    }


}
