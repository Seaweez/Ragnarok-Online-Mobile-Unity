using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class MapUnitConfEditor : Editor 
{
	[MenuItem("RO/Scene/Generate Map Unit Info")]
	public static void GenerateMapUnitInfo()
	{
		string sceneDirectoryRootPath = Application.dataPath + "/../../../client-export/Scene";
		bool sdrExist = Directory.Exists(sceneDirectoryRootPath);
		if (sdrExist)
		{
			string[] scenesDirectoryPath = Directory.GetDirectories(sceneDirectoryRootPath, "Scene*");
			if (scenesDirectoryPath != null)
			{
				for (int i = 0; i < scenesDirectoryPath.Length; i++)
				{
					string sceneDirectoryPath = scenesDirectoryPath[i];
					bool sdExist = Directory.Exists(sceneDirectoryPath);
					if (sdExist)
					{
						DirectoryInfo sceneDirectoryInfo = new DirectoryInfo(sceneDirectoryPath);
						string sceneInfoFilePath = sceneDirectoryPath + "/SceneInfo.lua";
						bool sifExist = File.Exists(sceneInfoFilePath);
						if (sifExist)
						{
							byte[] bytes = File.ReadAllBytes(sceneInfoFilePath);
							string destinationDirectoryPath = Application.dataPath + "/Resources/Script/Config/Scene";
							if (!Directory.Exists(destinationDirectoryPath))
							{
								Directory.CreateDirectory(destinationDirectoryPath);
							}
							File.WriteAllBytes(destinationDirectoryPath + "/" + sceneDirectoryInfo.Name + ".txt", bytes);
						}
					}
				}
			}
		}
	}
}