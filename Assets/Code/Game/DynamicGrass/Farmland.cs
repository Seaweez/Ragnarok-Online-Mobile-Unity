using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RO;

[SLua.CustomLuaClassAttribute]
public class Farmland : MonoBehaviour
{
	[HideInInspector]
	public DynamicGrass.E_PlantType plantType;

	public int id;
	[HideInInspector]
	public int widthBaseUnit;
	[HideInInspector]
	public int heightBaseUnit;
	public int[] SizeBaseUnit
	{
		get
		{
			return new int[2]{widthBaseUnit, heightBaseUnit};
		}
	}

//	[HideInInspector]
	public float widthOfUnitLand;
//	[HideInInspector]
	public float heightOfUnitLand;
	public float[] SizeOfUnitLand
	{
		get
		{
			return new float[2]{widthOfUnitLand, heightOfUnitLand};
		}
	}
	
	public float Width
	{
		get
		{
			return widthBaseUnit * widthOfUnitLand;
		}
	}
	public float Height
	{
		get
		{
			return heightBaseUnit * heightOfUnitLand;
		}
	}
	public float[] Size
	{
		get
		{
			return new float[2]{Width, Height};
		}
	}

	public Vector3 AnchorPoint
	{
		get
		{
			return transform.position;
		}
	}
	public Vector3 FarestPoint
	{
		get
		{
			Vector3 vec3 = AnchorPoint;
			vec3.x += Width;
			vec3.z += Height;
			return vec3;
		}
	}

	public int[] occupyUnitLands;
	public int unitIndexMax
	{
		get
		{
			return widthBaseUnit * heightBaseUnit;
		}
	}

	private DynamicGrass[] dynamicGrasses;
	#region NO APPROPRIATE
	public DynamicGrass[] DynamicGrasses
	{
		get
		{
			return dynamicGrasses;
		}
	}
	#endregion NO APPROPRIATE
	public float effectiveDistance = 2;

	private Dictionary<int, int[]> lastEffectiveUnitsOfRoles;

	public Rect FarmlandRect
	{
		get {
			return new Rect (AnchorPoint.x, AnchorPoint.z, Width, Height);
		}
	}

	void Awake()
	{
//		if (FarmlandManager.Ins != null)
//		{
//			FarmlandManager.Ins.Add(this);
//		}
	}
	
	void Start ()
	{
//		dynamicGrasses = transform.GetComponentsInChildren<DynamicGrass>();
//		foreach (DynamicGrass dg in dynamicGrasses)
//		{
//			dg.Launch(effectiveDistance);
//		}
//		lastEffectiveUnitsOfRoles = new Dictionary<int, int[]>();
	}

	void OnDestroy()
	{
//		if (FarmlandManager.Ins != null)
//		{
//			FarmlandManager.Ins.Remove(this);
//		}
//		Release();
	}

	public int[] GetPosBaseUnit(int unit_index)
	{
		if (unit_index > 0 && unit_index <= unitIndexMax)
		{
			if (widthBaseUnit > 0)
			{
				int line = 0;
				int column = 0;
				int divisor = unit_index / widthBaseUnit;
				int remainder = unit_index % widthBaseUnit;
				if (remainder > 0)
				{
					column = remainder;
					line = divisor + 1;
				}
				else
				{
					column = widthBaseUnit;
					line = divisor;
				}
				return new int[2]{line, column};
			}
		}
		return null;
	}

	public Vector3 GetLocalPosOfUnit(int unit_index)
	{
		int[] posBaseUnit = GetPosBaseUnit(unit_index);
		if (posBaseUnit != null && posBaseUnit.Length == 2)
		{
			int line = posBaseUnit[0];
			int column = posBaseUnit[1];
			float xValue = (column - 0.5f) * widthOfUnitLand;
			float zValue = (line - 0.5f) * heightOfUnitLand;
			return new Vector3(xValue, 0, zValue);
		}
		return Vector3.zero;
	}

	public Vector3 GetPosOfUnit(int unit_index)
	{
		Vector3 localPosOfUnit = GetLocalPosOfUnit(unit_index);
		return transform.TransformPoint(localPosOfUnit);
	}

	public bool IsPointInside(Vector2 p)
	{
		return FarmlandRect.Contains(p);
	}

	public bool IsPointInside(Vector3 p)
	{
		return IsPointInside(new Vector2(p.x, p.z));
	}

