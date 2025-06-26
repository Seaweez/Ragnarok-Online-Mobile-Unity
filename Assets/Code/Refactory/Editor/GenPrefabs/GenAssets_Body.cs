using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using SLua;
using Ghost.Utils;
using Ghost.Extensions;
using RO;
using RO.Config;

namespace EditorTool
{
	public static partial class GenAssets 
	{
		public class StateActionInfo
		{
			public int ID1 = 0;
			public int ID2 = 0;
			public AnimatorState state = null;
		}
		
		public class ChooseActionInfo
		{
			public AnimatorState stateChooseWait = null;
			public AnimatorState stateChooseWait2 = null;
			
			public AnimatorState stateChooseWaitCross = null;
			public AnimatorState stateChooseWait2Cross = null;
			
			public AnimatorState stateChooseEasterEggs = null;
		}

		public static readonly string[] RoleAnimatorDefaultState = {
			"wait",
			"ride_wait"
		};

		public enum RoleAudioEffectIndex{
			Wait = 0,
			Walk = 1,
			Attack = 2,
			Hited = 3,
			Die = 4
		}
		
		public static void GenRoleAnimatorControllerCallback(
			Dictionary<int, List<StateActionInfo>> stateActionInfos, 
			ChooseActionInfo chooseActionInfo,
			AnimatorState state, AnimationClip clip)
		{
			var stateName = state.name;

			#region state action
			var match = Regex.Match(stateName, @"state\d+", RegexOptions.IgnoreCase);
			if (match.Success)
			{
				match = Regex.Match(match.Value, @"\d+", RegexOptions.RightToLeft);
				if (match.Success)
				{
					var stateNumber = int.Parse(match.Value);
					StateActionInfo info = new StateActionInfo();
					info.ID1 = stateNumber/1000;
					info.ID2 = stateNumber%1000;
					info.state = state;
					List<StateActionInfo> infoList;
					if (!stateActionInfos.TryGetValue(info.ID1, out infoList))
					{
						infoList = new List<StateActionInfo>();
						stateActionInfos.Add(info.ID1, infoList);
					}
					infoList.Add(info);
				}
			}
			#endregion state action
			
			#region choose
			if (stateName.Equals ("choose_wait"))
			{
				chooseActionInfo.stateChooseWait = state;
			}
			else if (stateName.Equals ("choose_wait2"))
			{
				chooseActionInfo.stateChooseWait2 = state;
			}
			else if (stateName.Equals ("choose_turn2"))
			{
				chooseActionInfo.stateChooseWaitCross = state;
			}
			else if (stateName.Equals ("choose_turn"))
			{
				chooseActionInfo.stateChooseWait2Cross = state;
			}
			else if (stateName.Equals ("choose_easter_eggs"))
			{
				chooseActionInfo.stateChooseEasterEggs = state;
			}
			#endregion choose
		}

