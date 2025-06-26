using RO;
using System.IO;
using UnityEngine;

namespace EditorTool
{
    public partial class ScriptChecker
    {
		public static void CheckEffecthandle(ref string error, string path)
		{
			if(File.Exists(path))
			{
				GameObject go = AssetChecker.TryLoadAsset<GameObject> (path);
				if (go != null) 
				{
					ScriptChecker.CheckTransform (go, ref error);
					EffectHandle eh = go.GetComponent<EffectHandle> ();
					if (eh != null)
						CheckEffectHandle (ref error, eh);
					else
						AppendError (ref error, string.Format ("{0}上EffectHandle脚本丢失！", path));
				}
			}
			else
				AppendError(ref error, string.Format("{0}文件不存在！", path));
		}

		public static void CheckEffectHandle(ref string error, EffectHandle handle,  string prefixPath = "")
        {
			prefixPath += "EffectHandle->";
			CheckEffectHandle_Render (ref error, handle, prefixPath);
			CheckEffectHandle_Particles (ref error, handle, prefixPath);
			CheckEffectHandle_Animator (ref error, handle, prefixPath);
			CheckEffectHandle_Logic (ref error, handle, prefixPath);
        }

		private static void CheckEffectHandle_Render(ref string error, EffectHandle handle, string prefixPath = "")
		{
			MeshRenderer[] mrs = handle.gameObject.GetComponentsInChildren<MeshRenderer> ();
			for(int i=0; i<mrs.Length; i++)
				CheckMeshRenderer (mrs [i], ref error, prefixPath);

			SkinnedMeshRenderer[] smrs = handle.gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
			for(int i=0; i<smrs.Length; i++)
				CheckSkinnedMeshRenderer ( smrs [i], ref error, prefixPath);
		}

		private static void CheckEffectHandle_Particles(ref string error, EffectHandle handle, string prefixPath = "")
		{
			int actived = 0;
			for(int i=0; i < handle.particles.Length; i++)
			{
				if (handle.particles [i] == null) 
					AppendError (ref error, string.Format("{0}Particles数组中含空！index = {1}", prefixPath, i));
				else
					actived++;
			}

			ParticleSystem[] ps = handle.gameObject.GetComponentsInChildren<ParticleSystem> (true);
			if(ps.Length > actived)
				AppendError(ref error, prefixPath + "Particles数组内ParticleSystem少于实际数量！");
			else if(ps.Length < actived)
				AppendError(ref error, prefixPath + "Particles数组内ParticleSystem多于实际数量！");
			
			for(int i=0; i<ps.Length; i++)
				CheckParticle (ref error, ps [i], prefixPath);
		}

		private static void CheckEffectHandle_Animator(ref string error, EffectHandle handle, string prefixPath = "")
		{
			int actived = 0;
			for(int i=0; i < handle.animators.Length; i++)
			{
				if (handle.animators [i] == null) 
					AppendError (ref error, string.Format("{0}Animators数组中含空！index = {1}", prefixPath, i));
				else
					actived++;
			}

			Animator[] Anims = handle.gameObject.GetComponentsInChildren<Animator> (true);
			if(Anims.Length > actived)
				AppendError(ref error, prefixPath + "Animators数组内Animator少于实际数量！");
			else if(Anims.Length < actived)
				AppendError(ref error, prefixPath + "Animators数组内Animator多于实际数量！");

			for(int i=0; i<Anims.Length; i++)
				CheckAnimator (ref error, Anims [i], prefixPath);
		}

		private static void CheckEffectHandle_Logic(ref string error, EffectHandle handle, string prefixPath = "")
		{
			int actived = 0;
			for(int i=0; i < handle.logics.Length; i++)
			{
				if (handle.logics [i] == null)
					AppendError (ref error, string.Format("{0}Logics数组中含空！index = {1}", prefixPath, i));
				else
					actived++;
			}

			EffectLogic[] logics = handle.gameObject.GetComponentsInChildren<EffectLogic> (true);
			if(logics.Length > actived)
				AppendError(ref error, prefixPath + "logics数组内EffectLogic少于实际数量！");
			else if(logics.Length < actived)
				AppendError(ref error, prefixPath + "logics数组内EffectLogic多于实际数量！");
		}
    }
}