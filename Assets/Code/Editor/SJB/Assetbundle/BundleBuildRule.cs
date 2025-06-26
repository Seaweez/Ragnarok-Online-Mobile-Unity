using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EditorTool
{
	public static class BundleBuildRule
	{
		static SDictionary<string,BuildRule> _buildRules = new SDictionary<string, BuildRule> ();

		public static BuildRule GetRule (string name)
		{
			return _buildRules [name];
		}

		public static BuildRule MatchRule (string name)
		{
			foreach (KeyValuePair<string,BuildRule> kvp in _buildRules) {
				if(name.Contains(kvp.Key))
				{
					if(kvp.Value.subRules!=null)
					{
						BuildRule subRule = kvp.Value.SubMatch(name);
						if(subRule!=null)
							return subRule;
					}
					return kvp.Value;
				}
			}
			return null;
		}

		static BundleBuildRule ()
		{
			ArtFolderRule ();
			ResourcesFolderRule ();
			SceneFolderRule ();
			BuildSceneFolderRule ();
		}

		#region Resources目录
		static void ResourcesFolderRule ()
		{
			ResourcesShaders ();
			Scripts ();
		}
		#endregion

		#region Resources/Public/Shader目录
		static void ResourcesShaders ()
		{
			AddRule ("Resources/Public/Shader/", null, EBuildRule.FileFolderNoDirectory);
			AddRule ("Resources/GUI/v1/", null, EBuildRule.SelfName);
//			AddRule ("Resources/Role/", null, EBuildRule.FileFolder);
		}
		#endregion

		#region Resources/Scripts 目录
		static void Scripts ()
		{
#if LUA_STREAMINGASSETS || LUA_FASTPACKING 
            AddRule("Resources/Script/", null, EBuildRule.SubFolderName);
#else
            AddRule("Resources/Script2/", null, EBuildRule.SubFolderName);
#endif
        }
#endregion
		
#region Art目录
		static void ArtFolderRule ()
		{
			ArtModelFolderRule ();
			ArtPublicFolderRule ();
		}
#endregion

#region Art/Model目录
		static void ArtModelFolderRule ()
		{
			AddRule ("Art/Model/Bus/", null, EBuildRule.FileFolderNoDirectory);
			AddRule ("Art/Model/Effect/", null, EBuildRule.FolderName);
			AddRule ("Art/Model/Item/", null, EBuildRule.FolderName);
			//还未做切场景释放动态资源BUNDLE，所以只能先把art/scene下得Bundle包数量减少
			AddRule ("Art/Model/Scene/", null, EBuildRule.FileFolderNoDirectory);
			ArtModelRoleBody ();
			AddRule ("Art/Model/Role/Hair/", null, EBuildRule.FolderName);
			AddRule ("Art/Model/Role/Face/", null, EBuildRule.FolderName);
			AddRule ("Art/Model/Role/Head/", null, EBuildRule.FolderName);
			AddRule ("Art/Model/Role/Weapon/", null, EBuildRule.FolderName);
			AddRule ("Art/Model/Role/Wing/", null, EBuildRule.FolderName);
		}

		static void ArtModelRoleBody()
		{
			AddRule ("Art/Model/Role/Body/MiniBoss", null, EBuildRule.FileFolderNoDirectory);
			AddRule ("Art/Model/Role/Body/Monster", null, EBuildRule.FileFolderNoDirectory);
			AddRule ("Art/Model/Role/Body/Mount", null, EBuildRule.FolderName);
			AddRule ("Art/Model/Role/Body/MVP", null, EBuildRule.FileFolderNoDirectory);
			AddRule ("Art/Model/Role/Body/NPC", null, EBuildRule.FileFolderNoDirectory);
			AddRule ("Art/Model/Role/Body/Player", null, EBuildRule.FileFolderNoDirectory);
		}
#endregion

#region Art/Public目录
		static void ArtPublicFolderRule ()
		{
			AddRule ("Art/Public/Animation", null, EBuildRule.FileFolderNoDirectory);
			AddRule ("Art/Public/Material", null, EBuildRule.FolderName);
			AddRule ("Art/Public/mesh", null, EBuildRule.FolderName);
			AddRule ("Art/Public/Standard", null, EBuildRule.FolderName);
			BuildRule artTextureRule = CreateRule ("Art/Public/Texture", null, EBuildRule.FileFolderNoDirectory);
//			artTextureRule.AddRule (CreateRule ("Art/Public/Texture/Scene", null, EBuildRule.SelfName));
			AddRule (artTextureRule);

		}
#endregion

#region scene目录
		static void SceneFolderRule ()
		{
			AddRule ("/Scene/", null, EBuildRule.FileFolderNoDirectory);
		}
#endregion

#region DevelopScene目录
		static void BuildSceneFolderRule ()
		{
			AddRule ("DevelopScene/Terrains/Material/", null, EBuildRule.SelfName);
			AddRule ("DevelopScene/Terrains/Texture/", null, EBuildRule.SelfName);
			AddRule ("Code/3Party/T4M/Shaders/", "resources/public/shader/t4m", EBuildRule.SpecifyName);
		}
#endregion

		static BuildRule CreateRule(string folder, string specifyName, EBuildRule rule)
		{
			return new BuildRule (folder, specifyName, rule);
		}

		static void AddRule (string folder, string specifyName, EBuildRule rule)
		{
			AddRule (CreateRule (folder, specifyName, rule));
		}
		
		static void AddRule (BuildRule r)
		{
			_buildRules [r.checkFolder] = r;
		}
	}
} // namespace EditorTool
