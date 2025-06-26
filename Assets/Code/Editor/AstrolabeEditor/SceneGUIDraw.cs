using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Star))]
public class StarScriptSceneGUIDraw : Editor
{
    void Awake()
    {
    }

    void OnSceneGUI()
    {
        Handles.BeginGUI();
        //if (AstrolabeManager.instance.chooseList.Count == 1)
        //{
        //    if (GUI.Button(new Rect(10, 10, 100, 50), "删除点"))
        //        AstrolabeManager.instance.HideStar();
        //}
        //else if(AstrolabeManager.instance.chooseList.Count == 2)
        //{
        //    if (GUI.Button(new Rect(10, 10, 100, 50), "删除线"))
        //        AstrolabeManager.instance.UnLink2Star();
        //    if (GUI.Button(new Rect(120, 10, 100, 50), "连线"))
        //        AstrolabeManager.instance.Link2Star();
        //}
        Handles.EndGUI();
        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.Return))
                    {
                        AstrolabeManager.instance.Link2Star();
                    }
                    if (Event.current.keyCode == (KeyCode.Backspace))
                    {
                        if (AstrolabeManager.instance.chooseList.Count == 1)
                            AstrolabeManager.instance.HideStar();
                        else
                            AstrolabeManager.instance.UnLink2Star();
                    }
                    break;
                }
        }
    }
}

[CustomEditor(typeof(AstrolabePath))]
public class AstrolabePathSceneGUIDraw : Editor
{
    public AstrolabePathSceneGUIDraw()
    {

    }

    void OnSceneGUI()
    {
        Handles.BeginGUI();
        //if (GUI.Button(new Rect(10, 10, 100, 50), "删除线"))
        //    PathUnLink();
        Handles.EndGUI();

        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    //if (Event.current.keyCode == (KeyCode.Return))
                    //{
                    //    AstrolabeManager.instance.Link2Star();
                    //}
                    if (Event.current.keyCode == (KeyCode.Backspace))
                    {
                        PathUnLink();
                    }
                    break;
                }
        }
    }

    bool PathUnLink()
    {
        AstrolabePath path = Selection.activeGameObject.GetComponent<AstrolabePath>();
        path.UnLink();
        return true;
    }
}

[CustomEditor(typeof(Astrolabe))]
public class AstrolabeSceneGUIDraw : Editor
{
    public AstrolabeSceneGUIDraw()
    {

    }

    int evoValue = -1;
    int unlocklv = -1;
    void OnSceneGUI()
    {
        Astrolabe _Astrolabe = target as Astrolabe;
        if (evoValue < 0)
        {
            evoValue = _Astrolabe.evo;
        }
        if (unlocklv < 0)
        {
            unlocklv = _Astrolabe.unlockLv;
        }
        Handles.BeginGUI();
        //if (GUI.Button(new Rect(10, 10, 100, 50), "删除星盘"))
        //    DeleteAstrolabe(_Astrolabe);
        GUI.Label(new Rect(10, 10, 50, 20), "解锁职业", EditorStyles.miniLabel);
        evoValue = EditorGUI.IntField(new Rect(60, 10, 100, 20), evoValue);
        GUI.Label(new Rect(10, 40, 50, 20), "解锁等级", EditorStyles.miniLabel);
        unlocklv = EditorGUI.IntField(new Rect(60, 40, 50, 20), unlocklv);
        if(GUI.Button(new Rect(10, 70, 100, 30), "更改解锁条件"))
        {
            _Astrolabe.ModiffyUnlock(evoValue, unlocklv);
        }
        Handles.EndGUI();

        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.Backspace))
                    {
                        DeleteAstrolabe(_Astrolabe);
                    }
                    break;
                }
        }
    }

    bool DeleteAstrolabe(Astrolabe _Astrolabe)
    {
        bool b = false;
        if (_Astrolabe != null)
        {
            _Astrolabe.UnLinkOuterConnects();
            AstrolabeManager.instance.RemoveAstrolabe(_Astrolabe);
            GameObject.DestroyImmediate(_Astrolabe.gameObject);
        }
        return b;
    }
}

[CustomEditor(typeof(AstrolabeUIElement))]
public class AstrolabeUIElementSceneGUIDraw : Editor
{
    AstrolabeUIElement _UI;
    void Awake()
    {
        _UI = target as AstrolabeUIElement;
    }

    void OnSceneGUI()
    {
        _UI = target as AstrolabeUIElement;
        Handles.BeginGUI();
        //if (GUI.Button(new Rect(10, 10, 100, 50), "删除UI元素"))
        //    DeleteUIElement(_UI);
        Handles.EndGUI();

        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.Backspace))
                    {
                        DeleteUIElement(_UI);
                    }
                    break;
                }
        }
    }

    void DeleteUIElement(AstrolabeUIElement ui)
    {
        AstrolabeManager.instance.DeleteUIElement(ui);
    }
}