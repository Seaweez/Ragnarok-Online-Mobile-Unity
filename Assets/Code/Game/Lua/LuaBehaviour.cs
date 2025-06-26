using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLua;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LuaBehaviour : FLEventDispatcherMono
	{
		[SerializeField]
		protected string
			_LuaFile;
		[SerializeField]
		protected string
			_LuaClassName;
		protected LuaWorker _worker;

		public string LuaFile {
			get {
				return _LuaFile;
			}
			set {
				_LuaFile = value;
				if (string.IsNullOrEmpty (_LuaFile) == false) {
					string[] splits = _LuaFile.Split ('.', '/');
					_LuaClassName = splits [splits.Length - 1];
				}
			}
		}

		public string LuaClassName {
			get {
				return _LuaClassName;
			}
		}

		public LuaWorker worker {
			get {
				return _worker;
			}
		}

		protected override void Start ()
		{
			CallLuaMethod ("Start");
		}

		protected override void Awake ()
		{
			if (_worker == null)
				_worker = new LuaWorker (_LuaFile);
			else
				_worker.LuaFile = _LuaFile;
			_worker.DoInit = setBehaviour;
			_worker.DoFile (null);
		}

		protected override void OnDestroy ()
		{
			CallLuaMethod ("OnDestroy");
			_worker.Dispose ();
		}

		public override void Init ()
		{
			base.Init ();
			CallLuaMethod ("Init");
		}

		//设置执行的table对象
		virtual public void setBehaviour (LuaTable myTable)
		{
			_worker.SetEnv ("this", this, false);
			_worker.SetEnv ("transform", transform, false);
			_worker.SetEnv ("gameObject", gameObject, false);
		}

		virtual public object CallLuaMethod (string func, params object[] args)
		{
			return _worker.CallLuaStaticMethod (func, args);
		}
	}
}
