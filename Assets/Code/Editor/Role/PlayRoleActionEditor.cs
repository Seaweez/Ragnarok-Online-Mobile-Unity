using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(RoleActionPlayer), true)]
	public class PlayRoleActionEditor : Editor
	{
		private int[] layerIndexes_ = null;
		private string[] layerNames_ = null;
		private int layerIndex_ = 0;
		
		private int[] stateNameHashes_ = null;
		private string[] stateNames_ = null;
		private int stateNameHash_ = 0;

		private GameObject prefab_ = null;
		private GameObject effect_ = null;
		private int effectPointIndex_ = 0;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var player = target as RoleActionPlayer;
			var animator = player.animator;
			if (!(null != animator && 0 < animator.layerCount))
			{
				return;
			}
			
			if (null == layerIndexes_ || layerIndexes_.Length != animator.layerCount)
			{
				layerIndexes_ = new int[animator.layerCount];
				layerNames_ = new string[layerIndexes_.Length];
			}
			for (int i = 0; i < layerIndexes_.Length; ++i)
			{
				layerNames_[i] = animator.GetLayerName(i);
				layerIndexes_[i] = i;
			}
			layerIndex_ = EditorGUILayout.IntPopup("Layer", layerIndex_, layerNames_, layerIndexes_);
			
			UnityEditor.Animations.AnimatorController controller = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
			if (null == controller)
			{
				return;
			}
			
			var layer = controller.layers[layerIndex_];
			var states = layer.stateMachine.states;
			if (states.IsNullOrEmpty())
			{
				return;
			}

			if (null == stateNameHashes_ || stateNameHashes_.Length != states.Length)
			{
				stateNameHashes_ = new int[states.Length];
				stateNames_ = new string[stateNameHashes_.Length];
			}
			for (int i = 0; i < stateNameHashes_.Length; ++i)
			{
				var state = states[i].state;
				stateNameHashes_[i] = state.nameHash;
				stateNames_[i] = state.name;
			}

			prefab_ = EditorGUILayout.ObjectField("Effect", prefab_, typeof(GameObject), false) as GameObject;

			if (Application.isPlaying)
			{
				if (0 == stateNameHash_)
				{
					stateNameHash_ = stateNameHashes_[0];
				}
				stateNameHash_ = EditorGUILayout.IntPopup("State", stateNameHash_, stateNames_, stateNameHashes_);

				var effectParent = animator.transform;
				if (!player.effectPoints.IsNullOrEmpty())
				{
					var effectNames = new string[player.effectPoints.Length];
					for (int i = 0; i < effectNames.Length; ++i)
					{
						var ep = player.effectPoints[i];
						effectNames[i] = null != ep ? ep.name : "(invalid)";
					}
					effectPointIndex_ = EditorGUILayout.Popup("EffectPoint", effectPointIndex_, effectNames);
					var selectEP = player.effectPoints[effectPointIndex_];
					if (null != selectEP)
					{
						effectParent = selectEP.transform;
					}
				}

				if (GUILayout.Button("Play"))
				{
					#if OBSOLETE
					var role = animator.GetComponent<Role>();
					if (null != role)
					{
						role.animatorHelper.PlayForce(stateNames_[ArrayUtility.IndexOf(stateNameHashes_, stateNameHash_)]);
					}
					else
#endif
					{
						animator.Play(stateNameHash_, -1, 0);
					}
					if (null != prefab_)
					{
						if (null != effect_)
						{
							GameObject.Destroy(effect_);
						}
						effect_ = GameObject.Instantiate(prefab_) as GameObject;
						effect_.transform.parent = effectParent;
						effect_.transform.localPosition = Vector3.zero;
						#if OBSOLETE
						effect_.AddComponent<EffectAutoDestroy>();
#endif
					}
				}
			}

		}
	
	}
} // namespace EditorTool