		public static bool SetRoleActionEvent(AnimatorState state, AnimationClip clip)
		{
			var stateNameWithoutPrefix = state.name;
			if (stateNameWithoutPrefix.StartsWith(RoleAction.MOUNT_PREFIX))
			{
				stateNameWithoutPrefix = stateNameWithoutPrefix.Substring(RoleAction.MOUNT_PREFIX.Length);
			}

			var events = AnimationUtility.GetAnimationEvents (clip);
			if (null == events)
			{
				events = new AnimationEvent[0];
			}
			else
			{
				for (int i = events.Length-1; i >= 0; --i)
				{
					if (null == events[i] || string.IsNullOrEmpty(events[i].functionName))
					{
						ArrayUtility.RemoveAt(ref events, i);
					}
				}
			}
			var seEvent = FindOrCreateUniqueAnimationEventByFunctionName(ref events, "ActionEventPlayAudioEffect");
			if (RoleAction.WAIT == stateNameWithoutPrefix 
			    || RoleAction.ATTACK_WAIT == stateNameWithoutPrefix)
			{
				seEvent.intParameter = (int)RoleAudioEffectIndex.Wait;
				seEvent.time = 0f;
			}
			else if (RoleAction.WALK == stateNameWithoutPrefix)
			{
				seEvent.intParameter = (int)RoleAudioEffectIndex.Walk;
				seEvent.time = clip.length / 2f;
			}
			else if (RoleAction.ATTACK == stateNameWithoutPrefix 
			         || RoleAction.USE_SKILL == stateNameWithoutPrefix
			         || RoleAction.USE_SKILL_2 == stateNameWithoutPrefix
			         /*|| RoleAction.USE_MAGIC == stateNameWithoutPrefix*/)
			{
				var fireEvent = FindAnimationEventByFunctionName(events, "ActionEventFire");
				if (null != fireEvent)
				{
					seEvent.intParameter = (int)RoleAudioEffectIndex.Attack;
					seEvent.time = fireEvent.time;
				}

                var interruptEvent = FindAnimationEventByFunctionName(events, "ActionEventInterrupt");
                if (null != interruptEvent)
                {
                  
                }
            }
			else if (RoleAction.HIT == stateNameWithoutPrefix)
			{
				seEvent.intParameter = (int)RoleAudioEffectIndex.Hited;
				seEvent.time = 0f;
			}
			else if (RoleAction.DIE == stateNameWithoutPrefix)
			{
				seEvent.intParameter = (int)RoleAudioEffectIndex.Die;
				seEvent.time = 0f;
			}
			else
			{
				if (RemoveAnimationEventByFunctionName(ref events, "ActionEventPlayAudioEffect"))
				{
					AnimationUtility.SetAnimationEvents (clip, events);
				}
				return false;
			}
  	
			var eventList = new List<AnimationEvent>(events);
			eventList.Sort(delegate(AnimationEvent x, AnimationEvent y) {
				if (x.time == y.time)
				{
					return string.Compare(x.functionName, y.functionName);
				}
				return (x.time < y.time) ? -1 : 1;
			});
			events = eventList.ToArray();
			AnimationUtility.SetAnimationEvents (clip, events);
			return true;
		}

		public static void SetUniqueExitTransition(AnimatorState state1, AnimatorState state2)
		{
			AnimatorStateTransition exitTransition = null;
			var oldTransitions = state1.transitions;
			if (!oldTransitions.IsNullOrEmpty())
			{
				for (int i = oldTransitions.Length-1; 0 <= i; --i)
				{
					var t = oldTransitions[i];
					if (null == exitTransition && t.destinationState == state2)
					{
						exitTransition = t;
					}
					else
					{
						state1.RemoveTransition(t);
					}
				}
			}
			if (null == exitTransition)
			{
				exitTransition = state1.AddTransition(state2);
			}
			exitTransition.hasExitTime = true;
		}

		public static void SetAnimatorStateTransition(
			Dictionary<int, List<StateActionInfo>> stateActionInfos, 
			ChooseActionInfo chooseActionInfo)
		{
			#region state action
			if (0 < stateActionInfos.Count)
			{
				foreach (var infoList in stateActionInfos.Values)
				{
					infoList.Sort(delegate(StateActionInfo x, StateActionInfo y) {
						return x.ID2-y.ID2;
					});
					StateActionInfo prevInfo = null;
					foreach (var info in infoList)
					{
						if (null != prevInfo)
						{
							SetUniqueExitTransition(prevInfo.state, info.state);
						}
						prevInfo = info;
					}
				}
			}
			#endregion state action
			
			#region choose
			if (null != chooseActionInfo.stateChooseWait && null != chooseActionInfo.stateChooseWaitCross)
			{
				SetUniqueExitTransition(chooseActionInfo.stateChooseWaitCross, chooseActionInfo.stateChooseWait);
			}
			if (null != chooseActionInfo.stateChooseWait2 && null != chooseActionInfo.stateChooseWait2Cross)
			{
				SetUniqueExitTransition(chooseActionInfo.stateChooseWait2Cross, chooseActionInfo.stateChooseWait2);
			}
			if (null != chooseActionInfo.stateChooseEasterEggs && null != chooseActionInfo.stateChooseWait)
			{
				SetUniqueExitTransition(chooseActionInfo.stateChooseEasterEggs, chooseActionInfo.stateChooseWait);
			}
			#endregion choose
		}

		public static string GetBodyModlePath(LuaTable info)
		{
			var path = PathUnity.Combine(LuaWorker.GetFieldString(info, "ModelDir"), LuaWorker.GetFieldString(info, "ModelName"));
			path.TrimStart('/');
			return path;
		}

		public static Dictionary<int, LuaTable> GetBodyInfos(out WorkerContainer workerContainer)
		{
			return GetAssetsInfo_1(out workerContainer, "Body", GetBodyModlePath, "Table_Body");
		}

