//Missile Pack v1.0 Luandun Games
using UnityEngine;
using System.Collections;

public class MissileControl : MonoBehaviour
{
	//Define flame size(length), 0 means not available.
	public float flameSize = 1.0f;
	
	//Define flame color(texute), red is the default color
	public FlameColorStyle flameColor = MissileControl.FlameColorStyle.Red;
	
	//Define missile moving Speed
	public float moveSpeed = 20.0f;

	//Define missile rotating Speed;
	public float rotateSpeed = 2.0f;

	//Define missile acceleration speed
	public float accelerationSpeed = 0.0f;

	//Automaticly distroy the missile in given seconds, 0 means don't need to auto destroy .
	public float disappearAfterSeconds = 20.0f;

	//Define the missile trail style, you can choose particles or simple trail renderer
	public TrailStyle missileTrailStyle = TrailStyle.Particles;

	//Define the tag of game object you want to shoot 
	public string targetTag = "Enemy";

	//Define the missile can auto track the nearest enemy
	public bool AutoTracking = true;
	
	//You can put your own explosion prefab here in inspector 
	public GameObject explosion;
	
	//Define 3 flame color texture
	public Texture redFlame;
	public Texture greenFlame;
	public Texture blueFlame;

	
	//Using this for transform cache
	private Transform myTrans;
	
	//Using this for smoke particle child transform cache
	private Transform smoke;
	
	//Using this for missile model child transform cache 
	private Transform missileModel;
	
	//Using this for flame object transform cache
	private Transform flame;
	
	//Store all the enemies in the scene
	private GameObject[] allEnemies;
	
	//If the missile can auto track, this will be the target object(nearest enemy)
	private Transform targetEnemy;
	
	//Define the trail style of missile
	public enum TrailStyle
	{
		Particles,
		SimpleLine,
		None,
	}
	
	//Define the colors of missile flame
	public enum FlameColorStyle
	{
		Red,
		Green,
		Blue,
	}


	// Use this for initialization
	void Start ()
	{
		//Cache transform
		myTrans = transform;
		
		//Set auto disappear after given seconds, 0 means do not disappear automaticly
		if (disappearAfterSeconds != 0.0f) {
			Invoke ("SelfDestroy", disappearAfterSeconds);
		}
		
		//Cache smoke child object
		smoke = myTrans.FindChild ("Smoke");
		
		//Cache missile model child object
		missileModel = myTrans.FindChild ("Missile");
		
		//Set the flame size/length to the given number
		flame = myTrans.FindChild("Flame");
		flame.localScale = new Vector3(flame.localScale.x, flame.localScale.y, flameSize * flame.localScale.z);
		
		//If flame is not available, deactive it.
		if(flameSize == 0){
			flame.gameObject.SetActive(false);
		}
		
		//Set the given flame color
		switch (flameColor) {
		case FlameColorStyle.Red:
			flame.Find("Flame").renderer.material.SetTexture("_MainTex",redFlame);
			break;
		case FlameColorStyle.Green:
			flame.Find("Flame").renderer.material.SetTexture("_MainTex",greenFlame);
			break;
		case FlameColorStyle.Blue:
			flame.Find("Flame").renderer.material.SetTexture("_MainTex",blueFlame);
			break;
		default:
		break;
		}
		
		
			
		//If the missile can auto track, cache all enemies in the scene and set the nearest enemy object as target object
		if (AutoTracking) {
			allEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
			targetEnemy = FindNearestEnemy (allEnemies);
		}

		//Set the correct trail style
		ActiveAllTrail();
		switch (missileTrailStyle) {
		case TrailStyle.Particles:
			missileModel.GetComponent<TrailRenderer> ().enabled = false;
			break;
		case TrailStyle.SimpleLine:
			smoke.gameObject.SetActive(false);
			break;
		case TrailStyle.None:
			missileModel.GetComponent<TrailRenderer> ().enabled = false;
			smoke.gameObject.SetActive(false);
			break;
		default:
			break;
		}
	}

	//If hit the enemy, then destroy the missile, you can change the "Enemy" to your own tag.
	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("Enemy")) {
			SelfDestroy ();
		}
	}

	void FixedUpdate ()
	{
		//Rotate the missile forward direction to the target object
		if (AutoTracking && targetEnemy!=null) {
			myTrans.forward = Vector3.RotateTowards (myTrans.forward , targetEnemy.position  - myTrans.position, 0.02f, 0.02f);
		}
		
		//Move the missile going forward according given speed
		if (moveSpeed != 0) {
			myTrans.Translate (myTrans.forward * moveSpeed * Time.deltaTime,Space.World);
		}
		
		//Accelerate the missile 
		if (accelerationSpeed != 0) {
			myTrans.rigidbody.AddForce (myTrans.forward * accelerationSpeed * Time.deltaTime);
		}
		
		//Rotate the missile 
		if (rotateSpeed != 0) {
			missileModel.RotateAroundLocal (Vector3.forward, rotateSpeed * Time.deltaTime);
		}
		
	}

	//Destroy missile
	void SelfDestroy ()
	{
		//When destroy the missile , Set the smoke to fade out, the smoke had fade out animation.
		if (missileTrailStyle == MissileControl.TrailStyle.Particles) {
			smoke.parent = null;
			smoke.GetComponent<DestroyAfterSeconds> ().DestroyAfter (3);
		}
		
		//Create explosion with given prefab
		if(explosion!=null){
			Instantiate (explosion, myTrans.position, myTrans.rotation);
		}
		
		//Delete missile object 
		Destroy (gameObject);
	}

	//Using this function to get the nearest enemy in the scene
	Transform FindNearestEnemy (GameObject[] enemies)
	{
		float nearDistance = 100000.0f;
		Transform nearestEnemy = null;
		foreach (GameObject i in enemies) {
			//Get the distance between current missile and each enemy
			float distance = Vector3.Distance (i.transform.position, myTrans.position);
			
			//Store the nearest one
			if (distance < nearDistance) {
				nearDistance = distance;
				nearestEnemy = i.transform;
			}
		}
		
//		Debug.Log (nearestEnemy.name);
		return nearestEnemy;
	}
	
	//Set all trail component and object available
	void ActiveAllTrail(){
		missileModel.GetComponent<TrailRenderer> ().enabled = true;
		smoke.gameObject.SetActive(true);
	}
}
