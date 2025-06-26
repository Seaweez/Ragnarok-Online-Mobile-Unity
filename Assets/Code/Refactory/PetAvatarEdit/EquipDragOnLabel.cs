using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipDragOnLabel : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public PetEquipEditHelper.Mode mode;
    public Axis axis;

    PetEquipEditHelper manager;

    void Start()
    {
        manager = FindObjectOfType<PetEquipEditHelper>();
        UIEventTrigger eventTrigger = gameObject.AddComponent<UIEventTrigger>();
        eventTrigger.onDrag.Add(new EventDelegate(OnDrag));
        eventTrigger.onRelease.Add(new EventDelegate(OnDragOver));
    }

    public void OnDrag()
    {
        manager.OnLabelDrag(mode, axis);
    }

    public void OnDragOver()
    {
        if (!Input.GetMouseButton(0))
            manager.OnLabelDragOver(mode);
    }
}
