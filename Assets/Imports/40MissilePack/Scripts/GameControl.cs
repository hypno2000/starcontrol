//Missile Pack v1.0 Luandun Games
using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
	//Define an array to store all the missile prefabs
	public GameObject[] missiles;
	
	//Define three flame textures
	public Texture redFlame;
	public Texture greenFlame;
	public Texture blueFlame;

	//Define one counter of missile order
	private int currentMissileNumber = 0;

	//Define flame size(length)
	private float flameSize = 1.0f;
	
	//Define missile moving Speed
	private float moveSpeed = 20.0f;

	//Define missile rotating Speed
	private float rotateSpeed = 2.0f;

	//Define missile acceleration speed
	private float accelerationSpeed = 0.0f;

	//Define the missile trail style, you can choose particles or simple trail renderer
	private MissileControl.TrailStyle missileTrailStyle = MissileControl.TrailStyle.Particles;

	//Define the missile can auto track the nearest enemy
	private bool AutoTracking = true;
	
	//Define the current flame color
	private MissileControl.FlameColorStyle flameColor = MissileControl.FlameColorStyle.Red;

	//Define 3 missile trail state
	private bool isParticleTrail = true;
	private bool isSimpleLineTrail = false;
	private bool isNoneTrail = false;
	
	//Define 3 flame color
	private bool isRedFlame = true;
	private bool isGreenFlame = false;
	private bool isBlueFlame = false;

	//Draw GUI
	void OnGUI ()
	{
		//Draw Fire button
		if (GUI.Button (new Rect (Screen.width * 0.5f - 50, Screen.height * 0.8f, 100, 30), "Fire")) {
			//Spawn one missile 
			GameObject currentMissile = Instantiate (missiles[currentMissileNumber], Vector3.zero, Quaternion.identity) as GameObject;
			
			//Set spawned missile moving speed
			currentMissile.GetComponent<MissileControl> ().moveSpeed = moveSpeed;
			
			//Set spawned missile rotation speed
			currentMissile.GetComponent<MissileControl> ().rotateSpeed = rotateSpeed;
			
			//Set spawned missile acceleration speed
			currentMissile.GetComponent<MissileControl> ().accelerationSpeed = accelerationSpeed;
			
			//Set spawned missile flame size
			currentMissile.GetComponent<MissileControl>().flameSize = flameSize;
			
			//Set spawned missile auto tracking
			currentMissile.GetComponent<MissileControl>().AutoTracking  = AutoTracking;
			
			//Set spawned missile trail style
			currentMissile.GetComponent<MissileControl>().missileTrailStyle = missileTrailStyle;
			
			//Set spawned missile flame texture
			currentMissile.GetComponent<MissileControl>().flameColor = flameColor;
			
			//Loop the missile counter
			currentMissileNumber = currentMissileNumber >= missiles.Length - 1 ? 0 : ++currentMissileNumber;
			
//			Debug.Log (currentMissileNumber);
		}
	
		
		//Draw Text
		GUI.Label (new Rect (Screen.width * 0.1f - 75, Screen.height * 0.45f - 5, 150, 30),"Missile Setup");
		
		//Show moving speed control slider
		GUI.Label (new Rect (Screen.width * 0.1f - 75, Screen.height * 0.5f - 5, 150, 30), "Moving Speed");
		moveSpeed = GUI.HorizontalSlider (new Rect (Screen.width * 0.2f - 75, Screen.height * 0.5f, 150, 30), moveSpeed, 5.0f, 50.0f);
		
		//Show rotation speed control slider
		GUI.Label (new Rect (Screen.width * 0.1f - 75, Screen.height * 0.55f - 5, 150, 30), "Rotation Speed");
		rotateSpeed = GUI.HorizontalSlider (new Rect (Screen.width * 0.2f - 75, Screen.height * 0.55f, 150, 30), rotateSpeed, 0.0f, 50.0f);
		
		//Show flame size control slider
		GUI.Label (new Rect (Screen.width * 0.1f - 75, Screen.height * 0.6f - 5, 150, 30), "Flame Size");
		flameSize = GUI.HorizontalSlider (new Rect (Screen.width * 0.2f - 75, Screen.height * 0.6f, 150, 30), flameSize, 0.0f, 2.0f);
		
		//Show acceleration speed control slider
		GUI.Label (new Rect (Screen.width * 0.1f - 75, Screen.height * 0.65f - 5, 150, 30), "Acceleration");
		accelerationSpeed = GUI.HorizontalSlider (new Rect (Screen.width * 0.2f - 75, Screen.height * 0.65f, 150, 30), accelerationSpeed, 0.0f, 100.0f);
		
		//Show Auto Tracking toggle
		GUI.Label (new Rect (Screen.width * 0.1f - 75, Screen.height * 0.7f - 5, 150, 30), "Auto Tracking");
		AutoTracking = GUI.Toggle (new Rect (Screen.width * 0.2f - 75, Screen.height * 0.7f - 5, 150, 30), AutoTracking, "");
		
		//Show missile trail style toggle
		//1.Particle trail toggle
		GUI.Label (new Rect (Screen.width * 0.1f - 75, Screen.height * 0.8f, 150, 30), "Particle Trail");
		isParticleTrail = GUI.Toggle (new Rect (Screen.width * 0.2f - 75, Screen.height * 0.8f, 150, 30), isParticleTrail, "");
		//If select the particle trail , then disable other 2 trail style.
		if (isParticleTrail) {
			SetAllTrailToggleFalse ();
			isParticleTrail = true;
			missileTrailStyle = MissileControl.TrailStyle.Particles;
		}
		
		//2.Simple line trail toggle
		GUI.Label (new Rect (Screen.width * 0.1f - 75, Screen.height * 0.85f , 150, 30), "SimpleLine Trail");
		isSimpleLineTrail = GUI.Toggle (new Rect (Screen.width * 0.2f - 75, Screen.height * 0.85f, 150, 30), isSimpleLineTrail, "");
		//If select the simple line trail , then disable other 2 trail style.
		if (isSimpleLineTrail) {
			SetAllTrailToggleFalse ();
			isSimpleLineTrail = true;
			missileTrailStyle = MissileControl.TrailStyle.SimpleLine;
		}
		
		//3.None trail toggle
		GUI.Label (new Rect (Screen.width * 0.1f - 75, Screen.height * 0.9f , 150, 30), "None Trail");
		isNoneTrail = GUI.Toggle (new Rect (Screen.width * 0.2f - 75, Screen.height * 0.9f, 150, 30), isNoneTrail, "");
		//If select the none trail , then disable other 2 trail style.
		if (isNoneTrail) {
			SetAllTrailToggleFalse ();
			isNoneTrail = true;
			missileTrailStyle = MissileControl.TrailStyle.None;
		}
		
		
		//Show missile flame color toggle
		//1.Red flame toggle
		GUI.Label (new Rect (Screen.width * 0.3f - 100, Screen.height * 0.8f , 150, 30), "Red Flame");
		isRedFlame = GUI.Toggle (new Rect (Screen.width * 0.4f - 100, Screen.height * 0.8f, 150, 30), isRedFlame , "");
		//If select the particle trail , then disable other 2 trail style.
		if (isRedFlame) {
			SetAllFlameColorFalse ();
			isRedFlame = true;
			flameColor = MissileControl.FlameColorStyle.Red;
		}
		
		//2.Green flame toggle
		GUI.Label (new Rect (Screen.width * 0.3f - 100, Screen.height * 0.85f , 150, 30), "Green Flame");
		isGreenFlame = GUI.Toggle (new Rect (Screen.width * 0.4f - 100, Screen.height * 0.85f, 150, 30), isGreenFlame, "");
		//If select the simple line trail , then disable other 2 trail style.
		if (isGreenFlame) {
			SetAllFlameColorFalse ();
			isGreenFlame = true;
			flameColor = MissileControl.FlameColorStyle.Green;
		}
		
		//3.Blue flame toggle
		GUI.Label (new Rect (Screen.width * 0.3f - 100, Screen.height * 0.9f , 150, 30), "Blue Flame");
		isBlueFlame  = GUI.Toggle (new Rect (Screen.width * 0.4f - 100, Screen.height * 0.9f, 150, 30), isBlueFlame, "");
		//If select the none trail , then disable other 2 trail style.
		if (isBlueFlame) {
			SetAllFlameColorFalse ();
			isBlueFlame = true;
			flameColor = MissileControl.FlameColorStyle.Blue;
		}
		
	}

	//Set three missile trail state false
	void SetAllTrailToggleFalse ()
	{
		isParticleTrail = false;
		isSimpleLineTrail = false;
		isNoneTrail = false;
	}
	
	//Set three missile flame color false
	void SetAllFlameColorFalse ()
	{
		isRedFlame=false;
		isGreenFlame=false;
		isBlueFlame=false;
	}
	
}
