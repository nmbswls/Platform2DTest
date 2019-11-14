using UnityEngine;
using System.Collections;

public class Climbable : MonoBehaviour
{
    /// the different types of ladders
    public enum LadderTypes { Simple, BiDirectional }
    public GameObject LadderPlatform;

    public BoxCollider2D LadderPlatformBoxCollider;
    public LadderTypes LadderType;
    public bool CenterCharacterOnLadder;





    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        // we check that the object colliding with the ladder is actually a corgi controller and a character
        ActCharacterNew actChar = collider.GetComponent<ActCharacterNew>();
        if (actChar == null || actChar.GetAbility(typeof(ActClimbing))==null)
        {
            return;                 
        }
        ActClimbing actClimbing = actChar.GetAbility(typeof(ActClimbing)) as ActClimbing;
        actClimbing.LadderColliding=true;
        actClimbing.CurrentLadder = this;
    }

    /// <summary>
    /// Triggered when something stays on the ladder
    /// </summary>
    /// <param name="collider">Something colliding with the ladder.</param>
    protected virtual void OnTriggerStay2D(Collider2D collider)
    {


        //ActCharacterNew actChar = collider.GetComponent<ActCharacterNew>();
        //if (actChar == null || actChar.GetAbility(typeof(ActClimbing)) == null)
        //{
        //    return;
        //}

        //ActClimbing actClimbing = actChar.GetAbility(typeof(ActClimbing)) as ActClimbing;
        //actClimbing.LadderColliding = true;
        //actClimbing.CurrentLadder = this;
    }

    /// <summary>
    /// Triggered when something exits the ladder
    /// </summary>
    /// <param name="collider">Something colliding with the ladder.</param>
    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        // we check that the object colliding with the ladder is actually a corgi controller and a character
        ActCharacterNew actChar = collider.GetComponent<ActCharacterNew>();
        if (actChar == null || actChar.GetAbility(typeof(ActClimbing)) == null)
        {
            return;
        }
        ActClimbing actClimbing = actChar.GetAbility(typeof(ActClimbing)) as ActClimbing;
        // when the character is not colliding with the ladder anymore, we reset its various ladder related states          
        actClimbing.LadderColliding = false;
        actClimbing.CurrentLadder = null;               
    }
}
