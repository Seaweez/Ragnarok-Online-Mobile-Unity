using UnityEngine;
using System.Collections;
using RO;
using System.Collections.Generic;

public class TestManager : SingleTonGO<TestManager>
{
	private GrassSlopeV2[] m_grassSlopes;
	private Transform m_transRole;
	private Dictionary<string, int> m_cachedEffectiveGrass;
	public float m_effectiveDistance = 2;
	private float m_forceID;
	private Vector3 m_lastPosRole;

	void Update()
	{
		if (m_transRole != null)
		{
			Vector3 posRole = m_transRole.position;
			if (posRole != m_lastPosRole)
			{
				m_lastPosRole = posRole;
				if (m_grassSlopes != null && m_grassSlopes.Length > 0)
				{
					for (int i = 0; i < m_grassSlopes.Length; ++i)
					{
						GrassSlopeV2 grassSlope = m_grassSlopes[i];
						grassSlope.gameObject.name = (i + 1).ToString();
						string name = grassSlope.gameObject.name;
						Vector3 posGrass = grassSlope.transform.position;
						Vector2 posXZGrass = new Vector2(posGrass.x, posGrass.z);
						Vector2 posXZRole = new Vector2(posRole.x, posRole.z);
						Vector2 grassToRole = posXZRole - posXZGrass;
						float distance = grassToRole.magnitude;
						if (distance <= m_effectiveDistance)
						{
							Vector2 forceDirection = Vector2.zero;
							Vector3 localPosRoleToGrass = grassSlope.transform.InverseTransformPoint(posRole);
							Vector2 localPosXYRoleToGrass = new Vector2(localPosRoleToGrass.x, localPosRoleToGrass.y);
							forceDirection = localPosXYRoleToGrass * -1;
							int force = Mathf.FloorToInt(- 100 * distance / m_effectiveDistance + 100);
							if (forceDirection != Vector2.zero && force > 0)
							{
								if (m_cachedEffectiveGrass.ContainsKey(name))
								{
									grassSlope.ChangeForce(m_cachedEffectiveGrass[name], forceDirection, force, 0.5f);
								}
								else
								{
									int id = grassSlope.AddForce(forceDirection, force, 0.5f);
									m_cachedEffectiveGrass.Add(name, id);
								}
							}
						}
						else
						{
							if (m_cachedEffectiveGrass.ContainsKey(name))
							{
								grassSlope.MinusForce(m_cachedEffectiveGrass[name]);
								m_cachedEffectiveGrass.Remove(name);
							}
						}
					}
				}
			}
		}
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 100, 50), "Start"))
		{
			GameObject goGrass = GameObject.Find("Grass");
			m_grassSlopes = goGrass.transform.GetComponentsInChildren<GrassSlopeV2>();
			m_transRole = GameObject.Find("150000").transform;
			Vector3 posRole = m_transRole.position;
			m_cachedEffectiveGrass = new Dictionary<string, int>();
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