		public static GameObject GenRolePrefabsAndController(
			AssetInfo assetInfo,
			string srcFolder, 
			string srcName, 
			string[] clipFolders, 
			string targetPath,
			LuaTable info,
			Dictionary<int, LuaTable> actionInfos,
			string actionBField,
			string[] defaultStateNames,
			System.Action<AnimatorState, AnimationClip> extraCallback = null)
		{
			var stateActionInfos = new Dictionary<int, List<StateActionInfo>>();
			ChooseActionInfo chooseActionInfo = new ChooseActionInfo();
			var asset = GenPrefabsAndController(
				assetInfo,
				srcFolder,
				srcName, 
				clipFolders, 
				targetPath, 
				info,
				actionInfos, 
				actionBField, 
				defaultStateNames,
				delegate(AnimatorState state, AnimationClip clip) {
					GenRoleAnimatorControllerCallback(stateActionInfos, chooseActionInfo, state, clip);
					if (null != extraCallback)
					{
						extraCallback(state, clip);
					}
				});
			SetAnimatorStateTransition(stateActionInfos, chooseActionInfo);
			return asset;
		}

		public static GameObject GenRolePrefabsAndControllerAndSE(
			AssetInfo assetInfo,
			string srcFolder, 
			string srcName, 
			string[] clipFolders, 
			string targetPath,
			LuaTable info,
			Dictionary<int, LuaTable> actionInfos,
			string actionBField,
			string[] defaultStateNames,
			out bool needAudioSource,
			System.Action<AnimatorState, AnimationClip> extraCallback = null)
		{
			var nas = false;
			var asset = GenRolePrefabsAndController(
				assetInfo,
				srcFolder,
				srcName, 
				clipFolders, 
				targetPath, 
				info,
				actionInfos, 
				actionBField, 
				defaultStateNames,
				delegate(AnimatorState state, AnimationClip clip) {
					var hasSEEvent = SetRoleActionEvent(state, clip);
					if (!nas)
					{
						if (hasSEEvent)
						{
							nas = true;
						}
						else
						{
							var events = AnimationUtility.GetAnimationEvents(clip);
							nas = (null != FindAnimationEventByFunctionName(events, "ActionEventPlaySE"));
						}
					}
					if (null != extraCallback)
					{
						extraCallback(state, clip);
					}
				});
			needAudioSource = nas;
//			needAudioSource = false; // no need audio source, get from parent(RoleAgent)
			return asset;
		}

		public static void InitRolePart(RolePart rolePart, string srcFolder, string srcName, string matName, string defaultMatName = null)
		{
			var asset = rolePart.gameObject;
			rolePart.eps = GameObjectHelper.GetPoints(asset, "EP_");
			rolePart.cps = GameObjectHelper.GetPoints(asset, "CP_");
			// mouth use face CP
			if (null != rolePart.cps && (!rolePart.cps.CheckIndex(9) || null == rolePart.cps[9]))
			{
				if (rolePart.cps.CheckIndex(6) && null != rolePart.cps[6])
				{
					if (!rolePart.cps.CheckIndex(9))
					{
						var cps = new Transform[10];
						for (int i = 0; i < rolePart.cps.Length; ++i)
						{
							cps[i] = rolePart.cps[i];
						}
						rolePart.cps = cps;
					}
					rolePart.cps[9] = rolePart.cps[6];
				}
			}
			rolePart.ecps = GameObjectHelper.GetPointsWithID(asset, "ECP_");
			var animators = GameObjectHelper.FindComponentsInChildren<Animator>(asset);
			if (null != animators)
			{
				var animatorList = new List<Animator>();
				foreach (var a in animators)
				{
					if (null != a.runtimeAnimatorController)
					{
						animatorList.Add(a);
					}
				}
				if (0 < animatorList.Count)
				{
					rolePart.animators = animatorList.ToArray();
				}
				else
				{
					rolePart.animators = null;
				}
			}
			else
			{
				rolePart.animators = null;
			}

			var objs = asset.FindGameObjectsInChildren(delegate(GameObject obj){
				return string.Equals("Obstacle", obj.name);
			});
			if (!objs.IsNullOrEmpty())
			{
				foreach (var obj in objs)
				{
					var obstacle = obj.GetComponent<UnityEngine.AI.NavMeshObstacle>();
					if (null == obstacle)
					{
						obstacle = obj.AddComponent<UnityEngine.AI.NavMeshObstacle>();
					}
					obstacle.carving = true;
					var meshFilters = obj.FindComponentsInChildren<MeshFilter>();
					if (!meshFilters.IsNullOrEmpty())
					{
						foreach (var component in meshFilters)
						{
							Component.DestroyImmediate(component, true);
						}
					}
					var renderers = obj.FindComponentsInChildren<Renderer>();
					if (!renderers.IsNullOrEmpty())
					{
						foreach (var component in renderers)
						{
							Component.DestroyImmediate(component, true);
						}
					}
				}
			}

			rolePart.smrs = GameObjectHelper.FindComponentsInChildren<SkinnedMeshRenderer>(asset);
			rolePart.mrs = GameObjectHelper.FindComponentsInChildren<MeshRenderer>(asset);
//			rolePart.pss = GameObjectHelper.FindComponentsInChildren<ParticleSystem>(asset);

			InitRolePart_MainRenderer(rolePart, srcName);
			SetMaterial(rolePart, srcFolder, matName, defaultMatName);
			InitRolePart_Materials(rolePart, srcFolder, matName);
		}

