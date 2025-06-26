using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.IO;
using RO;
using RO.Config;
using SLua;
using Ghost.Utils;
using Ghost.Extensions;

namespace EditorTool
{
	public static partial class GenAssets
	{
		public static string AssetsSrcRootFolder = "Assets/Art";

		public static string AssetsSrcFolder_Model;
		public static string AssetsSrcFolder_Role;

		public static string AssetsSrcFolder_Public;
		public static string AssetsSrcFolder_Action;
		public static string AssetsSrcFolder_ActionBody;
		public static string AssetsSrcFolder_ActionHair;
		public static string AssetsSrcFolder_ActionEye;

		public static string AssetsRootFolder = "Assets/Resources";
		public static string AssetsFolder_SkillData;
		public static string AssetsFolder_NPC;

		public static string AssetsFolder_Role;
		public static string AssetsFolder_Hair;
		public static string AssetsFolder_Eye;
		public static string AssetsFolder_Body;
		public static string AssetsFolder_Mount;
		public static string AssetsFolder_Head;
		public static string AssetsFolder_Face;
		public static string AssetsFolder_Weapon;
		public static string AssetsFolder_Wing;
		public static string AssetsFolder_Tail;
		public static string AssetsFolder_Mouth;

		public static string AssetsFolder_Public;
		public static string AssetsFolder_Effect;
		public static string AssetsFolder_BusCarrier;
		public static string AssetsFolder_Item;
		public static string AssetsFolder_Stage;

		public static string AssetsFolder_UIEffect;

		public static string ScriptFolder;
		public static string TableFolder;
		public static string SkillLogicFolder;

		public static string AssetsPath_RoleComplete = "Assets/Engine/Refactory/RoleComplete";

		public static string ScriptExtension = "txt";
		public static string AssetExtension = "asset";
		public static string PrefabExtension = "prefab";
		public static string ModelExtension = "FBX";
		public static string AnimatorControllerExtension = "controller";
		public static string MaterialExtension = "mat";

		static GenAssets()
		{
			AssetsSrcFolder_Public = PathUnity.Combine(AssetsSrcRootFolder, "Public");
			AssetsSrcFolder_Action = PathUnity.Combine(AssetsSrcFolder_Public, "Animation");
			AssetsSrcFolder_ActionBody = PathUnity.Combine(AssetsSrcFolder_Action, "Body");
			AssetsSrcFolder_ActionHair = PathUnity.Combine(AssetsSrcFolder_Action, "Hair");
			AssetsSrcFolder_ActionEye = PathUnity.Combine(AssetsSrcFolder_Action, "Eye");

			AssetsSrcFolder_Model = PathUnity.Combine(AssetsSrcRootFolder, "Model");
			AssetsSrcFolder_Role = PathUnity.Combine(AssetsSrcFolder_Model, "Role");

			AssetsFolder_SkillData = PathUnity.Combine(AssetsRootFolder, "Skill");
			AssetsFolder_NPC = PathUnity.Combine(AssetsRootFolder, "NPC");

			AssetsFolder_Role = PathUnity.Combine(AssetsRootFolder, "Role");
			AssetsFolder_Hair = PathUnity.Combine(AssetsFolder_Role, "Hair");
			AssetsFolder_Eye = PathUnity.Combine(AssetsFolder_Role, "Eye");
			AssetsFolder_Body = PathUnity.Combine(AssetsFolder_Role, "Body");
			AssetsFolder_Mount = PathUnity.Combine(AssetsFolder_Role, "Mount");
			AssetsFolder_Head = PathUnity.Combine(AssetsFolder_Role, "Head");
			AssetsFolder_Face = PathUnity.Combine(AssetsFolder_Role, "Face");
			AssetsFolder_Weapon = PathUnity.Combine(AssetsFolder_Role, "Weapon");
			AssetsFolder_Wing = PathUnity.Combine(AssetsFolder_Role, "Wing");
			AssetsFolder_Tail = PathUnity.Combine(AssetsFolder_Role, "Tail");
			AssetsFolder_Mouth = PathUnity.Combine(AssetsFolder_Role, "Mouth");
			EquipAssetFolders = new string[]{
				AssetsFolder_Head,
				AssetsFolder_Face,
				AssetsFolder_Weapon,
				AssetsFolder_Wing,
				AssetsFolder_Tail,
				AssetsFolder_Mouth,
			};

			AssetsFolder_Public = PathUnity.Combine(AssetsRootFolder, "Public");
			AssetsFolder_Effect = PathUnity.Combine(AssetsFolder_Public, "Effect");
			AssetsFolder_BusCarrier = PathUnity.Combine(AssetsFolder_Public, "BusCarrier");
			AssetsFolder_Item = PathUnity.Combine(AssetsFolder_Public, "Item");
			AssetsFolder_Stage = PathUnity.Combine(AssetsFolder_Public,"StageParts");

			AssetsFolder_UIEffect = PathUnity.Combine(AssetsFolder_Effect, "UI");

			ScriptFolder = PathUnity.Combine(AssetsRootFolder, "Script");
//			TableFolder = PathUnity.Combine(ScriptFolder, "Config");
			var cehuaLuaTableDir = Application.dataPath;
			for (int i = 0; i < 3; ++i)
			{
				cehuaLuaTableDir = Path.GetDirectoryName(cehuaLuaTableDir);
			}
			TableFolder = PathUnity.Combine(cehuaLuaTableDir, "Cehua/Lua/Table");
			SkillLogicFolder = PathUnity.Combine(ScriptFolder, "Skill/Logic");
		}

		
		public class WorkerContainer : System.IDisposable
		{
			public LuaTable[] workers;
			public bool debug = false;

