using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;

namespace RO.Config
{
	public static class Export
	{
		public const string ROOT_DIRECTORY = "../../client-export/";
		public static readonly string SCENE_DIRECTORY = PathUnity.Combine(ROOT_DIRECTORY, "Scene");
	}
} // namespace RO.Config
