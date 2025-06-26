using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;
using System.Reflection;

namespace EditorTool
{
	[CustomPropertyDrawer(typeof(DataValue))]
	public class DataValuePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
//			base.OnGUI (position, property, label);
			Prop parent = GetParentObjectOfProperty (property.propertyPath, property.serializedObject.targetObject) as Prop;
			parent.Validate ();
			SerializedProperty data = property.FindPropertyRelative ("data");
			SerializedProperty dataName = data.FindPropertyRelative ("name");
			SerializedProperty idProp = data.FindPropertyRelative ("id");
			SerializedProperty value = property.FindPropertyRelative ("_value");
			int id = idProp.intValue;
			if (property.displayName.IndexOf ("Element ") > -1) {
				id = int.Parse (property.displayName.Replace ("Element ", ""));
			}
			EditorGUI.LabelField (position, id.ToString ());
			position.x += 20; 
			EditorGUI.LabelField (position, string.IsNullOrEmpty (dataName.stringValue) ? "--未定义--" : dataName.stringValue);
			position.x += 100; 
			EditorGUI.LabelField (position, value.floatValue.ToString ());
			position.x += 60; 
			float raw = EditorGUI.FloatField (position, parent.rawValue [id]);
			if (raw != parent.rawValue [id]) {
				parent.rawValue [id] = raw;
//				parent.Set (id, raw);
			}
		}

		private object GetParentObjectOfProperty (string path, object obj)
		{
			string[] fields = path.Split ('.');
			
			// We've finally arrived at the final object that contains the property
			if (fields.Length == 1) {
				return obj;
			} else if (3 == fields.Length) {
				if (string.Equals ("Array", fields [1])
					&& 5 < fields [2].Length
					&& string.Equals ("data[", fields [2].Substring (0, 5))) {
					return obj;
				}
			}
			
			// We may have to walk public or private fields along the chain to finding our container object, so we have to allow for both
			FieldInfo fi = obj.GetType ().GetField (fields [0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (null == fi) {
				return obj;
			}
			obj = fi.GetValue (obj);
			
			// Keep searching for our object that contains the property
			return GetParentObjectOfProperty (string.Join (".", fields, 1, fields.Length - 1), obj);
		}
	}
} // namespace EditorTool
