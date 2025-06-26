using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	public class NewInstallPanel : MonoBehaviour
	{
		public GameObject checkContent;
		public GameObject progressContent;
		public GameObject confirmContent;
		public UISlider progressBar;
		public UILabel progressText;
		public UILabel updateTip;
		public UILabel checkTip;
		public UILabel confirmMsg;
		public UILabel confirmBtnLabel;
		public UILabel cancelBtnLabel;
		public UILabel cancelBtnLabel2;
		public UIGrid btnGrid;
		public UIButton confirmBtn;
		public UIButton cancelBtn;
		public UIMultiSprite cancelBtnBg;
		public UITexture texture;
		//error
		public GameObject errorContent;
		public UILabel errorContentLabel;
		public UILabel errorTipLabel;
		public UIButton errorBtn;
		Action onClickConfirm;
		Action onClickCancel;
		Action onErrorHandler;
		string _progressText = ROWords.UNZIPING_PROGRESS;

		void Awake ()
		{
			if (texture != null) {
				texture.MakePixelPerfect ();
			}
			InitConfirm ();
			InitError ();
			ShowUpdate ("", ROWords.CHECKING_REMOTE_VERSION);
			Progress = 0;

		

		}

		// ฟังก์ชันสำหรับตรวจสอบอุปกรณ์ที่ root
		private bool IsDeviceRooted()
		{
			bool isRooted = false;

			try
			{
				string[] dangerousPaths = 
				{
					"/system/app/Superuser.apk",
					"/sbin/su",
					"/system/bin/su",
					"/system/xbin/su",
					"/data/local/xbin/su",
					"/data/local/bin/su",
					"/system/sd/xbin/su",
					"/system/bin/failsafe/su",
					"/data/local/su"
				};

				foreach (string path in dangerousPaths)
				{
					if (System.IO.File.Exists(path))
					{
						isRooted = true;
						break;
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Error checking for root: " + e.Message);
			}

			return isRooted;
		}

		// ฟังก์ชันสำหรับปิดแอปพลิเคชัน
		private void CloseApplication()
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}

		public float Progress {
			set {
				progressBar.value = value;
				progressText.text = string.Format (_progressText, value * 100);
			}
		}

		public void ShowCheck (string tip)
		{
			checkContent.SetActive (true);
			progressContent.SetActive (false);
			checkTip.text = tip;
		}

		public void ShowUpdate (string tip, string progressText)
		{
			checkContent.SetActive (false);
			progressContent.SetActive (true);
			updateTip.text = tip;
			_progressText = progressText;
		}

		void InitConfirm ()
		{
			EventDelegate.Add (confirmBtn.onClick, OnClickConfirm);
			EventDelegate.Add (cancelBtn.onClick, OnCancelConfirm);
			HideConfirm ();
		}

		void InitError ()
		{
			EventDelegate.Add (errorBtn.onClick, OnErrorBtnClick);
			HideError ();
		}

		void OnErrorBtnClick ()
		{
			if (onErrorHandler != null) {
				onErrorHandler ();
			}
			onErrorHandler = null;
			HideError ();
		}

		void OnClickConfirm ()
		{
			if (onClickConfirm != null)
				onClickConfirm ();
			onClickConfirm = null;
			HideConfirm ();
		}

		void OnCancelConfirm ()
		{
			if (onClickCancel != null)
				onClickCancel ();
			onClickCancel = null;
			HideConfirm ();
			
		}

		public void ShowError (string content, string tip, Action errorHandler)
		{
			this.onErrorHandler = errorHandler;
			errorContentLabel.text = content;
			errorTipLabel.text = tip;
			errorContent.SetActive (true);
		}

		public void ShowYesConfirm (string msg, string confirmBtnText, Action confirmHandler)
		{
			ShowConfirm (msg, confirmHandler, null, confirmBtnText, null);
		}

		public void ShowConfirm (string msg, Action confirmHandler, Action cancelHandler, string confirmBtnText = "", string cancelBtnText = "", int cancelBtnBgState = 0)
		{
			cancelBtnBg.CurrentState = cancelBtnBgState;
			if (cancelBtnBgState == 0) {
				cancelBtnLabel2.gameObject.SetActive (false);
				cancelBtnLabel.gameObject.SetActive (true);
			} else {
				cancelBtnLabel2.gameObject.SetActive (true);
				cancelBtnLabel.gameObject.SetActive (false);
			}
			onClickConfirm = confirmHandler;
			onClickCancel = cancelHandler;
			confirmMsg.text = msg;
			confirmBtnLabel.text = confirmBtnText;
			cancelBtnLabel.text = cancelBtnText;
			cancelBtnLabel2.text = cancelBtnText;
			confirmContent.SetActive (true);
			if (confirmHandler == null || string.IsNullOrEmpty (confirmBtnText)) {
				confirmBtn.gameObject.SetActive (false);
			} else {
				confirmBtn.gameObject.SetActive (true);
			}

			if (cancelHandler == null || string.IsNullOrEmpty (cancelBtnText)) {
				cancelBtn.gameObject.SetActive (false);
			} else {
				cancelBtn.gameObject.SetActive (true);
			}
			btnGrid.Reposition ();
		}

		public void HideConfirm ()
		{
			confirmContent.SetActive (false);
		}

		public void HideError ()
		{
			errorContent.SetActive (false);
		}

		void OnDestroy ()
		{
			Dispose ();
		}

		public void Dispose ()
		{
			if (this.texture != null) {
				Resources.UnloadAsset (this.texture.mainTexture);
				this.texture.mainTexture = null;
				this.texture = null;
			}
			Resources.UnloadUnusedAssets ();
		}
	}
}
// namespace RO
