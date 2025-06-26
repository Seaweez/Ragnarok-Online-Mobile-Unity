using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EditorTool
{
    public class ETopToolBar : EComponentBase
    {
        public List<EComponentBase> drawQueue = new List<EComponentBase>();
        public ETopToolBar()
        {

        }

        public override void Draw()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            drawQueue.ForEach(p => p.Draw());
            EditorGUILayout.EndHorizontal();
        }

        virtual public void AddToDrawQueue(EComponentBase component)
        {
            if (component != null)
            {
                drawQueue.Add(component);
            }
            else
            {
                Debug.LogError("component is null !");
            }
        }

        public void InsertFlexibleSpace()
        {
            EComponentBase space = new EFlexibleSpace();
            drawQueue.Add(space);
        }
    }
}
