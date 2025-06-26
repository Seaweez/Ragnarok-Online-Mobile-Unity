using UnityEngine;

[SLua.CustomLuaClassAttribute]
[AddComponentMenu("Custom/Follow Target")]
public class UIFollowTarget : MonoBehaviour
{
	//更随的目标
	public Transform target {
		get {
			return _target;
		}
		set {
			_target = value;
			CameraInit ();
		}
	}

	public Vector3 offset = Vector3.zero;
	Transform _target;
	//UI使用的摄像机
	public Camera UI_Camera;
	//游戏对象的摄像机
	public Camera Game_Camera;
	public bool alwaysShow = true;
	Transform _cacheCamera;
	//cache Postion
	Vector3 _targetPos;
	Vector3 _cameraPos;
	Vector3 _pos;

	void Awake ()
	{
		CameraInit ();
	}

	void Start ()
	{
		CameraInit (false);
	}

	public void CameraInit (bool needEnable=true)
	{
		if (target != null) {
			if (Game_Camera == null)
				Game_Camera = NGUITools.FindCameraForLayer (target.gameObject.layer);
			if (UI_Camera == null)
				UI_Camera = NGUITools.FindCameraForLayer (gameObject.layer);
			if (Game_Camera != null)
				_cacheCamera = Game_Camera.transform;
		} else {
			enabled = needEnable;
		}
	}

	void LateUpdate ()
	{
		UpdatePos ();
	}

	void UpdatePos ()
	{
		if (_target != null) {
			if (_targetPos != _target.position || (_cacheCamera != null && _cameraPos != _cacheCamera.position)) {
				_pos = Game_Camera.WorldToViewportPoint (_target.position);
				if (alwaysShow)
					_pos.y = Mathf.Clamp (_pos.y, 0, 1);
				transform.position = UI_Camera.ViewportToWorldPoint (_pos);
				_pos = transform.localPosition;
				_pos.x = Mathf.FloorToInt (_pos.x);
				_pos.y = Mathf.FloorToInt (_pos.y);
				_pos.z = 0f;
				transform.localPosition = new Vector3 (_pos.x + offset.x, _pos.y + offset.y, _pos.z + offset.z);
				_targetPos = _target.position;
				_cameraPos = _cacheCamera.position;
			}
		}
	}
}
