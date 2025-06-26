using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[Serializable]
	public class DataValue
	{
		public DataVO data;
		[SerializeField]
		protected float _value;

		public float value {
			set {
				if (data != null && data.dataType == DataType.Bool)
					_value = value >= 1 ? 1 : 0;
				else
					_value = value;
			}
			get {
				return _value;
			}
		}
		
		public int id {
			get{ return data.id;}
		}
		
		public string name {
			get{ return data.name;}
		}
		
		public int typeId {
			get{ return data.typeID;}
		}

		public DataType dataType {
			get{ return data.dataType;}
		}
		
		public int priority {
			get{ return data.priority;}
		}
		
		public DataValue Clone ()
		{
			return new DataValue
			{
				data = this.data,
				value = this.value
			};
		}
		
		public static DataValue Create (int id, float value)
		{
			return Create (DataVO.GetData (id), value);
		}
		
		public static DataValue Create (DataVO data, float value)
		{
			DataValue d = new DataValue ();
			d.data = data;
			d.value = value;
			return d;
		}
		
		public static DataValue operator * (DataValue d, float f)
		{
			if (d.data.CanbeCalculate) {
				DataValue data = d.Clone ();
				data.value *= f;
				return data;
			}
			return d;
		}
	
	}
} // namespace RO
