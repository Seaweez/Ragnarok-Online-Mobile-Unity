using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;
using Ghost.Utils;
using Ghost.Config;
using RO;
using RO.Config;

namespace EditorTool
{
	public static class AnimationCommands 
	{
		static AnimationCommands()
		{
			var attackAnimations = new string[]
			{
				RoleAction.ATTACK,
				RoleAction.USE_SKILL,
				RoleAction.USE_SKILL_2,
				RoleAction.USE_MAGIC,
			};
			var animationList = new List<string>();
			animationList.AddRange(attackAnimations);

			if (string.IsNullOrEmpty(RoleAction.MOUNT_PREFIX))
			{
				foreach (var attackAnimation in attackAnimations)
				{
					animationList.Add(StringUtils.ConnectToString(RoleAction.MOUNT_PREFIX, attackAnimation));
				}
			}

			animationList.ToArray();
		}

		private const string ANIMATION_EVENT_NAME_FIRE = "ActionEventFire";
        private const string ANIMATION_EVENT_NAME_INTERRUPT = "ActionEventInterrupt";

        public static AnimationEvent FindAnimationEvent(AnimationEvent[] events, string eventFuncName)
		{
			if (events.IsNullOrEmpty())
			{
				return null;
			}
			foreach (var e in events)
			{
				if (string.Equals(e.functionName, eventFuncName))
				{
					return e;
				}
			}
			return null;
		}

		public static bool IsAttackAnimation(AnimationClip clip)
		{
			return (clip.name.StartsWith("attack") && !clip.name.StartsWith("attack_wait"))
				|| clip.name.StartsWith("use_skill")
				|| clip.name.StartsWith("use_magic");
		}

		public static bool CheckAttackFire(AnimationEvent[] events)
		{
			return null != FindAnimationEvent(events, ANIMATION_EVENT_NAME_FIRE);
		}

        public static bool CheckAttackInterrupt(AnimationEvent[] events)
        {
            return null != FindAnimationEvent(events, ANIMATION_EVENT_NAME_INTERRUPT);
        }

        private static void DoCheck()
		{
			var objs = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets);
			if (!objs.IsNullOrEmpty())
			{
				foreach (var obj in objs)
				{
					var clip = obj as AnimationClip;
					var clipPath = AssetDatabase.GetAssetPath(clip);
					if (IsAttackAnimation(clip))
					{
						var events = AnimationUtility.GetAnimationEvents(clip);
						if (!CheckAttackFire(events))
						{
							Debug.LogErrorFormat(clip, "[CheckAnimation Failed]: No Event Fire\n{0}", clipPath);
						}
                        if (!CheckAttackInterrupt(events))
                        {
                            Debug.LogErrorFormat(clip, "[CheckAnimation Failed]: No Event Interrupt\n{0}", clipPath);
                        }
                        if (1 != clip.length)
						{
							Debug.LogErrorFormat(clip, "[CheckAnimation Failed]: SkillAction Length({0}) is not 1 second: </color>\n{1}", clip.length, clipPath);
						}
					}
					var memSize = UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(clip);
					if (1024*1024 < memSize)
					{
						Debug.LogFormat(clip, "Runtime Memory Size: <color=red>{0}</color>, length = {1}(seconds)\n{2}", EditorUtility.FormatBytes(memSize), clip.length, clipPath);
					}
					else if (500*1024 < memSize)
					{
						Debug.LogFormat(clip, "Runtime Memory Size: <color=yellow>{0}</color>, length = {1}(seconds)\n{2}", EditorUtility.FormatBytes(memSize), clip.length, clipPath);
					}
				}
			}
			Debug.LogFormat("[CheckAnimation Finished]");
		}

		[MenuItem("Assets/Animation/Check")]
		static void Check()
		{
			DoCheck();
		}

		private static bool IsAnimationFBX(string path)
		{
			var fileName = Path.GetFileName(path);
			return string.Equals("fbx", Path.GetExtension(fileName).TrimStart('.').ToLower())
				&& fileName.Contains("@");
		}

