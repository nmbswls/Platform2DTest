using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapFootHoldTree : MonoBehaviour
{

    private Vector2Int p1;
    private Vector2Int p2;
    private Vector2Int center;

    private List<MapFootHold> footholds;

    public MapFootHoldTree(Vector2Int p1, Vector2Int p2)
    {
        //ss
        this.p1 = p1;
        this.p2 = p2;
        center = new Vector2Int((p2.x - p1.x) / 2, (p2.y - p1.y) / 2);
    }

    public MapFootHoldTree(Vector2Int p1, Vector2Int p2, int depth)
    {
        this.p1 = p1;
        this.p2 = p2;
        center = new Vector2Int((p2.x - p1.x) / 2, (p2.y - p1.y) / 2);
    }


    private List<MapFootHold> getRelevants(Vector2Int p)
    {
        return getRelevants(p, new List<MapFootHold>());
    }

    private List<MapFootHold> getRelevants(Vector2Int p, List<MapFootHold> list)
    {
        list.AddRange(footholds);

        return list;
    }

    public MapFootHold findBelow(Vector2Int p)
    {
        List<MapFootHold> relevants = getRelevants(p);
        // find fhs with matching x coordinates
        List<MapFootHold> xMatches = new List<MapFootHold>();
        foreach (MapFootHold fh in relevants)
        {
            if (fh.getX1() <= p.x && fh.getX2() >= p.x)
            {
                xMatches.Add(fh);
            }
        }
        xMatches.Sort();
        foreach (MapFootHold fh in xMatches)
        {
            if (!fh.isWall() && fh.getY1() != fh.getY2())
            {
                float calcY;
                float s1 = Mathf.Abs(fh.getY2() - fh.getY1());
                float s2 = Mathf.Abs(fh.getX2() - fh.getX1());
                float s4 = Mathf.Abs(p.x - fh.getX1());
                float alpha = Mathf.Atan(s2 / s1);
                float beta = Mathf.Atan(s1 / s2);
                float s5 = Mathf.Cos(alpha) * (s4 / Mathf.Cos(beta));
                if (fh.getY2() < fh.getY1())
                {
                    calcY = fh.getY1() - (int)s5;
                }
                else
                {
                    calcY = fh.getY1() + (int)s5;
                }
                if (calcY >= p.y)
                {
                    return fh;
                }
            }
            else if (!fh.isWall())
            {
                if (fh.getY1() >= p.y)
                {
                    return fh;
                }
            }
        }
        return null;
    }



    public int getX1()
    {
        return p1.x;
    }

    public int getX2()
    {
        return p2.x;
    }

    public int getY1()
    {
        return p1.y;
    }

    public int getY2()
    {
        return p2.y;
    }
}
