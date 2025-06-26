using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	[System.Serializable]
	public class LogoPanelTweenParam
	{
		public float fadeInDuration;
		public float stayDuration;
		public float fadeOutDuration;
		public UIWidget logoContainer;
	}

	public class LogoPanel : MonoBehaviour
	{
		public UIWidget Bg;
		public List<LogoPanelTweenParam> configs;
		public float BgFadeOutDuration;
		public UITexture[] disposePics;
		bool _isPlaying;
		Action _allCompleteCall;
		Action _allCompleteCall1;
		LogoPanelTweenParam _currentConfig;
		UITweener _currentTweening;
		EventDelegate _currentDele;
		int useCallMode;
		//屏幕 左上为1，右上为2，右下为3，左下为4
		List<int> _mode1Secret = new List<int> (){ 1, 1, 2, 2, 3, 4 };
		int _currentSecretIndex;
		Vector3 _touchPoint;

		void Awake ()
		{
			useCallMode = 0;
			_currentSecretIndex = 0;
			if (configs != null) {
				for (int i = 0; i < configs.Count; i++) {
					configs [i].logoContainer.alpha = 0;
				}
			}

			UIEventListener.Get (Bg.gameObject).onClick = ClickBg;
		}

		public void SetAllLogoPlayCompleteCall (Action call)
		{
			_allCompleteCall = call;
		}

		public void SetAllLogoPlayCompleteCall1 (Action call)
		{
			_allCompleteCall1 = call;
		}

		public void StartPlayLogo ()
		{
			if (_isPlaying) {
				return;
			}
			_isPlaying = true;
			_StartPlay ();
		}

		protected void _StartPlay ()
		{
			if (configs != null && configs.Count > 0) {
				_currentConfig = configs [0];
				configs.RemoveAt (0);
				FadeIn (_currentConfig.logoContainer, _currentConfig.fadeInDuration);
			} else {
				FadeOutBg ();
			}
		}

		protected void FadeIn (UIWidget widget, float duration)
		{
			widget.alpha = 0;
			TweenAlpha ta = TweenAlpha.Begin (widget.gameObject, duration, 1);
			_currentTweening = ta;
			ta.ignoreTimeScale = false;
			ta.ResetToBeginning ();
			ta.from = 0;
			ta.to = 1;
			SetTweenFinishCall (ta, FadeInFinish);
		}

		protected void FadeInFinish ()
		{
			if (_currentConfig != null) {
				FadeOut (_currentConfig.logoContainer, _currentConfig.stayDuration, _currentConfig.fadeOutDuration);
			} else {
				AllFinish ();
			}
		}

		protected void FadeOut (UIWidget widget, float stayDuration, float duration)
		{
			widget.alpha = 1;
			TweenAlpha ta = TweenAlpha.Begin (widget.gameObject, duration, 0);
			_currentTweening = ta;
			ta.ignoreTimeScale = false;
			ta.delay = stayDuration;
			ta.from = 1;
			ta.to = 0;
			SetTweenFinishCall (ta, FadeOutFinish);
		}

		protected void FadeOutFinish ()
		{
			ClearTweenerEvents ();
			_StartPlay ();
		}

		protected void FadeOutBg ()
		{
			Bg.alpha = 1;
			TweenAlpha ta = TweenAlpha.Begin (Bg.gameObject, BgFadeOutDuration, 0);
			_currentTweening = ta;
			ta.ignoreTimeScale = false;
			ta.from = 1;
			ta.to = 0;
			SetTweenFinishCall (ta, AllFinish);
		}

		protected void AllFinish ()
		{
			ClearTweenerEvents ();
			_isPlaying = false;
			Hide ();
			FireAllFinishCall ();
		}

		protected void FireAllFinishCall ()
		{
			Action complete = useCallMode == 0 ? _allCompleteCall : _allCompleteCall1;
			if (complete != null) {
				Action call = complete;
				_allCompleteCall = null;
				_allCompleteCall1 = null;
				call ();
			}
		}

		protected void SetTweenFinishCall (UITweener tween, EventDelegate.Callback call)
		{
			ClearTweenerEvents ();
			if (tween != null) {
				_currentTweening = tween;
				_currentDele = EventDelegate.Set (_currentTweening.onFinished, call);
			}
		}

		protected void ClearTweenerEvents ()
		{
			if (_currentTweening != null && _currentDele != null) {
				_currentTweening.RemoveOnFinished (_currentDele);
			}
			_currentTweening = null;
			_currentDele = null;
		}

		public void Hide ()
		{
			this.gameObject.SetActive (false);
		}

		void OnDestroy ()
		{
			Dispose ();
		}

		public void Dispose ()
		{
			if (this.disposePics != null) {
				for (int i = this.disposePics.Length - 1; i > 0; i--) {
					Resources.UnloadAsset (this.disposePics [i].mainTexture);
					this.disposePics [i].mainTexture = null;
					this.disposePics [i] = null;
				}
				this.disposePics = null;
			}
			Resources.UnloadUnusedAssets ();
		}

		public void ClickBg(GameObject go)
		{
			if (_isPlaying) {
				if (_currentConfig != null)
					_currentConfig.logoContainer.alpha = 0;
				
				if (_currentTweening != null)
					_currentTweening.enabled = false;
				
				FadeOutFinish ();
			}			
		}

		void Update ()
		{
			bool clicked = false;
			if (Input.touchSupported) {
				if (0 < Input.touchCount) {
					var touch = Input.GetTouch (0);
					if (TouchPhase.Ended == touch.phase) {
						_touchPoint = touch.position;
						clicked = true;
					}
				}
			} else if (Input.GetMouseButtonUp (0)) {
				_touchPoint = Input.mousePosition;
				clicked = true;
			}
			if (clicked && _currentSecretIndex < _mode1Secret.Count) {
				int currentSecretShould = _mode1Secret [_currentSecretIndex];
				int width = Screen.width;
				int height = Screen.height;
				int secret = -1;
				if (_touchPoint.x >= width / 2) {
					if (_touchPoint.y >= height / 2) {
						secret = 2;
					} else {
						secret = 3;
					}
				} else {
					if (_touchPoint.y >= height / 2) {
						secret = 1;
					} else {
						secret = 4;
					}
				}
				if (secret == currentSecretShould) {
					_currentSecretIndex++;
					if (_currentSecretIndex >= _mode1Secret.Count) {
						useCallMode = 1;
					}
				} else {
					_currentSecretIndex = 0;
				}
			}
		}
	}
}
// namespace RO
