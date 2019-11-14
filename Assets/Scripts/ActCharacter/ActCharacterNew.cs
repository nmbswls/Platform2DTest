using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterStates
{


    /// The possible character conditions
    public enum CharacterConditions
    {
        Normal,
        ControlledMovement,
        Frozen,
        Paused,
        Dead
    }

    /// The possible Movement States the character can be in. These usually correspond to their own class, 
    /// but it's not mandatory
    public enum MovementStates
    {
        Null,
        Idle,
        Walking,
        Falling,
        Running,
        Crouching,
        Crawling,
        Dashing,
        LookingUp,
        WallClinging,
        Jetpacking,
        Diving,
        Gripping,
        Dangling,
        Jumping,
        Pushing,
        DoubleJumping,
        WallJumping,
        LadderClimbing,
        SwimmingIdle,
        Gliding,
        Flying,
        FollowingPath,
        LedgeHanging,
        LedgeClimbing
    }
}

public class ActCharacterNew : MapObject
{

    public enum FacingDirections { Left, Right }

    public StateMachine<CharacterStates.MovementStates> MovementState;
    public StateMachine<CharacterStates.CharacterConditions> ConditionState;

    public Dictionary<Type,ActAbility> AbilityList = new Dictionary<Type,ActAbility>();

    public Controller controller;


    public bool IsFacingRight { get; set; }
    public bool CanFlip { get; set; }
    public Vector3 ModelFlipValue = new Vector3(-1, 1, 1);

    public GameObject CharacterModel;

    public float HorizontalMove;
    public PlatformController platformer;




    public void Start()
    {
        Init();
    }

    public void Update()
    {
        //get global rate
        Tick(Time.deltaTime);
    }

    public ActAbility GetAbility(Type type)
    {
        if (AbilityList.ContainsKey(type))
        {
            return AbilityList[type];
        }
        return null;
    }

    protected virtual void Init()
    {
        platformer = GetComponent<PlatformController>();
        CharacterModel = transform.GetChild(0).gameObject;

        MovementState = new StateMachine<CharacterStates.MovementStates>(gameObject,false);
        ConditionState = new StateMachine<CharacterStates.CharacterConditions>(gameObject, false);

        RegisterAbility();
        CanFlip = true;
    }

    private void RegisterAbility()
    {
        {
            ActAbility ability = new ActMoveH();
            ability.Initialization(this);
            AbilityList.Add(typeof(ActMoveH), ability);
        }
        {
            ActAbility ability = new ActClimbing();
            ability.Initialization(this);
            AbilityList.Add(typeof(ActClimbing), ability);
        }
        {
            ActAbility ability = new ActJump();
            ability.Initialization(this);
            AbilityList.Add(typeof(ActJump), ability);
        }
        {
            ActAbility ability = new ActDash();
            ability.Initialization(this);
            AbilityList.Add(typeof(ActDash), ability);
        }
        {
            ActAbility ability = new ActCrouch();
            ability.Initialization(this);
            AbilityList.Add(typeof(ActCrouch), ability);
        }
        {
            ActAbility ability = new ActWallCling();
            ability.Initialization(this);
            AbilityList.Add(typeof(ActWallCling), ability);
        }
        {
            ActAbility ability = new ActWallJump();
            ability.Initialization(this);
            AbilityList.Add(typeof(ActWallJump), ability);
        }
        {
            ActAbility ability = new ActGrip();
            ability.Initialization(this);
            AbilityList.Add(typeof(ActGrip), ability);
        }
    }


    public void Tick(float dTime)
    {

        //handle input
        foreach (ActAbility ability in AbilityList.Values)
        {
            ability.EarlyTick();
        }

        foreach (ActAbility ability in AbilityList.Values)
        {
            ability.Tick(dTime);
        }
    }

    public virtual void RecalculateRays()
    {
        platformer.SetRaysParameters();
    }

    public virtual void Face(FacingDirections facingDirection)
    {
        if (!CanFlip)
        {
            return;
        }

        // Flips the character horizontally
        if (facingDirection == FacingDirections.Right)
        {
            if (!IsFacingRight)
            {
                Flip();
            }
        }
        else
        {
            if (IsFacingRight)
            {
                Flip();
            }
        }
    }

    public virtual void Flip()
    {
        if (!CanFlip)
        {
            return;
        }
        FlipModel();
        IsFacingRight = !IsFacingRight;
    }
    public virtual void FlipModel()
    {
        if (CharacterModel != null)
        {
            CharacterModel.transform.localScale = Vector3.Scale(CharacterModel.transform.localScale, ModelFlipValue);
        }
    }

    public void ResetHorizontalSpeed()
    {
        ActMoveH ability = AbilityList[typeof(ActMoveH)] as ActMoveH;
        if(ability != null) { 
            ability.ResetHorizontalSpeed();
        }

    }

    public void GetOffTheLadder()
    {

    }

}
