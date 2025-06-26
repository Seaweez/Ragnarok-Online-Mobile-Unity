using UnityEngine;

namespace EditorTool
{
	public partial class ScriptChecker
	{
		public static void CheckParticle(ref string error, ParticleSystem ps, string prefixPath = "")
		{
			prefixPath += ps.name + " ParticleSystem->";
			Renderer r = ps.gameObject.GetComponent<Renderer> ();
			if (r == null)
				AppendError (ref error, prefixPath + "ParticleSystem中Renderer为空！");
			else
				CheckRenderer (ref error, r, prefixPath);
		}
	}
}