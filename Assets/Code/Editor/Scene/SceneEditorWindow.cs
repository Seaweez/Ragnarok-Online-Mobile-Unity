using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Utils;
using Ghost.Config;
using Ghost.Extensions;
using RO;
using UnityEditor.SceneManagement;

namespace EditorTool
{
	public class SceneEditorWindow : EditorWindow
	{
		[MenuItem ("Window/SceneEditor")]
		static void ShowWindow ()
		{       
			EditorWindow.GetWindow<SceneEditorWindow>(false, "SceneEditor", true);	
		}

		private string outputPath = string.Empty;

		private Object outputFolder = null;
		private GameObject outputObj = null;
		private bool copyNavMesh = false;
		private bool copyLightMap = false;
		private bool bakeNavMesh = false;
		private bool bakeLightMap = false;

		int phaseCount = 4;
		int phase = 0;

		private bool running
		{
			get
			{
				return phase < phaseCount;
			}
		}

		private string Progress()
		{
			string phaseName = "Running";
			switch (phase)
			{
			case 0:
				if (bakeNavMesh)
				{
					phase = 1;
					NavMeshCommands.DoSetStatic();
					UnityEditor.AI.NavMeshBuilder.BuildNavMeshAsync();
				}
				else if (bakeLightMap)
				{
					phase = 2;
					Lightmapping.BakeAsync();
				}
				else
				{
					phase = 3;
				}
				break;
			case 1:
				if (bakeNavMesh && UnityEditor.AI.NavMeshBuilder.isRunning)
				{
					phaseName = "Baking NavMesh";
				}
				else
				{
					++phase;
				}
				break;
			case 2:
				if (bakeLightMap && Lightmapping.isRunning)
				{
					phaseName = "Baking LightMap";
				}
				else
				{
					++phase;
				}
				break;
			default:
				phaseName = "Ending";
				if (!string.IsNullOrEmpty(outputPath))
				{
					var sceneFilter = Path.ChangeExtension("*", PathConfig.EXTENSION_SCENE);
					var scenes = Directory.GetFiles(outputPath, sceneFilter);
					if (null != scenes)
					{
						if (1 == scenes.Length)
						{
							var bakeFolderName = Path.GetFileNameWithoutExtension(scenes[0]);
							var bakeFolderPath = PathUnity.Combine(outputPath, bakeFolderName);
							
							if (Directory.Exists(bakeFolderPath))
							{
								var srcPath = Path.ChangeExtension(EditorSceneManager.GetActiveScene().path, "").TrimEnd('.');
								if (copyNavMesh)
								{
									var navMeshAsset = PathUnity.Combine(srcPath, PathConfig.FILE_NAV_MESH);
									if (File.Exists(navMeshAsset))
									{
										File.Copy(navMeshAsset, PathUnity.Combine(bakeFolderPath, PathConfig.FILE_NAV_MESH), true);
									}
								}
								if (copyLightMap)
								{
									var lightMaps = Directory.GetFiles(srcPath, Path.ChangeExtension("*", PathConfig.EXTENSION_LIGHT_MAP));
									if (!lightMaps.IsNullOrEmpty())
									{
										foreach (var file in lightMaps)
										{
											File.Copy(file, PathUnity.Combine(bakeFolderPath, Path.GetFileName(file)), true);
										}
									}
								}
							}
						}
					}
				}

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				phase = phaseCount;
				break;
			}
			return phaseName;
		}

