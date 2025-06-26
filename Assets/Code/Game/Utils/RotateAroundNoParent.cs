using UnityEngine;
using System.Collections;
using RO;

[SLua.CustomLuaClassAttribute]
public class RotateAroundNoParent : MonoBehaviour
{
	private Vector3 m_targetPoint;
	public Vector3 TargetPoint
	{
		get{return m_targetPoint;}
		set{m_targetPoint = value;}
	}

	private bool m_bSwitch;
	// rotate angle per second
	private float m_speed;
	private float m_r;
	private float m_angleAccumulation;
	private float m_height;

	private float m_rotateTime;
	private float m_deltaRotateTime;
	public UILabel m_lab;

	private bool m_flagReachRotateTime;
	
	void LateUpdate ()
	{
		if (m_bSwitch)
		{
			transform.LookAt(m_targetPoint);
			m_angleAccumulation += 3.14f * (m_speed * Time.deltaTime) / 180;
			float xAxisValue = Mathf.Cos(m_angleAccumulation) * m_r;
			float zAxisValue = Mathf.Sin(m_angleAccumulation) * m_r;
			transform.position = new Vector3(m_targetPoint.x + xAxisValue, m_height, m_targetPoint.z + zAxisValue);

			m_deltaRotateTime += Time.deltaTime;
			//Logger.Log(m_deltaRotateTime + ", " + m_rotateTime + ", " + (m_deltaRotateTime >= m_rotateTime) + ", " + m_flagReachRotateTime);
			if ((m_deltaRotateTime >= m_rotateTime) && !m_flagReachRotateTime)
			{
				m_flagReachRotateTime = true;
				TweenAlpha ta = TweenAlpha.Begin(gameObject, 1, 0);
				ta.SetOnFinished(() => {
					DoStop();
				});
			}
		}
	}

	public void Initialize(float speed, float r, float height)
	{
		m_speed = speed;
		m_r = r;
		m_height = height;

		float angle = 0;
		float xAxis = transform.position.x;
		float zAxis = transform.position.z;
		float deltaXAxis = xAxis - m_targetPoint.x;
		float deltaZAxis = zAxis - m_targetPoint.z;
		float tanValue = deltaZAxis / deltaXAxis;
		float rawAngle = Mathf.Atan(tanValue);
		if (tanValue >= 0)
		{
			if (deltaXAxis > 0)
				angle = rawAngle;
			else
				angle = Mathf.PI + rawAngle;
		}
		else
		{
			if (deltaXAxis > 0)
				angle = 2 * Mathf.PI + rawAngle;
			else
				angle = Mathf.PI + rawAngle;
		}
		m_angleAccumulation = angle;
		//Logger.Log("m_angleAccumulation = " + m_angleAccumulation);

		m_rotateTime = 360 / speed;
		m_deltaRotateTime = 0;
		m_flagReachRotateTime = false;
	}

	public void DoStart()
	{
		m_bSwitch = true;
	}

	public void DoStop()
	{
		m_bSwitch = false;
	}
}
