using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Ghost.Attribute;
namespace RO{

	[SLua.CustomLuaClassAttribute]
	public class GuideTagCollection : MonoBehaviour
	{
		public static List<GuideTagCollection> arrays = new List<GuideTagCollection>();

		[SerializeField, SetProperty("id")]
		private int buttonId = -1;

		public int id 
		{ 
			set
			{
//				print(value);
				if(this.buttonId ==-1 && value !=-1)
				{
					if(getGuideItemByObj(this) == null && isActiveAndEnabled)
						Add();
				}

				if(value ==-1 && this.buttonId != -1)
					Remove();

				buttonId = value;
			}
			get
			{

				return this.buttonId;
			}
		}

		// Use this for initialization
		void Start ()
		{
			if(getGuideItemByObj(this) == null && this.id != -1)
				Add();
			else
				RO.LoggerUnused.Log("error has same guide id or id ==-1!!!!!!!!!!!");
		}

		public static GuideTagCollection getGuideItemById(int id)
		{
			foreach(var single in arrays)
			{
				if(single.id == id)
					return single;
			}
			return null;
		}

		public static GuideTagCollection getGuideItemByObj(GuideTagCollection obj)
		{
			foreach(var single in arrays)
			{
				if(single == obj)
					return single;
			}
			return null;
		}

		private void Add()
		{
			arrays.Add(this);
		}

		private void Remove()
		{
			arrays.Remove(this);
		}

		void OnDisable()
		{
			Remove();
		}

		void OnEnable()
		{
			if(this.buttonId !=-1)
			{
				if(getGuideItemByObj(this) == null)
					Add();
			}
		}
		
		// Update is called once per frame
//		void Update ()
//		{
//			arrays.Remove(null);	
//		}
		void OnDestroy()
		{
			Remove();
		}
	}
}
