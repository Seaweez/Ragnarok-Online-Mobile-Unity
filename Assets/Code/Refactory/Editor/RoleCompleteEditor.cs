using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RO;
using Ghost.Extensions;

namespace EditorTool
{
	[CustomEditor(typeof(RoleComplete)),CanEditMultipleObjects]
	public class RoleCompleteEditor : Editor
	{
		static string[] partFolders = {
			"Body",
			"Hair",
			"Weapon",
			"Weapon",
			"Head",
			"Wing",
			"Face",
			"Tail",
			"Eye",
			"Mouth",
			"Mount"
		};
		class PartPool{
			public string[] names;
			public int[] ids;
			public GameObject[] prefabs;

			public PartPool(string folder)
			{
				var folderPath = "Assets/Resources/Role/"+folder;
				var guids = AssetDatabase.FindAssets("t:Prefab", new string[]{folderPath}); 

				if (!guids.IsNullOrEmpty())
				{
					var nameList = new List<string>();
					var idList = new List<int>();
					var prefabList = new List<GameObject>();

					nameList.Add("null");
					idList.Add(0);
					prefabList.Add(null);
					foreach (var guid in guids)
					{
						var objPath = AssetDatabase.GUIDToAssetPath(guid);
						var obj = AssetDatabase.LoadAssetAtPath<GameObject>(objPath);
						var match = Regex.Match(obj.name, @"\d+");
						if (match.Success)
						{
							nameList.Add(obj.name);
							idList.Add(int.Parse(match.Value));
							prefabList.Add(obj);
						}
					}

					names = nameList.ToArray();
					ids = idList.ToArray();
					prefabs = prefabList.ToArray();
				}
				else
				{
					names = new string[]{"null"};
					ids = new int[]{0};
					prefabs = new GameObject[]{null};
				}
			}
		}
		private bool testDress = false;
		private int[] newPartIDs = new int[11];

		private bool playAction = false;
		private int[] stateNameHashes_ = null;
		private string[] stateNames_ = null;
		private int stateNameHash_ = 0;

		private bool testData = false;
		private float moveSpeed = 1f;
		private Dictionary<string, object> datas = new Dictionary<string, object>();

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying)
			{
				EditorGUILayout.Separator();
				if (GUILayout.Button("Debug") && null != LuaLuancher.Me)
				{
					for (int i = 0; i < targets.Length; ++i)
					{
						var role = targets[i] as RoleComplete;
						LuaLuancher.Me.Call("Debug_Creature", role.GUID);
					}
				}

				EditorGUILayout.Separator();
				if (GUILayout.Button("TestHandInHand") && null != LuaLuancher.Me)
				{
					var role = target as RoleComplete;
					LuaLuancher.Me.Call("TestHandInHand", role.GUID);
				}

				EditorGUILayout.Separator();
				if (GUILayout.Button("UpdateShadow"))
				{
					for (int i = 0; i < targets.Length; ++i)
					{
						var role = targets[i] as RoleComplete;
						role.UpdateShadow();
					}
				}

				EditorGUILayout.Separator();
				if (GUILayout.Button("UpdateCollider"))
				{
					for (int i = 0; i < targets.Length; ++i)
					{
						var role = targets[i] as RoleComplete;
						if (null != role.body)
						{
							role.body.UpdateCollider();
						}
					}
				}

				if (1 >= targets.Length)
				{
					var role = target as RoleComplete;

					EditorGUILayout.Separator();
					testData = EditorGUILayout.BeginToggleGroup("Test Data", testData);
					if (testData)
					{
						var newMoveSpeed = EditorGUILayout.FloatField("Move Speed", moveSpeed);
						if (moveSpeed != newMoveSpeed)
						{
							moveSpeed = newMoveSpeed;
							datas["MoveSpd"] = moveSpeed*1000;
						}
						EditorGUILayout.Separator();
						if (0 < datas.Count && GUILayout.Button("Refresh Data") && null != LuaLuancher.Me)
						{
							LuaLuancher.Me.Call("Debug_SetAttrs", 
								role.GUID, 
								datas.Keys.ToArray(), 
								datas.Values.ToArray());
							datas.Clear();
						}
					}
					EditorGUILayout.EndToggleGroup();

					EditorGUILayout.Separator();
					playAction = EditorGUILayout.BeginToggleGroup("Play Action", playAction);
					if (playAction && null != role.body && null != role.body.mainAnimator)
					{
						var animator = role.body.mainAnimator;
						UnityEditor.Animations.AnimatorController controller = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
						if (null != controller && 0 < controller.layers.Length)
						{
							var layer = controller.layers[0];
							var states = layer.stateMachine.states;
							if (!states.IsNullOrEmpty())
							{
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
							}

							if (0 == stateNameHash_)
							{
								stateNameHash_ = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
							}
							stateNameHash_ = EditorGUILayout.IntPopup("State", stateNameHash_, stateNames_, stateNameHashes_);
							if (GUILayout.Button("Play"))
							{
								role.PlayAction(stateNameHash_, stateNameHash_, 1, 0, false, null, null);
							}
						}
					}
					EditorGUILayout.EndToggleGroup();
				
					EditorGUILayout.Separator();
					testDress = EditorGUILayout.BeginToggleGroup("Test Dress", testDress);
					if (testDress)
					{
						role.weaponEnable = EditorGUILayout.ToggleLeft("Show Weapon", role.weaponEnable);
						role.mountEnable = EditorGUILayout.ToggleLeft("Show Mount", role.mountEnable);
						for (int i = 0; i < partFolders.Length; ++i)
						{
							newPartIDs[i] = EditorGUILayout.IntField(partFolders[i], newPartIDs[i]);
						}
						EditorGUILayout.Separator();
						if (GUILayout.Button("Dress"))
						{
							for (int i = 0; i < newPartIDs.Length; ++i)
							{
								var part = role.parts[i];
								var oldID = 0;
								if (null != part)
								{
									var match = Regex.Match(part.name, @"\d+");
									if (match.Success)
									{
										oldID = int.Parse(match.Value);
									}
								}
								if (oldID != newPartIDs[i])
								{
									var path = string.Format("Assets/Resources/Role/{0}/{1}.prefab", partFolders[i], newPartIDs[i]);
									var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
									if (null != prefab)
									{
										var partPrefab = prefab.GetComponent<RolePart>();
										if (null != partPrefab)
										{
											var newPart = Object.Instantiate<RolePart>(partPrefab);
											newPart.name = partPrefab.name;
											role.SetPart(i, newPart, true);
										}
										else
										{
											newPartIDs[i] = 0;
										}
									}
									else
									{
										newPartIDs[i] = 0;
									}
									if (null != part)
									{
										GameObject.Destroy(part.gameObject);
									}
								}
							}
						}
					}
					else
					{
						for (int i = 0; i < newPartIDs.Length; ++i)
						{
							var part = role.parts[i];
							var ID = 0;
							if (null != part)
							{
								var match = Regex.Match(part.name, @"\d+");
								if (match.Success)
								{
									ID = int.Parse(match.Value);
								}
							}
							newPartIDs[i] = ID;
						}
					}
					EditorGUILayout.EndToggleGroup();
				}
			}
		}
	}
} // namespace EditorTool
