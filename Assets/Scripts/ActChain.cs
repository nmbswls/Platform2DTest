using UnityEngine;
using System.Collections;

public class ActChain : MonoBehaviour
{

    Vector3 originPos;
    Vector3 dir;


    float MaxLength = 5;
    SpriteRenderer chainBody;
    // Use this for initialization
    public void Init(Vector2 dir) {
        originPos = transform.position;
        this.dir = dir;
        chainBody = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * Time.deltaTime * 5f;
        float nowLength = (transform.position - originPos).magnitude;
        chainBody.size = new Vector2(nowLength, chainBody.size.y);
        chainBody.transform.localPosition = new Vector3(-nowLength/2,0,0);
        if(nowLength > MaxLength)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
