using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(Farmland))]
public class FarmlandEditor : Editor
{
	private bool isFoldOfSizeBaseUnit;
	private bool isFoldOfSizeOfUnit;
	private int selectPlantIndex;
	private int occupyUnitCountForRandom;
	private bool isOriginScale;

	void Awake()
	{
		isOriginScale = true;
	}
	
	public override void OnInspectorGUI ()
	{
        

		if (CouldBeEdited())
		{
			base.OnInspectorGUI ();
			
			Farmland fl = target as Farmland;
            if (fl.gameObject.GetComponent<FarmlandEditorConfigureValue>() == null)
            {   
               if( EditorUtility.DisplayDialog("有一个错误", "FarmlandEditorConfigureValue没有和Farmland出现在一个物体上 点确认帮你加上", "确认", "取消"))
                {
                    fl.gameObject.AddComponent<FarmlandEditorConfigureValue>();
                }
            }

			isFoldOfSizeBaseUnit = EditorGUILayout.Foldout(isFoldOfSizeBaseUnit, "Size Base Unit");
			if (isFoldOfSizeBaseUnit)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Width");
				string strWidthBaseUnit = EditorGUILayout.TextField(fl.widthBaseUnit.ToString());
				int widthBaseUnit = 0;
				if (int.TryParse(strWidthBaseUnit, out widthBaseUnit))
				{
					fl.widthBaseUnit = widthBaseUnit;
				}
				GUILayout.Label("Height");
				string strHeightBaseUnit = EditorGUILayout.TextField(fl.heightBaseUnit.ToString());
				int heightBaseUnit = 0;
				if (int.TryParse(strHeightBaseUnit, out heightBaseUnit))
				{
					fl.heightBaseUnit = heightBaseUnit;
				}
				EditorGUILayout.EndHorizontal();
			}
			isFoldOfSizeOfUnit = EditorGUILayout.Foldout(isFoldOfSizeOfUnit, "Size of Unit");
			if (isFoldOfSizeOfUnit)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Width");
				fl.widthOfUnitLand = EditorGUILayout.FloatField(fl.widthOfUnitLand);
				GUILayout.Label("Height");
				fl.heightOfUnitLand = EditorGUILayout.FloatField(fl.heightOfUnitLand);
				EditorGUILayout.EndHorizontal();
			}
			fl.plantType = (DynamicGrass.E_PlantType)EditorGUILayout.EnumPopup("Plant", fl.plantType);
			EditorGUILayout.BeginHorizontal();
			FarmlandEditorConfigureValue configureValue = fl.transform.GetComponent<FarmlandEditorConfigureValue> ();
			if (configureValue != null) {
				configureValue.isOriginScale = GUILayout.Toggle (configureValue.isOriginScale, "Origin Scale");
				isOriginScale = configureValue.isOriginScale;
			} else {
				isOriginScale = GUILayout.Toggle (isOriginScale, "Origin Scale");
			}
			if (GUILayout.Button("Full"))
			{
				fl.occupyUnitLands = new int[fl.widthBaseUnit * fl.heightBaseUnit];
				for (int i = 0; i < fl.occupyUnitLands.Length; ++i)
				{
					fl.occupyUnitLands[i] = i + 1;
				}
			}
			if (GUILayout.Button("Empty"))
			{
				fl.occupyUnitLands = null;
			}
			if (configureValue != null) {
				configureValue.occupyUnitCountForRandom = EditorGUILayout.IntField (configureValue.occupyUnitCountForRandom);
				occupyUnitCountForRandom = configureValue.occupyUnitCountForRandom;
			} else {
				occupyUnitCountForRandom = EditorGUILayout.IntField (occupyUnitCountForRandom);
			}
			if (GUILayout.Button("Random"))
			{
				fl.occupyUnitLands = new int[occupyUnitCountForRandom];
				int[] indexs = new int[fl.widthBaseUnit * fl.heightBaseUnit];
				int lengthOfIndexs = indexs.Length;
				System.Random random = new System.Random ();
				for (int i = 0; i < indexs.Length; ++i) {
					indexs [i] = i + 1;
				}
				for (int i = 0; i < indexs.Length; ++i) {
					int randomIndex = random.Next (i, lengthOfIndexs - 1);
					int randomNum = indexs[randomIndex];
					fl.occupyUnitLands [i] = randomNum;
					indexs [i] = indexs [i] ^ indexs [randomIndex];
					indexs [randomIndex] = indexs [randomIndex] ^ indexs [i];
					indexs [i] = indexs [i] ^ indexs [randomIndex];
					if (i == occupyUnitCountForRandom - 1) {
						break;
					}
				}
			}
			EditorGUILayout.EndHorizontal();
			if (GUI.changed)
			{
				GeneratePlants();

				fl.gameObject.SetActive(false);
				fl.gameObject.SetActive(true);
			}
			if (GUILayout.Button("FreeRotation"))
			{
				Transform[] transOfGrasses = GetTransOfGrasses();
				foreach (Transform transOfGrass in transOfGrasses)
				{
					float zRotationValue = UnityEngine.Random.Range (0, 360);
					Quaternion quaternion = transOfGrass.localRotation;
					Vector3 eulerAngles = quaternion.eulerAngles;
					eulerAngles.z = zRotationValue;
					quaternion.eulerAngles = eulerAngles;
					transOfGrass.localRotation = quaternion;
				}
			}

