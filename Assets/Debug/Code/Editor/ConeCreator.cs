using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ConeCreator
{

    [MenuItem("GameObject/3D Object/Cone",false,priority = 7)]
    public static void CreateCone()
    {
        SpawnConeInHierarchy();
    }

    private static void SpawnConeInHierarchy()
    {
        Transform[] selections = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);

        if (selections.Length <= 0)
        {
            GameObject cone = new GameObject("Cone");
            cone.transform.position = Vector3.zero;
            cone.transform.rotation = Quaternion.identity;
            cone.transform.localScale = Vector3.one;
            //设置创建操作可撤销
            Undo.RegisterCreatedObjectUndo(cone, "Undo Creating Cone");
            ConeCreatorInPlaying.SetMesh(cone);
            return;
        }

        foreach (Transform selection in selections)
        {
            GameObject cone = new GameObject("Cone");
            cone.transform.SetParent(selection);
            cone.transform.localPosition = Vector3.zero;
            cone.transform.localRotation = Quaternion.identity;
            cone.transform.localScale = Vector3.one;
            //设置创建操作可撤销
            Undo.RegisterCreatedObjectUndo(cone, "Undo Creating Cone");
            ConeCreatorInPlaying.SetMesh(cone);
        }
    }
}