using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlatformMoveParameters
{
    [Header("Gravity")]
    /// Gravity
    public float Gravity = -30f;
    /// a multiplier applied to the character's gravity when going down
    public float FallMultiplier = 1f;
    /// a multiplier applied to the character's gravity when going up
    public float AscentMultiplier = 1f;

    [Header("Speed")]
    /// Maximum velocity for your character, to prevent it from moving too fast on a slope for example
    public Vector2 MaxVelocity = new Vector2(100f, 100f);
    /// Speed factor on the ground
    public float SpeedAccelerationOnGround = 20f;
    /// Speed factor in the air
    public float SpeedAccelerationInAir = 5f;
    /// general speed factor
    public float SpeedFactor = 1f;

    [Header("Slopes")]
    /// Maximum angle (in degrees) the character can walk on
    [Range(0, 90)]
    public float MaximumSlopeAngle = 45f;
    /// the speed multiplier to apply when walking on a slope
    public AnimationCurve SlopeAngleSpeedFactor = new AnimationCurve(new Keyframe(-90f, 1f), new Keyframe(0f, 1f), new Keyframe(90f, 1f));

    [Header("Physics2D Interaction [Experimental]")]
    /// if set to true, the character will transfer its force to all the rigidbodies it collides with horizontally
    public bool Physics2DInteraction = false;
    /// the force applied to the objects the character encounters
    public float Physics2DPushForce = 2.0f;

    [Header("Gizmos")]
    /// if set to true, will draw the various raycasts used by the CorgiController to detect collisions in scene view if gizmos are active
    public bool DrawRaycastsGizmos = true;
    /// if this is true, warnings will be displayed if settings are not done properly
    public bool DisplayWarnings = true;
}

