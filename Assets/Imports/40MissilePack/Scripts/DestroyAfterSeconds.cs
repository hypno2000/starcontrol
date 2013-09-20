//Missile Pack v1.0 Luandun Games
using UnityEngine;
using System.Collections;

public class DestroyAfterSeconds : MonoBehaviour {
	//Destroy automaticly after given seconds
	public void DestroyAfter (float time){
		animation.Play("SmokeDisappear");
		Invoke ("SelfDestroy", time);
	}
	
	public void SelfDestroy(){
		Destroy(gameObject);
	}
}
