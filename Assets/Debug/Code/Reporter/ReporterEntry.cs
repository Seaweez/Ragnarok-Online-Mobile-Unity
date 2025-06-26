using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
	public class ReporterEntry : MonoBehaviour
	{
		public GameObject[] Logs;
		bool buttonState = false;

		void Start ()
		{
			if (Logs == null || Logs.Length == 0) {
				return;
			}
			ActiveButton (buttonState);
			AddBtnClickCallAutoHide (0, () => {
				Reporter.Instance.OnClick ();
			});

			AddBtnClickCallAutoHide (1, () => {
				RODebugInfo.Instance.Enable = !RODebugInfo.Instance.Enable;
			});

			AddBtnClickCallAutoHide (2, () => {
				MyLuaSrv.EnablePrint = !MyLuaSrv.EnablePrint;
			});
		}

		void AddBtnClickCallAutoHide (int index, Action call)
		{
			AddBtnClickCall (index, (g) => {
				if (call != null) {
					call ();
				}
				buttonState = false;
				ActiveButton (buttonState);
			});
		}

		void AddBtnClickCall (int index, UIEventListener.VoidDelegate d)
		{
			if (Logs != null && index >= 0 && index < Logs.Length) {
				UIEventListener.Get (Logs [index]).onClick = d;
			}
		}

		void OnClick ()
		{
			buttonState = !buttonState;
			ActiveButton (buttonState);
		}

		void ActiveButton (bool b)
		{
			if (Logs != null) {
				for (int i=0; i<Logs.Length; i++) {
					Logs [i].SetActive (b);
				}
			}
		}
	}
} // namespace RO
