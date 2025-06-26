using UnityEngine;
using System.Collections;
using System;

namespace RO
{
	public class Speech{
		public enum State {
			UnFinish,
			Finish,	
		};

		public static string name = "asr.pcm";
		public State state = State.UnFinish;
		public byte[] bytes;
		public PCMHelper pcm;
		public float time;
		public string result;

		public Speech(string result,Action<byte[],float,string> handler)
		{
			this.result = result;
			this.handler = handler;  		
		}

		public void exeHandler()
		{
			if(handler!=null)
			{
				AudioClip audioClip = AudioClip.Create( "ChatSpeech", pcm.SampleCount, 1, pcm.Frequency, false);
			
				audioClip.SetData(pcm.LeftChannel, 0);

				time = Mathf.Ceil(audioClip.length);
				
				handler(bytes,time,result);
			}
		}

		public void speechByGet()
		{
			bytes = FileDirectoryHandler.LoadFile (name);
			if (bytes != null && bytes.Length > 0) {
				pcm = new PCMHelper(bytes);
				state = State.Finish;
				RO.LoggerUnused.Log("Speech State.Finish ~ bytes.Length :" + bytes.Length);
			} else {
				state = State.UnFinish;
			}
		}

		private Action<byte[],float,string> handler{
			set;
			get;
		}
	}
} // namespace RO