using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Ghost.Extensions;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(RolePart)),CanEditMultipleObjects]
	public class RolePartEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying)
			{
				EditorGUILayout.Separator();
				if (GUILayout.Button("CheckOrignalMaterials") && null != RolePartMaterialManager.Me)
				{
					var manager = RolePartMaterialManager.Me;
					for (int i = 0; i < targets.Length; ++i)
					{
						var role = targets[i] as RolePart;
						if (!role.smrs.IsNullOrEmpty())
						{
							foreach (var r in role.smrs)
							{
								if (!r.sharedMaterials.IsNullOrEmpty())
								{
									foreach (var m in r.sharedMaterials)
									{
										Debug.LogFormat("Material in RolePartMaterialManager: {0}", manager.FindInfoIndex(m));
									}
								}
							}
						}
					}
				}
			}
		}
	
	}
} // namespace EditorTool
