using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	public class Cloneable : ICloneable
	{
		public object Clone()
		{
			return MemberwiseClone();
		}
	}
} // namespace RO