		[MenuItem("Assets/Animation/PickUp", true)]
		static bool PickUpValidFunc()
		{
			var objs = Selection.objects;
			if (objs.IsNullOrEmpty())
			{
				return false;
			}

			foreach (var obj in objs)
			{
				var objPath = AssetDatabase.GetAssetPath(obj);
				if (AssetDatabase.IsValidFolder(objPath))
				{
					return true;
				}

				if (obj is GameObject && IsAnimationFBX(objPath))
				{
					return true;
				}
			}

			return false;
		}

		[MenuItem("Assets/Animation/PickUp %e")]
		static void PickUp()
		{
			var objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			if (objs.IsNullOrEmpty())
			{
				Debug.LogFormat("<color=yellow>No Selected Assets</color>");
				return;
			}
			try
			{
				var progressTitle = "Pick Up Animation Clips";
				EditorUtility.DisplayProgressBar(progressTitle, "", 0);
				for (int i = 0; i < objs.Length; ++i)
				{
					var obj = objs[i];
					if (!(obj is GameObject))
					{
						continue;
					}
					var objPath = AssetDatabase.GetAssetPath(obj);
					if (!IsAnimationFBX(objPath))
					{
						continue;
					}
					
					var newClip = AssetDatabase.LoadAssetAtPath(objPath, typeof(AnimationClip)) as AnimationClip;
					if (null == newClip)
					{
						continue;
					}

					var savePath = Path.ChangeExtension(PathUnity.Combine(Path.GetDirectoryName(objPath), newClip.name), PathConfig.EXTENSION_ANIMATION);
					var saveClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(savePath);
					string op = null;
					if (null == saveClip)
					{
						saveClip = new AnimationClip();
						AssetDatabase.CreateAsset(saveClip, savePath);
						op = "New";
					}
					else
					{
						var events = AnimationUtility.GetAnimationEvents(saveClip);
						AnimationUtility.SetAnimationEvents(newClip, events);
						op = "Replace";
					}
					
					var originLooping = saveClip.isLooping;
					EditorUtility.CopySerialized(newClip, saveClip);
					if (originLooping)
					{
						var settings = AnimationUtility.GetAnimationClipSettings(saveClip);
						if (null != settings)
						{
							settings.loopTime = originLooping;
							AnimationUtility.SetAnimationClipSettings(saveClip, settings);
						}
					}

                    AnimationClipOptimizer.Optimal(saveClip, 3);

                    EditorUtility.DisplayProgressBar(progressTitle, string.Format("[{0}, {1}]: {2}", newClip.name, op, objPath), (float)i/objs.Length);
					Debug.LogFormat(obj, "<color=green>Pick Up Animation({0}): </color>{1}", op, objPath);
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		[MenuItem("Assets/Animation/Reimport")]
		static void Reimport()
		{
			var objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			if (objs.IsNullOrEmpty())
			{
				Debug.LogFormat("<color=yellow>No Selected Assets</color>");
				return;
			}
			try
			{
				var progressTitle = "Reimport Animation FBX";
				EditorUtility.DisplayProgressBar(progressTitle, "", 0);
				for (int i = 0; i < objs.Length; ++i)
				{
					var obj = objs[i];
					if (!(obj is GameObject))
					{
						continue;
					}
					var objPath = AssetDatabase.GetAssetPath(obj);
					if (!IsAnimationFBX(objPath))
					{
						continue;
					}
					
					var newClip = AssetDatabase.LoadAssetAtPath(objPath, typeof(AnimationClip)) as AnimationClip;
					if (null == newClip)
					{
						continue;
					}

					AssetDatabase.ImportAsset(objPath);

					EditorUtility.DisplayProgressBar(progressTitle, string.Format("[{0}]: {1}", newClip.name, objPath), (float)i/objs.Length);
					Debug.LogFormat(obj, "<color=green>{0}: </color>{1}", progressTitle, objPath);
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		public static void DoAnimatorController(Object selectionObj, System.Action<AnimatorController> doFunc)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = "t:AnimatorController";
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 
				
				if (guids.IsNullOrEmpty())
				{
					return;
				}
				
				var totalCount = guids.Length;
				try
				{
					int i = 0;
					foreach (var guid in guids)
					{
						var objPath = AssetDatabase.GUIDToAssetPath(guid);
						var obj = AssetDatabase.LoadAssetAtPath<AnimatorController>(objPath);
						
						++i;
						EditorUtility.DisplayProgressBar(
							string.Format("Proccessing Folder: {0}", path), 
							string.Format("{0}/{1}, {2}", i, totalCount, obj.name), 
							(float)i/totalCount);
						doFunc(obj);
					}
				}
				finally
				{
					EditorUtility.ClearProgressBar();
				}
				
			}
			else
			{
				doFunc(selectionObj as AnimatorController);
			}
		}
		
		public static void DoAnimatorControllers(System.Action<AnimatorController> doFunc)
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoAnimatorController(obj, doFunc);
			}
			AssetDatabase.SaveAssets();
			Debug.LogFormat("<color=green>[DoAnimatorControllers Finished]</color>");
		}

		public static void DoClip(Object selectionObj, System.Action<AnimationClip> doFunc)
		{
			var path = AssetDatabase.GetAssetPath(selectionObj);
			if (AssetDatabase.IsValidFolder(path))
			{
				var filterString = "t:AnimationClip";
				var guids = AssetDatabase.FindAssets(filterString, new string[]{path}); 

				if (guids.IsNullOrEmpty())
				{
					return;
				}

				var totalCount = guids.Length;
				try
				{
					int i = 0;
					foreach (var guid in guids)
					{
						var objPath = AssetDatabase.GUIDToAssetPath(guid);
						var obj = AssetDatabase.LoadAssetAtPath<AnimationClip>(objPath);

						++i;
						EditorUtility.DisplayProgressBar(
							string.Format("Proccessing Folder: {0}", path), 
							string.Format("{0}/{1}, {2}", i, totalCount, obj.name), 
							(float)i/totalCount);
						doFunc(obj);
					}
				}
				finally
				{
					EditorUtility.ClearProgressBar();
				}

			}
			else
			{
				doFunc(selectionObj as AnimationClip);
			}
		}

		public static void DoClips(System.Action<AnimationClip> doFunc)
		{
			if (Selection.objects.IsNullOrEmpty())
			{
				return;
			}
			foreach (var obj in Selection.objects)
			{
				DoClip(obj, doFunc);
			}
			AssetDatabase.SaveAssets();
			Debug.LogFormat("<color=green>[DoClips Finished]</color>");
		}
		
		[MenuItem("Assets/Animation/CheckAnimatorController")]
		static void CheckAnimatorController()
		{
			DoAnimatorControllers(delegate(AnimatorController controller) {
				var layer = controller.layers[0];
				var states = layer.stateMachine.states;
				if (states.IsNullOrEmpty())
				{
					Debug.LogFormat(controller, "<color=red>No States: </color>{0}", AssetDatabase.GetAssetPath(controller));
					return;
				}
				
				for (int i = 0; i < states.Length; ++i)
				{
					var state = states[i].state;
					if ("wait" == state.name)
					{
						return;
					}
				}
				Debug.LogFormat(controller, "<color=red>No wait: </color>{0}", AssetDatabase.GetAssetPath(controller));
			});
		}

		[MenuItem("Assets/Animation/CheckClip")]
		static void CheckClip()
		{
			DoClips(delegate(AnimationClip clip) {
				if (IsAttackAnimation(clip))
				{
					if (1 != clip.length)
					{
						Debug.LogFormat(clip, "<color=red>SkillAction Length({0}) is not 1 second: </color>\n{1}", clip.length, AssetDatabase.GetAssetPath(clip));
						return;
					}
				}
			});
		}
	}
} // namespace EditorTool
