using UnityEngine;
using RO;
using System;

[SLua.CustomLuaClassAttribute]
public class PetAvatarEditorHelper : MonoBehaviour
{
    Animator animMain;
    string[] strsStates;
    Action cbUndo;
    Action cbRedo;

    //目前返回所有AnimationClip的名字，若Clip名字与状态机的State名字不一致会出问题
    public string[] GetStates(Animator animator)
    {
        if (!animator) return new string[0];
        animMain = animator;
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        strsStates = new string[controller.animationClips.Length];
        for (int i = 0, max = strsStates.Length; i < max; ++i)
            strsStates[i] = controller.animationClips[i].name;
        return strsStates;
    }

    public void PlayAnim(string action, string defaultAction)
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        int nameHash = Animator.StringToHash(action);
        int defaultNameHash = Animator.StringToHash(defaultAction);
        for (int i = 0, max = animators.Length; i < max; ++i)
        {
            var a = animators[i];
            if (a.HasState(0, nameHash))
                a.Play(nameHash, -1);
            else if (a.HasState(0, defaultNameHash))
                a.Play(defaultNameHash, -1);
            else if (null != LuaLuancher.Me)
            {
                int hash = LuaLuancher.Me.defaultActionNameHash;
                if (a.HasState(0, hash))
                    a.Play(hash, -1);
            }
        }
    }

    public BoxCollider AutoCalculateCollider(Transform go)
    {
        Vector3 postion = go.position;
        Quaternion rotation = go.rotation;
        Vector3 scale = go.localScale;
        go.position = Vector3.zero;
        go.rotation = Quaternion.identity;
        go.localScale = Vector3.one;

        Vector3 center = Vector3.zero;
        Renderer[] renders = go.GetComponentsInChildren<Renderer>();
        for (int i = 0, length = renders.Length; i < length; ++i)
            center += renders[i].bounds.center;
        center /= renders.Length;

        Bounds bounds = new Bounds(center, Vector3.zero);
        for (int i = 0, length = renders.Length; i < length; ++i)
            bounds.Encapsulate(renders[i].bounds);

        BoxCollider boxCollider = go.gameObject.AddComponent<BoxCollider>();
        boxCollider.center = bounds.center - go.position;
        boxCollider.size = bounds.size;
        boxCollider.isTrigger = true;

        go.position = postion;
        go.rotation = rotation;
        go.localScale = scale;

        return boxCollider;
    }

    public void SetUndoListener(Action undo, Action redo)
    {
        cbUndo = undo;
        cbRedo = redo;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
            Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (cbRedo != null) cbRedo();
                }
                else
                {
                    if (cbUndo != null) cbUndo();
                }
            }
        }
    }
}
