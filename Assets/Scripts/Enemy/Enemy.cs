using UnityEngine;
using System.Collections;

public class Enemy : MapObject, IHittable
{
    [HideInInspector]
    public HitBox hitBox { get { return mHitBox; }}

    public int Hp = 40;

    private HitBox mHitBox;
    // Use this for initialization
    void Start()
    {
        mHitBox = GetComponentInChildren<HitBox>();
        mHitBox.Init(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetHit()
    {
        Hp -= 2;
    }
}
