using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO.Test
{
	public class TestFarMove : MonoBehaviour 
	{
		public new Camera camera;
		public float cameraDistance = 50;

		public Vector3[] farPoints;
		public float speed = 1;

		private int index = 0;
		private Vector3 currentPosition = Vector3.zero;
		private Vector3 nextPoint = Vector3.zero;

		private Vector3 GetNextPoint()
		{
			var nextIndex = index + 1;
			if (!farPoints.CheckIndex(nextIndex))
			{
				nextIndex = 0;
			}
			index = nextIndex;
			return farPoints[index];
		}

		private Vector3 TranslatePoint(Vector3 p)
		{
			if (null == camera)
			{
				return p;
			}
			var dir = (p - camera.transform.position);
			return camera.transform.position + dir/dir.magnitude * cameraDistance;
		}

		private void Init()
		{
			nextPoint = farPoints[0];
			currentPosition = nextPoint;
			transform.position = TranslatePoint(currentPosition);
		}

		void Start()
		{
			if (!farPoints.IsNullOrEmpty())
			{
				Init();
			}
		}
	
		void Update()
		{
			if (farPoints.IsNullOrEmpty())
			{
				return;
			}
			if (1 == farPoints.Length)
			{
				Init();
			}
			else
			{
				if (Vector3.Equals(nextPoint, currentPosition))
				{
					nextPoint = GetNextPoint();
				}
				else
				{
					currentPosition = Vector3.MoveTowards(currentPosition, nextPoint, Time.deltaTime*speed);
					transform.position = TranslatePoint(currentPosition);
				}
			}
		}

	}
} // namespace RO.Test
