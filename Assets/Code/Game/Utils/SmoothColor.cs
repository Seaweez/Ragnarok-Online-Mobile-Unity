using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[System.Serializable]
	public class SmoothColor 
	{
		public List<Color> colors = null;
		public float duration = 1f;

		public Color currentColor{get;private set;}

		private float progress = 0f;

		public bool valid
		{
			get
			{
				return 0 < duration && null != colors && 0 < colors.Count;
			}
		}

		public bool Update()
		{
			if (!valid)
			{
				return false;
			}

			progress += Time.deltaTime / duration;
			if (colors.Count < progress)
			{
				progress -= colors.Count;
			}

			var index = (int)progress;
			var nextIndex = (index+1) % colors.Count;

			currentColor = Color.Lerp(colors[index], colors[nextIndex], progress-index);

			return true;
		}
	}
} // namespace RO
