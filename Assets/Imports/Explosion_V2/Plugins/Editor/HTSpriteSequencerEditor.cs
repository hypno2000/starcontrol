// HTSpriteSequencersEditor v1.0 (Aout 2012)
// HTSpriteSequencersEditor.cs library is copyright (c) of Hedgehog Team
// Please send feedback or bug reports to the.hedgehog.team@gmail.com
using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// HTsprite sequencer editor.
/// </summary>
[CustomEditor(typeof(HTSpriteSequencer))]
public class HTSpriteSequencerEditor : Editor {
	
	
	public override void OnInspectorGUI(){
		
		HTSpriteSequencer t;
		GUIStyle boldStyle = new GUIStyle();
		GUIStyle foldOutStyle =  EditorStyles.foldout;
		GUIStyle paddingStyle = new GUIStyle();
		GUIStyle paddingStyle2 = new GUIStyle();
		
		foldOutStyle.fontStyle = FontStyle.Bold;
		boldStyle.fontStyle = FontStyle.Bold;
		paddingStyle.padding = new RectOffset(30,0,0,0);
		paddingStyle2.padding = new RectOffset(60,0,0,0);
			
		
		string assetPath="";
		
		t = (HTSpriteSequencer)target;
		
		#region Sequencer
		// Sequencer
		EditorGUILayout.Space();
		t.editorMode = EditorGUILayout.Toggle("Editor mode",t.editorMode);
		t.name = EditorGUILayout.TextField("Name",t.name);
		t.billboarding = (HTSpriteSheet.CameraFacingMode)EditorGUILayout.EnumPopup("Camera facing",t.billboarding);
		// playmode disable the autodestruc
		if (Application.isPlaying && Application.isEditor && t.editorMode){
			t.autoDestruct=false;
		}
		else{
			t.autoDestruct = EditorGUILayout.Toggle("Auto destruct",t.autoDestruct);
		}
		EditorGUILayout.Space();
		
		// button
		Rect rec = EditorGUILayout.BeginHorizontal();
		rec.x=5;
		rec.width =150;
		rec.height =20;
		if (!Application.isPlaying){
			if (GUI.Button( rec,"Add spritesheet prefab")){
				HTSequence seq = new HTSequence();
				seq.color = new Color( Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
				t.sequences.Add( seq);		
			}
		}
		else{
			if (GUI.Button( rec,"Replay")){
				t.KillAllSequences();	
			}
		}

		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Spritesheet prefab list",MessageType.None);
		EditorGUILayout.Space();
		#endregion
		
		#region Sequences
		// Sequences
		int count=1;
		//foreach (HTSequence seq in t.sequences){
		for(int i=0;i<t.sequences.Count;i++){
			HTSequence seq = t.sequences[i];

			//folding name
			EditorGUILayout.BeginVertical();
			string name="Spritesheet ?";
			if (seq.spriteSheet!=null){
				name = seq.spriteSheet.name;	
			}

			seq.foldOut = EditorGUILayout.Foldout(seq.foldOut, name);
			EditorGUILayout.Space();
			
			// properties
			if (seq.foldOut){
				EditorGUILayout.BeginVertical(paddingStyle);
				// spritesheet prefab
				seq.spriteSheetRef = (GameObject)EditorGUILayout.ObjectField( "Spritesheet Prefab ", seq.spriteSheetRef,typeof(GameObject),false);
				
				// Gizmos color
				seq.color = EditorGUILayout.ColorField( "Gizmos color",seq.color);
				
				// prefab creation process
				if (seq.spriteSheetRef != seq.oldSpriteSheetRef && seq.spriteSheetRef != null){
					
					// Dynamic gameObject creation
					GameObject tmp = (GameObject)Instantiate(seq.spriteSheetRef,new Vector3(0,0,0),Quaternion.identity);
					tmp.name= t.name + "_s"+i+"_"+seq.spriteSheetRef.name;
					
					// deleting the current prefab
					if (seq.spriteSheet!=null){
						string tmpPath = AssetDatabase.GetAssetPath(seq.spriteSheet.GetInstanceID());
						AssetDatabase.DeleteAsset(tmpPath);
						assetPath = GetAssetRootPath(tmpPath);
					}
					else{
						assetPath = "Assets/UserSpriteSheetEffects/"+t.name+"/";
					}
					
					// create directory
					CreateAssetDirectory( t.name);
					
					// Dynamic prefab creation
					seq.spriteSheet = PrefabUtility.CreatePrefab(assetPath + tmp.name+".prefab",tmp,ReplacePrefabOptions.ConnectToPrefab);
					seq.spriteSheet.GetComponent<HTSpriteSheet>().billboarding = HTSpriteSheet.CameraFacingMode.Never;	
					DestroyImmediate(tmp);
					
					// for test if you must create a new prefab
					seq.spriteSheetRef = seq.spriteSheet;
					seq.oldSpriteSheetRef = seq.spriteSheetRef;
				}
				
				#region SpriteSheet properties
				// spritesheet propertie
				if (seq.spriteSheet!=null){
					
					HTSpriteSheet spriteSheet= seq.spriteSheet.GetComponent<HTSpriteSheet>();
					
					if (!Application.isPlaying && spriteSheet.copy && t.editorMode){
						seq.offset = spriteSheet.offset;
						seq.waittingTime = spriteSheet.waittingTime;
						spriteSheet.copy=false;
					}
					spriteSheet.foldOut = EditorGUILayout.Foldout(spriteSheet.foldOut,"Spritesheet properties");
					
					if (spriteSheet.foldOut ){
						EditorGUILayout.BeginVertical(paddingStyle);
						// Frame per second
						spriteSheet.framesPerSecond = EditorGUILayout.IntField("Frames per second",spriteSheet.framesPerSecond);
						// One shot
						spriteSheet.isOneShot = EditorGUILayout.Toggle( "One shot",spriteSheet.isOneShot);
						if (!spriteSheet.isOneShot){
							EditorGUILayout.HelpBox("0 = infite loop",MessageType.Info);
							// life
							spriteSheet.life = EditorGUILayout.FloatField( "life",spriteSheet.life);	
						}	
						
						EditorGUILayout.Space();
						EditorGUILayout.Space();
						
						GUILayout.Label("Sprite properties",boldStyle);
						//size
						spriteSheet.sizeStart = EditorGUILayout.Vector2Field("Start size",spriteSheet.sizeStart);
						spriteSheet.sizeEnd = EditorGUILayout.Vector2Field("End size",spriteSheet.sizeEnd);
						EditorGUILayout.Space();
						// Rotation
						spriteSheet.randomRotation = EditorGUILayout.Toggle( "Random rotation",spriteSheet.randomRotation);
						if (!spriteSheet.randomRotation){
							spriteSheet.rotationStart = EditorGUILayout.FloatField( "Start rotation",spriteSheet.rotationStart);
						}
						spriteSheet.rotationEnd = EditorGUILayout.FloatField( "End rotation",spriteSheet.rotationEnd);
						
						EditorGUILayout.Space();
						EditorGUILayout.Space();
						
						GUILayout.Label("Color properties",boldStyle);
						spriteSheet.addColorEffect= EditorGUILayout.Toggle( "Add color effect",spriteSheet.addColorEffect);
						if (spriteSheet.addColorEffect){
							spriteSheet.colorStart = EditorGUILayout.ColorField( "Start color",spriteSheet.colorStart);
							spriteSheet.colorEnd = EditorGUILayout.ColorField( "Start color",spriteSheet.colorEnd);
						}
						EditorGUILayout.EndVertical();
					}
				}
				#endregion
				
				#region Sequence properties
				// sequence properties
				EditorGUILayout.Space();
				seq.offset = EditorGUILayout.Vector3Field("Offset",seq.offset);
				seq.waittingTime = EditorGUILayout.FloatField("Wait before start",seq.waittingTime);
				EditorGUILayout.EndVertical();
				
				// remove button
				rec = EditorGUILayout.BeginHorizontal();
				rec.x=30;
				rec.width =60;
				rec.height=20;
				if (GUI.Button( rec,"Remove")){
					if (seq.spriteSheet!=null){
						   AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(seq.spriteSheet.GetInstanceID()));
					}
					t.sequences.Remove(seq);		
				}
				#endregion
				
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();
				EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();
				
				EditorGUILayout.EndVertical();
			}
			count++;
		}
		#endregion
		
		// Update
		if(GUI.changed){
			EditorUtility.SetDirty(t);	
			if (Application.isPlaying){
				t.KillAllSequences();
			}
		}
		
		
	}
	
	
	void OnSceneGUI (){
		HTSpriteSequencer t = (HTSpriteSequencer)target;
		
		foreach (HTSequence seq in t.sequences){
			if (seq.spriteSheet!=null){
				seq.offset =  Handles.PositionHandle (t.transform.position + seq.offset, Quaternion.identity) - t.transform.position;
				Handles.Label( t.transform.position+seq.offset, seq.spriteSheet.name);
			}
		}
		
		if(GUI.changed){
			EditorUtility.SetDirty(t);	
			if (Application.isPlaying){
				t.KillAllSequences();	
			}
		}
		
	}
	
	string GetAssetRootPath( string path){
		
		string[] tokens = path.Split('/');
		
		path="";
		for (int i=0;i<tokens.Length-1;i++){
			path+= tokens[i] +"/";
		}
		return path;
	}
	
	public static bool CreateAssetDirectory(string name){
		string directory = "Assets/UserSpriteSheetEffects";
		if (!System.IO.Directory.Exists(directory)){
			AssetDatabase.CreateFolder("Assets","UserSpriteSheetEffects");
		}
		directory = "Assets/UserSpriteSheetEffects/" + name;
		if (!System.IO.Directory.Exists(directory)){
			AssetDatabase.CreateFolder("Assets/UserSpriteSheetEffects",name);
			return true;
		}
		return false;
	}
	
	
}
