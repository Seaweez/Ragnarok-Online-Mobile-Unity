using UnityEngine;
using System.Collections;

public static class AlgorithmForSwing
{
	public static float SwingValueBaseTime(float time, float cycleTime)
	{
		while (time > cycleTime)
		{
			time -= cycleTime;
		}
		float angle = time * Mathf.PI * 2 / cycleTime;
		float value = Mathf.Sin(angle);
		return value;
	}

	public static float SwingVelocityBaseTimeAndDeceleration(float v0, float a, float time)
	{
		return v0 + a * time;
	}
}
