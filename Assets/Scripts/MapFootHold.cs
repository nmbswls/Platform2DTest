using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapFootHold : MonoBehaviour, IComparable<MapFootHold>
{
    public Vector2Int P1;
    public Vector2Int P2;

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





    public int CompareTo(MapFootHold other)
    {
        throw new NotImplementedException();
    }
}
