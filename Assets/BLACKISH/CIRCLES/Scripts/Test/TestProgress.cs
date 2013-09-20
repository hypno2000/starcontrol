using UnityEngine;
using System.Collections;

public class TestProgress : MonoBehaviour {
	
	public BuildCircleMesh circle;
	
	private float prog = 0f;
	public bool startAngleInsteadOfEndAngle = false;
	public bool bothAngles = false;
	
	void Update () {
		
		prog += Time.deltaTime * 30f;
		if(prog > 380f) prog = 0f;
		
		if(startAngleInsteadOfEndAngle) circle.startAngle = prog;
		else if(bothAngles) {
			float tProg = prog;
			if(tProg > 180f) tProg = 360f - prog;
			circle.startAngle = tProg;
			circle.endAngle = 360f - tProg;
		} else circle.endAngle = prog;
	}
}
