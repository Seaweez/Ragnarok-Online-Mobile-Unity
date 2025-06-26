using UnityEngine;
using UnityEditor;
using System.Collections;
using RO;
using Ghost.Utils;

public class UISpineCreateHelper : MonoBehaviour
{

	[MenuItem ("Assets/Spine/CreateUISpine")]
	static void CreateUISpine ()
	{
		Object[] selects = AssetManager.getAllAssets (Path_SPINE_EMOJI);
		foreach (Object o in selects) {
			SkeletonDataAsset s = o as SkeletonDataAsset;
			if (s != null && s.atlasAsset != null) {
				AtlasAsset a = s.atlasAsset;
				if (a.materials.Length > 0) {
					Material m = a.materials [0];

					string path = "Assets/";
					path = PathUnity.Combine (path, "Resources/Public/Emoji");
					path = PathUnity.Combine (path, o.name + ".prefab");
					GameObject g = new GameObject ();
					g.name = o.name;

					g.AddComponent<MeshRenderer> ();
					g.AddComponent<MeshFilter> ();
					
					NGUISpine nus = g.AddComponent<NGUISpine> ();
					nus.skeletonDataAsset = s;
					var anims = s.GetSkeletonData (true).Animations;
					if (anims.Count > 0) {
						nus.Reset ();
						nus.AnimationName = anims [1].Name;
					}

					UISpine us = g.AddComponent<UISpine> ();
					us.spineMaterial = m;
					us.depth = 100;

					GameObject newGo = PrefabUtility.CreatePrefab (path, g);
					newGo.name = g.name;
					GameObject.DestroyImmediate (g);
				}
			}
		}
		AssetDatabase.Refresh ();
	}


	public const string Path_SPINE_EMOJI = "Assets/Art/Public/Texture/Spine/Emoji";
	public const string Path_Res_SPINE_EMOJI = "Assets/Resources/Public/Emoji";

	public const string Path_Skeleton = "skeleton";

	[MenuItem ("RO/Spine/CreateEmojiSpine")]
	static void CreateEmojiSpine ()
	{
		string skeleton_path = Path_SPINE_EMOJI + "/" + Path_Skeleton;

		Object[] emojiReses = AssetManager.getAllAssets (skeleton_path);
		foreach (Object o in emojiReses) {
			if (o.GetType () == typeof(TextAsset)) {
				// find skdata begin
				string skdata_path = skeleton_path + "/" + o.name + ".asset";
				SkeletonDataAsset s = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset> (skdata_path);
				if (s == null) {
					s = SkeletonDataAsset.CreateInstance<SkeletonDataAsset> ();
					s.atlasAsset = _getAtlasAsset (Path_SPINE_EMOJI);
					s.skeletonJSON = o as TextAsset;
					s.fromAnimation = new string[0];
					AssetDatabase.CreateAsset (s, skdata_path);
				}
				// find skdata end

				string pfb_path = Path_Res_SPINE_EMOJI + "/" + o.name + ".prefab";
				GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject> (pfb_path);
				if (prefab == null) {
					prefab = new GameObject ();
					prefab.name = o.name;
									
					prefab.AddComponent<MeshRenderer> ();
					prefab.AddComponent<MeshFilter> ();
									
					NGUISpine nus = prefab.AddComponent<NGUISpine> ();
					nus.skeletonDataAsset = s;
					var anims = s.GetSkeletonData (true).Animations;
					if (anims.Count > 0) {
						nus.Reset ();
						nus.AnimationName = anims [1].Name;
					}
									
					UISpine us = prefab.AddComponent<UISpine> ();
					us.spineMaterial = s.atlasAsset.materials [0];
					us.depth = 100;
									
					GameObject newGo = PrefabUtility.CreatePrefab (pfb_path, prefab);
					newGo.name = prefab.name;
					GameObject.DestroyImmediate (prefab);
				}
			}
			AssetDatabase.Refresh ();
		}
	}

	public const string Path_Atlas = "atlas";

	static AtlasAsset _getAtlasAsset (string path)
	{
		AtlasAsset atlasAsset = null;
		string atlas_path = path + "/" + Path_Atlas;
		Object[] atlasReses = AssetManager.getAllAssets (atlas_path);
		foreach (Object o in atlasReses) {
			if (o.GetType () == typeof(TextAsset)) {
				string atlasasset_path = atlas_path + "/" + o.name + ".asset";
				atlasAsset = AssetDatabase.LoadAssetAtPath<AtlasAsset> (atlasasset_path);
				if (atlasAsset == null) {
					atlasAsset = new AtlasAsset ();
					atlasAsset.atlasFile = o as TextAsset;
					AssetDatabase.CreateAsset (atlasAsset, atlasasset_path);
				}
			}
		}
		return atlasAsset;
	}
}
