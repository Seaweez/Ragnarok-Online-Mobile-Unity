using UnityEngine;
using System.Collections;
using RO;

public class FlyingMessageOffset : MonoBehaviour 
{
	public Transform m_transMainCamera;
	private Vector2 m_lastRotation;
	private float m_speed;
	public UILabel m_lab;
	
	void Start () 
	{
		m_transMainCamera = GameObject.Find("Main Camera").transform;
		m_lastRotation = new Vector2(m_transMainCamera.localRotation.eulerAngles.y, m_transMainCamera.localRotation.eulerAngles.x);
	}

	void LateUpdate ()
	{
		float deltaX = - Time.deltaTime * m_speed;
		float deltaY = 0;

		Vector2 currentRotation = new Vector2(m_transMainCamera.localRotation.eulerAngles.y, m_transMainCamera.localRotation.eulerAngles.x);
		//Logger.Log("currentRotation = " + currentRotation + ", m_lastRotation = " + m_lastRotation);
		if (currentRotation != m_lastRotation)
		{
			Vector2 deltaRotation = currentRotation - m_lastRotation;
			if (Mathf.Abs(deltaRotation.x) > 180)
			{
				if (deltaRotation.x > 0)
					deltaRotation.x = 360 - deltaRotation.x;
				else
					deltaRotation.x = 360 + deltaRotation.x;
			}
			if (Mathf.Abs(deltaRotation.y) > 180)
			{
				if (deltaRotation.y > 0)
					deltaRotation.y = -360 + deltaRotation.y;
				else
					deltaRotation.y = 360 + deltaRotation.y;
			}

			float offsetX = -deltaRotation.x * 1280 / 90;
			float offsetY = deltaRotation.y * 720 / 90;
			//Logger.Log("offsetX = " + offsetX + ", offsetY = " + offsetY);
			deltaX += offsetX;
			deltaY += offsetY;
			//Logger.Log("deltaX = " + deltaX + ", deltaY = " + deltaY);
			m_lastRotation = currentRotation;
		}

		Vector3 localPos = transform.localPosition;
		localPos.x += deltaX;
		localPos.y += deltaY;
		transform.localPosition = localPos;

		if (localPos.x < - (1280 / 2 + m_lab.width))
			Destroy(gameObject);
	}

	public void Initialize(float speed)
	{
		m_speed = speed;
	}
}