			SceneView.RepaintAll();
		}
	}

	private Transform GetTransOfPlants()
	{
		Farmland fl = target as Farmland;
		Transform transFL = fl.transform;
		Transform transPlants = transFL.Find("Plants");
		return transPlants;
	}

	private Transform[] GetTransOfGrasses()
	{
		Transform transPlants = GetTransOfPlants();
		int grassCount = transPlants.childCount;
		Transform[] transOfGrasses = new Transform[grassCount];
		for (int i = 0; i < grassCount; ++i)
		{
			transOfGrasses[i] = transPlants.GetChild(i);
		}
		return transOfGrasses;
	}

	private string pathOfPlantPrefab = "Assets/Resources/Public/plant";
	private GameObject LoadPlant(string prefab_name)
	{
		GameObject prefabOfPlant = AssetDatabase.LoadAssetAtPath<GameObject>(pathOfPlantPrefab + "/" + prefab_name);
		if (prefabOfPlant != null)
		{
			return Instantiate<GameObject>(prefabOfPlant);
		}
		return null;
	}

	private void GeneratePlants()
	{
		RemoveAllPlants();
		Farmland fl = target as Farmland;
		Transform transFL = fl.transform;
		Transform transPlants = transFL.Find("Plants");
		int[] unitsIndex = fl.occupyUnitLands;
		if (unitsIndex != null && unitsIndex.Length > 0)
		{
			for (int i = 0; i < unitsIndex.Length; ++i)
			{
				int unitIndex = unitsIndex[i];
				GeneratePlant(transPlants, unitIndex, fl.plantType);
			}
		}
	}

	private void GeneratePlant(Transform trans_parent, int unit_index, Enum plant_type)
	{
		Farmland fl = target as Farmland;
		int unitIndexMax = fl.unitIndexMax;
		if (unit_index > 0 && unit_index <= unitIndexMax)
		{
			RemovePlant(unit_index);
			string prefabName = "";
			DynamicGrass.E_PlantType plantType = (DynamicGrass.E_PlantType)plant_type;
			if (plantType == DynamicGrass.E_PlantType.Wheat) {
				prefabName = "botany_pay_fild02_xiaomai1.prefab";
			} else if (plantType == DynamicGrass.E_PlantType.Convallaria) {
				prefabName = "Convallaria_interact.prefab";
			}
			else if (plantType == DynamicGrass.E_PlantType.Clover) {
				prefabName = "Clover_interact.prefab";
			}
			GameObject goPlant = LoadPlant(prefabName);
			if (goPlant != null)
			{
				goPlant.layer = LayerMask.NameToLayer("Default");
				goPlant.isStatic = false;
//				goPlant.AddComponent<DynamicGrass>();
				goPlant.AddComponent<GrassSlopeV2>().shapeChangePercent = 100;
				goPlant.name = unit_index.ToString();
				Transform transPlant = goPlant.transform;
				transPlant.SetParent(trans_parent);
				transPlant.localPosition = fl.GetLocalPosOfUnit(unit_index);
				if (!isOriginScale) {
					Vector3 localScale = transPlant.localScale;
					float scaleCoefficient = UnityEngine.Random.Range (0.7f, 1f);
					localScale = localScale * scaleCoefficient;
					transPlant.localScale = localScale;
				}
			}
		}
		else
		{
			Debug.LogError(new IndexOutOfRangeException());
		}
	}

	private bool ExistPlant(int unit_index)
	{
		Transform transPlants = GetTransOfPlants();
		Transform transPlant = transPlants.Find(unit_index.ToString());
		return transPlant != null;
	}

	private void RemovePlant(int unit_index)
	{
		Transform transPlants = GetTransOfPlants();
		Transform transPlant = transPlants.Find(unit_index.ToString());
		if (transPlant != null)
		{
			GameObject.DestroyImmediate(transPlant.gameObject);
		}
	}

	private void RemoveAllPlants()
	{
		Farmland fl = target as Farmland;
		Transform transFL = fl.transform;
		Transform transPlants = transFL.Find("Plants");
		int childCount = transPlants.childCount;
		int indicator = childCount - 1;
		while (indicator >= 0)
		{
			GameObject.DestroyImmediate(transPlants.GetChild(indicator).gameObject);
			indicator--;
		}
	}

	private bool CouldBeEdited()
	{
		return !Application.isPlaying;
	}
}
