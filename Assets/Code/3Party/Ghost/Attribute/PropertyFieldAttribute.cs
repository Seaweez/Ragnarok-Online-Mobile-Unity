using UnityEngine;
using System.Collections;

namespace Ghost.Attribute
{
	[System.AttributeUsage(System.AttributeTargets.Property)]
	public class PropertyFieldAttribute : System.Attribute
	{
		public string name { get; private set; }
		
		public PropertyFieldAttribute(string n)
		{
			name = n;
		}
	}
}
