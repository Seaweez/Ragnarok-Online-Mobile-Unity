using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using RO;
using UnityEditor.SceneManagement;
namespace EditorTool
{
    public class SceneLightMapHelper
    {
        [MenuItem("LightMap/Helper")]
        static public void LightMapHelper()
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[]  goes = scene.GetRootGameObjects();
            GameObject go = null;
            for (int i = 0; i < goes.Length; i++)
            {
                Transform tf = goes[i].transform.Find("static");
                
                if(null != tf)
                {
                    go = goes[i];
                    break;
                }
            }

            //ÕÒµ½½Úµã
            if(null != go)
            {
                SceneLightMapMono sl = go.GetComponent<SceneLightMapMono>();
                if(null == sl)
                {
                    sl = go.AddComponent<SceneLightMapMono>();
                }
                sl.Clear();
                sl.SaveLightInfo();


                Object p = PrefabUtility.GetPrefabParent(go);

                PrefabUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);
            }
                


        }

    }
}
