using UnityEngine;
using System.Collections;
using RO;

public class FlyingMessageOffset2 : MonoBehaviour
{
	private Transform m_transMainCamera;
	private Camera m_mainCamera;
	private float m_rotateSpeed;
	private float m_angleXZ;
//	private float m_initializeAngleXZ;
	private float m_deltaXZAngleSum;
	private float m_angleYZ;
	private const int SCREEN_WIDTH = 1280;
	private const int SCREEN_HEIGHT = 720;
	private Vector2 m_lastRotation;
	private bool m_switch = true;
	public UILabel m_lab;
	
	void Start ()
	{
		m_transMainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform;
		m_mainCamera = m_transMainCamera.GetComponent<Camera>();
//		m_rotateSpeed = 20;
//		m_angleXZ = 0;
//		m_angleYZ = 30;
	}
	
	void LateUpdate ()
	{
		if (!m_switch) return;

		float deltaAngle = Time.deltaTime * m_rotateSpeed;

		m_deltaXZAngleSum += deltaAngle;
		if (m_deltaXZAngleSum >= 360)
		{
			m_deltaXZAngleSum = 0;
			m_lab.alpha = 1;
			TweenAlpha ta = TweenAlpha.Begin(gameObject, 0.5F, 0);
			ta.SetOnFinished(() => {
				DoStop();
			});
		}

		m_angleXZ -= deltaAngle;
		if (m_angleXZ < 0)
			m_angleXZ = m_angleXZ + 360;

		// x
		float angleXZBetweenFlyerAndCamera = AngleXZWithMainCameraRight();
		//Logger.Log("angleXZBetweenFlyerAndCamera = " + angleXZBetweenFlyerAndCamera);

		float xFOV = RealyXFOV();
		//Logger.Log("xFOV = " + xFOV);

		float viewPortX = 0;
		if (XZIsInMainCamera())
		{
			viewPortX = 1 - (360 - angleXZBetweenFlyerAndCamera) / xFOV;
		}
		else
		{
			viewPortX = angleXZBetweenFlyerAndCamera / xFOV + 1;
		}
		//Logger.Log("viewPortX = " + viewPortX);

		// y
		float angleYZWithCameraCenterDirection = AngleYZWithCameraCenterDirection();
		//Logger.Log("angleYZWithCameraCenterDirection = " + angleYZWithCameraCenterDirection);
		float viewPortY = 0;
		bool b = NearTopOrBottom();
		float fov = m_mainCamera.fieldOfView;
		if (b)
			viewPortY = (angleYZWithCameraCenterDirection + fov / 2) / fov;
		else
			viewPortY = 1 - (angleYZWithCameraCenterDirection + fov / 2) / fov;
		//Logger.Log("viewPortY = " + viewPortY);

//		float deltaY = 0;
//		Vector2 currentRotation = new Vector2(m_transMainCamera.localRotation.eulerAngles.y, m_transMainCamera.localRotation.eulerAngles.x);
//		if (currentRotation != m_lastRotation)
//		{
//			Vector2 deltaRotation = currentRotation - m_lastRotation;
//			if (Mathf.Abs(deltaRotation.y) > 180)
//			{
//				if (deltaRotation.y > 0)
//					deltaRotation.y = -360 + deltaRotation.y;
//				else
//					deltaRotation.y = 360 + deltaRotation.y;
//			}
//			float offsetY = deltaRotation.y * 720 / 90;
//			deltaY += offsetY;
//			m_lastRotation = currentRotation;
//		}

		Vector3 screenPos = new Vector3((viewPortX - 0.5F) * SCREEN_WIDTH, (viewPortY - 0.5F) * SCREEN_HEIGHT, 0);
		//Logger.Log("screenPos = " + screenPos);
		//screenPos.y += deltaY;
		transform.localPosition = screenPos;
	}

	public void Initialize(float rotateSpeed, float angleXZ, float angleYZ)
	{
		m_rotateSpeed = rotateSpeed;
		m_angleXZ = angleXZ;
//		m_initializeAngleXZ = angleXZ;
		m_deltaXZAngleSum = 0;
		m_angleYZ = angleYZ;
	}

	public void DoStart()
	{
		m_switch = true;
	}

	public void DoStop()
	{
		m_switch = false;
	}

