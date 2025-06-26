using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[Serializable]
	public class Prop
	{
		protected bool _dirty;
		[SerializeField]
		protected DataValue[] _datas;
		protected float[] _rawValue;
		
		public float[] rawValue {
			get {
				return _rawValue;
			}
			set {
				if (_rawValue.Length == value.Length) {
					_rawValue = value;
					_dirty = true;
				} else
					Debuger.LogError ("prop rawValue length not match");
			}
		}
		
		public float[] values {
			get {
				Validate ();
				return _rawValue;
			}
		}

		[SerializeField]
		public DataValue[] datas {
			get {
				Validate ();
				return _datas;
			}
		}
		
		public Prop ()
		{
			_datas = new DataValue[ 64 ];
			_rawValue = new float[64];
			for (int i = 0; i < 64; i++) {
				_datas [i] = DataValue.Create (i, 0f);
			}
		}
		
		public void Clear ()
		{
			for (int i = 0; i < _rawValue.Length; i++) {
				_rawValue [i] = 0f;
			}
			_dirty = true;
		}
		
		public virtual Prop Clone ()
		{
			return new Prop
			{
				rawValue = _rawValue.Clone() as float[]
			};
		}
		
		public void SetDirty ()
		{
			_dirty = true;
		}

		public void Set (int id, float value)
		{
			_rawValue [id] = value;
			_dirty = true;
		}

		public void Set (DataValue d)
		{
			_rawValue [d.id] = d.value;
			_dirty = true;
		}

		public void Set (DataValue[] ds)
		{
			for (int i=0; i<ds.Length; i++) {
				DataValue d = ds [i];
				_rawValue [d.id] = d.value;
			}
			_dirty = true;
		}
		
		public void Plus (int id, float value)
		{
			_rawValue [id] += value;
			_dirty = true;
		}
		
		public void Plus (DataValue d)
		{
			if (d.data.CanbeCalculate) {
				_rawValue [d.id] += d.value;
				_dirty = true;
			}
		}
		
		public void Plus (DataValue[] ds)
		{
			for (int i=0; i<ds.Length; i++) {
				DataValue d = ds [i];
				if (d.data.CanbeCalculate)
					_rawValue [d.id] += d.value;
			}
			_dirty = true;
		}
		
		public void Minus (int id, float value)
		{
			_rawValue [id] -= value;
			_dirty = true;
		}
		
		public void Minus (DataValue d)
		{
			if (d.data.CanbeCalculate) {
				_rawValue [d.id] -= d.value;
				_dirty = true;
			}
		}
		
		public void Minus (DataValue[] ds)
		{
			for (int i=0; i<ds.Length; i++) {
				DataValue d = ds [i];
				if (d.data.CanbeCalculate)
					_rawValue [d.id] -= d.value;
			}
			_dirty = true;
		}
		
		virtual public void Validate ()
		{
			if (_dirty) {
				_dirty = false;
				for (int i = 0; i < _datas.Length; i++) {
					_datas [i].value = _rawValue [i];
				}
			}
		}
	}
} // namespace RO
