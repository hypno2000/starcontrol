///////////////////////////////////////////////
////         BuildCircleMesh v1.2          ////
////  Copyright (c) 2012 by Markus Hofer   ////
////      BLACKISH - GameAssets.net        ////
///////////////////////////////////////////////

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class BuildCircleMesh : MonoBehaviour {
	
	public int elements = 60; //how many quads should the full circle consist of?
	public float startAngle = 0f;
	private float internalStartAngle = -1f;
	public float endAngle = 360f;
	private float internalEndAngle = -1f;
	public float innerRadius = 50f; //distance from center to inner edge
	private float savedInnerRadius = 50f;
	public float circleWidth = 10f; //distance from inner edge to outer edge
	private float savedCircleWidth = 10f;
	
	public bool addCaps = false; //Adds caps - needs special texture! (See example)
	
	private Vector3[] allVertices = new Vector3[0];
	private Vector2[] allUVs = new Vector2[0];
	private int[] allTriangles = new int[0];
	
	public Vector2 uv1 = new Vector2(0f, 0f); //use your own values in case you want to work from a texture atlas...
	public Vector2 uv2 = new Vector2(0f, 1f);
	
	public Mesh mesh = null;
	public bool createNewMeshInAwake = true;
	
	public bool fullCircle = true; //is the circle closed? value is assigned automatically!
	private bool savedFullCircle = false;
	
	public int count = 0; //how many quads are currently used? (max is set above in elements)
	private int savedCount = -1;
	
	private bool busy = false;
	
	//BONUS SECTION :]
	public float innerSinTime = 0f; //make inner radius pulse
	public float outerSinTime = 0f; //make outer radius pulse
	

	
	void Awake() {
		if(createNewMeshInAwake) { //to make sure duplicating a circle really creates its own mesh (otherwise both circles would try to work with the same sharedMesh!)
			MeshFilter mF = transform.GetComponent<MeshFilter>();
			if(mF) mF.sharedMesh = null;
			mesh = null;
			
			RecalculateMesh();
		}
	}	
	
	void Update () {
		if(startAngle != internalStartAngle || endAngle != internalEndAngle || innerSinTime > 0f || outerSinTime > 0f || innerRadius != savedInnerRadius || circleWidth != savedCircleWidth || mesh == null) {
			RecalculateMesh(uv1, uv2, (mesh == null) ? true : false);
		}
	}
	
	public void ForceRefreshMesh() { StartCoroutine(ForceRefreshMeshNow()); }
	IEnumerator ForceRefreshMeshNow() {
		while(busy) yield return null;
		RecalculateMesh(uv1, uv2, true);
	}
	
	void RecalculateMesh () { RecalculateMesh(uv1, uv2, false); }
	void RecalculateMesh (Vector2 uvOne, Vector2 uvTwo, bool forceRefresh) {
		
		if(busy) return;
				
		busy = true;
		float degreeStep = 360f / elements;
		
		internalStartAngle = startAngle;
		internalEndAngle = endAngle;
		
		if(internalEndAngle > 360f) internalEndAngle = 360f;
		if(internalStartAngle < 0f) internalStartAngle = 0f;

		if(internalStartAngle > 0f || internalEndAngle < 360f) fullCircle = false;
		else fullCircle = true;		
		
		count = 0;
		float deg = 0f;
		for(int c = 0; c < elements; c++) {
			if(deg >= internalStartAngle && deg <= internalEndAngle) count++; 
			deg += degreeStep;
		}
		
		if(count < 2) { //Only continue with at least 2 elements
			renderer.enabled = false; 
			busy = false;
			return; 
		} else renderer.enabled = true;
		
		if(!forceRefresh && count == savedCount && fullCircle == savedFullCircle && innerSinTime == 0f && outerSinTime == 0f && innerRadius == savedInnerRadius && circleWidth == savedCircleWidth) { //CHECK HERE!
			busy = false;
			return; //don't continue if count is same as in previous run! (unless fullCircle changed)
		}
		savedCount = count;
		savedFullCircle = fullCircle;
		savedInnerRadius = innerRadius;
		savedCircleWidth = circleWidth;
		
		if(addCaps && !fullCircle) count += 2; //add 2 elements for caps...
		
		allVertices = new Vector3[count * 2];
		allUVs = new Vector2[count * 2];
		int numTris = count * 6;
		if(!fullCircle) numTris -= 6;
		allTriangles = new int[numTris];
				
		if(addCaps && !fullCircle) count -= 2; //...but forget about them again for now...
		
		if(!gameObject.GetComponent("MeshFilter")) gameObject.AddComponent("MeshFilter");
		if(!gameObject.GetComponent("MeshRenderer")) gameObject.AddComponent("MeshRenderer");
        if(!mesh) mesh = GetComponent<MeshFilter>().sharedMesh;
		if(!mesh) {
			mesh = new Mesh();
			mesh.name = "Circle Mesh for " + gameObject.name;
			GetComponent<MeshFilter>().sharedMesh = mesh;
		}
        mesh.Clear();
		
		Quaternion quat = Quaternion.identity;
		
		deg = 0f;
		while(deg < internalStartAngle)	deg += degreeStep; //start at the right position
		for(int i = 0; i < count * 2; i += 2) {
			quat = Quaternion.AngleAxis(deg, -Vector3.forward);
			
			if(innerSinTime != 0f) allVertices[i] = quat * new Vector3(0f, innerRadius + innerRadius * 0.4f * Mathf.Sin(Time.time * innerSinTime), 0f);
			else allVertices[i] = quat * new Vector3(0f, innerRadius, 0f);
			if(outerSinTime != 0f) allVertices[i + 1] = quat * new Vector3(0f, innerRadius + circleWidth + circleWidth * 0.4f * Mathf.Sin(Time.time * outerSinTime), 0f);
			else allVertices[i + 1] = quat * new Vector3(0f, innerRadius + circleWidth, 0f);
			
			allUVs[i] = uvOne;
			allUVs[i+1] = uvTwo;
			
			//Tris
			int nextDown = i + 2;
			int nextUp = i + 3;
			if(i+2 >= count * 2) { nextUp = 1; nextDown = 0; }			
			if(i+2 >= count * 2 && !fullCircle) break; //if not full circle, ignore last two polygons
			
			allTriangles[(i * 3)] = i; //if i == 4: 12, 13, 14, 15, 16, 17
			allTriangles[(i * 3) + 1] = i + 1;
			allTriangles[(i * 3) + 2] = nextUp;
			allTriangles[(i * 3) + 3] = i;
			allTriangles[(i * 3) + 4] = nextUp;
			allTriangles[(i * 3) + 5] = nextDown;
		
			
			deg += degreeStep;
		}
		
		//CAPS
		if(addCaps && !fullCircle) {
			float capAngleOffset = Mathf.Lerp(2f, 30f, circleWidth/innerRadius);
			
			//StartCap
			quat = Quaternion.AngleAxis(internalStartAngle - capAngleOffset, -Vector3.forward);
			if(innerSinTime != 0f) allVertices[count * 2] = quat * new Vector3(0f, innerRadius + innerRadius * 0.4f * Mathf.Sin(Time.time * innerSinTime), 0f);
			else allVertices[count * 2] = quat * new Vector3(0f, innerRadius, 0f);
			if(outerSinTime != 0f) allVertices[count * 2 + 1] = quat * new Vector3(0f, innerRadius + circleWidth + circleWidth * 0.4f * Mathf.Sin(Time.time * outerSinTime), 0f);
			else allVertices[count * 2 + 1] = quat * new Vector3(0f, innerRadius + circleWidth, 0f);
			
			allUVs[count * 2] = uvOne + new Vector2(1f, 0f);
			allUVs[count * 2 + 1] = uvTwo + new Vector2(1f, 0f);
			
			allTriangles[(count * 2 * 3)] = count * 2; //if i == 4: 12, 13, 14, 15, 16, 17
			allTriangles[(count * 2 * 3) + 1] = count * 2 + 1;
			allTriangles[(count * 2 * 3) + 2] = 0;
			allTriangles[(count * 2 * 3) + 3] = count * 2 + 1;
			allTriangles[(count * 2 * 3) + 4] = 1;
			allTriangles[(count * 2 * 3) + 5] = 0;			
			
			//EndCap
			quat = Quaternion.AngleAxis(internalEndAngle + capAngleOffset, -Vector3.forward);
			if(innerSinTime != 0f) allVertices[count * 2 + 2] = quat * new Vector3(0f, innerRadius + innerRadius * 0.4f * Mathf.Sin(Time.time * innerSinTime), 0f);
			else allVertices[count * 2 + 2] = quat * new Vector3(0f, innerRadius, 0f);
			if(outerSinTime != 0f) allVertices[count * 2 + 3] = quat * new Vector3(0f, innerRadius + circleWidth + circleWidth * 0.4f * Mathf.Sin(Time.time * outerSinTime), 0f);
			else allVertices[count * 2 + 3] = quat * new Vector3(0f, innerRadius + circleWidth, 0f);
			
			allUVs[count * 2 + 2] = uvOne + new Vector2(1f, 0f);
			allUVs[count * 2 + 3] = uvTwo + new Vector2(1f, 0f);
			
			allTriangles[(count * 2 * 3) - 6] = count * 2 - 2; //last normal one down	
			allTriangles[(count * 2 * 3) - 5] = count * 2 - 1; //last normal one up
			allTriangles[(count * 2 * 3) - 4] = count * 2 + 3; //outer
			allTriangles[(count * 2 * 3) - 3] = count * 2 - 2; //last normal one down
			allTriangles[(count * 2 * 3) - 2] = count * 2 + 3; //outer
			allTriangles[(count * 2 * 3) - 1] = count * 2 + 2; //inner
					
		}
		
//		for(int x = 0; x < numTris; x+=2) {
//			Debug.DrawLine(allVertices[allTriangles[x]], allVertices[allTriangles[x+1]], Color.blue);	
//		}
 
        mesh.vertices = allVertices;
        mesh.uv = allUVs;
        mesh.triangles = allTriangles;
		
		//assigning bounds manually = faster!
		mesh.bounds = new Bounds(Vector3.zero, new Vector3(innerRadius + circleWidth, 0.1f, innerRadius + circleWidth) * 100f);

		busy = false;
	
	}

	
}
