using UnityEngine;
using UnityEditor;

namespace EditorTool
{
    public class EFlexibleSpace : EComponentBase
    {
        public override void Draw()
        {
            GUILayout.FlexibleSpace();
        }
    }
}