		public static void InitRolePart_MainRenderer(RolePart rolePart, string srcName)
		{
			rolePart.mainSMR = null;
			rolePart.mainMR = null;

			var mainRenderName = srcName.ToLower();
			var smrs = rolePart.smrs;
			if (!smrs.IsNullOrEmpty())
			{
				for (int i = 0; i < smrs.Length; ++i)
				{
					var r = smrs[i];
					if (rolePart.gameObject == r.gameObject || r.name.ToLower() == mainRenderName)
					{
						rolePart.mainSMR = r;
						return;
					}
				}
			}
			var mrs = rolePart.mrs;
			if (!mrs.IsNullOrEmpty())
			{
				for (int i = 0; i < mrs.Length; ++i)
				{
					var r = mrs[i];
					if (rolePart.gameObject == r.gameObject || r.name.ToLower() == mainRenderName)
					{
						rolePart.mainMR = r;
						return;
					}
				}
			}
		}
		
		public static void InitRolePart_Materials(RolePart rolePart, string srcFolder, string matName)
		{
			var materialList = new List<Material>();
			
			var matNumber = 1;
			do
			{
				var matPath = Path.ChangeExtension(
					PathUnity.Combine(srcFolder, string.Format("{0}_{1}", matName, matNumber)), 
					MaterialExtension);
				var material = AssetDatabase.LoadAssetAtPath<Material>(matPath);
				if (null != material)
				{
					materialList.Add(material);
					++matNumber;
				}
				else
				{
					break;
				}
			}
			while (true);
			
			if (0 < materialList.Count)
			{
				rolePart.materials = materialList.ToArray();
			}
			else
			{
				rolePart.materials = null;
			}
		}

		private static void InitRolePartBody_Bounds(RolePartBody roleRody)
		{
			roleRody.localBounds = null;
			
			var localBounds = roleRody.GetComponent<LocalBounds>();
			if (null != localBounds)
			{
				roleRody.localBounds = localBounds;
				return;
			}
		}

		private static void InitRolePartBody(RolePartBody roleRody)
		{
			InitRolePartBody_Bounds(roleRody);
			var collider = roleRody.GetComponent<BoxCollider>();
			if (roleRody.hasBounds)
			{
				if (null == collider)
				{
					collider = roleRody.gameObject.AddComponent<BoxCollider>();
				}
				collider.isTrigger = true;
				roleRody.AdjustCollider(collider);
				collider.enabled = false;
				collider.gameObject.layer = RO.Config.Layer.ACCESSABLE.Value;
			}
			else
			{
				if (null != collider)
				{
					Component.DestroyImmediate(collider, true);
					collider = null;
				}
			}
			roleRody.collider = collider;

			var a = roleRody.GetComponent<Animator>();
			if (null != a && null != a.runtimeAnimatorController)
			{
				roleRody.mainAnimator = a;
			}
			else
			{
				roleRody.mainAnimator = null;
			}
		}

