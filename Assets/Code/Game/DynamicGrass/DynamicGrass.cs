using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RO;

#region NO APPROPRIATE
[SLua.CustomLuaClassAttribute]
#endregion NO APPROPRIATE
public class DynamicGrass : MonoBehaviour
{
	public enum E_PlantType
	{
		Wheat,
		Convallaria,
		Clover
	}

	private class EffectiveBody
	{
		public int id;
		public int force;
		public bool isNew;
		public bool isMoving;
	}
	private List<EffectiveBody> cachedEffectiveBodys;

	#region NO APPROPRIATE
	public int EffectiveBodysCount
	{
		get
		{
			return cachedEffectiveBodys.Count;
		}
	}
	#endregion NO APPROPRIATE

	private GrassSlopeV2 grassSlope;

	public float effectiveDistance;

	void Awake()
	{
		grassSlope = transform.GetComponent<GrassSlopeV2>();
	}

	void Update()
	{
		if (cachedEffectiveBodys != null && cachedEffectiveBodys.Count > 0)
		{
			foreach (EffectiveBody body in cachedEffectiveBodys)
			{
				int roleID = body.id;
				string name = gameObject.name;
				Vector3 posGrass = transform.position;
				Vector2 posXZGrass = new Vector2(posGrass.x, posGrass.z);
				Vector3 posOfRole = RolesOnCurrentMap.ins.GetPosOfRole(roleID);
				Vector2 posXZRole = new Vector2(posOfRole.x, posOfRole.z);
				Vector2 grassToRole = posXZRole - posXZGrass;
				float distance = grassToRole.magnitude;
				if (distance <= effectiveDistance)
				{
					Vector2 forceDirection = Vector2.zero;
					Vector3 localPosRoleToGrass = transform.InverseTransformPoint(posOfRole);
					Vector2 localPosXYRoleToGrass = new Vector2(localPosRoleToGrass.x, localPosRoleToGrass.y);
					forceDirection = localPosXYRoleToGrass * -1;
					int force = Mathf.FloorToInt(- 100 * distance / effectiveDistance + 100);
					if (forceDirection != Vector2.zero && force > 0)
					{
						if (body.isNew)
						{
							int id = grassSlope.AddForce(forceDirection, force, 0.5f);
							body.force = id;
							body.isNew = false;
						}
						else if (body.isMoving)
						{
							grassSlope.ChangeForce(body.force, forceDirection, force, 0.5f);
						}
					}
				}
			}
		}
	}

	public void AddEffectiveBody(int role_id)
	{
		if (role_id > 0)
		{
			EffectiveBody body = new EffectiveBody();
			body.id = role_id;
			body.isNew = true;
			cachedEffectiveBodys.Add(body);
		}
	}

	public void MoveEffectiveBody(int role_id)
	{
		EffectiveBody effectiveBody = cachedEffectiveBodys.Find(x => x.id == role_id);
		if (effectiveBody != null)
		{
			effectiveBody.isMoving = true;
		}
	}

	public void IdleEffectiveBody(int role_id)
	{
		EffectiveBody effectiveBody = cachedEffectiveBodys.Find(x => x.id == role_id);
		if (effectiveBody != null)
		{
			effectiveBody.isMoving = false;
		}
	}

	public void RemoveEffectiveBody(int role_id)
	{
		EffectiveBody effectiveBody = cachedEffectiveBodys.Find(x => x.id == role_id);
		if (effectiveBody != null)
		{
			DoRemoveEffectiveBody(effectiveBody);
		}
	}

	private void DoRemoveEffectiveBody(EffectiveBody effective_body)
	{
		grassSlope.MinusForce(effective_body.force);
		cachedEffectiveBodys.Remove(effective_body);
	}

	public void Launch(float effective_distance)
	{
		effectiveDistance = effective_distance;
		if (cachedEffectiveBodys == null)
			cachedEffectiveBodys = new List<EffectiveBody>();
	}

	public void Close()
	{
		Release();
	}

	private void Release()
	{
		grassSlope = null;
		if (cachedEffectiveBodys != null)
		{
			cachedEffectiveBodys.Clear();
		}
		cachedEffectiveBodys = null;
	}

	void OnDestroy()
	{
		Release();
	}
}