using UnityEngine;
using UnityEditor;

public class RingCreator
{
    [MenuItem("GameObject/3D Object/Ring", false, priority = 7)]
    public static void CreateCone()
    {
        SpawnRingInHierarchy();
    }

    private static void SpawnRingInHierarchy()
    {
        Transform[] selections = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);

        if (selections.Length <= 0)
        {
            GameObject cone = new GameObject("Ring");
            cone.transform.position = Vector3.zero;
            cone.transform.rotation = Quaternion.identity;
            cone.transform.localScale = Vector3.one;
            //设置创建操作可撤销
            Undo.RegisterCreatedObjectUndo(cone, "Undo Creating Ring");
            RingCreatorInPlaying.SetMesh(cone);
            return;
        }

        foreach (Transform selection in selections)
        {
            GameObject cone = new GameObject("Ring");
            cone.transform.SetParent(selection);
            cone.transform.localPosition = Vector3.zero;
            cone.transform.localRotation = Quaternion.identity;
            cone.transform.localScale = Vector3.one;
            //设置创建操作可撤销
            Undo.RegisterCreatedObjectUndo(cone, "Undo Creating Ring");
            RingCreatorInPlaying.SetMesh(cone);
        }
    }
}