#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LocalizeUILabelsEditor
{
    [MenuItem("Localization/Add UILocalize to UILabels in Prefabs")]
    public static void AddUILocalizeToUILabelsInPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            bool prefabModified = false;

            foreach (UILabel label in prefab.GetComponentsInChildren<UILabel>(true))
            {
                if (label.gameObject.GetComponent<UILocalize>() == null)
                {
                    label.gameObject.AddComponent<UILocalize>();
                    prefabModified = true;
                }
            }

            if (prefabModified && PrefabUtility.IsPartOfAnyPrefab(prefab))
            {
                PrefabUtility.SavePrefabAsset(prefab);
                EditorUtility.SetDirty(prefab); // Mark the prefab as dirty
            }
        }
    }


    [MenuItem("Localization/Remove UILocalize from UILabels in Prefabs")]
    public static void RemoveUILocalizeFromUILabelsInPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            bool prefabModified = false;

            foreach (UILabel label in prefab.GetComponentsInChildren<UILabel>(true))
            {
                UILocalize localize = label.gameObject.GetComponent<UILocalize>();
                if (localize != null)
                {
                    Undo.DestroyObjectImmediate(localize); // Use Undo.DestroyObjectImmediate instead of Object.DestroyImmediate
                    prefabModified = true;
                }
            }

            if (prefabModified && PrefabUtility.IsPartOfAnyPrefab(prefab))
            {
                PrefabUtility.SavePrefabAsset(prefab);
                EditorUtility.SetDirty(prefab); // Mark the prefab as dirty
            }
        }
    }
}
#endif