			public WorkerContainer(params string[] names)
			{
				var workerList = new List<LuaTable>();
				foreach (var name in names)
				{
					var worker = GetTableWorker(name);
					if (null != worker)
					{
						workerList.Add(worker);
					}
					else
					{
						Debug.LogFormat("<color=red>GetTableWorker({0})</color> failed", name);
					}
				}
				if (0 < workerList.Count)
				{
					workers = workerList.ToArray();
				}
			}

			public void Dispose()
			{
				if (!workers.IsNullOrEmpty())
				{
					foreach (var worker in workers)
					{
						worker.Dispose();
					}
					workers = null;
				}
			}
		}

		public static LuaTable GetTableWorker(string name)
		{
			var path = Path.ChangeExtension(PathUnity.Combine(TableFolder, name), ScriptExtension);
//			var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
//			if (null == asset)
//			{
//				return null;
//			}
//			var worker = LuaSvrForEditor.Me.DoString(asset.text) as LuaTable;
			try
			{
				var text = File.ReadAllText(path, System.Text.Encoding.UTF8);
				var worker = LuaSvrForEditor.Me.DoString(text) as LuaTable;
				return worker;
			}
			catch (System.Exception e)
			{
				Debug.LogException(e);
			}

			return null;
		}

		public static List<T> GetAllAssets<T>(string path, string typeName = null) where T:Object
		{
			if (string.IsNullOrEmpty(typeName))
			{
				typeName = typeof(T).Name;
			}
			var assets = new List<T>();
			var guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeName), new string[]{path});
			if (!guids.IsNullOrEmpty())
			{
				foreach (var guid in guids)
				{
					var obj = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
					if (null != obj)
					{
						assets.Add(obj);
					}
				}
			}
			return assets;
		}

		public static List<T>[] GetAllAssets<T>(string[] paths, string typeName = null) where T:Object
		{
			var assetsArray = new List<T>[paths.Length];
			for (int i = 0; i < assetsArray.Length; ++i)
			{
				assetsArray[i] = GetAllAssets<T>(paths[i], typeName);
			}
			return assetsArray;
		}

		public static Dictionary<string, T> GetUnhandledAssets<T>(string path, string typeName = null) where T:Object
		{
			if (string.IsNullOrEmpty(typeName))
			{
				typeName = typeof(T).Name;
			}
			var unhandled = new Dictionary<string, T>();
			var guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeName), new string[]{path});
			if (!guids.IsNullOrEmpty())
			{
				foreach (var guid in guids)
				{
					var obj = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
					if (null != obj)
					{
						if (unhandled.ContainsKey(obj.name))
						{
							Debug.LogFormat("<color=red>GetUnhandledAssets has same name: </color>{0}\n{1}", obj.name, path);
						}
						unhandled.Add(obj.name, obj);
					}
				}
			}
			return unhandled;
		}

		public static Dictionary<string, T>[] GetUnhandledAssets<T>(string[] paths, string typeName = null) where T:Object
		{
			var unhandled = new Dictionary<string, T>[paths.Length];
			for (int i = 0; i < unhandled.Length; ++i)
			{
				unhandled[i] = GetUnhandledAssets<T>(paths[i], typeName);
			}
			return unhandled;
		}

		public static Color GetDefaultColor(Dictionary<int, LuaTable> infos, int ID)
		{
			if (null != infos)
			{
				LuaTable info;
				if (infos.TryGetValue(ID, out info))
				{
					return GetDefaultColor(info);
				}
			}
			return Color.clear;
		}

		public static Color GetDefaultColor(LuaTable info)
		{
			if (null != info)
		    {
				var colorIndex = LuaWorker.GetFieldInt(info, "DefaultColor");
				return GetColor(info, colorIndex);
			}
			return Color.clear;
		}

		public static Color GetColor(Dictionary<int, LuaTable> infos, int ID, int colorIndex)
		{
			if (null != infos)
			{
				LuaTable info;
				if (infos.TryGetValue(ID, out info))
 				{
					return GetColor(info, colorIndex);
				}
			}
			return Color.clear;
		}

		public static Color GetColor(LuaTable info, int colorIndex)
		{
			if (null != info && 0 < colorIndex)
			{
				var paintColorTable = LuaWorker.GetFieldTable(info, "PaintColor");
				if (null != paintColorTable)
				{
					var colorNumber = LuaWorker.GetFieldLong(paintColorTable, colorIndex);
					if (0 != colorNumber)
					{
						var colorString = string.Format("#{0}", colorNumber.ToString("x8"));
						Color color;
						if (ColorUtility.TryParseHtmlString(colorString, out color))
						{
							return color;
						}
					}
				}
			}
			return Color.clear;
		}

		public static Dictionary<string, T> LoadAssetsInFolder<T>(string folder, string searchPattern) where T:Object
		{
			if (!Directory.Exists(folder))
			{
				return null;
			}
			var assets = new Dictionary<string, T>();
			string[] paths = Directory.GetFiles(folder, searchPattern);
			foreach (string path in paths) 
			{									
				var asset = AssetDatabase.LoadAssetAtPath<T>(path);
				if (null != asset)
				{
					assets.Add(asset.name, asset);
				}
			}
			return assets;
		}

		public static Dictionary<int, LuaTable> GetAssetsInfo(out WorkerContainer workerContainer, string assetName, params string[] tableNames)
		{
			return GetAssetsInfo_1(out workerContainer, assetName, null, tableNames);
		}

		public static Dictionary<int, LuaTable> GetAssetsInfo_1(out WorkerContainer workerContainer, string assetName, System.Func<LuaTable, string> getModelPathFunc, params string[] tableNames)
		{
			workerContainer = new WorkerContainer(tableNames);
			if (workerContainer.workers.IsNullOrEmpty())
			{
				return null;
			}
			var tables = workerContainer.workers;
			
			var infos = new Dictionary<int, LuaTable>();
			HashSet<string> debugModelPaths = null;

			HashSet<int> debugIDs = new HashSet<int>();
			var debugInfos = AssetDatabase.LoadAssetAtPath<GenAssetsDebug>("Assets/GenAssetsDebugInfo.asset");
			if (null != debugInfos && !debugInfos.infos.IsNullOrEmpty())
			{
				debugModelPaths = new HashSet<string>();
				foreach (var debugInfo in debugInfos.infos)
				{
					if (debugInfo.enable
					    && ArrayUtility.Contains(tableNames, debugInfo.tableName)
					    && !debugInfo.debugIDs.IsNullOrEmpty())
					{
						foreach (var ID in debugInfo.debugIDs)
						{
							debugIDs.Add(ID);
							foreach (var t in tables)
							{
								if (null != t)
								{
									var info = t[ID] as LuaTable;
									if (null != info)
									{
										var modelPath = getModelPathFunc(info);
										debugModelPaths.Add(modelPath);
									}
								}
							}
						}
					}
				}
			}

			if (0 < debugIDs.Count)
			{
				workerContainer.debug = true;
			}

			foreach (var table in tables)
			{
				if (null != table)
				{
					foreach (var key_value in table)
					{
						var info = key_value.value as LuaTable;
						var ID = System.Convert.ToInt32(key_value.key);
						if (0 < debugIDs.Count 
							&& !debugIDs.Contains(ID)
							&& !debugModelPaths.Contains(getModelPathFunc(info)))
						{
							continue;
						}
						if (infos.ContainsKey(ID))
						{
							var name = LuaWorker.GetFieldString(info, "NameZh");
							if (string.IsNullOrEmpty(name))
							{
								name = LuaWorker.GetFieldString(info, "NameEn");
								if (string.IsNullOrEmpty(name))
								{
									name = LuaWorker.GetFieldString(info, "Name");
								}
							}
							Debug.LogFormat("<color=red>{0} ID is not Unique: </color>ID={1}, name={2}", assetName, ID, name);
							continue;
						}
						infos.Add(ID, info);
					}
				}
			}
			
			return infos;
		}

		private static Dictionary<string, AnimationClip> GetAllClips(
			LuaTable info,
			string[] clipFolders, 
			Dictionary<int, LuaTable> actionInfos,
		    string actionBField)
		{
			if (clipFolders.IsNullOrEmpty())
			{
				return null;
			}
			var allClips = new Dictionary<string, AnimationClip>();
			foreach (var folder in clipFolders)
			{
				var clips = LoadAssetsInFolder<AnimationClip>(folder, "*.anim");
				if (!clips.IsNullOrEmpty())
				{
					foreach (var key_value in clips)
					{
						if (!allClips.ContainsKey(key_value.Key))
						{
							allClips.Add(key_value.Key, key_value.Value);
						}
					}
				}
			}
			
			#region action B
			if (!actionInfos.IsNullOrEmpty())
			{
				foreach (var key_value in actionInfos)
				{
					var actionInfo = key_value.Value;
					var actionName = LuaWorker.GetFieldString(actionInfo, "Name");
					if (allClips.ContainsKey(actionName))
					{
						continue;
					}
					var actionBName = LuaWorker.GetFieldString(actionInfo, actionBField);
					if (string.IsNullOrEmpty(actionBName))
					{
						continue;
					}
					AnimationClip clip;
					if (allClips.TryGetValue(actionBName, out clip))
					{
						allClips.Add(actionName, clip);
					}
				}
			}
			#endregion action B

			#region action reuse
			var reuseInfo = LuaWorker.GetFieldTable(info, "ReuseActions");
			if (null != reuseInfo)
			{
				foreach (var key_value in reuseInfo)
				{
					var actionName = key_value.key as string;
					var actionBName = key_value.value as string;
					if (null != actionName && null != actionBName)
					{
						AnimationClip clip;
						if (allClips.TryGetValue(actionBName, out clip))
						{
							allClips.Add(actionName, clip);
						}
					}
				}
			}
			#endregion action reuse
			return allClips;
		}
		private static AnimatorController GenAnimatorController(
			Dictionary<string, AnimationClip> allClips,
			string acPath,
			string[] defaultStateNames,
			System.Action<AnimatorState, AnimationClip> callback,
			Dictionary<string, List<string>> loopClips)
		{
			if (allClips.IsNullOrEmpty())
			{
				return null;
			}
			var animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(acPath);
			if (null == animatorController 
				|| null == animatorController.layers
				|| 0 >= animatorController.layers.Length)
			{
				animatorController = AnimatorController.CreateAnimatorControllerAtPath (acPath);
			}
			
			var acLayer = animatorController.layers [0];
			var acStateMachine = acLayer.stateMachine;
			
			var unhandledACStates = new Dictionary<string, AnimatorState>();
			var acStates = acStateMachine.states;
			if (!acStates.IsNullOrEmpty())
			{
				foreach (var s in acStates)
				{
					unhandledACStates.Add(s.state.name, s.state);
				}
			}
			
			int defaultStateIndex = 9999;
			foreach (var key_value in allClips)
			{
				var stateName = key_value.Key;
				var clip = key_value.Value;
				AnimatorState state;
				if (unhandledACStates.TryGetValue(stateName, out state))
				{
					unhandledACStates.Remove(stateName);
				}
				else
				{
					state = acStateMachine.AddState (stateName);
				}
				state.motion = clip;
				
				if (!defaultStateNames.IsNullOrEmpty())
				{
					var stateIndex = ArrayUtility.IndexOf(defaultStateNames, stateName);
					if (0 <= stateIndex && stateIndex < defaultStateIndex)
					{
						defaultStateIndex = stateIndex;
						acStateMachine.defaultState = state;
					}
				}

				if (null != callback)
				{
					callback(state, clip);
				}
			}

			if (null != loopClips)
			{
				foreach (var key_value in loopClips)
				{
					var clips = new List<AnimationClip>();
					foreach (var clipName in key_value.Value)
					{
						AnimationClip clip;
						if (allClips.TryGetValue(clipName, out clip))
						{
							clips.Add(clip);
						}
					}
					if (1 < clips.Count)
					{
						var loopClipName = key_value.Key;
						AnimatorState firstState = null;
						AnimatorState prevState = null;
						for (int i = 0; i < clips.Count; ++i)
						{
							var state = acStateMachine.AddState (key_value.Key);
							var clip = clips[i];
							state.motion = clip;
							state.name = string.Format("{0}_{1}", loopClipName, i+1);

							if (null != prevState)
							{
								SetUniqueExitTransition(prevState, state);
							}

							if (null != callback)
							{
								callback(state, clip);
							}
							prevState = state;
							if (null == firstState)
							{
								firstState = state;
							}
						}
						SetUniqueExitTransition(prevState, firstState);
					}
				}
			}
			
			foreach (var state in unhandledACStates.Values)
			{
				acStateMachine.RemoveState(state);
			}

			return animatorController;
		}
		public static GameObject GenPrefabsAndController(
			AssetInfo assetInfo,
			string srcFolder, 
			string srcName, 
			string[] clipFolders, 
			string targetPath,
			LuaTable info,
			Dictionary<int, LuaTable> actionInfos,
			string actionBField,
			string[] defaultStateNames,
			System.Action<AnimatorState, AnimationClip> callback = null)
		{
			Path.GetFileNameWithoutExtension(targetPath);
			var srcPath = PathUnity.Combine(srcFolder, srcName);
			var acFolder = clipFolders[0];
			var acFolderName = Path.GetFileName(acFolder.TrimEnd('/'));
			var acPath = clipFolders.IsNullOrEmpty() ? null : Path.ChangeExtension(PathUnity.Combine(acFolder, string.Format("AC_{0}", acFolderName)), AnimatorControllerExtension);

			#region prefab
			srcPath = Path.ChangeExtension(srcPath, ModelExtension);
			GameObject asset;
			var srcChanged = assetInfo.GetAsset(srcPath, out asset);
			if (null == asset)
			{
				srcPath = Path.ChangeExtension(srcPath, PrefabExtension);
				srcChanged = assetInfo.GetAsset(srcPath, out asset);
			}
			else
			{
				string[] depends = AssetDatabase.GetDependencies(new string[]{targetPath});
				if (!ArrayUtility.Contains(depends, srcPath))
				{
					srcChanged = true;
				}
			}
			if (null == asset)
			{
				return null;
			}

			GameObject prefab = null;
			if (!srcChanged)
			{
				prefab = AssetDatabase.LoadAssetAtPath<GameObject>(targetPath);
			}
			if (null == prefab)
			{
				prefab = PrefabUtility.CreatePrefab (targetPath, asset);
			}
			#endregion prefab

			var renderers = GameObjectHelper.FindComponentsInChildren<Renderer>(prefab);
			if (!renderers.IsNullOrEmpty())
			{
				foreach (var r in renderers)
				{
					OptimizationUtils.OptiRenderer(r);
				}
			}

			#region animator controller
			var allClips = GetAllClips(info, clipFolders, actionInfos, actionBField);
			Dictionary<string, List<string>> loopClips = null;
			if (null != info)
			{
				foreach (var key_value in info)
				{
					var key = key_value.key as string;
					if (null != key && key.StartsWith("LoopAction_"))
					{
						var clipsInfo = key_value.value as LuaTable;
						if (null != clipsInfo && 0 < clipsInfo.length())
						{
							var loopActionName = key.Substring(11);
							if (null != loopClips && loopClips.ContainsKey(loopActionName))
							{
								continue;
							}
							var clipNames = new List<string>();
							for (int i = 0; i < clipsInfo.length(); ++i)
							{
								var clipName = clipsInfo[i+1] as string;
								if (null != clipName && !clipNames.Contains(clipName))
								{
									clipNames.Add(clipName);
								}
							}
							if (0 < clipNames.Count)
							{
								if (null == loopClips)
								{
									loopClips = new Dictionary<string, List<string>>();
								}
								loopClips.Add(loopActionName, clipNames);
							}
						}
					}
				}
			}
			var animatorController = GenAnimatorController(allClips, acPath, defaultStateNames, callback, loopClips);
			var animtor = prefab.GetComponent<Animator>();
			if (null != animatorController)
			{
				if (null == animtor)
				{
					animtor = prefab.AddComponent<Animator>();
				}
				animtor.runtimeAnimatorController = animatorController;
			}
			else
			{
				if (null != prefab.FindComponentInChildren<SkinnedMeshRenderer>())
				{
					if (null == animtor)
					{
						animtor = prefab.AddComponent<Animator>();
					}
					animtor.runtimeAnimatorController = null;
				}
				else if (null != animtor)
				{
					Component.DestroyImmediate(animtor, true);
					animtor = null;
				}
			}
			if (null != animtor && null != animtor.runtimeAnimatorController)
			{
				var alwaysAnimate = LuaWorker.GetFieldInt(info, "AlwaysAnimate");
				if (0 != alwaysAnimate)
				{
					animtor.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				}
				else
				{
					animtor.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
				}
			}
			#endregion animator controller

			assetInfo.SetAssetHandled(srcPath, asset);
			return prefab;
		}
		public static Material SetMaterial(
			GameObject asset,
			string srcFolder, 
			string matName)
		{
			var renderer = asset.FindComponentInChildren<SkinnedMeshRenderer>();
			if (null != renderer)
			{
				var matPath = Path.ChangeExtension(PathUnity.Combine(srcFolder, matName), MaterialExtension);
				var material = AssetDatabase.LoadAssetAtPath<Material>(matPath);
				renderer.sharedMaterial = material;
				return material;
			}
			return null;
		}
		public static Material SetMaterial(
			RolePart part,
			string srcFolder, 
			string matName,
			string defaultMatName)
		{
			Material mat = null;
			if (!string.IsNullOrEmpty(defaultMatName))
			{
				mat = SetMaterial(part, srcFolder, defaultMatName);
			}
			if (null == mat)
			{
				mat = SetMaterial(part, srcFolder, matName);
			}
			return mat;
		}
		public static Material SetMaterial(
			RolePart part,
			string srcFolder, 
			string matName)
		{
			if (null != part.mainSMR)
			{
				var matPath = Path.ChangeExtension(PathUnity.Combine(srcFolder, matName), MaterialExtension);
				var material = AssetDatabase.LoadAssetAtPath<Material>(matPath);
				part.mainSMR.sharedMaterial = material;
				return material;
			}
			else if (null != part.mainMR)
			{
				var matPath = Path.ChangeExtension(PathUnity.Combine(srcFolder, matName), MaterialExtension);
				var material = AssetDatabase.LoadAssetAtPath<Material>(matPath);
				part.mainMR.sharedMaterial = material;
				return material;
			}
			return null;
		}
		public static AudioSource SetAudioSource(
			GameObject asset,
			AudioSource model)
		{
			AudioSource audioSource = null;
			if (null != model)
			{
				UnityEditorInternal.ComponentUtility.CopyComponent (model);
				audioSource = asset.GetComponent<AudioSource>();
				if (null == audioSource)
				{
					UnityEditorInternal.ComponentUtility.PasteComponentAsNew (asset);
					audioSource = asset.GetComponent<AudioSource>();
				}
				else
				{
					UnityEditorInternal.ComponentUtility.PasteComponentValues (audioSource);
				}
			}
			if (null == audioSource)
			{
				audioSource = asset.AddComponent<AudioSource>();
			}
			return audioSource;
		}

		public static void ShowProgress(string title, int i, int totalCount, string name)
		{
			EditorUtility.DisplayProgressBar(
				string.Format("Generating {0}", title), 
				string.Format("{0}/{1}, {2}", i, totalCount, name), 
				(float)i/totalCount);
		}

		public static AnimationEvent FindAnimationEventByFunctionName(AnimationEvent[] events, string name)
		{
			if (events.IsNullOrEmpty())
			{
				return null;
			}
			return ArrayUtility.Find(events, delegate(AnimationEvent e) {
				return string.Equals(e.functionName, name);
			});
		}

		public static AnimationEvent FindOrCreateUniqueAnimationEventByFunctionName(ref AnimationEvent[] events, string name)
		{
			AnimationEvent foundEvent = null;
			if (!events.IsNullOrEmpty())
			{
				var eventList = ArrayUtility.FindAll(events, delegate(AnimationEvent e) {
					return string.Equals(e.functionName, name);
				});
				if (!eventList.IsNullOrEmpty())
				{
					if (1 < eventList.Count)
					{
						for (int i = 1; i < eventList.Count; ++i)
						{
							ArrayUtility.Remove(ref events, eventList[i]);
						}
					}
					foundEvent = eventList[0];
				}
			}

			if (null == foundEvent)
			{
				foundEvent = new AnimationEvent();
				foundEvent.functionName = name;
				ArrayUtility.Add(ref events, foundEvent);
			}
			return foundEvent;
		}

		public static bool RemoveAnimationEventByFunctionName(ref AnimationEvent[] events, string name)
		{
			var eventList = ArrayUtility.FindAll(events, delegate(AnimationEvent e) {
				return string.Equals(e.functionName, name);
			});
			if (!eventList.IsNullOrEmpty())
			{
				for (int i = 0; i < eventList.Count; ++i)
				{
					ArrayUtility.Remove(ref events, eventList[i]);
				}
				return true;
			}
			return false;
		}

		[MenuItem( "RO/GenAssets/All", false, 0)]
		public static void GenAll()
		{
//			GenNPCs();

			GenBodies();
			GenHairs();
			GenEyes();
			GenMounts();
			GenEquips();

//			GenSkillDatas();
			GenBusCarriers();
			GenItemObjects();
//			GenActions();
			GenEffectsForECP();
			GenEffectsForEffectHandle();
		}

		static void CheckMaterial(Shader[] shaders, GameObject obj, string path, Material[] mats)
		{
			if (!mats.IsNullOrEmpty())
			{
				for (int i = 0; i < mats.Length; ++i)
				{
					var m = mats[i];
					if (null != m)
					{
						if (!ArrayUtility.Contains(shaders, m.shader))
						{
							Debug.LogFormat(obj, "<color=red>Invalid Shader: </color>{0}\n{1}",
								(null != m.shader ? m.shader.name : "null"),
								path);
						}
					}
					else
					{
						Debug.LogFormat(obj, "<color=red>Material is missing: </color>\n{0}",
							path);
					}
				}
			}
		}

		static void CheckRenderer<T>(Shader[] shaders, RolePart rolePart, string path, T[] rs) where T:Renderer
		{
			if (!rs.IsNullOrEmpty())
			{
				var missing = false;
				for (int i = 0; i < rs.Length; ++i)
				{
					var r = rs[i];
					if (null != r)
					{
						var mats = r.sharedMaterials;
						CheckMaterial(shaders, r.gameObject, path, mats);
					}
					else
					{
						missing = true;
					}
				}
				if (missing)
				{
					Debug.LogFormat(rolePart, "<color=red>SkinnedMeshRenderer is missing: </color>\n{0}",
						path);
				}
			}
		}

		static void CheckPart(string folder, Shader[] shaders)
		{
			var assets = GetUnhandledAssets<RolePart>(folder, "prefab");
			foreach (var key_value in assets)
			{
				var path = AssetDatabase.GetAssetPath(key_value.Value);
				var smrs = key_value.Value.smrs;
				CheckRenderer<SkinnedMeshRenderer>(shaders, key_value.Value, path, smrs);
				var mrs = key_value.Value.mrs;
				CheckRenderer<MeshRenderer>(shaders, key_value.Value, path, mrs);
				var ms = key_value.Value.materials;
				CheckMaterial(shaders, key_value.Value.gameObject, path, ms);
			}
		}

		[MenuItem( "RO/CheckAssets/RoleParts")]
		public static void CheckRoleParts()
		{
			Debug.Log("<color=green>CheckRoleParts Begin</color>{0}");
			var shaders = new Shader[]{
				Shader.Find("RO/Role/Part"),
				Shader.Find("RO/Role/PartOutline")
			};
			CheckPart(AssetsFolder_Body, shaders);
			CheckPart(AssetsFolder_Eye, shaders);
			CheckPart(AssetsFolder_Face, shaders);
			CheckPart(AssetsFolder_Hair, shaders);
			CheckPart(AssetsFolder_Head, shaders);
			CheckPart(AssetsFolder_Mount, shaders);
			CheckPart(AssetsFolder_Tail, shaders);
			CheckPart(AssetsFolder_Weapon, shaders);
			CheckPart(AssetsFolder_Wing, shaders);
			Debug.Log("<color=green>CheckRoleParts End</color>{0}");
		}

		[MenuItem( "RO/CheckAssets/RoleParts_Hair")]
		public static void CheckRoleParts_Hair()
		{
			Debug.Log("<color=green>CheckRoleParts_Hair Begin</color>");
			var shaders = new Shader[]{
				Shader.Find("RO/Role/Part"),
				Shader.Find("RO/Role/PartOutline")
			};
			CheckPart(AssetsFolder_Hair, shaders);
			Debug.Log("<color=green>CheckRoleParts_Hair End</color>");
		}
	
	}
} // namespace EditorTool
