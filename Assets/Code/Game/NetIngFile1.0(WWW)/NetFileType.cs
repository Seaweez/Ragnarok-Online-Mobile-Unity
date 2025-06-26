using UnityEngine;
using System.Collections;
using System;

public class NetFileType
{
	public enum E_Type
	{
		None,
		Type_TextAsset
	}

	public static E_Type TranslateFileType(string type_name)
	{
		E_Type type = E_Type.None;
		foreach (var value in Enum.GetValues(typeof(E_Type)))
		{
			E_Type tempType = (E_Type)value;
			if (tempType.ToString() == "Type_" + type_name)
			{
				type = tempType;
				break;
			}
		}
		return type;
	}

	protected bool TypeIsCompatibility(string type_name)
	{
		E_Type type = TranslateFileType(type_name);
		bool b = type != E_Type.None;
		return b;
	}
}