	public int[] GetEffectiveUnitsFromLocalPos(Vector3 localPos)
	{
		Vector3 posRelativeToAnchorPoint = localPos;
		float xValue = posRelativeToAnchorPoint.x;
		float zValue = posRelativeToAnchorPoint.z;
		List<int> effectiveColumn = new List<int>();
		float tempXValue = 0;
		int indicator = 0;
		while (tempXValue < Width || Mathf.Approximately(tempXValue, Width))
		{
			if (tempXValue == xValue)
			{
				int leftColumn = indicator;
				if (leftColumn > 0)
				{
					effectiveColumn.Add(leftColumn);
				}
				int rightColumn = indicator + 1;
				if (rightColumn <= widthBaseUnit)
				{
					effectiveColumn.Add(rightColumn);
				}
				break;
			}
			else if (tempXValue > xValue)
			{
				int middleColumn = indicator;
				effectiveColumn.Add(middleColumn);
				int leftColumn = indicator - 1;
				if (leftColumn > 0)
				{
					effectiveColumn.Add(leftColumn);
				}
				int rightColumn = indicator + 1;
				if (rightColumn <= widthBaseUnit)
				{
					effectiveColumn.Add(rightColumn);
				}
				break;
			}
			tempXValue += widthOfUnitLand;
			indicator++;
		}
		List<int> effectiveLine = new List<int>();
		float tempYValue = 0;
		indicator = 0;
		while (tempYValue < Height || Mathf.Approximately(tempYValue, Height))
		{
			if (tempYValue == zValue)
			{
				int topLine = indicator + 1;
				if (topLine <= heightBaseUnit)
				{
					effectiveLine.Add(topLine);
				}
				int bottomLine = indicator;
				if (bottomLine > 0)
				{
					effectiveLine.Add(bottomLine);
				}
				break;
			}
			else if (tempYValue > zValue)
			{
				int middleLine = indicator;
				effectiveLine.Add(middleLine);
				int topLine = indicator + 1;
				if (topLine <= heightBaseUnit)
				{
					effectiveLine.Add(topLine);
				}
				int bottomLine = indicator - 1;
				if (bottomLine > 0)
				{
					effectiveLine.Add(bottomLine);
				}
				break;
			}
			tempYValue += heightOfUnitLand;
			indicator++;
		}
		int[] unitsIndex = new int[effectiveLine.Count * effectiveColumn.Count];
		for (int i = 0; i < effectiveLine.Count; ++i)
		{
			int line = effectiveLine[i];
			int startIndex = i * effectiveColumn.Count;
			for (int j = 0; j < effectiveColumn.Count; ++j)
			{
				int column = effectiveColumn[j];
				int unitIndex = GetUnitIndexFromPosBaseUnit(line, column);
				unitsIndex[startIndex + j] = unitIndex;
			}
		}

		// remove invalid units index
		int[] validUnitsIndex = ArrayUtil.Intersection(unitsIndex, occupyUnitLands);

		return validUnitsIndex;
	}

	public int[] GetEffectiveUnitsFromPos(Vector3 pos)
	{
		if (IsPointInside(pos))
		{
			Vector3 posRelativeToAnchorPoint = pos - AnchorPoint;
			return GetEffectiveUnitsFromLocalPos(posRelativeToAnchorPoint);
		}
		return null;
	}

	public int GetUnitIndexFromPosBaseUnit(int line, int column)
	{
		return (line - 1) * widthBaseUnit + column;
	}

//	private bool isDynamic;
//	public void BeDynamic()
//	{
//		if (!isDynamic)
//		{
//			foreach (DynamicGrass dg in dynamicGrasses)
//			{
//				dg.grassSlope.enabled = true;
//			}
//		}
//		isDynamic = true;
//	}
//
//	public void BeStatic()
//	{
//		if (isDynamic)
//		{
//			foreach (DynamicGrass dg in dynamicGrasses)
//			{
//				dg.grassSlope.enabled = false;
//			}
//		}
//		isDynamic = false;
//	}
	