		[MenuItem( "RO/GenAssets/Bodies", false, 101)]
		public static void GenBodies()
		{
			WorkerContainer workerContainer;
			var infos = GetBodyInfos(out workerContainer);
			using(workerContainer)
			{
				if (null == infos)
				{
					return;
				}

				var totalCount = infos.Count;
				if (0 == totalCount)
				{
					Debug.LogFormat("No Bodys");
					return;
				}

				var assetInfo = AssetInfo.GetInstance("Body");
				try
				{
					assetInfo.Begin();

					WorkerContainer actionWorkerContainer;
					var actionInfos = GetActionInfos(out actionWorkerContainer);
					if (!actionWorkerContainer.workers.IsNullOrEmpty())
					{
						ArrayUtility.AddRange(ref workerContainer.workers, actionWorkerContainer.workers);
					}

					var unhandledAssets = GetUnhandledAssets<GameObject>(AssetsFolder_Body, "prefab");

					var audioSourceModel = AssetDatabase.LoadAssetAtPath<AudioSource>(Path.ChangeExtension(AssetsPath_RoleComplete, PrefabExtension));

					var i = 0;
					foreach (var key_value in infos)
					{
						++i;
						var ID = key_value.Key;
						var info = key_value.Value;
						LuaWorker.GetFieldString(info, "NameEn");
						var assetName = ID.ToString();
						
						ShowProgress("Bodies", i, totalCount, assetName);

						// 1.
						var srcFolder = PathUnity.Combine(AssetsSrcFolder_Role, LuaWorker.GetFieldString(info, "ModelDir"));
						var srcName = LuaWorker.GetFieldString(info, "ModelName");
						var clipFolders = new string[]{LuaWorker.GetFieldString(info, "Anime")};
						if (!string.IsNullOrEmpty(clipFolders[0]))
						{
							clipFolders[0] = PathUnity.Combine(srcFolder, clipFolders[0]);
						}
						else
						{
							clipFolders[0] = srcFolder;
						}
						var publicClipFolder = LuaWorker.GetFieldString(info, "PublicAnime");
						if (!string.IsNullOrEmpty(publicClipFolder))
						{
							ArrayUtility.Add(ref clipFolders, PathUnity.Combine(AssetsSrcFolder_ActionBody, publicClipFolder));
						}
						var targetPath = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_Body, assetName), PrefabExtension);

						bool needAudioSource;
						var asset = GenRolePrefabsAndControllerAndSE(
							assetInfo,
							srcFolder,
							srcName, 
							clipFolders, 
							targetPath, 
							info,
							actionInfos, 
							"PlayerActionB", 
							RoleAnimatorDefaultState,
							out needAudioSource);
						if (null == asset)
						{
							continue;
						}
						var assetObj = asset.gameObject;

						// 2.
						if (!unhandledAssets.Remove(assetName))
						{
							Debug.LogFormat("<color=green>New Created: </color>{0}", asset.name);
						}

						asset.name = assetName;
						var matName = LuaWorker.GetFieldString(info, "Texture");
						if (string.IsNullOrEmpty(matName))
						{
							matName = srcName;
						}
//						SetMaterial(asset, srcFolder, matName);

						var rolePart = asset.GetComponent<RolePartBody>();
						if (null == rolePart)
						{
							rolePart = asset.AddComponent<RolePartBody>();
						}
						rolePart.audioEffects = new string[]{
							LuaWorker.GetFieldString(info, "StandSE"),
							LuaWorker.GetFieldString(info, "WalkSE"),
							LuaWorker.GetFieldString(info, "AttackSE"),
							LuaWorker.GetFieldString(info, "BeHitSE"),
							LuaWorker.GetFieldString(info, "DieSE")
						};

						if (needAudioSource)
						{
							rolePart.audioSource = SetAudioSource(asset, audioSourceModel);
						}
						else
						{
							var audioSource = asset.GetComponent<AudioSource>();
							if (null != audioSource)
							{
								Component.DestroyImmediate(audioSource, true);
							}
							rolePart.audioSource = null;
						}

						InitRolePart(rolePart, srcFolder, srcName, matName);
						InitRolePartBody(rolePart);

						if (null != asset)
						{
							EditorUtility.SetDirty(asset);
						}
						else if (null != assetObj)
						{
							EditorUtility.SetDirty(assetObj);
						}
					}

					if (!workerContainer.debug)
					{
						foreach (var asset in unhandledAssets.Values)
						{
							if (null != asset)
							{
								Debug.LogFormat("<color=red>Deleted: </color>{0}", asset.name);
								AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
							}
						}
					}
				}
				finally
				{
					try
					{
						assetInfo.End();
					}
					catch(System.Exception)
					{
					}
					EditorUtility.ClearProgressBar();
					AssetDatabase.SaveAssets();
				}
			}
		}
	}
} // namespace EditorTool
