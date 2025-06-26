using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[SLua.CustomLuaClassAttribute]
[RequireComponent(typeof(UIWidget))]
public class OutsideClick : MonoBehaviour
{
	private const int GENERAL_WIDTH = 1280;
	private const int GENERAL_HEIGHT = 720;
	private Vector4 m_rectOnScreen;
	private List<Action> m_callbacks = new List<Action>();

	private float GeneralRatio
	{
		get
		{
			return (float)GENERAL_WIDTH / GENERAL_HEIGHT;
		}
	}

	private float ScreenRatio
	{
		get
		{
			return (float)Screen.width / Screen.height;
		}
	}

	void Start ()
	{
		float activeWidth = Screen.width;
		float f = activeWidth / GENERAL_WIDTH;
		Vector2 localPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
		Vector2 posOnScreenOriginCenter = localPos * f;
		Vector2 posOnScreenOriginLeftBottom = new Vector2(posOnScreenOriginCenter.x + Screen.width / 2, posOnScreenOriginCenter.y + Screen.height / 2);
		UIWidget uiWidget = GetComponent<UIWidget>();
		Vector2 widgetSize = uiWidget.localSize;
		Vector2 sizeOnScreen = widgetSize * f;
		m_rectOnScreen = new Vector4(posOnScreenOriginLeftBottom.x, posOnScreenOriginLeftBottom.y, sizeOnScreen.x, sizeOnScreen.y);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 pos = Input.mousePosition;
			Vector2 point = new Vector2(pos.x, pos.y);
			if (!PointIsInsideOfRect(point))
			{
				for (int i = 0; i < m_callbacks.Count; i++)
				{
					Action callback = m_callbacks[i];
					if (callback != null)
					{
						callback();
					}
				}
			}
		}
	}

	private bool PointIsInsideOfRect(Vector2 point)
	{
		float deltaX = Mathf.Abs(point.x - m_rectOnScreen.x);
		float deltaY = Mathf.Abs(point.y - m_rectOnScreen.y);
		float halfOfWidth = m_rectOnScreen.z / 2;
		float halfOfHeight = m_rectOnScreen.w / 2;
		if (deltaX > halfOfWidth)
		{
			return false;
		}
		else if (deltaY > halfOfHeight)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public void RegisterCallback(Action action)
	{
		if (!m_callbacks.Contains(action))
		{
			m_callbacks.Add(action);
		}
	}
}