	public void SomebodyOccur(int roleID)
	{
		if (RolesOnCurrentMap.ins.Exist(roleID))
		{
			Vector3 posOfRole = RolesOnCurrentMap.ins.GetPosOfRole(roleID);
			int[] unitsIndex = GetEffectiveUnitsFromPos(posOfRole);
			if (unitsIndex != null && unitsIndex.Length > 0)
			{
				foreach (int unitIndex in unitsIndex)
				{
					DynamicGrass dg = GetDynamicGrassFromUnitIndex(unitIndex);
					dg.AddEffectiveBody(roleID);
				}
			}
			if (lastEffectiveUnitsOfRoles.ContainsKey(roleID))
			{
				lastEffectiveUnitsOfRoles[roleID] = unitsIndex;
			}
			else
			{
				lastEffectiveUnitsOfRoles.Add(roleID, unitsIndex);
			}
		}
	}
	public void SomebodyMove(int roleID)
	{
		if (RolesOnCurrentMap.ins.Exist(roleID))
		{
			Vector3 posOfRole = RolesOnCurrentMap.ins.GetPosOfRole(roleID);
			int[] unitsIndex = GetEffectiveUnitsFromPos(posOfRole);
			int[] lastEffectiveUnits = lastEffectiveUnitsOfRoles[roleID];
			int[] willMoveUnits = ArrayUtil.Intersection(unitsIndex, lastEffectiveUnits);
			if (willMoveUnits != null) {
				foreach (int unitIndex in willMoveUnits) {
					DynamicGrass dg = GetDynamicGrassFromUnitIndex (unitIndex);
					dg.MoveEffectiveBody (roleID);
				}
			}
			int[] willRemoveUnits = ArrayUtil.Complementary(lastEffectiveUnits, willMoveUnits);
			if (willRemoveUnits != null) {
				foreach (int unitIndex in willRemoveUnits) {
					DynamicGrass dg = GetDynamicGrassFromUnitIndex (unitIndex);
					dg.RemoveEffectiveBody (roleID);
				}
			}
			int[] willAddUnits = ArrayUtil.Complementary(unitsIndex, willMoveUnits);
			if (willAddUnits != null) {
				foreach (int unitIndex in willAddUnits) {
					DynamicGrass dg = GetDynamicGrassFromUnitIndex (unitIndex);
					dg.AddEffectiveBody (roleID);
				}
			}
			lastEffectiveUnitsOfRoles[roleID] = unitsIndex;
		}
	}
	public void SomebodyIdle(int roleID)
	{
		int[] lastEffectiveUnits = lastEffectiveUnitsOfRoles[roleID];
		if (lastEffectiveUnits != null && lastEffectiveUnits.Length > 0)
		{
			foreach (int unitIndex in lastEffectiveUnits)
			{
				DynamicGrass dg = GetDynamicGrassFromUnitIndex(unitIndex);
				dg.IdleEffectiveBody(roleID);
			}
		}
	}
	public void SomebodyLeave(int roleID)
	{
		int[] lastEffectiveUnits = lastEffectiveUnitsOfRoles[roleID];
		if (lastEffectiveUnits != null && lastEffectiveUnits.Length > 0)
		{
			foreach (int unitIndex in lastEffectiveUnits)
			{
				DynamicGrass dg = GetDynamicGrassFromUnitIndex(unitIndex);
				dg.RemoveEffectiveBody(roleID);
			}
			lastEffectiveUnitsOfRoles.Remove(roleID);
		}
	}

	private DynamicGrass GetDynamicGrassFromUnitIndex(int unitIndex)
	{
		if (dynamicGrasses != null && dynamicGrasses.Length > 0)
		{
			foreach (DynamicGrass dg in dynamicGrasses)
			{
				if (string.Equals(dg.gameObject.name, unitIndex.ToString()))
				{
					return dg;
				}
			}
		}
		return null;
	}

	private void Release()
	{
		for (int i = 0; i < dynamicGrasses.Length; ++i)
		{
			dynamicGrasses[i] = null;
		}
		dynamicGrasses = null;
	}

	private void DrawSelf()
	{
		Vector3 firstPoint = AnchorPoint;
		int unitCount = 0;
		while (unitCount <= widthBaseUnit)
		{
			Vector3 point = firstPoint;
			point.x += unitCount * widthOfUnitLand;
			Vector3 oppositePoint = point;
			oppositePoint.z += Height;
			Debug.DrawLine(point, oppositePoint);
			unitCount++;
		}
		unitCount = 0;
		while (unitCount <= heightBaseUnit)
		{
			Vector3 point = firstPoint;
			point.z += unitCount * heightOfUnitLand;
			Vector3 oppositePoint = point;
			oppositePoint.x += Width;
			Debug.DrawLine(point, oppositePoint);
			unitCount++;
		}
	}

	void OnDrawGizmos()
	{
		DrawSelf();
	}
}
