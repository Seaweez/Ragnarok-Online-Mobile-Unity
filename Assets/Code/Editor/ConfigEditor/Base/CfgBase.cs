
using SLua;
using System.Collections.Generic;

namespace EditorTool
{
	/// <summary>
	/// 配置观察事件
	/// </summary>
	public enum CfgBindEvent
	{
		/// <summary>
		/// 删除观察者
		/// </summary>
		ObserverDelete,
		/// <summary>
		/// 删除被观察者
		/// </summary>
		SubjectDelete,
		/// <summary>
		/// 错误信息变更
		/// </summary>
		ErrorsChanged,
		/// <summary>
		/// 警告信息变更
		/// </summary>
		WarnsChanged
	}

	public abstract class CfgBase
    {
		const string _UnusedWarning = "没有使用过的配置！";
		const string _SpriteMissErrorFormat = "图集:{0}中不包含图片:{1}";
		const string _AltasMissErrorFormat = "图集:{0}不存在!";
        private List<CfgEntry> _Observers = new List<CfgEntry>();
        public static string[] nameArr = { "NameZh", "NameEn" };

		public string id = "0";
        public string name = string.Empty;
		public static string fileMissErrorFormat = "\"{0}\"文件不存在！";
		public abstract string errorFormat { get; }
		public abstract string warnsFormat { get; }
		public abstract string[] searchFieldArr{ get; }
		public abstract Dictionary<string, CfgEntry> map{ get; set; }
		public abstract List<CfgEntry> data{ get; set; }
		public HashSet<string> warns = new HashSet<string>();
        public HashSet<string> errors = new HashSet<string>();
		public HashSet<string> checkingErrors = new HashSet<string>();
		public HashSet<string> checkingWarns = new HashSet<string>();
		public int errornum { get { return errors.Count; } }
		public int warnnum { get { return warns.Count; } }
        /// <summary>
        /// table key，在LuaTable中的索引
        /// </summary>
        public int key;

        public CfgBase()
        {
		}

		public virtual void Init(int k, LuaTable table)
		{
			key = k;
			for (int i = 0; i < data.Count; i++)
			{
				data [i].Init (table, this);
				map.Add (data [i].key, data [i]);
			}
			id = GetField (table, "id");
            SetName();
        }

        void SetName()
        {
            for(int i = 0; i < nameArr.Length; i++)
            {
                CfgEntry entry = FindEntry(nameArr[i]);
                if (entry != null && !string.IsNullOrEmpty(entry.value))
                {
                    name = entry.value;
                    return;
                }
            }
        }

        public string GetField(LuaTable t, string key)
        {
            string ret = RO.LuaWorker.GetFieldString(t, key);
            return ret;
        }

        public virtual void InitBind()
        {

        }

        public virtual void Check()
        {

        }

        public virtual void ReCheck()
        {

        }

        public virtual void DeepCheck() { }

        /// <summary>
        /// isRecheck:是否为重新检查, false为DeepCheck
        /// </summary>
        /// <param name="isNewCheck"></param>
        public void TryNotifyChanged(bool isRecheck = true)
        {
            bool errorChange = FindErrorsChanged(isRecheck);
			bool warnChange = FindWarnsChanged(isRecheck);
            CombineErrorHashset(isRecheck);
			CombineWarnHashset (isRecheck);
			if (isRecheck) 
			{
				if(errorChange)
					NotifyAllObservers (CfgBindEvent.ErrorsChanged);
				if(warnChange)
					NotifyAllObservers (CfgBindEvent.WarnsChanged);
			}
        }

        private bool FindErrorsChanged(bool isRecheck)
        {
            if (isRecheck)
            {
                foreach (var item in errors)
                {
                    //error reduce
                    if (!checkingErrors.Contains(item))
                       return true;;
                }
            }

            foreach (var item in checkingErrors)
            {
                //new error
                if (!errors.Contains(item))
                    return true;
            }
            return false;
		}

		private bool FindWarnsChanged(bool isRecheck)
		{
			if (isRecheck)
			{
				foreach (var item in warns)
				{
					//error reduce
					if (!checkingWarns.Contains(item))
						return true;;
				}
			}

			foreach (var item in checkingWarns)
			{
				//new error
				if (!warns.Contains(item))
					return true;
			}
			return false;
		}

		public void AddError(string error, CfgEntry entry)
		{
			error = string.Format(errorFormat, id, error);
			checkingErrors.Add(error);
			entry.AppendError(error);
		}

		public void AddWarning(string warn, CfgEntry entry)
		{
			warn = string.Format(warnsFormat, id, warn);
			checkingWarns.Add (warn);
			entry.AppendWarning (warn);
		}

		public CfgEntry FindEntry(string key)
		{
			if (map.ContainsKey(key))
				return map [key];
			return null;
		}

		public bool Search(string searchStr)
		{
			bool isContains = false;
			for (int i = 0; i < searchFieldArr.Length; i++)
			{
				CfgEntry entry = FindEntry(searchFieldArr[i].ToString());
				if (!string.IsNullOrEmpty(entry.value) && entry.value.Contains(searchStr))
				{
					isContains = true;
					break;
				}
			}
			return isContains;
		}

        public void CombineErrorHashset(bool isRecheck)
        {
            if (isRecheck)
                errors.Clear();
            foreach (var e in checkingErrors)
            {
                errors.Add(e);
            }
		}

		public void CombineWarnHashset(bool isRecheck)
		{
			if (isRecheck)
				warns.Clear();
			foreach (var w in checkingWarns)
			{
				warns.Add(w);
			}
		}

		#region

		public void CheckUnusedCfg()
		{
			if (_Observers.Count < 1)
				AddWarning (_UnusedWarning, FindEntry ("id"));
		}

		#endregion

		#region 绑定关系的观察模式相关代码
        public void AddObserver(CfgEntry entry)
        {
            if (!_Observers.Contains(entry))
            {
                _Observers.Add(entry);
                entry.Observe(this);
            }
            else
                UnityEngine.Debug.LogError("重复添加Observer, entry.key = " + entry.key + " entry.value = " + entry.value);
        }

        public void RemoveObserver(CfgEntry entry)
        {
            if(_Observers.Contains(entry))
            {
                entry.OnNotify(CfgBindEvent.ObserverDelete, this);
                _Observers.Remove(entry);
            }
        }

        public void NotifyAllObservers(CfgBindEvent evt)
        {
            for(int i = 0; i < _Observers.Count; i++)
            {
                _Observers[i].OnNotify(evt, this);
            }
        }

        public void OnErrorChanged(CfgEntry entry)
        {
            bool isnew = false;
            foreach (var e in entry.errors)
            {
                if (!errors.Contains(e))
                {
                    errors.Add(e);
                    isnew = true;
                }
            }
            if (isnew)
                NotifyAllObservers(CfgBindEvent.ErrorsChanged);
        }

		public void OnWarnsChanged(CfgEntry entry)
		{
			bool hasnew = false;
			foreach (var w in entry.errors)
			{
				if (!warns.Contains (w)) 
				{
					warns.Add (w);
					hasnew = true;
				}
			}
			if (hasnew)
				NotifyAllObservers (CfgBindEvent.WarnsChanged);
		}

        /// <summary>
        /// 删除被观察者，即自身
        /// </summary>
        public void DeleteSubject()
        {
            for(int i = 0; i < _Observers.Count; i++)
            {
                _Observers[i].OnNotify(CfgBindEvent.SubjectDelete, this);
            }
            _Observers.Clear();
        }
		#endregion
    }
}
