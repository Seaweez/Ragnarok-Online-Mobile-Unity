using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	[RequireComponent(typeof(UISprite))]
	public class UICursorWithTween: MonoBehaviour
	{
		static UICursorWithTween mInstance;
		// Camera used to draw this cursor
		public Camera uiCamera;
		GameObject mGO;
		Transform mTrans;
		UISprite mSprite;
		UITweener tweener;
		UIAtlas mAtlas;
		string mSpriteName;

		public static UICursorWithTween Instance {
			get {
				return mInstance;
			}
		}
		
		/// <summary>
		/// Keep an instance reference so this class can be easily found.
		/// </summary>
		
		void Awake ()
		{
			mInstance = this;
		}

		void OnDestroy ()
		{
			mInstance = null;
		}
		
		/// <summary>
		/// Cache the expected components and starting values.
		/// </summary>
		
		void Start ()
		{
			mTrans = transform;
			mSprite = GetComponentInChildren<UISprite> ();
			mAtlas = mSprite.atlas;
			mSpriteName = mSprite.spriteName;
			mSprite.depth = 100;
			tweener = mSprite.gameObject.GetComponent<UITweener> ();
			if (uiCamera == null)
				uiCamera = NGUITools.FindCameraForLayer (gameObject.layer);
		}
		
		/// <summary>
		/// Reposition the sprite.
		/// </summary>
		
		void Update ()
		{
			if (mSprite.atlas != null || mGO != null) {
				Vector3 pos = Input.mousePosition;
				
				if (uiCamera != null) {
					// Since the screen can be of different than expected size, we want to convert
					// mouse coordinates to view space, then convert that to world position.
					pos.x = Mathf.Clamp01 (pos.x / Screen.width);
					pos.y = Mathf.Clamp01 (pos.y / Screen.height);
					mTrans.position = uiCamera.ViewportToWorldPoint (pos);
					
					// For pixel-perfect results
					if (uiCamera.orthographic) {
						Vector3 lp = mTrans.localPosition;
						lp.x = Mathf.Round (lp.x);
						lp.y = Mathf.Round (lp.y);
						mTrans.localPosition = lp;
					}
				} else {
					// Simple calculation that assumes that the camera is of fixed size
					pos.x -= Screen.width * 0.5f;
					pos.y -= Screen.height * 0.5f;
					pos.x = Mathf.Round (pos.x);
					pos.y = Mathf.Round (pos.y);
					mTrans.localPosition = pos;
				}
			}
		}

		/// <summary>
		/// Clear the cursor back to its original value.
		/// </summary>
		
		static public void Clear ()
		{
			Set (mInstance.mAtlas, mInstance.mSpriteName);
			mInstance.mSprite.gameObject.SetActive (false);
		}
		
		/// <summary>
		/// Override the cursor with the specified sprite.
		/// </summary>
		
		static public void Set (UIAtlas atlas, string sprite)
		{
			if (mInstance != null) {
				mInstance.mSprite.gameObject.SetActive (true);
				mInstance.mSprite.atlas = atlas;
				mInstance.mSprite.spriteName = sprite;
				mInstance.mSprite.MakePixelPerfect ();
				mInstance.Update ();
				if (string.IsNullOrEmpty (sprite) == false && mInstance.mGO != null) {
					mInstance.mGO.SetActive (false);
				}
				PlayTween ();
			}
		}

		static public void SetGameObject (GameObject go)
		{
			if (mInstance != null) {
				if (mInstance.mGO != go) {
					GameObject.DestroyImmediate (mInstance.mGO);
					mInstance.mGO = go;
					go.transform.parent = mInstance.gameObject.transform;
					go.transform.localPosition = Vector3.zero;
				}
				mInstance.mGO.SetActive (true);
			}
			Set (mInstance.mAtlas, mInstance.mSpriteName);
//			PlayTween ();
		}

		static void PlayTween ()
		{
			UITweener t = mInstance.tweener;
			if (t != null) {
				t.ResetToBeginning ();
				t.PlayForward ();
			}
		}
	}
} // namespace RO
