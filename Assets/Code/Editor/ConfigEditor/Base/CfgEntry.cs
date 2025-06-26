using System.Collections.Generic;
using SLua;


namespace EditorTool
{
	public class CfgEntry
	{
		public enum ValueType
		{
			String = 1,
			ArrayTable = 2,
			HashTable = 4,
			MixTable = 6
		}

		public HashSet<string> errors;
		public HashSet<string> warns;
		private static string kvformat = "{0}：{1}";

		private string _Value;
		public string value
		{
			get { return _Value; }
			set
			{
				_Value = value;
				SetShowString();
			}
		}
		public string key;
		public string descKey;
		public string showString;
		/// <summary>
		/// 被观察者
		/// </summary>
		public CfgBase bindCfg;
		/// <summary>
		/// 父级
		/// </summary>
		public CfgBase parent;
		public bool bIsInited { get { return value != null; } }
		public bool isError { get { return this.errors.Count > 0; } }
		public bool isWarning { get { return this.warns.Count > 0; } }
		public int errornum { get { return this.errors.Count; } }
		public EComponentBase.VoidDelegate callback = null;
		bool _DirtyFlag = false;
		public bool dirtyflag
		{
			get { return _DirtyFlag; }
			set { _DirtyFlag = value; }
		}

		public CfgEntry(string k, string desc = null)
		{
			key = k;
			value = null;
			descKey = desc;
			bindCfg = null;
			this.errors = new HashSet<string>();
			this.warns = new HashSet<string>();
		}

		public void Init(LuaTable table, CfgBase p, ValueType t = ValueType.String)
		{
			this.parent = p;
			this.value = RO.LuaWorker.GetFieldString (table, key);
			SetShowString ();
		}

		public void AppendError(string e)
		{
			this.errors.Add(e);
		}

		public void AppendWarning(string w)
		{
			this.warns.Add (w);
		}

		public string GetErrorString()
		{
			if (isError)
			{
				string[] e = new string[this.errors.Count];
				this.errors.CopyTo(e);
				return string.Join("\n", e);
			}
			else
				return string.Empty;
		}

		public string GetWarningString()
		{
			if (isWarning)
			{
				string[] e = new string[this.warns.Count];
				this.warns.CopyTo(e);
				return string.Join("\n", e);
			}
			else
				return string.Empty;
		}

		public void SetShowString()
		{
			if (!string.IsNullOrEmpty(descKey))
				showString = string.Format(kvformat, descKey, value);
			else
				showString = string.Format(kvformat, key, value);
		}

		public void SetCallback(EComponentBase.VoidDelegate cb = null)
		{
			callback = cb;
		}

		/// <summary>
		/// 绑定某条配置，即观察某个被观察者
		/// </summary>
		/// <param name="cfg"></param>
		public void Observe(CfgBase cfg)
		{
			bindCfg = cfg;
			//error.AddRange(cfg.errors);
		}

		public void OnNotify(CfgBindEvent evt, CfgBase subject)
		{
			switch(evt)
			{
			case CfgBindEvent.ObserverDelete:
				break;
			case CfgBindEvent.SubjectDelete:
				break;
			case CfgBindEvent.ErrorsChanged:
				OnErrorChanged(subject);
				break;
			default:
				break;
			}
		}

		void OnErrorChanged(CfgBase subject)
		{
			bool isnew = false;
			foreach (var e in subject.errors)
			{
				if (!this.errors.Contains(e))
				{
					this.errors.Add(e);
					isnew = true;
				}
			}
			if (isnew)
				parent.OnErrorChanged(this);
		}

		void OnWarnsChanged(CfgBase subject)
		{
			bool hasnew = false;
			foreach (var w in subject.warns)
			{
				if (!this.warns.Contains (w))
				{
					this.warns.Add (w);
					hasnew = true;
				}
			}
			if (hasnew)
				parent.OnWarnsChanged(this);
		}
	}
}

