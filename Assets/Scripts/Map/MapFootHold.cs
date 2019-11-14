using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
public class MapFootHold : MonoBehaviour, IComparable<MapFootHold>
{


    //public Vector2 Center;
    public Vector2 P1;
    public Vector2 P2;

    public bool CanJumpDown;
    public bool IsSlope;

    public SpriteRenderer sprite;

    public float shieldAre;

    //ss
    private int id;
    private int next, prev;

    [ContextMenu("Ajust")]
    void Adjust()
    {
        P1 = AdjustPos(P1,1);
        P2 = AdjustPos(P2,1);
        transform.localPosition = AdjustPos(transform.localPosition,1);
        InitCollider();
    }

    public static Vector2 AdjustPos(Vector2 pos, int digit)
    {
        float rate = 1;
        for(int i = 0; i < digit; i++)
        {
            rate *= 10;
        }
        float x = (int)Mathf.Round(pos.x * rate) / rate;
        float y = (int)Mathf.Round(pos.y * rate) / rate;

        return new Vector2(x,y);
    }



    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        float width = sprite.bounds.size.x;

        //float y = transform.position.y;
        //float x1 = transform.position.x - width * 0.5f;
        //float x2 = transform.position.x + width * 0.5f;

        //y = float.Parse(y.ToString("f3"));
        //x1 = float.Parse(x1.ToString("f3"));
        //x2 = float.Parse(x2.ToString("f3"));

        //P1 = new Vector2(x1, y);
        //P2 = new Vector2(x2, y);
        Adjust();

        //float originHeight = sprite.bounds.size.y;
        //sprite.size = new Vector2(P2.x-P1.x, originHeight);
        //InitCollider();

    }

    private void InitCollider()
    {


        DestroyImmediate(gameObject.GetComponent<Collider2D>()); 
        if (!isSlope())
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            float ColliderThick = 1f;
            ColliderThick = ColliderThick < sprite.size.y ? ColliderThick : sprite.size.y;
            float offsetY = sprite.size.y * 0.5f - ColliderThick * 0.5f;
            collider.size = new Vector2(sprite.size.x, ColliderThick);
            collider.offset = new Vector2(0, offsetY);
            collider.usedByEffector = true;
        }
        else
        {
            EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
            collider.points = new Vector2[] {P1,P2, P1.y>P2.y ? new Vector2(P1.x,P2.y): new Vector2(P2.x, P1.y) };
            collider.usedByEffector = true;
        }

        if (!isWall())
        {
            PlatformEffector2D effector2D = gameObject.GetComponent<PlatformEffector2D>();
            if (effector2D == null)
            {
                effector2D = gameObject.AddComponent<PlatformEffector2D>();
            }

            effector2D.sideArc = 160f;
            effector2D.useOneWay = true;
            effector2D.useOneWayGrouping = true;
        }


    }

    public float GetSlopeAngle()
    {
        return 0;
    }

    public bool isWall()
    {
        return (P1.x == P2.x);
    }

    public bool isSlope()
    {

        return P1.y != P2.y;
    }

    public float getX1() {
        return P1.x;
    }

    public float getX2() {
        return P2.x;
    }

    public float getY1() {
        return P1.y;
    }

    public float getY2() {
        return P2.y;
    }

    public int getId()
    {
        return id;
    }

    public int getNext()
    {
        return next;
    }

    public void setNext(int next)
    {
        this.next = next;
    }

    public int getPrev()
    {
        return prev;
    }

    public void setPrev(int prev)
    {
        this.prev = prev;
    }



    public int CompareTo(MapFootHold o)
    {
        MapFootHold other = (MapFootHold)o;
        if (P2.y < other.getY1())
        {
            return -1;
        }
        else if (P1.y > other.getY2())
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
