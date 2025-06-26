using UnityEngine;
using System.Collections;

namespace xdsdk
{
	// XDCallback
	public abstract class XDCallback
    {
        //callback
		public abstract void OnInitSucceed ();

		public abstract void OnInitFailed (string msg);

		public abstract void OnLoginSucceed (string token);

		public abstract void OnLoginFailed (string msg);

		public abstract void OnLoginCanceled ();

		public abstract void OnGuestBindSucceed (string token);

		public abstract void OnRealNameSucceed ();

		public abstract void OnRealNameFailed(string msg);

		public abstract void OnLogoutSucceed ();

		public abstract void OnPayCompleted ();

		public abstract void OnPayFailed (string msg);

		public abstract void OnPayCanceled ();

		public abstract void OnExitConfirm ();

		public abstract void OnExitCancel ();

		public virtual void OnWXShareSucceed (){}

		public virtual void OnWXShareFailed (string msg){}

    }
}
