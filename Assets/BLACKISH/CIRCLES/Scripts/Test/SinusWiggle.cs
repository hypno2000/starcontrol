using UnityEngine;
using System.Collections;

public class SinusWiggle: MonoBehaviour {
	
	public bool animate = false;
	public float multiplier = 5f;
	public float timeFactor = 0.5f;
	
	void Update () {
		if(animate) {
			transform.eulerAngles = new Vector3(0f, Mathf.Sin(Time.time * timeFactor) * multiplier, 0f);	
		}
	}
}
