using UnityEngine;
using System.Collections;

public class ThrusterEffect : MonoBehaviour {
	
	[HideInInspector]
	public bool isActive; // Boolean value to say if thruster is active or not

	void Update() {
		// Alter the intensity of the child light of the thruster depending on the number of particles active (0 if no particles)
		transform.Find("ThrusterLight").light.intensity = particleEmitter.particleCount / 20;
	}

	// Call startThruster() function to start the thruster
	public void startThruster() {
		audio.volume = 1.0f; // Reset audio volume (fade out in stopThruster function sets it to 0)
		audio.loop = true; // Ensure that the audio is set to be looped
		audio.Play(); // Play the sound
		particleEmitter.emit = true;// Enable the paritcle emitter for the visuals of the thruster
		isActive = true; // Set the thruster active flag to true
	}

	// Call stopThruster() function to stop the thruster
	public IEnumerator stopThruster() {
		isActive = false; // Set the thruster active flag to false
		particleEmitter.emit = false; // Stop emitting particles for the thruster
		
		// Workaround: If audio.Stop() is called directly it will result in a audible click - therefore we need to fade it out instead
		while (audio.volume > 0.01f) {
			audio.volume -= 0.1f;
			if (isActive) {
				// If the thruster ignites again before it has faded out - exit this function
				yield return false;
			}
			yield return new WaitForSeconds(0.01f); // Wait one hundred of a second
		}
		audio.Stop(); // Stop the audio playing
	}
	
}