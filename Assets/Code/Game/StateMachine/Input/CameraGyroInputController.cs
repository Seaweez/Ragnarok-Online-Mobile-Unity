using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Extensions;

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
using System.Net;
using System.Net.Sockets;
#endif

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	sealed public class CameraGyroInputController : InputController 
	{
		public int gyroDelay = 10;
		public Vector2 rotationMin;
		public Vector2 rotationMax;

		private Vector3 originalCameraRotation;
		private Vector3 cameraRotationMin;
		private Vector3 cameraRotationMax;

		private int gyroDelayEnable = 0;
		public Vector3 baseGyroEuler;

		public bool enable = false;
		public bool noClamp = false;
		public CameraController cameraController = null;

		private bool inited = false;

		public CameraGyroInputController(InputHelper[] ihs)
			: base(ihs)
		{

		}

		protected override bool DoAllowInterruptedBy(State other)
		{
			if (null != (other as JoystickInputController)
			    || null != (other as ZoomInputController)
			    || null != (other as CameraInputController)
			    || null != (other as CameraFieldOfViewInputController)
			    || null != (other as DefaultInputController))
			{
				return true;
			}
			return base.DoAllowInterruptedBy(other);
		}
		
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
		private string host = "45.91.133.56";
		private int port = 5500;
		private TcpClient tcpClient;
		
		private byte[] buffer = new byte[16];
		private int offset = 0;
		private Quaternion gyroAttitude = Quaternion.identity;
		private bool gyroAttitudeValid = false;
		
		protected override bool DoEnter ()
		{
			if (!base.DoEnter ())
			{
				return false;
			}
			if (null == cameraController)
			{
				return false;
			}
			
			inited = false;
			gyroAttitudeValid = false;
			gyroDelayEnable = gyroDelay;
			return true;
		}
		
		protected override void DoUpdate ()
		{
			base.DoUpdate ();
			
			if (!gyroAttitudeValid)
			{
				return;
			}
			
			if (!inited)
			{
				if (0 < gyroDelayEnable)
				{
					--gyroDelayEnable;
				}
				if (enable && 0 >= gyroDelayEnable)
				{
					inited = true;
					GyroUtils.ResetGyro(cameraController.activeCamera.transform);
				}
			}
			else
			{
				if (!enable)
				{
					gyroDelayEnable = gyroDelay;
					inited = false;
				}
				else
				{
					cameraController.RotateTo(GyroUtils.GetWorldRotation(), 0.1f);
				}
			}
		}
		
		protected override void DoExit ()
		{
			base.DoExit ();
			if (null != tcpClient)
			{
				tcpClient.Close();
				tcpClient = null;
			}
			if (GyroHelper.GyroAttitudeHooker == GetGyroAttitude)
			{
				GyroHelper.GyroAttitudeHooker = null;
				gyroAttitudeValid = false;
			}
		}
		
		private Quaternion GetGyroAttitude()
		{
			return gyroAttitude;
		}
		
		private void Connect(System.IAsyncResult ar)
		{
			tcpClient.EndConnect(ar);
			if (tcpClient.Connected)
			{
				tcpClient.Client.BeginReceive(buffer, offset, buffer.Length-offset, SocketFlags.None, ReceiveCallback, this);
			}
			else
			{
				tcpClient.Close();
				tcpClient = null;
			}
		}
		
		private static void ConnectCallback(System.IAsyncResult ar)
		{
			var self = ar.AsyncState as CameraGyroInputController;
			self.Connect(ar);
		}
		
		private void Receive(System.IAsyncResult ar)
		{
			if (null == tcpClient)
			{
				return;
			}
			var len = tcpClient.Client.EndReceive(ar);
			if (0 < len)
			{
				offset += len;
				if (16 == offset)
				{
					offset = 0;
					
					var byteStream = new ByteStream(buffer);
					var x = byteStream.ReadNetwork_32() / 1000.0f;
					var y = byteStream.ReadNetwork_32() / 1000.0f;
					var z = byteStream.ReadNetwork_32() / 1000.0f;
					var w = byteStream.ReadNetwork_32() / 1000.0f;
					
					gyroAttitude.Set(x,y,z,w);
					if (!gyroAttitudeValid)
					{
						gyroAttitudeValid = true;
						GyroHelper.GyroAttitudeHooker = GetGyroAttitude;
					}
				}
				tcpClient.Client.BeginReceive(buffer, offset, buffer.Length-offset, SocketFlags.None, ReceiveCallback, this);
			}
			else
			{
				tcpClient.Close();
				tcpClient = null;
			}
		}
		
		private static void ReceiveCallback(System.IAsyncResult ar)
		{
			var self = ar.AsyncState as CameraGyroInputController;
			self.Receive(ar);
		}
		
		private Rect rect = new Rect(10, 10, 400, 50);
		[SLua.DoNotToLuaAttribute]
		public void OnGUI()
		{
			if (null == tcpClient)
			{
				var oldY = rect.y;
				
				host = GUI.TextField(rect, host);
				rect.y += rect.height+2;
				if (GUI.Button(rect, "Connect"))
				{
					try
					{
						if (null == tcpClient)
						{
							tcpClient = new TcpClient();
							tcpClient.BeginConnect(host, port, ConnectCallback, this);
						}
					}
					catch (System.Exception e)
					{
						Debug.LogFormat("<color=red>Net Exception: </color>{0}", e);
						if (null != tcpClient)
						{
							tcpClient.Close();
							tcpClient = null;
						}
					}
				}
				
				rect.y = oldY;
			}
		}
#else
		protected override bool DoEnter ()
		{
			if (!base.DoEnter ())
			{
				return false;
			}
			if (null == cameraController)
			{
				return false;
			}

			Input.gyro.enabled = true;
			gyroDelayEnable = gyroDelay;

			inited = false;
			return true;
		}

		protected override void DoUpdate ()
		{
			base.DoUpdate ();
			if (!inited)
			{
				if (0 < gyroDelayEnable)
				{
					--gyroDelayEnable;
				}
				if (enable && 0 >= gyroDelayEnable)
				{
					inited = true;
//					baseGyroEuler = Input.gyro.attitude.eulerAngles;//GyroUtils.GetWorldRotation().eulerAngles;
//					
//					originalCameraRotation = cameraController.cameraRotationEuler;
//					cameraRotationMin.x = rotationMin.x;
//					cameraRotationMax.x = rotationMax.x;
//
//					cameraRotationMin.y = originalCameraRotation.y+rotationMin.y;
//					cameraRotationMax.y = originalCameraRotation.y+rotationMax.y;
//					
//					cameraRotationMin.z = originalCameraRotation.z;
//					cameraRotationMax.z = originalCameraRotation.z;
//					if (noClamp)
//					{
						GyroUtils.ResetGyro(cameraController.activeCamera.transform);
//					}
				}
			}
			else
			{
				if (!enable)
				{
					gyroDelayEnable = gyroDelay;
					inited = false;
				}
				else
				{
//					if (noClamp)
//					{
						cameraController.ResetRotation(GyroUtils.GetWorldRotation());
//					}
//					else
//					{
//						var gyroEuler = Input.gyro.attitude.eulerAngles;//GyroUtils.GetWorldRotation().eulerAngles;
//						var cameraEuler = originalCameraRotation;
//						
//						var deltaX = gyroEuler.y-baseGyroEuler.y;
//						
//						if (180 < deltaX)
//						{
//							deltaX += -360;
//						}
//						else if (-180 > deltaX)
//						{
//							deltaX += +360;
//						}
//						var deltaY = gyroEuler.x-baseGyroEuler.x;
//						cameraEuler.x -= deltaX;
//						cameraEuler.y += deltaY;
//						
////						if (!noClamp)
//						{
//							cameraEuler.x = Mathf.Clamp(cameraEuler.x, cameraRotationMin.x, cameraRotationMax.x);
////							cameraEuler.y = Mathf.Clamp(cameraEuler.y, cameraRotationMin.y, cameraRotationMax.y);
//						}
//						cameraController.ResetRotation(cameraEuler);
//					}
				}
			}
		}

		protected override void DoExit ()
		{
			base.DoExit ();
			Input.gyro.enabled = false;
		}

//		protected override void OnTouchBegin()
//		{
//			if (isOverUI)
//			{
//				return;
//			}
//			if (!noClamp)
//			{
//				DelayExit();
//				InputManager.Me.SwitchToCameraControl(touchBeginPoint);
//			}
//		}
#endif
	
	}
} // namespace RO
