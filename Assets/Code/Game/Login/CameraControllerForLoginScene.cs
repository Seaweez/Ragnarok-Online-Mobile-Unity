using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class CameraControllerForLoginScene : MonoBehaviour
{
	private class CameraParams
	{
		public Vector3 pos;
		public Vector3 rotation;
		public float fov;
	}

	public new Camera camera;
	private Transform TransCamera
	{
		get
		{
			return camera.transform;
		}
	}
	private CameraParams paramsForLogin;
	private CameraParams paramsForSelectRole;

	void Awake()
	{
		paramsForLogin = new CameraParams();
		paramsForLogin.pos = TransCamera.position;
		paramsForLogin.rotation = TransCamera.rotation.eulerAngles;
		paramsForLogin.fov = camera.fieldOfView;

		paramsForSelectRole = new CameraParams();
		paramsForSelectRole.pos = new Vector3(0.63f, -0.71f, -8.54f);
		paramsForSelectRole.rotation = new Vector3(1.52f, 0, 0);
		paramsForSelectRole.fov = 35;
	}

	void Start ()
	{
		
	}

	public void GoToLogin()
	{
		TransCamera.position = paramsForLogin.pos;
		Quaternion quaternion = TransCamera.rotation;
		quaternion.eulerAngles = paramsForLogin.rotation;
		TransCamera.rotation = quaternion;
		camera.fieldOfView = paramsForLogin.fov;
	}

	public void GoToSelectRole()
	{
		TransCamera.position = paramsForSelectRole.pos;
		Quaternion quaternion = TransCamera.rotation;
		quaternion.eulerAngles = paramsForSelectRole.rotation;
		TransCamera.rotation = quaternion;
		camera.fieldOfView = paramsForSelectRole.fov;
	}
}
