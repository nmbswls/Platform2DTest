using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapFootHold : MonoBehaviour, IComparable<MapFootHold>
{
    public Vector2Int P1;
    public Vector2Int P2;
    //ss
    private int id;
    private int next, prev;

    public bool isWall()
    {
        return (P1.x == P2.x);
    }

    public bool isSlope()
    {
        return P1.y != P2.y;
    }

    public int getX1() {
        return P1.x;
    }

    public int getX2() {
        return P2.x;
    }

    public int getY1() {
        return P1.y;
    }

    public int getY2() {
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
