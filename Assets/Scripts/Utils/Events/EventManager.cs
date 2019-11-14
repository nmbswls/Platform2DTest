using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CtmEventManager
{
    private static Dictionary<Type, List<string>> ListenerDict;

    static CtmEventManager()
    {
        ListenerDict = new Dictionary<Type, List<string>>();

    }
}
