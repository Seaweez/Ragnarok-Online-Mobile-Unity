using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	public class GyroHelper
	{
		private readonly Quaternion baseIdentity = Quaternion.Euler(90, 0, 0);
//		private readonly Quaternion landscapeRight = Quaternion.Euler(0, 0, 90);
//		private readonly Quaternion landscapeLeft = Quaternion.Euler(0, 0, -90);
//		private readonly Quaternion upsideDown = Quaternion.Euler(0, 0, 180);
		private Quaternion cameraBase = Quaternion.identity;
		private Quaternion calibration = Quaternion.identity;
		private Quaternion baseOrientation = Quaternion.Euler(90, 0, 0);
		private Quaternion baseOrientationRotationFix = Quaternion.identity;
		private Quaternion referanceRotation = Quaternion.identity;

		public static System.Func<Quaternion> GyroAttitudeHooker = null;

		private static Quaternion GetGyroAttitude()
		{
			if (null != GyroAttitudeHooker)
			{
				return GyroAttitudeHooker();
			}
			return Input.gyro.attitude;
		}

		private static Quaternion ConvertRotation(Quaternion q)
		{
			return new Quaternion(q.x, q.y, -q.z, -q.w);
		}

		public GyroHelper()
		{
			Reset();
		}

		public void Reset(Transform cameraTransform = null)
		{
			ResetBaseOrientation();
			UpdateCalibration(true);
			UpdateCameraBaseRotation(true, cameraTransform);
			RecalculateReferenceRotation();
		}
		
		private Quaternion GetRotFix()
		{
			#if UNITY_3_5
			if (Screen.orientation == ScreenOrientation.Portrait)
				return Quaternion.identity;
			
			if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape)
				return landscapeLeft;
			
			if (Screen.orientation == ScreenOrientation.LandscapeRight)
				return landscapeRight;
			
			if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
				return upsideDown;
			
			return Quaternion.identity;
			#else
			return Quaternion.identity;
			#endif
			
		}
		
		private void ResetBaseOrientation()
		{
			baseOrientationRotationFix = GetRotFix();
			baseOrientation = baseOrientationRotationFix * baseIdentity;
		}
		
		private void RecalculateReferenceRotation()
		{
			referanceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);
		}
		
		private void UpdateCalibration(bool onlyHorizontal)
		{
			if (onlyHorizontal)
			{
				var fw = (GetGyroAttitude()) * (-Vector3.forward);
				fw.z = 0;
				if (fw == Vector3.zero)
				{
					calibration = Quaternion.identity;
				}
				else
				{
					calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
				}
				
			}
			else
			{
				calibration = GetGyroAttitude();
			}
		}

		private void UpdateCameraBaseRotation(bool onlyHorizontal, Transform cameraTransform = null)
		{
			if (onlyHorizontal)
			{
				var fw = (null != cameraTransform) ? cameraTransform.forward : Vector3.forward;
				fw.y = 0;
				if (fw == Vector3.zero)
				{
					cameraBase = Quaternion.identity;
				}
				else
				{
					cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
				}
			}
			else
			{
				cameraBase = (null != cameraTransform) ? cameraTransform.rotation : Quaternion.identity;
			}
		}

		public Quaternion GetWorldRotation()
		{
			return cameraBase * (ConvertRotation(referanceRotation * GetGyroAttitude()) * GetRotFix());
		}
	}

	public static class GyroUtils 
	{
		private static GyroHelper helper_ = null;
		private static GyroHelper helper
		{
			get
			{
				if (null == helper_)
				{
					helper_ = new GyroHelper();
				}
				return helper_;
			}
		}

		public static void ResetGyro(Transform cameraTransform = null)
		{
			helper.Reset(cameraTransform);
		}
	
		public static Quaternion GetWorldRotation()
		{
			return helper.GetWorldRotation();
		}

	}
} // namespace RO
