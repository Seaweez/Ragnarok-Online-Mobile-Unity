using UnityEngine;
using System.Collections;
public class ModelCheckGizmos : MonoBehaviour {
	
	float width=1;
	float height=1;
	float zaxis=0;
	public void SetGizmos(float x,float y,float z)
	{
		width = x;
		height= y;
		zaxis = z;
	}
	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 0.5f, 1, 1);
		Gizmos.DrawWireCube(transform.position, new Vector3(width,height, zaxis));
	}
}
