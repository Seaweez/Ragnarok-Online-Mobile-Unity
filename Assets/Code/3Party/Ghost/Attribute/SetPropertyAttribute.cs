using UnityEngine;
using System.Collections;

namespace Ghost.Attribute
{
	public class SetPropertyAttribute : PropertyAttribute
	{
		public string name { get; private set; }
		public bool isDirty { get; set; }
		
		public SetPropertyAttribute(string n)
		{
			this.name = n;
		}
	}
}
