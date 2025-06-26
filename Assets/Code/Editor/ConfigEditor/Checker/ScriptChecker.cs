using System;
using UnityEngine;
using RO;
using UnityEditor.Animations;
using UnityEditor;

namespace EditorTool
{
	public partial class ScriptChecker
	{
		static float _Epsilon = 0.001f;
        public static string fileMissErrorFormat = "\"{0}\"文件不存在！";
        public ScriptChecker ()
		{
		}

		[MenuItem("Assets/Animation/test")]
		public static void test()
		{
			GameObject go = Selection.activeGameObject;
            //RolePartBody body = go.GetComponent<RolePartBody> ();
            //if(body.mainSMR.sharedMesh != null)
            //	Debug.LogError (body.mainSMR.sharedMesh.ToString());
            Animator animator = go.GetComponent<Animator>();
            RuntimeAnimatorController controller = animator.runtimeAnimatorController;
            if (controller != null)
            {
                for (int i = 0; i < controller.animationClips.Length; i++)
                {
                    Debug.LogError(controller.animationClips[i].name);
                }
            }
        }

		/// <summary>
		/// GO上有这些脚本那么进行脚本属性检查
		/// </summary>
		/// <param name="go">Go.</param>
		public static string ScriptsCheck(GameObject go)
		{
			string error = string.Empty;
			CheckRolePartBody (go, ref error);
			CheckAudioSource (go, ref error);
			CheckUIAtlas (go, ref error);
			return error;
		}

		public static void CheckRolePart(GameObject go, ref string error)
		{
			RolePart part = go.GetComponent<RolePart>();
			if (part != null)
			{
				CheckTransform (go, ref error);
                CheckRolePartMainRenderer(part, ref error);
			}
		}

		public static void CheckRolePartBody(GameObject go, ref string error)
		{
			RolePartBody body = go.GetComponent<RolePartBody> ();
			if (body != null)
			{
				CheckTransform (go, ref error);
				CheckRolePartBodyAnimator (body, ref error);
                CheckRolePartBodyMainRenderer(body, ref error);
            }
		}

		static void CheckRolePartBodyAnimator(RolePartBody body, ref string error)
		{
			if (body.mainAnimator != null)
			{
				CheckAnimator (body.gameObject, ref error);
			}
			else
			{
				if(body.animators.Length > 0)
				{
					for(int i = 0; i < body.animators.Length; i++)
					{
						if (body.animators [i] != null)
							CheckAnimator (body.animators [i].gameObject, ref error);
						else
						{
							AppendError (ref error, "RolePartBody中animators含空！");	
							break;
						}
					}
				}
				else
				{
					AppendError (ref error, "RolePartBody中animators和mainAnimator都为空！");
				}
			}
		}

		static void CheckRolePartBodyMainRenderer(RolePartBody body, ref string error)
		{
            CheckRolePartMainRenderer(body, ref error);
            if (body.mainSMR == null && body.mainMR == null && body.localBounds == null)
				AppendError (ref error, "RolePartBody中mainSMR/MR/LocalBounds全为空！");
		}

        static void CheckRolePartMainRenderer(RolePart part, ref string error)
        {
            if (part.mainSMR != null)
            {
                if (part.mainSMR.sharedMaterial != null && part.mainSMR.sharedMaterial.shader != null)
                {
                    Shader shader = part.mainSMR.sharedMaterial.shader;
                    if (shader.name != "RO/Role/PartOutline" && shader.name != "RO/Role/Part")
                        AppendError(ref error, "RolePart上的mainSMR中的材质球既不是Part也不是PartOutline！");
                    if (part.mainSMR.sharedMesh == null)
                        AppendError(ref error, "RolePart上的mainSMR中的Mesh丢失！");
                    for (int i = 0; i < part.mainSMR.sharedMaterials.Length; i++)
                    {
                        if (part.mainSMR.sharedMaterials[i] == null)
                        {
                            AppendError(ref error, "RolePart上的mainSMR中的Materials中含空!");
                            break;
                        }
                    }
                }
            }
            else if (part.mainMR != null)
            {
                CheckMeshRenderer(part.mainMR, ref error);
				CheckRolePartShader (part.mainMR, ref error);
            }
        }


