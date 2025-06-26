using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xdsdk{
	
	public class XDSDKListener : MonoSingleton<XDSDKListener> {

		void Start () {
			this.name = "XDSDK";
		}

		//callback
		public void OnInitSucceed (){
			XDSDKImp.GetInstance ().GetXDCallback ().OnInitSucceed ();
		}

		public void OnInitFailed (string msg){
			XDSDKImp.GetInstance ().GetXDCallback ().OnInitFailed (msg);
		}

		public void OnLoginSucceed (string token){
			XDSDKImp.GetInstance ().GetXDCallback ().OnLoginSucceed (token);
		}

		public void OnLoginFailed (string msg){
			XDSDKImp.GetInstance ().GetXDCallback ().OnLoginFailed (msg);
		}

		public void OnLoginCanceled (){
			XDSDKImp.GetInstance ().GetXDCallback ().OnLoginCanceled ();
		}

		public void OnGuestBindSucceed (string token){
			XDSDKImp.GetInstance ().GetXDCallback ().OnGuestBindSucceed (token);
		}

		public void OnRealNameSucceed (){
			XDSDKImp.GetInstance().GetXDCallback().OnRealNameSucceed();
		}

		public void OnRealNameFailed(string msg){
			XDSDKImp.GetInstance().GetXDCallback().OnRealNameFailed(msg);
		}
		public void OnLogoutSucceed (){
			XDSDKImp.GetInstance ().GetXDCallback ().OnLogoutSucceed ();
		}

		public void OnPayCompleted (){
			XDSDKImp.GetInstance ().GetXDCallback ().OnPayCompleted ();
		}

		public void OnPayFailed (string msg){
			XDSDKImp.GetInstance ().GetXDCallback ().OnPayFailed (msg);
		}

		public void OnPayCanceled (){
			XDSDKImp.GetInstance ().GetXDCallback ().OnPayCanceled ();
		}

		public void OnExitConfirm (){
			XDSDKImp.GetInstance ().GetXDCallback ().OnExitConfirm ();
		}

		public void OnExitCancel (){
			XDSDKImp.GetInstance ().GetXDCallback ().OnExitCancel ();
		}

		public void OnWXShareSucceed (){
			XDSDKImp.GetInstance ().GetXDCallback ().OnWXShareSucceed ();
		}

		public void OnWXShareFailed (string msg){
			XDSDKImp.GetInstance ().GetXDCallback ().OnWXShareFailed (msg);
		}

	}
}
