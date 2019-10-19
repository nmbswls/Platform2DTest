using UnityEngine;
using System.Collections;
using UnityEditor;
using CreativeSpore.SmartColliders;

[CustomEditor(typeof(MapFootHold))]
[CanEditMultipleObjects]
public class HandlerTest : Editor
{
    int arrowSize = 1;

    void OnSceneGUI()
    {
        MapFootHold footHold = (MapFootHold)target;
        Handles.color = Color.green;
        //显示的坐标文字
        //Handles.Label(arraw.transform.position + Vector3.up * 2, arraw.transform.position.ToString()
            //+ "\nShieldArea: " + arraw.shieldAre.ToString());

        //GUILayout.BeginArea(new Rect(Screen.width - 0, Screen.height - 80, 90, 50));
        //GUILayout.EndArea();

        //if (GUILayout.Button("Rest Area"))
        //{
        //    arraw.shieldAre += 1.0f;  //改变数值
        //}

        _DoBodyFreeMoveHandle(new Vector3(footHold.P1.x, footHold.P1.y),0);
        _DoBodyFreeMoveHandle(new Vector3(footHold.P2.x, footHold.P2.y),1);


        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        //画出弧线
        //Handles.DrawWireArc(arraw.transform.position, arraw.transform.up, -arraw.transform.right, 180, arraw.shieldAre);
        ////计算值
        //arraw.shieldAre = Handles.ScaleValueHandle(arraw.shieldAre,
            //arraw.transform.position + arraw.transform.forward * arraw.shieldAre,
            //arraw.transform.rotation, 1, Handles.ConeCap, 1);
    }



    private void _DoBodyFreeMoveHandle(Vector3 vPos, int idx)
    {
        MapFootHold footHold = (MapFootHold)target;
        //Vector3 Center = (footHold.P1 + footHold.P2) * 0.5f;
        Vector3 vTransform = footHold.transform.TransformPoint(vPos);

        EditorGUI.BeginChangeCheck();
        if(idx == 0)
        {
            Handles.color = Color.green;
            Handles.Label(vTransform + Vector3.up * 0.2f, "P1");
        }
        else if (idx == 1)
        {
            Handles.color = Color.red;
            Handles.Label(vTransform + Vector3.up * 0.2f, "P2");
        }

        //NOTE: vBodyHandler will be the body size change difference
        Vector3 vBodyHandler = Handles.FreeMoveHandle(vTransform, Quaternion.identity, 0.15f * HandleUtility.GetHandleSize(footHold.transform.position), Vector3.zero, EditorCompatibilityUtils.SphereCap) - vTransform;
        //Vector3 vBodyHandler = Handles.FreeMoveHandle(vTransform, Quaternion.identity, 0.15f * HandleUtility.GetHandleSize(footHold.transform.position), Vector3.zero, EditorCompatibilityUtils.SphereCap) - vTransform;
        vBodyHandler = footHold.transform.InverseTransformVector(vBodyHandler);

        if (EditorGUI.EndChangeCheck())
        {
            if (idx == 0)
            {
                Undo.RecordObject(target, "Modified Body Right");
                footHold.P1 += new Vector2(vBodyHandler.x, vBodyHandler.y);
            }else if (idx == 1)
            {
                Undo.RecordObject(target, "Modified Body Right");
                footHold.P2 += new Vector2(vBodyHandler.x, vBodyHandler.y);
            }
            EditorUtility.SetDirty(target);
        }
    }

}
