// HTSpriteSheetEditor v2.0 (Aout 2012)
// HTSpriteSheetEditor.cs library is copyright (c) of Hedgehog Team
// Please send feedback or bug reports to the.hedgehog.team@gmail.com

using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// HTSpriteSheet editor.
/// </summary>
[CustomEditor(typeof(HTSpriteSheet))]
public class HTSpriteSheetEditor : Editor {

	// Use this for initialization
	public override void OnInspectorGUI(){
		
		GUIStyle style;
		HTSpriteSheet t;
		
		t = (HTSpriteSheet)target;
		style = new GUIStyle();
		style.fontStyle =FontStyle.Bold;
			
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		// Turret properties
		GUILayout.Label("Sprite sheet properties",style);	
		EditorGUILayout.Space();
		t.spriteSheetMaterial = (Material)EditorGUILayout.ObjectField("Sprite sheet material", t.spriteSheetMaterial,typeof(Material),true); 
		t.uvAnimationTileX = EditorGUILayout.IntField("Tile X",t.uvAnimationTileX);
		t.uvAnimationTileY = EditorGUILayout.IntField("Tile Y",t.uvAnimationTileY);
		t.spriteCount = EditorGUILayout.IntField("Number of sprite",t.spriteCount);
		t.framesPerSecond = EditorGUILayout.IntField("Frames per second",t.framesPerSecond);
		t.isOneShot = EditorGUILayout.Toggle( "One shot",t.isOneShot);
		if (!t.isOneShot){
			EditorGUILayout.HelpBox("0 = infite loop",MessageType.Info);
			t.life = EditorGUILayout.FloatField( "life",t.life);	
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		GUILayout.Label("Sprite properties",style);
		EditorGUILayout.Space();
		t.billboarding = (HTSpriteSheet.CameraFacingMode)EditorGUILayout.EnumPopup("Camera facing",t.billboarding);
		EditorGUILayout.Space();
		t.sizeStart = EditorGUILayout.Vector2Field("Start size",t.sizeStart);
		t.sizeEnd = EditorGUILayout.Vector2Field("End size",t.sizeEnd);
		EditorGUILayout.Space();
		t.randomRotation = EditorGUILayout.Toggle( "Random rotation",t.randomRotation);
		if (!t.randomRotation){
			t.rotationStart = EditorGUILayout.FloatField( "Start rotation",t.rotationStart);
		}
		t.rotationEnd = EditorGUILayout.FloatField( "End rotation",t.rotationEnd);
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		GUILayout.Label("Color properties",style);
		t.addColorEffect= EditorGUILayout.Toggle( "Add color effect",t.addColorEffect);
		if (t.addColorEffect){
			t.colorStart = EditorGUILayout.ColorField( "Start color",t.colorStart);
			t.colorEnd = EditorGUILayout.ColorField( "Start color",t.colorEnd);
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		// Light Effect
		GUILayout.Label("Light properties",style);			
		t.addLightEffect = EditorGUILayout.Toggle( "Add light effect",t.addLightEffect);
		if ( t.addLightEffect ){
			t.lightRange = EditorGUILayout.FloatField("Light range",t.lightRange);	
			t.lightColor = EditorGUILayout.ColorField( "Light color", t.lightColor);
			t.lightFadeSpeed = EditorGUILayout.FloatField("Light fade speed",t.lightFadeSpeed);	
		}

		// Refresh
		if (GUI.changed){
			EditorUtility.SetDirty (target);
		}
		 		 	
		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}
}