        public static void CheckRolePartHair(GameObject go, ref string error)
		{
			RolePartHair hair = go.GetComponent<RolePartHair> ();
			if(hair != null)
				CheckRolePart(go, ref error);
		}

		private static void CheckRolePartMount(GameObject go, ref string error)
		{
			RolePartMount mount = go.GetComponent<RolePartMount> ();
			if(mount != null)
				CheckRolePart(go, ref error);
		}

		private static void CheckSkinnedMeshRenderer(SkinnedMeshRenderer smr, ref string error, string prefixPath = "")
		{
            if (smr != null)
			{
				prefixPath += smr.name + " SkinnedMeshRenderer->";
                if (smr.sharedMesh == null)
					AppendError(ref error, prefixPath + "mesh为空!");
                if (smr.sharedMaterials.Length > 0)
                {
                    for(int i = 0; i < smr.sharedMaterials.Length; i++)
                    {
                        if(smr.sharedMaterials[i] == null)
                        {
							AppendError(ref error, prefixPath + "materials数组中含有空项 !");
                            break;
                        }
                    }
                }
                else
					AppendError(ref error, prefixPath + "materials数组为空!");
            }
        }

		private static void CheckRolePartShader(MeshRenderer mr, ref string error, string prefixPath = "")
		{
			if(mr.sharedMaterial != null)
			{
				prefixPath += mr.name + " MeshRenderer->";
				Shader shader = mr.sharedMaterial.shader;
				if (shader.name != "RO/Role/PartOutline" && shader.name != "RO/Role/Part")
					AppendError(ref error, prefixPath + "材质球既不是Part也不是PartOutline！");
			}
		}

		private static void CheckMeshRenderer(MeshRenderer mr, ref string error, string prefixPath = "")
		{
            if (mr != null)
            {
				prefixPath += mr.name + " MeshRenderer->";
                CheckMeshFilter(mr.gameObject, ref error);
                if(mr.sharedMaterials.Length > 0)
				{
                    for (int i = 0; i < mr.sharedMaterials.Length; i++)
                    {
                        if (mr.sharedMaterials[i] == null)
                        {
							AppendError(ref error, prefixPath + "materials数组中含有空项 !");
                            break;
                        }
                    }
                }
                else
					AppendError(ref error, prefixPath + "materials数组为空!");
            }
        }

		private static void CheckAnimator(GameObject go, ref string error)
		{
			Animator animator = go.GetComponent<Animator> ();
			CheckAnimator (ref error, animator);
		}

		private static void CheckMeshFilter(GameObject go, ref string error)
		{
			MeshFilter meshfilter = go.GetComponent<MeshFilter> ();
            if (meshfilter == null)
                AppendError(ref error, "GameObject上不存在MeshFilter脚本!");
            else if (meshfilter.sharedMesh == null)
                AppendError(ref error, "MeshFilter脚本中的mesh丢失了!");
        }

		private static void CheckAudioSource(GameObject go, ref string error)
		{
			//			AudioSource source = go.GetComponent<AudioSource> ();
			//			if(source != null)
			//				Debug.LogError ("this is AudioSource !");
		}

		private static void CheckAudioClip(GameObject go, ref string error)
		{
			//			AudioSource audio = go.GetComponent<AudioClip> ();
			//			if(audio != null)
			//				Debug.LogError ("this is AudioClip !");
		}

		private static void CheckUIAtlas(GameObject go, ref string error)
		{
			UIAtlas uiatlas = go.GetComponent<UIAtlas> ();
			if (uiatlas != null)
			{
				if (uiatlas.spriteMaterial == null)
				{
					AppendError (ref error, "UIAtlas脚本上的材质丢失了！");
				}
			}
		}

		private static void CheckTransform (GameObject go, ref string error)
		{
			if (go.transform.localPosition != Vector3.zero)
				AppendError (ref error, "localPosition没有归零！");
			if(go.transform.localRotation.eulerAngles.magnitude > _Epsilon)
				AppendError (ref error, "localRotation没有归零！");
			if(go.transform.localScale != Vector3.one)
				AppendError (ref error, "localScale没有归一！");
		}

		static void AppendError(ref string error, string newError)
		{
			if(error != string.Empty)
				error += "\n";
			error += newError;
		}
	}
}

