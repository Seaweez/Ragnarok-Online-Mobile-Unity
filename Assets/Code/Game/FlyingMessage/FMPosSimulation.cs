using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RO;

public class FlyingInfo
{
	public int id;
	public Vector3 posBorn;
	public float fSpeed;
	public bool isClockwise;
	public bool isInitialized;
	public Vector3 pos;
	public float r;
	public float angle;
	public System.Action<Vector3, float> callback;
	public bool isRotating;
	public float angleSum;

	public FlyingInfo(int id, Vector3 pos_born, float speed, bool is_clockwise = false, System.Action<Vector3, float> callback = null)
	{
		this.id = id;
		posBorn = pos_born;
		fSpeed = speed;
		isClockwise = is_clockwise;
		isInitialized = false;
		pos = default(Vector3);
		r = 0;
		angle = 0;
		this.callback = callback;
		isRotating = true;
		angleSum = 0;
	}

	public float Rad
	{
		get{return angle * Mathf.Deg2Rad;}
	}

	public void Reset(Vector3 pos_born, float speed)
	{
		posBorn = pos_born;
		fSpeed = speed;
		isInitialized = false;
		pos = default(Vector3);
		r = 0;
		angle = 0;
		isRotating = false;
		angleSum = 0;
	}

	public void Start()
	{
		isRotating = true;
	}

	public void Stop()
	{
		isRotating = false;
	}
}

[SLua.CustomLuaClassAttribute]
public class FMPosSimulation : MonoBehaviour
{
	private int m_indicator;
	private Dictionary<int, FlyingInfo> m_dictFI;
	private Vector3 m_posCenter;

	void LateUpdate()
	{
		if (m_dictFI != null && m_dictFI.Count > 0)
		{
			List<int> listKey = new List<int>();
			foreach (int key in m_dictFI.Keys)
			{
				listKey.Add(key);
			}
			int[] keys = listKey.ToArray();
			for (int i = 0; i < keys.Length; i++)
			{
				int id = keys[i];
				FlyingInfo fi = GetFI(id);
				if (fi != null && fi.isRotating)
				{
					float deltaAngle = fi.fSpeed * Time.deltaTime;
					fi.angleSum += deltaAngle;
					Simulate(id, (fi.isClockwise ? -1 : 1) * deltaAngle);
					if (fi.id > 0)
					{
						Vector3 pos = new Vector3(fi.r * Mathf.Cos(fi.Rad), fi.pos.y, fi.r * Mathf.Sin(fi.Rad));
						fi.pos = pos;
						if (fi.callback != null)
						{
							fi.callback(pos, fi.angleSum);
						}
					}
				}
			}
		}
	}

	public void Initialize(Vector3 pos_center)
	{
		m_posCenter = pos_center;
	}

	public int Register(Vector3 pos_born, float speed, bool is_clockwise = false, System.Action<Vector3, float> callback = null)
	{
		RO.LoggerUnused.Log("FUN >>> FMPosSimulation:Register");
		int id = ++m_indicator;
		FlyingInfo fi = new FlyingInfo(id, pos_born, speed, is_clockwise, callback);
		if (m_dictFI == null)
		{
			m_dictFI = new Dictionary<int, FlyingInfo>();
		}
		m_dictFI.Add(id, fi);
		InitializeFI(id);
		return id;
	}

	private void Simulate(int id, float delta_angle)
	{
		FlyingInfo fi = GetFI(id);
		if (fi != null && fi.isInitialized)
		{
			float currentAngle = fi.angle + delta_angle;
			while (Mathf.Abs(currentAngle) >= 360)
			{
				currentAngle = currentAngle + (currentAngle > 0 ? -1 : 1) * 360;
			}
			fi.angle = currentAngle;
		}
	}

	public void InitializeFI(int id)
	{
		FlyingInfo fi = GetFI(id);
		if (fi != null)
		{
			if (fi.isInitialized) return;
			fi.pos = fi.posBorn;
			float yValue = (fi.posBorn - m_posCenter).y;
			float distanceToCenter = Vector3.Distance(fi.posBorn, m_posCenter);
			float r = Mathf.Sqrt(Mathf.Pow(distanceToCenter, 2) - Mathf.Pow(yValue, 2));
			fi.r = r;
			int quadrant = Quadrant(id);
			Vector3 vCenter2FI = fi.pos - m_posCenter;
			float angleWithRight = Vector3.Angle(Vector3.right, Vector3.down * yValue + vCenter2FI);
			float angleWithRightRealy = 0;
			if (quadrant == 0)
				angleWithRightRealy = 0;
			else if (quadrant == 5)
				if (vCenter2FI.x > 0)
					angleWithRightRealy = 0;
				else
					angleWithRightRealy = 180;
			else if (quadrant == 6)
				if (vCenter2FI.z > 0)
					angleWithRightRealy = 90;
			else
				angleWithRightRealy = 270;
			else if (quadrant == 1 || quadrant == 2)
				angleWithRightRealy = angleWithRight;
			else if (quadrant == 3 || quadrant == 4)
				angleWithRightRealy = 360 - angleWithRight;
			fi.angle = angleWithRightRealy;
			fi.isInitialized = true;
		}
	}

	private int Quadrant(int id)
	{
		FlyingInfo fi = GetFI(id);
		if (fi != null)
		{
			Vector3 pos = fi.isInitialized ? fi.pos : fi.posBorn;
			Vector3 vCenter2FI = pos - m_posCenter;
			if (vCenter2FI.x == 0 && vCenter2FI.z == 0)
				return 0;
			else if (vCenter2FI.x == 0)
				return 6;
			else if (vCenter2FI.z == 0)
				return 5;
			else if (vCenter2FI.x > 0 && vCenter2FI.z > 0)
				return 1;
			else if (vCenter2FI.x < 0 && vCenter2FI.z > 0)
				return 2;
			else if (vCenter2FI.x < 0 && vCenter2FI.z < 0)
				return 3;
			else if (vCenter2FI.x > 0 && vCenter2FI.z < 0)
				return 4;
		}
		return -1;
	}

	public FlyingInfo GetFI(int id)
	{
		if (m_dictFI != null && m_dictFI.ContainsKey(id))
		{
			return m_dictFI[id];
		}
		return null;
	}

	public Vector3 GetPos(int id)
	{
		FlyingInfo fi = GetFI(id);
		if (fi != null) return fi.pos;
		return Vector3.zero;
	}

	public void ResetFI(int id, Vector3 pos_born, float speed)
	{
		FlyingInfo fi = GetFI(id);
		if (fi != null)
		{
			fi.Reset(pos_born, speed);
		}
	}

	public void StartFI(int id)
	{
		RO.LoggerUnused.Log(string.Format("FUN >>> StartFI, <param>id:{0}</>", id));
		FlyingInfo fi = GetFI(id);
		if (fi != null)
		{
			fi.Start();
		}
	}

	public void StopFI(int id)
	{
		FlyingInfo fi = GetFI(id);
		if (fi != null)
		{
			fi.Stop();
		}
	}

	// real far from lxy
	public Vector3 TranslatePoint(Vector3 p)
	{
		Camera camera = Camera.main;
		if (null == camera)
		{
			return p;
		}
		var dir = (p - camera.transform.position);
		return camera.transform.position + dir/dir.magnitude * 1 * (180-camera.fieldOfView);
	}
}