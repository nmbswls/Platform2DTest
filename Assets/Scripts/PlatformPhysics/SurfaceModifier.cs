using UnityEngine;
using System.Collections;

public class SurfaceModifier : MonoBehaviour 
    {
        [Header("Friction")]
        /// the amount of friction to apply to a CorgiController walking over this surface      
        public float Friction;

        [Header("Force")]
        /// the amount of force to add to a CorgiController walking over this surface
        public Vector2 AddedForce=Vector2.zero;

        protected PlatformController _controller;
        //protected Character _character;

        /// <summary>
        /// Triggered when a CorgiController collides with the surface
        /// </summary>
        /// <param name="collider">Collider.</param>
        public virtual void OnTriggerStay2D(Collider2D collider)
        {
            _controller = collider.gameObject.GetComponent<PlatformController>();
            //_character = collider.gameObject.GetComponentNoAlloc<Character>();
        }

        /// <summary>
        /// On trigger exit, we lose all reference to the controller and character
        /// </summary>
        /// <param name="collision"></param>
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            _controller = null;
            //_character = null;
        }

        /// <summary>
        /// On Update, we make sure we have a controller and a live character, and if we do, we apply a force to it
        /// </summary>
        protected virtual void Update()
        {
            if (_controller == null)
            {
                return;
            }

            //if (_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
            //{
            //    _character = null;
            //    _controller = null;
            //    return;
            //}

            _controller.AddHorizontalForce(AddedForce.x);
            _controller.AddVerticalForce(Mathf.Sqrt(2f * AddedForce.y * -_controller.Parameters.Gravity));
        }
    }
