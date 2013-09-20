using UnityEngine;
using System.Collections;

public class TestFilled : MonoBehaviour {

	public BuildFilledCircleMesh circle;
	
	public bool animateElements = false;
	private float rememberRadius = 50f;
	private int rememberElements = 60;
	
	void Start () {
		rememberRadius = circle.radius;
		rememberElements = circle.elements;
	}

	void Update () {
		if(animateElements) {
			circle.elements = (int) (rememberElements * Mathf.Abs(Mathf.Sin(Time.time * 0.5f)));
			if(circle.elements < 3) circle.elements = 3;
		} else circle.radius = rememberRadius * (Mathf.Abs(Mathf.Sin(Time.time * 0.5f))); 
		
	}
}
