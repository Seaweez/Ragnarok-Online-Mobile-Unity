using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using RO;
using SLua;
using Ghost.Utils;
using Ghost.Extensions;

namespace EditorTool
{
	public static partial class GenAssets 
	{
		private static string[] GetSkillEffectsPaths (LuaTable skill)
		{
			string[] paths = new string[5]{
				LuaWorker.GetFieldString(skill, "E_Cast"), 
				LuaWorker.GetFieldString(skill, "E_Attack"), 
				LuaWorker.GetFieldString(skill, "E_Fire"), 
				LuaWorker.GetFieldString(skill, "E_Hit"), 
				LuaWorker.GetFieldString(skill, "E_Miss")};
			return paths;
		}
//		private static RandomArrayAudioClip[] GetSkillAudioEffectsPaths (LuaTable skill)
//		{
//			RandomArrayAudioClip [] audioEffects = new RandomArrayAudioClip[5];	
//			
//			var splitSymbol = new char[]{'-'};
//			var paths = StringUtils.Split(LuaWorker.GetFieldString(skill, "SE_cast"), splitSymbol);
//			if (!paths.IsNullOrEmpty())
//			{
//				var effect = new RandomArrayAudioClip();
//				effect.randomArray = paths;
//				audioEffects[0] = effect;
//			}
//			paths = StringUtils.Split(LuaWorker.GetFieldString(skill, "SE_attack"), splitSymbol);
//			if (!paths.IsNullOrEmpty())
//			{
//				var effect = new RandomArrayAudioClip();
//				effect.randomArray = paths;
//				audioEffects[1] = effect;
//			}
//			paths = StringUtils.Split(LuaWorker.GetFieldString(skill, "SE_fire"), splitSymbol);
//			if (!paths.IsNullOrEmpty())
//			{
//				var effect = new RandomArrayAudioClip();
//				effect.randomArray = paths;
//				audioEffects[2] = effect;
//			}
//			paths = StringUtils.Split(LuaWorker.GetFieldString(skill, "SE_hit"), splitSymbol);
//			if (!paths.IsNullOrEmpty())
//			{
//				var effect = new RandomArrayAudioClip();
//				effect.randomArray = paths;
//				audioEffects[3] = effect;
//			}
//			paths = StringUtils.Split(LuaWorker.GetFieldString(skill, "SE_miss"), splitSymbol);
//			if (!paths.IsNullOrEmpty())
//			{
//				var effect = new RandomArrayAudioClip();
//				effect.randomArray = paths;
//				audioEffects[4] = effect;
//			}
//			
//			return audioEffects;
//		}

		public static Dictionary<int, LuaTable> GetSkillInfos(out WorkerContainer workerContainer)
		{
			workerContainer = new WorkerContainer("Table_Skill");
			if (workerContainer.workers.IsNullOrEmpty())
			{
				return null;
			}
			
			var infos = new Dictionary<int, LuaTable>();
			
			foreach (var table in workerContainer.workers)
			{
				if (null != table)
				{
					foreach (var key_value in table)
					{
						var skillIDAndLevel = System.Convert.ToInt32(key_value.key);
						var ID = skillIDAndLevel / 1000;
						var info = key_value.value as LuaTable;
						if (infos.ContainsKey(ID))
						{
							continue;
						}
						infos.Add(ID, info);
					}
				}
			}
			
			return infos;
		}

//		[MenuItem( "RO/GenAssets/SkillDatas", false, 1001)]
//		public static void GenSkillDatas()
//		{
//			WorkerContainer workerContainer;
//			var infos = GetSkillInfos(out workerContainer);
//			using(workerContainer)
//			{
//				if (null == infos)
//				{
//					return;
//				}
//
//				var totalCount = infos.Count;
//				if (0 == totalCount)
//				{
//					Debug.LogFormat("No Skills");
//					return;
//				}
//				
//				try
//				{
//					var unhandledAssets = GetUnhandledAssets<SkillData>(AssetsFolder_SkillData);
//					
//					var i = 0;
//					foreach (var key_value in infos)
//					{
//						++i;
//						var ID = key_value.Key;
//						var info = key_value.Value;
//						var name = LuaWorker.GetFieldString(info, "NameEn");
//						var assetName = string.Format("{0}{1}", ID, name);
//						
//						ShowProgress("Skills", i, totalCount, assetName);
//
//						// 1.
//						var scriptPath = Path.ChangeExtension(PathUnity.Combine(SkillLogicFolder, LuaWorker.GetFieldString(info, "Logic")), ScriptExtension);
//						var scriptAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(scriptPath);
//						if (null == scriptAsset)
//						{
//							continue;
//						}
//
//						// 2.
//						SkillData asset;
//						if (unhandledAssets.TryGetValue(assetName, out asset))
//						{
//							unhandledAssets.Remove(assetName);
//						}
//						else
//						{
//							asset = ScriptableObject.CreateInstance<SkillData>();
//							asset.name = assetName;
//							
//							var path = Path.ChangeExtension(PathUnity.Combine(AssetsFolder_SkillData, assetName), AssetExtension);
//							AssetDatabase.CreateAsset (asset, path);
//							Debug.LogFormat("<color=green>New Created: </color>{0}", asset.name);
//						}
//						
//						asset.ID = ID;
//						asset.launchRange = LuaWorker.GetFieldFloat(info, "Launch_Range");
//						asset.level = 1;
//						asset.effects = GetSkillEffectsPaths(info);
//						asset.audioEffects = GetSkillAudioEffectsPaths(info);
//						asset.attackWait = !string.Equals("Buff", LuaWorker.GetFieldString(info, "SkillType"));
//						asset.endToWait = (1 != LuaWorker.GetFieldInt(info, "AttackStatus"));
//						asset.scriptPath = scriptPath;
//						if (null != asset)
//						{
//							EditorUtility.SetDirty(asset);
//						}
//					}
//
//					if (!workerContainer.debug)
//					{
//						foreach (var asset in unhandledAssets.Values)
//						{
//							Debug.LogFormat("<color=red>Deleted: </color>{0}", asset.name);
//							AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
//						}
//					}
//				}
//				finally
//				{
//					EditorUtility.ClearProgressBar();
//					AssetDatabase.SaveAssets();
//				}
//			}
//		}
	}
} // namespace EditorTool
