using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActHud
{
    public Transform root;


    public List<GameObject> hints = new List<GameObject>();
    public void BindView(Transform root)
    {
        this.root = root;
    }

    public void ShowReactHud(MapReactor reactor)
    {
        GameObject HudHint = Resources.Load<GameObject>("ReactorHint");
        GameObject h = GameObject.Instantiate(HudHint, reactor.transform.position + new Vector3(0,0.4f),Quaternion.identity,root);
        hints.Add(h);
    }

    public void HideReactHud(int idx)
    {
        GameObject.Destroy(hints[idx].gameObject);
        hints.RemoveAt(idx);
    }
}

//public class ReactorHintView

public class ActGameMode
{
    public Map map;
    public ActHud HudRoot;

    public ActPlayerController playerController;

    public void Init(){
        map = new Map();
        HudRoot = new ActHud();
        HudRoot.BindView(GameObject.Find("Hud").transform);

        playerController = GameObject.Find("Player").GetComponent<ActPlayerController>();
    }

    public void LoadMap(string map = "default")
    {

    }


}
