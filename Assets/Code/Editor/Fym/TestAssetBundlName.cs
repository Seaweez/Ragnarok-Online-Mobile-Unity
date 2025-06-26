using UnityEngine;
using UnityEditor;

public class TestAssetBundlName : MonoBehaviour
{
	private static void DoGetDependenciesEach(SelectionMode mode)
	{
		var selectedAssets = Selection.GetFiltered<Object>(mode);
		foreach (Object obj in selectedAssets)
		{
			string assetPath = AssetDatabase.GetAssetPath(obj);
			Debug.Log($"{assetPath}\ndepends on:");

			string[] dependencies = AssetDatabase.GetDependencies(new string[] { assetPath });
			foreach (var dependencyPath in dependencies)
			{
				Debug.Log(dependencyPath);  // แสดง log ของ dependencyPath

				if (!dependencyPath.EndsWith(".cs")) // ป้องกันไม่ให้ตั้งค่า AssetBundle name สำหรับไฟล์สคริปต์
				{
					var assetImporter = AssetImporter.GetAtPath(dependencyPath);
					assetImporter.assetBundleName = GetAssetBundleNameFromPath(dependencyPath, obj.name); // ตั้งชื่อตามที่อยู่ของ dependency และชื่อของ object
				}
			}
		}
	}

	[MenuItem("Assets/GetInfo/SetAssetBundleName")]
	static void GetDependenciesEach()
	{
		DoGetDependenciesEach(SelectionMode.Assets);
	}

	static string GetAssetBundleNameFromPath(string dependencyPath, string objectName)
	{
		// ลบคำว่า "assets/" และ ลบนามสกุลไฟล์และเพิ่มชื่อ object พร้อมนามสกุล .unity3d ท้ายสุด
		string modifiedPath = dependencyPath.ToLower().Replace("assets/", "");
		string pathWithoutExtension = System.IO.Path.GetDirectoryName(modifiedPath).Replace("\\", "/");
		return $"{pathWithoutExtension}/{objectName}.unity3d";
	}
}