		private void HandlePrefab(GameObject prefab)
		{
			if (null == prefab)
			{
				return;
			}
			#region T4M
			var t4ms = prefab.FindComponentsInChildren<T4MObjSC>();
			if (!t4ms.IsNullOrEmpty())
			{
				foreach (var t4m in t4ms)
				{
					var go = t4m.gameObject;
					var collider = go.GetComponent<Collider>();
					if (null != collider)
					{
						Component.DestroyImmediate(collider, true);
					}
					Component.DestroyImmediate(t4m, true);
				}
			}
			var t4mps = prefab.FindComponentsInChildren<T4MPartSC>();
			if (!t4mps.IsNullOrEmpty())
			{
				foreach (var t4m in t4mps)
				{
					var go = t4m.gameObject;
					var collider = go.GetComponent<Collider>();
					if (null != collider)
					{
						Component.DestroyImmediate(collider, true);
					}
					Component.DestroyImmediate(t4m, true);
				}
			}
			#endregion T4M

			#region Renderer
//			var renderers = prefab.FindComponentsInChildren<Renderer>();
//			if (!renderers.IsNullOrEmpty())
//			{
//				foreach (var r in renderers)
//				{
//					OptimizationUtils.OptiRenderer(r);
//				}
//			}
			#endregion Renderer

//			#region layer
//			prefab.transform.Foreach(delegate(Transform t){
//				if (RO.Config.Layer.TERRAIN.Value == t.gameObject.layer)
//				{
//					t.gameObject.layer = RO.Config.Layer.STATIC_OBJECT.Value;
//				}
//				return true;
//			});
//			#endregion layer

//			var sbFinder = prefab.AddComponent<SceneObjectFinder>();
//			sbFinder.type = 4;
//			sbFinder.ID = 1;
		}

		void Awake () 
		{
			
		}
		
		//绘制窗口时调用
		void OnGUI () 
		{
			EditorGUILayout.BeginVertical();
			
			outputFolder = EditorGUILayout.ObjectField("Output Folder", outputFolder, typeof(Object), false);
			if (null != outputFolder)
			{
				outputPath = AssetDatabase.GetAssetPath(outputFolder);
				if (!AssetDatabase.IsValidFolder(outputPath))
				{
					outputFolder = null;
					outputPath = string.Empty;
				}
			}
			outputObj = EditorGUILayout.ObjectField("Output Object", outputObj, typeof(GameObject), true) as GameObject;
			
			copyNavMesh = EditorGUILayout.ToggleLeft("Copy NavMesh", copyNavMesh);
			copyLightMap = EditorGUILayout.ToggleLeft("Copy LightMap", copyLightMap);
			bakeNavMesh = EditorGUILayout.ToggleLeft("Bake NavMesh", bakeNavMesh);
			if (bakeNavMesh)
			{
				copyNavMesh = true;
			}
			bakeLightMap = EditorGUILayout.ToggleLeft("Bake LightMap", bakeLightMap);
			if (bakeLightMap)
			{
				copyLightMap = true;
			}
			
			if (GUILayout.Button("Run"))
			{
				if (!string.IsNullOrEmpty(outputPath))
				{
					if (null != outputObj)
					{
						var outputObjPath = Path.ChangeExtension(PathUnity.Combine(outputPath, outputObj.name), PathConfig.EXTENSION_PREFAB);
						HandlePrefab(PrefabUtility.CreatePrefab(outputObjPath, outputObj, ReplacePrefabOptions.ReplaceNameBased));
					}
				}
				
				if (bakeNavMesh)
				{
					if (UnityEditor.AI.NavMeshBuilder.isRunning)
					{
						UnityEditor.AI.NavMeshBuilder.Cancel();
					}
				}
				
				if (bakeLightMap)
				{
					if (Lightmapping.isRunning)
					{
						Lightmapping.Cancel();
					}
				}
				
				phase = 0;
			}
			
			EditorGUILayout.EndVertical();
		}
		
		//更新
		void Update()
		{
			if (running)
			{
				var phaseName = Progress();
				if (running)
				{
					EditorUtility.DisplayProgressBar("SceneEditor", phaseName, phase/(float)phaseCount);
				}
				else
				{
					EditorUtility.ClearProgressBar();
				}
			}
		}
		
		// 当窗口获得焦点时调用一次
		void OnFocus()
		{
			
		}
		
		// 当窗口丢失焦点时调用一次
		void OnLostFocus()
		{
			
		}
		
		// 当Hierarchy视图中的任何对象发生改变时调用一次
		void OnHierarchyChange()
		{
			Repaint();
		}
		
		// 当Project视图中的资源发生改变时调用一次
		void OnProjectChange()
		{
			Repaint();
		}
		
		// "窗口面板的更新
		void OnInspectorUpdate()
		{
			// 这里开启窗口的重绘，不然窗口信息不会刷新
			Repaint();
		}
		
		//当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
		void OnSelectionChange()
		{
			Repaint();
		}
		
		// 当窗口关闭时调用
		void OnDestroy()
		{
			
		}
	}
} // namespace EditorTool
