using UnityEngine;
using UnityEditor;
using System.Collections;

public class HTSpriteSequencerWizard : ScriptableWizard  {
		
	
	// Effect
	string effectName ="MyEffect";
	
	static HTSpriteSequencerWizard window;
	
	// Add Menu
	[MenuItem ("Hedgehog Team/Spritesheet effect Sequencer")]
	static void  SpriteSheetSequencer(){
			
		window = (HTSpriteSequencerWizard)EditorWindow.GetWindow (typeof (HTSpriteSequencerWizard),true,"Spritsheet effect sequencer wizard");
		window.minSize = new Vector2(256,80);
		window.maxSize = new Vector2(256,80);
	}

		
	void OnGUI(){
		
		// Title
		GUI.Label( new Rect(5,10,90,20),"Effect name : ");
		effectName = GUI.TextField( new Rect(100,10,150,20), effectName);
	
		if (GUI.Button( new Rect(5,50,246,20),"Generate")){
			Generate();	
		}
	}
	
	void Generate(){
		
		if (HTSpriteSequencerEditor.CreateAssetDirectory(effectName)){
			GameObject effect = new GameObject(effectName);
			effect.AddComponent<HTSpriteSequencer>();
			EditorApplication.RepaintProjectWindow();
			 Selection.activeGameObject = effect;
			EditorGUIUtility.PingObject(effect);
			window.Close();			
		}
		else{
			EditorUtility.DisplayDialog("Already exist","An effect already exist with this name.","OK");
		}	

	}

}
