using UnityEngine;
using System.Collections;

public class Gui : MonoBehaviour {
	
	public GameObject[] effects;
	public GameObject[] singleEffects;
	public GameObject[] sequences;
	private int currentEffectIndex=0;
	private GameObject[] currentEffects;
	
	private string label;
	private string label2;
	
	void OnGUI(){
		
		GUIStyle titleStyle = new  GUIStyle();
		titleStyle.fontStyle = FontStyle.Bold;
		titleStyle.fontSize = 24;
		titleStyle.normal.textColor = Color.white;
		
		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontStyle = FontStyle.Bold;
		
		if (GUI.Button( new Rect( 5,2,50,25), "Prev",buttonStyle)){
			currentEffectIndex--;
			if (currentEffectIndex<0){
				currentEffectIndex = 2;
			}
		}
			
		if (GUI.Button( new Rect( Screen.width-55,2,50,25), "Next",buttonStyle)){
			currentEffectIndex++;
			if (currentEffectIndex>2){
				currentEffectIndex = 0;
			}
		}
		
		
		switch(currentEffectIndex){
			case 0:
				currentEffects = effects;
				label="Fire & Explosion effect";
				label2="Some effects like furnaces, candle... are perfectly looped.";
				break;
			case 1:
				currentEffects = singleEffects;
				label="Single effect";
				label2="Some effects like furnaces, candle... are perfectly looped.";
				break;
			case 2:
				currentEffects = sequences;
				label="Sequences examples";
				label2="Create your sequences with our C# script";
				break;
			
		}
		
		GUI.color = Color.white;
		GUI.Label(new Rect(Screen.width/2-150,2,420,35),label,titleStyle);
		GUI.Label(new Rect(Screen.width/2-375,Screen.height-50,800,35),label2,titleStyle);
		
		for (int i=0;i<currentEffects.Length/2;i++){
			
			GUI.color = new Color(1f,0.75f,0.5f);
			if (GUI.Button(new Rect( 10,35+i*30,110,20),currentEffects[i].name)){
				Instantiate( currentEffects[i],new Vector3(-1.5f,2f,1.5f),Quaternion.identity);
			}
		}
			
		int j=0;
		for (int i=currentEffects.Length/2;i<currentEffects.Length;i++){
			GUI.color = new Color(1f,0.75f,0.5f);
			if (GUI.Button(new Rect( Screen.width-120,35+j*30,110,20),currentEffects[i].name)){
				Instantiate( currentEffects[i],new Vector3(-1.5f,2f,1.5f),Quaternion.identity);
			}
			j++;
		}
		
	}
	

}
