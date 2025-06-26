#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class AstrolabePath : MonoBehaviour
{
#if UNITY_EDITOR
    public Star PrevStar;
    public Star NextStar;
    public UISprite PathSprite;

    static Vector3 zeroVec3 = new Vector3(1, 0, 0);
    static string _PathSpritePrefabPath = "Assets/DevelopScene/AstrolabeEditorScene/line.prefab";

    public AstrolabePath()
    {

    }

    public void InitPathSprite(Star p, Star n)
    {
        PrevStar = p;
        NextStar = n;
        PathSprite.depth = 2;
    }

    public void BuildPath()
    {
        Vector3 pVec3 = PrevStar.transform.localPosition + PrevStar.transform.parent.localPosition;
        Vector3 nVec3 = NextStar.transform.localPosition + NextStar.transform.parent.localPosition;
        PathSprite.width = (int)Vector3.Distance(nVec3, pVec3);
        Vector3 dir = nVec3 - pVec3;
        float angleDir = Vector3.Cross(zeroVec3, dir).normalized.z;
        float angle = Vector3.Angle(zeroVec3, dir) * angleDir;
        transform.localEulerAngles = new Vector3(0, 0, angle);
        transform.localPosition = (nVec3 + pVec3) / 2 - transform.parent.localPosition;
    }

    public void Link()
    {
        PrevStar.Link(NextStar, this);
        NextStar.Link(PrevStar, this);
        PathSprite.gameObject.SetActive(true);
    }

    public void UnLink()
    {
        PrevStar.UnLink(NextStar);
        NextStar.UnLink(PrevStar);
        PathSprite.gameObject.SetActive(false);
    }

    public bool Contains(Star node)
    {
        if (node == PrevStar || node == NextStar)
            return true;
        return false;
    }

    public static AstrolabePath TryCreatePath(Star Prev, Star Next)
    {
        AstrolabePath _AsPath = Prev.TryGetPath(Next);
        if (_AsPath == null)
        {
#if UNITY_EDITOR
            string name = Prev.globalId.ToString() + "_" + Next.globalId.ToString();
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(_PathSpritePrefabPath);
            go = GameObject.Instantiate<GameObject>(go);
            go.transform.parent = Prev.transform.parent;
            go.transform.localScale = Vector3.one;
            go.name = name;
            _AsPath = go.GetComponent<AstrolabePath>();
#endif
        }
        return _AsPath;
    }
#endif
}