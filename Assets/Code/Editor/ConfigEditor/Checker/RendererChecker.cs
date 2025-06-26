using UnityEngine;

namespace EditorTool
{
	public partial class ScriptChecker
	{
		public static void CheckRenderer(ref string error, Renderer r, string prefixPath = "")
		{
			prefixPath += "Renderer->";
			if (r.sharedMaterial == null)
				AppendError (ref error, prefixPath + "Renderer中材质球为空！");
			else
				CheckMaterial (ref error, r.sharedMaterial, prefixPath);
		}
	}
}

