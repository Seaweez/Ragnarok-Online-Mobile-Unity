using RO;
using SLua;
using UnityEngine;

[ExecuteInEditMode]
public class AstrolabeUIElement : MonoBehaviour
{
#if UNITY_EDITOR
    public enum AstrolabeUIElementStyle
    {
        ditu2,
        ditu3,
        ditu4,
        ditu5
    }
    public AstrolabeUIElementStyle style;
    public UITexture texture;

    public void ResetElementTexture()
    {
        texture.mainTexture = AstrolabeManager.instance.GetElementTexture(style);
        texture.MakePixelPerfect();
    }

    public void SetElementStyle(AstrolabeUIElementStyle s)
    {
        style = s;
        ResetElementTexture();
    }

    public void InitByLuaTable(LuaTable table)
    {
        float[] info = new float[9];
        for(int i = 0; i <= 8; i++)
        {
            float f =LuaWorker.GetFieldFloat(table, i + 1);
            info[i] = f;
        }
        transform.localPosition = new Vector3(info[0], info[1], info[2]);
        transform.localEulerAngles = new Vector3(info[3], info[4], info[5]);
        transform.localScale = new Vector3(info[6], info[7], info[8]);
        if (transform.localScale == Vector3.zero)
            transform.localScale = Vector3.one;
    }
#endif
}