// HTSpriteSequencers v1.1 (Aout 2012)
// HTSpriteSequencers.cs library is copyright (c) of Hedgehog Team
// Please send feedback or bug reports to the.hedgehog.team@gmail.com

// Release note :
// V1.1
//	- Fixed placement of an effect in a sequence

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// HTspriteSequencer allows the creation of sequence of spritesheet
/// </summary>
[System.Serializable]
public class HTSpriteSequencer : MonoBehaviour {
	
	#region Public members
	/// <summary>
	/// The sequence list. A sequence is a prefab that content an GameObject with HTSpriteSheet.cs script
	/// </summary>
	[SerializeField]
	public List<HTSequence> sequences = new List<HTSequence>();
	/// <summary>
	/// The billboarding mode.
	/// </summary>
	public HTSpriteSheet.CameraFacingMode billboarding = HTSpriteSheet.CameraFacingMode.BillBoard;
	public bool autoDestruct=true;
	public bool editorMode=false;
	#endregion
	
	#region Private members
	/// <summary>
	/// The start time.
	/// </summary>
	private float startTime;
	/// <summary>
	/// The effects list
	/// </summary>
	private List<GameObject> effects = new List<GameObject>();
	/// <summary>
	/// Transform cache.
	/// </summary>
	private Transform myTransform;
	/// <summary>
	/// The main camea transform cache
	/// </summary>
	private Transform mainCamTransform;
	/// <summary>
	/// The in playing count.
	/// </summary>
	private int inPlayingCount=0;
	#endregion
	
	#region Monobehaviors methods
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake(){
		// We search the main camera
		mainCamTransform = Camera.main.transform;
	}
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start(){
		startTime = Time.time;
		myTransform = transform;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
		
		
		// play
		foreach (HTSequence seq in sequences){
			if (Time.time - startTime> seq.waittingTime && !seq.play){
				if (seq.spriteSheet!=null){
					GameObject effet = (GameObject)Instantiate( seq.spriteSheet,myTransform.position,myTransform.rotation);	
					effet.transform.parent = myTransform;
					effet.transform.localPosition= new Vector3(seq.offset.x*-1, seq.offset.y, seq.offset.z);
					effects.Add( effet);
					seq.play=true;
					inPlayingCount++;
					// Inspector copy
					if (Application.isEditor && Application.isPlaying && editorMode){
						HTSpriteSheet ss = seq.spriteSheet.GetComponent<HTSpriteSheet>();
						ss.offset = effet.transform.position-myTransform.position;
						ss.waittingTime = seq.waittingTime;
						ss.copy=true;
					}
				}
			}
		}
		
		
		// Destroy 
		if (autoDestruct){
			int endCount=0;
			if (inPlayingCount==sequences.Count){
				foreach( GameObject effect in effects){
					if (effect==null){
					endCount++;	
					}
				}
				if (endCount==inPlayingCount){
					Destroy(gameObject);	
				}
			}
		}
		
		Camera_BillboardingMode();
		 
	}
	
	void OnDrawGizmos(){
		
		bool childSeq=false;
		
		foreach (HTSequence seq in sequences){
			if (seq.spriteSheet!=null){
				HTSpriteSheet sprite =  seq.spriteSheet.GetComponent<HTSpriteSheet>();
				Gizmos.color= seq.color;
				Gizmos.DrawWireCube (transform.position + seq.offset, new Vector3 (sprite.sizeEnd.x/2,sprite.sizeEnd.y/2,0) );
				childSeq=true;
			}
		}
		
		if(!childSeq){
			Gizmos.DrawWireCube (transform.position, new Vector3 (1,1,0) );
		}
		
	}
	
	#endregion
	
	#region private methods
	/// <summary>
	/// Camera_s the billboarding mode.
	/// </summary>
	void Camera_BillboardingMode(){
		
		
		Vector3 lookAtVector =   myTransform.position-mainCamTransform.position ;
         
		switch (billboarding){
			case HTSpriteSheet.CameraFacingMode.BillBoard:
				myTransform.LookAt( mainCamTransform.position - lookAtVector); 
				break;
			case HTSpriteSheet.CameraFacingMode.Horizontal:
				lookAtVector.x = lookAtVector.z =0 ;
				myTransform.LookAt(mainCamTransform.position - lookAtVector);
				break;
			case HTSpriteSheet.CameraFacingMode.Vertical:
				lookAtVector.y=lookAtVector.z =0;
				myTransform.LookAt(mainCamTransform.position - lookAtVector);
				break;
		}	
		
		
	}
	#endregion
	
	#region public methods
	/// <summary>
	/// Kills all sequences.
	/// </summary>
	public void KillAllSequences(){
		
		for(int i=0;i<effects.Count;i++){
			Destroy(effects[i]);
		}
		effects.Clear();
		startTime = Time.time;
		foreach (HTSequence seq in sequences){
			seq.play=false;
		}
		
	}
	#endregion
	
}
