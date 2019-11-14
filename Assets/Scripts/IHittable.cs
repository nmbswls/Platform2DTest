using UnityEngine;
using System.Collections;

public interface IHittable
{
    HitBox hitBox { get;}
    void GetHit();
}
