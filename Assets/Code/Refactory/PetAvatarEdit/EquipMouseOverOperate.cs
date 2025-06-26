using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMouseOverOperate : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public Renderer[] renderers;
    public Axis axis;
    public bool bDisableDrag = false;
    public bool bEnableClick = false;

    Material[] materials;
    Color originColor;
    PetEquipEditHelper manager;

    private void Awake()
    {
        manager = FindObjectOfType<PetEquipEditHelper>();
        if (materials == null)
        {
            materials = new Material[renderers.Length];
            for (int i = 0, max = renderers.Length; i < max; ++i)
            {
                ConeCreatorInPlaying cone = renderers[i].GetComponent<ConeCreatorInPlaying>();
                if (cone) cone.Init();
                RingCreatorInPlaying ring = renderers[i].GetComponent<RingCreatorInPlaying>();
                if (ring) ring.Init();
                materials[i] = renderers[i].material;
            }
        }
        originColor = materials[0].color;

        UIEventTrigger eventTrigger = GetComponent<UIEventTrigger>();
        if (!eventTrigger) eventTrigger = gameObject.AddComponent<UIEventTrigger>();
        eventTrigger.onHoverOver.Add(new EventDelegate(OnMouseOverObj));
        eventTrigger.onHoverOut.Add(new EventDelegate(OnMouseExitObj));
        eventTrigger.onDrag.Add(new EventDelegate(OnMouseDragObj));
        eventTrigger.onRelease.Add(new EventDelegate(OnMouseUpObj));
        eventTrigger.onClick.Add(new EventDelegate(OnClickObj));
    }

    public void OnMouseOverObj()
    {
        for (int i = 0, max = materials.Length; i < max; ++i)
        {
            materials[i].color = Color.yellow;
        }
    }

    public void OnMouseExitObj()
    {
        for (int i = 0, max = materials.Length; i < max; ++i)
        {
            materials[i].color = originColor;
        }
    }

    public void OnMouseDragObj()
    {
        if (bDisableDrag) return;
        manager.OnDrag(axis);
    }

    public void OnMouseUpObj()
    {
        OnMouseExitObj();
        if (bDisableDrag) return;
        manager.OnDragOver();
    }

    public void OnClickObj()
    {
        OnMouseExitObj();
        if (!bEnableClick) return;
        manager.OnClickViewControl(gameObject.name);
    }

    private void OnDisable()
    {
        OnMouseExitObj();
    }
}