	private Vector3 GetMainCameraRightEdgeDirection()
	{
		Vector3 vec1 = m_transMainCamera.position;
		Vector3 vec2 = m_mainCamera.ViewportToWorldPoint(new Vector3(1, 1, m_mainCamera.nearClipPlane * 10));
		Vector3 vec3 = m_mainCamera.ViewportToWorldPoint(new Vector3(1, 0, m_mainCamera.nearClipPlane * 10));
		Vector3 vec4 = vec2 - vec1;
		Vector3 vec5 = vec3 - vec1;
		Vector3 vec6 = Vector3.Cross(vec4, vec5);
		Vector3 vec7 = Vector3.up;
		Vector3 vec8 = Vector3.Cross(vec6, vec7);
		return vec8.normalized;
	}

	private Vector3 GetMainCameraLeftEdgeDirection()
	{
		Vector3 vec1 = m_transMainCamera.position;
		Vector3 vec2 = m_mainCamera.ViewportToWorldPoint(new Vector3(0, 1, m_mainCamera.nearClipPlane * 10));
		Vector3 vec3 = m_mainCamera.ViewportToWorldPoint(new Vector3(0, 0, m_mainCamera.nearClipPlane * 10));
		Vector3 vec4 = vec2 - vec1;
		Vector3 vec5 = vec3 - vec1;
		Vector3 vec6 = Vector3.Cross(vec4, vec5);
		Vector3 vec7 = Vector3.up;
		Vector3 vec8 = Vector3.Cross(vec6, vec7);
		return vec8.normalized;
	}

//	private Vector3 GetMainCameraBottomEdgeDirection()
//	{
//		Vector3 vec1 = m_transMainCamera.position;
//		Vector3 vec2 = m_mainCamera.ViewportToWorldPoint(new Vector3(0.5F, 0, m_mainCamera.nearClipPlane * 10));
//		return (vec2 - vec1).normalized;
//	}

//	private Vector3 GetMainCameraTopEdgeDirection()
//	{
//		Vector3 vec1 = m_transMainCamera.position;
//		Vector3 vec2 = m_mainCamera.ViewportToWorldPoint(new Vector3(0.5F, 1, m_mainCamera.nearClipPlane * 10));
//		return (vec2 - vec1).normalized;
//	}

	private Vector3 GetMainCameraCenterDirection()
	{
		Vector3 vec1 = m_transMainCamera.position;
		Vector3 vec2 = m_mainCamera.ViewportToWorldPoint(new Vector3(0.5F, 0.5F, m_mainCamera.nearClipPlane * 10));
		Vector3 vec3 = vec2 - vec1;
		return vec3;

//		Vector3 vec1 = m_transMainCamera.position;
//		Vector3 vec2 = m_mainCamera.ViewportToWorldPoint(new Vector3(0.5F, 0.5F, m_mainCamera.nearClipPlane * 10));
//		Logger.Log("y = " + vec2.y + ", z = " + vec2.z);
//		Vector3 vec3 = vec2 - vec1;
//		vec3.z = Mathf.Abs(vec3.z);
//		Vector3 vec4 = vec3.x > 0 ? Vector3.right : Vector3.left;
//		float f1 = Vector3.Dot(vec3, vec4);
//		Vector3 vec5 = vec3 - vec4 * f1;
//		return vec5.normalized;
	}

	private float XFOV()
	{
		Vector3 cameraPos = m_transMainCamera.position;
		Vector3 cameraLeftPos = m_mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5F, m_mainCamera.nearClipPlane));
		Vector3 cameraRightPos = m_mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5F, m_mainCamera.nearClipPlane));
		float xFOV = Vector3.Angle(cameraLeftPos - cameraPos, cameraRightPos - cameraPos);
		return xFOV;
	}

	private float RealyXFOV()
	{
		return Vector3.Angle(GetMainCameraLeftEdgeDirection(), GetMainCameraRightEdgeDirection());
	}

	private bool XZIsInMainCamera()
	{
		Vector3 dot1 = Vector3.Cross(DirectionXandZ(), GetMainCameraLeftEdgeDirection());
		Vector3 dot2 = Vector3.Cross(DirectionXandZ(), GetMainCameraRightEdgeDirection());
		return dot1.y <= 0 && dot2.y > 0;

		//return AngleIsApproximate(Vector3.Angle(DirectionXandZ(), GetMainCameraLeftEdgeDirection()) + Vector3.Angle(DirectionXandZ(), GetMainCameraRightEdgeDirection()) , RealyXFOV());
	}

