using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class UIMultiSprite : UISprite
	{
		[System.Serializable]
		public class AtlasMapSprite
		{
			public UIAtlas atlas;
			public string spriteName;
		}
		protected int mCurrentState = -1;
		[HideInInspector]
		[SerializeField]
		List<AtlasMapSprite>
			_stateSpriteList = new List<AtlasMapSprite> ();

		public List<AtlasMapSprite> StateSpriteList {
			get {
				return _stateSpriteList;
			}
		}

		public bool isChangeSnap = true;

		public int CurrentState {
			get{ return mCurrentState;}
			set { 
				mCurrentState = value;
				mCurrentState = Mathf.Clamp (mCurrentState, -1, _stateSpriteList.Count - 1);
				if (mCurrentState >= 0 && mCurrentState < _stateSpriteList.Count) {
					AtlasMapSprite ams = _stateSpriteList [mCurrentState];
					atlas = ams.atlas;
					spriteName = ams.spriteName;
					if (isChangeSnap)
						MakePixelPerfect ();
				}
			}
		}

		public void AddState (int insertIndex, UIAtlas customAtlas, string spriteName)
		{
			customAtlas = customAtlas != null ? customAtlas : this.atlas;
			AtlasMapSprite ams = new AtlasMapSprite ();
			ams.atlas = customAtlas;
			ams.spriteName = spriteName;
			if (insertIndex == -1 || insertIndex >= _stateSpriteList.Count - 1)
				_stateSpriteList.Add (ams);
			else
				_stateSpriteList.Insert (insertIndex, ams);
		}

		public void RemoveState (int index)
		{
			if (index > 0 && index < _stateSpriteList.Count) {
				if (mCurrentState == index)
					CurrentState = index;
				_stateSpriteList.RemoveAt (index);
			}
		}
	}


} // namespace RO
