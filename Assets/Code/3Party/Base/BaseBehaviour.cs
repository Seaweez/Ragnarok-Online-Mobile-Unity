using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class BaseBehaviour : MonoBehaviour
{
		protected Transform _trans;
	
		public Transform trans {
				get { 
						if (_trans == null)
								_trans = this.transform;
						return _trans;
				}	
		}

		virtual public void Init ()
		{
				
		}
	
		virtual protected void Awake ()
		{
		
		}
	
		public T GetComponentInMy<T> ()where T : Component
		{
				T result = GetComponent<T> ();
				if (result == null) {
						result = GetComponentInChildren<T> ();
				}
				return result;
		}
	
		virtual protected void Start ()
		{			
		}
	
		virtual protected void Update ()
		{
		}
	
		virtual protected void FixedUpdate ()
		{
		}

		virtual protected void OnDestroy ()
		{
		}

		virtual public void Destory ()
		{
				Destroy (this.gameObject);
		}
}
