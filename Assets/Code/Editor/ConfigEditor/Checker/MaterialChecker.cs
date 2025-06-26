using UnityEngine;

namespace EditorTool
{
	public partial class ScriptChecker
	{
		public static void CheckMaterial(ref string error, Material mat, string prefixPath = "")
		{
			prefixPath += "Material->";
			if (mat.mainTexture == null)
				AppendError (ref error, prefixPath + "材质球上贴图为空！");
			if(!mat.shader.name.Contains("RO"))
				AppendError (ref error, prefixPath + "材质球使用了StandardShader！");
		}
	}
}