//	private bool YZIsInMainCamera()
//	{
//		return AngleIsApproximate(Vector3.Angle(DirectionYandZ(), GetMainCameraTopEdgeDirection()) + Vector3.Angle(DirectionYandZ(), GetMainCameraBottomEdgeDirection()) , m_mainCamera.fieldOfView);
//	}

	private Vector3 DirectionXandZ()
	{
		Vector3 direction = Vector3.zero;
		float realyAngle = 0;
		if (m_angleXZ >= 0 && m_angleXZ <= 90)
			realyAngle = 90 - m_angleXZ;
		else if (m_angleXZ > 90 && m_angleXZ <= 270)
			realyAngle = 360 - (m_angleXZ - 90);
		else if (m_angleXZ > 270 && m_angleXZ < 360)
			realyAngle = 360 - m_angleXZ + 90;
		direction.x = Mathf.Cos(realyAngle * Mathf.Deg2Rad);
		direction.z = Mathf.Sin(realyAngle * Mathf.Deg2Rad);
		return direction;
	}

	private Vector3 DirectionYandZ()
	{
		Vector3 vec1 = GetMainCameraCenterDirection();
		bool isUp = vec1.y > 0;
		Vector3 vec2 = isUp ? Vector3.up : Vector3.down;
		float f1 = Vector3.Dot(vec1, vec2);
		Vector3 vec3 = vec1 - vec2 * f1;
		if (m_angleYZ == 90)
		{
			return Vector3.up;
		}
		else if (m_angleYZ == 270)
		{
			return Vector3.down;
		}
		float f2 = Mathf.Tan(m_angleYZ * Mathf.Deg2Rad);
		float f3 = f2 * vec3.magnitude;
		Vector3 vec4 = vec3;
		bool yzIsUp = m_angleYZ >= 0 && m_angleYZ <= 180;
		vec4.y = yzIsUp ? f3 : -f3;
		return vec4.normalized;

//		Vector3 direction = Vector3.zero;
//		direction.z = Mathf.Cos(m_angleYZ * Mathf.Deg2Rad);
//		direction.y = Mathf.Sin(m_angleYZ * Mathf.Deg2Rad);
//		return direction;
	}

	private bool AngleIsApproximate(float angle1, float angle2)
	{
		float delta = angle1 - angle2;
		return Mathf.Abs(delta) <= 0.1F;
	}

	private float AngleXZWithMainCameraRight()
	{
		float angle = 0;
		Vector3 directionCameraRight = GetMainCameraRightEdgeDirection();
		Vector3 direction = DirectionXandZ();
		angle = Vector3.Angle(direction, directionCameraRight);
		Vector3 cross = Vector3.Cross(direction, directionCameraRight);
		if (cross.y > 0)
		{
			angle = 360 - angle;
		}
		return angle;
	}

//	private float AngleYZWithMainCameraBottom()
//	{
//		float angle = 0;
//		Vector3 direction = DirectionYandZ();
//		Vector3 directionCameraBottom = GetMainCameraBottomEdgeDirection();
//		angle = Vector3.Angle(direction, directionCameraBottom);
//		return angle;
//	}

//	private float AngleYZWithMainCameraTop()
//	{
//		float angle = 0;
//		Vector3 direction = DirectionYandZ();
//		Vector3 directionCameraTop = GetMainCameraTopEdgeDirection();
//		angle = Vector3.Angle(direction, directionCameraTop);
//		return angle;
//	}

	private bool NearTopOrBottom()
	{
		float angle = AngleYZWithCameraCenterDirection();
		if (angle == 0 || angle == 180) return true;

		bool b = false;
		Vector3 direction = DirectionYandZ();
		Vector3 cameraCenterDirection = GetMainCameraCenterDirection();
		Vector3 cross = Vector3.Cross(direction, cameraCenterDirection);
		bool b1 = cameraCenterDirection.z > 0;
		if (cross.x > 0)
			b = b1;
		else
			b = !b1;
		return b;
	}

	private float AngleYZWithCameraCenterDirection()
	{
		Vector3 direction = DirectionYandZ();

		Vector3 cameraCenterDirection = GetMainCameraCenterDirection();
		float angle = Vector3.Angle(direction, cameraCenterDirection);
		return angle;
	}
}
