using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	public enum DataType
	{
		Float,
		Int,
		Bool
	}

	[Serializable]
	public class DataVO
	{
		public int id;
		public int typeID;
		public string name;
		public int priority;
		public DataType dataType;
		public bool isPercent;

		public bool CanbeCalculate {
			get {
				return dataType != DataType.Bool;
			}
		}
		
		public static DataVO GetData (int id)
		{
			if (DataConfigs.Instance == null || DataConfigs.Instance.dataMaps == null) {
				Debuger.LogError ("DataConfigs has no datas");
				return null;
			}
			DataVO data = DataConfigs.Instance.dataMaps [id];
			if (data == null) {
				Debuger.LogError ("DataConfigs id has out of range " + id);
			}
			return data;
		}
	}

} // namespace RO
