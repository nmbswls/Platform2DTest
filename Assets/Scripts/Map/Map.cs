using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Map
{

    public MapFootHoldTree footholds = null;
    public List<MapReactor> Reactors = new List<MapReactor>();
    public List<GameObject> Climbables = new List<GameObject>();

    public void Init()
    {

    }

    public Bounds GetLevelBound()
    {
        return new Bounds();
    }
    //
}
