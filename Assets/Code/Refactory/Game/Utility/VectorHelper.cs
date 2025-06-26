using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class VectorHelper
	{
		public static float GetAngleByAxisY (Vector3 src, Vector3 target)
		{
			var direction = target - src;
			return Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
		}

		#region Vector2 SmoothDamp
		public static void Vector2SmoothDamp (Vector2 current, Vector2 target, Vector2 currentVelocity, float smoothTime, float maxSpeed, out float currentVelocityX, out float currentVelocityY, out float x, out float y)
		{
			Vector2SmoothDamp (current, target, currentVelocity, smoothTime, maxSpeed, Time.deltaTime, out currentVelocityX, out currentVelocityY, out x, out y);
		}

		public static void Vector2SmoothDamp (Vector2 current, Vector2 target, Vector2 currentVelocity, float smoothTime, out float currentVelocityX, out float currentVelocityY, out float x, out float y)
		{
			Vector2SmoothDamp (current, target, currentVelocity, smoothTime, float.PositiveInfinity, Time.deltaTime, out currentVelocityX, out currentVelocityY, out x, out y);
		}

		public static void Vector2SmoothDamp (Vector2 current, Vector2 target, Vector2 currentVelocity, float smoothTime, float maxSpeed, float deltaTime, out float currentVelocityX, out float currentVelocityY, out float x, out float y)
		{
			Vector2 v = Vector2.SmoothDamp (current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
			x = v.x;
			y = v.y;
			currentVelocityX = currentVelocity.x;
			currentVelocityY = currentVelocity.y;
		}
		#endregion

		#region Vector3 SmoothDamp
		public static void Vector3SmoothDamp (Vector3 current, Vector3 target, Vector3 currentVelocity, float smoothTime, float maxSpeed, out float currentVelocityX, out float currentVelocityY, out float currentVelocityZ, out float x, out float y, out float z)
		{
			Vector3SmoothDamp (current, target, currentVelocity, smoothTime, maxSpeed, Time.deltaTime, out currentVelocityX, out  currentVelocityY, out  currentVelocityZ, out x, out y, out z);
		}
	
		public static void Vector3SmoothDamp (Vector3 current, Vector3 target, Vector3 currentVelocity, float smoothTime, out float currentVelocityX, out float currentVelocityY, out float currentVelocityZ, out float x, out float y, out float z)
		{
			Vector3SmoothDamp (current, target, currentVelocity, smoothTime, float.PositiveInfinity, Time.deltaTime, out  currentVelocityX, out  currentVelocityY, out  currentVelocityZ, out x, out y, out z);
		}
		
		public static void Vector3SmoothDamp (Vector3 current, Vector3 target, Vector3 currentVelocity, float smoothTime, float maxSpeed, float deltaTime, out float currentVelocityX, out float currentVelocityY, out float currentVelocityZ, out float x, out float y, out float z)
		{
			Vector3 v = Vector3.SmoothDamp (current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
			x = v.x;
			y = v.y;
			z = v.z;
			currentVelocityX = currentVelocity.x;
			currentVelocityY = currentVelocity.y;
			currentVelocityZ = currentVelocity.z;
		}
		#endregion
		
		#region Vector3 Project
		public static void Vector3Project (Vector3 vector, Vector3 normal, out float x, out float y, out float z)
		{
			Vector3 v = Vector3.Project (vector, normal);
			x = v.x;
			y = v.y;
			z = v.z;
		}

		public static void Vector3ProjectOnPlane (Vector3 vector, Vector3 planeNormal, out float x, out float y, out float z)
		{
			Vector3 v = Vector3.ProjectOnPlane (vector, planeNormal);
			x = v.x;
			y = v.y;
			z = v.z;
		}
		#endregion

		#region Vector3 Slerp
		public static void Vector3Slerp (Vector3 current, Vector3 target, float t, out float x, out float y, out float z)
		{
			Vector3 v = Vector3.Slerp (current, target, t);
			x = v.x;
			y = v.y;
			z = v.z;
		}
		
		public static void Vector3SlerpUnclamped (Vector3 current, Vector3 target, float t, out float x, out float y, out float z)
		{
			Vector3 v = Vector3.SlerpUnclamped (current, target, t);
			x = v.x;
			y = v.y;
			z = v.z;
		}
		#endregion

		#region Vector3 RotateToward
		public static void Vector3RotateTowards (Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta, out float x, out float y, out float z)
		{
			Vector3 v = Vector3.RotateTowards (current, target, maxRadiansDelta, maxMagnitudeDelta);
			x = v.x;
			y = v.y;
			z = v.z;
		}
		#endregion

		#region Quaternion SetFromToRotation
		public static void QuaternionSetFromToRotation (Quaternion self, Vector3 from, Vector3 to, out float x, out float y, out float z, out float w)
		{
			self.SetFromToRotation (from, to);
			x = self.x;
			y = self.y;
			z = self.z;
			w = self.w;
		}
		#endregion

		#region Quaternion SetLookRotation
		public static void QuaternionSetLookRotation (Quaternion self, Vector3 view, Vector3 up, out float x, out float y, out float z, out float w)
		{
			self.SetLookRotation (view, up);
			x = self.x;
			y = self.y;
			z = self.z;
			w = self.w;
		}
		#endregion

		#region Quaternion Euler
		public static void QuaternionEuler (float x, float y, float z, out float qx, out float qy, out float qz, out float qw)
		{
			Quaternion q = Quaternion.Euler (x, y, z);
			qx = q.x;
			qy = q.y;
			qz = q.z;
			qw = q.w;
		}
		#endregion

		#region Quaternion Slerp
		public static void QuaternionSlerp (Quaternion current, Quaternion target, float t, out float x, out float y, out float z, out float w)
		{
			Quaternion v = Quaternion.Slerp (current, target, t);
			x = v.x;
			y = v.y;
			z = v.z;
			w = v.w;
		}
		
		public static void QuaternionSlerpUnclamped (Quaternion current, Quaternion target, float t, out float x, out float y, out float z, out float w)
		{
			Quaternion v = Quaternion.SlerpUnclamped (current, target, t);
			x = v.x;
			y = v.y;
			z = v.z;
			w = v.w;
		}
		#endregion

		#region Quaternion eulerAngles
		public static void QuaternionGetEulerAngles (Quaternion current, out float x, out float y, out float z)
		{
			Vector3 euler = current.eulerAngles;
			x = euler.x;
			y = euler.y;
			z = euler.z;
		}
		
		public static void QuaternionSetEulerAngles (Quaternion v, Vector3 euler, out float x, out float y, out float z, out float w)
		{
			v.eulerAngles = euler;
			x = v.x;
			y = v.y;
			z = v.z;
			w = v.w;
		}
		#endregion

		#region Quaternion RotateTowards
		public static void QuaternionRotateTowards (Quaternion from, Quaternion to, float maxDegreesDelta, out float x, out float y, out float z, out float w)
		{
			Quaternion res = Quaternion.RotateTowards (from, to, maxDegreesDelta);
			x = res.x;
			y = res.y;
			z = res.z;
			w = res.w;
		}

		#endregion

		#region Quaternion Axis
		public static void QuaternionAngleAxis (float angle, Vector3 axis, out float x, out float y, out float z, out float w)
		{
			Quaternion res = Quaternion.AngleAxis (angle, axis);
			x = res.x;
			y = res.y;
			z = res.z;
			w = res.w;
		}

		public static void QuaternionToAngleAxis (Quaternion q, out float angle, out float x, out float y, out float z)
		{
			Vector3 axis = Vector3.zero;
			q.ToAngleAxis (out angle, out axis);
			x = axis.x;
			y = axis.y;
			z = axis.z;
		}
		#endregion

		#region Quaternion Inverse
		public static void QuaternionInverse (Quaternion q, out float x, out float y, out float z, out float w)
		{
			q = Quaternion.Inverse (q);
			x = q.x;
			y = q.y;
			z = q.z;
			w = q.w;
		}
		#endregion
	}
} // namespace RO
