using UnityEngine;
using System.Collections;
using System;

public class PCMHelper{

	static float[] Convert16BitToFloat(byte[] input)
	{
		int inputSamples = input.Length / 2; // 16 bit input, so 2 bytes per sample
		float[] output = new float[inputSamples];
		int outputIndex = 0;
		for(int n = 0; n < inputSamples; n++)
		{
			short sample = BitConverter.ToInt16(input,n*2);
			output[outputIndex++] = sample / 32768f;
		}
		return output;
	}

	public float[] LeftChannel{get; internal set;}
	public int SampleCount {get;internal set;}
	public int Frequency = 16000;

	public PCMHelper(byte[] pcm){

		SampleCount = pcm.Length / 2;

		LeftChannel = Convert16BitToFloat (pcm);
	}

	public override string ToString ()
	{
		return string.Format ("[PCM: LeftChannel={0} , SampleCount={1}, Frequency={2}]", LeftChannel , SampleCount, Frequency);
	}
}
