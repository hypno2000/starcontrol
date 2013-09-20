using UnityEngine;
using System;
using System.Collections;

public class StarMapCamera : SceneAware<StarMapScene> {

	public IEnumerator LerpSize(float end, float time, Action whenDone = null) {
		float i = 0f;
		float start = camera.orthographicSize;
		isLerping = true;
		while (i < time) {
			camera.orthographicSize = Mathf.Lerp(start, end, i / time);
			i += Time.deltaTime;
			yield return null;
		}
		isLerping = false;
		if (whenDone != null) {
			whenDone();
		}
	}

	void Update() {

		// camera follow ship
		if (!isLerping) {
			transform.position = new Vector3(
				scene.playerShip.transform.position.x,
				scene.playerShip.transform.position.y,
				transform.position.z
			);
		}
	}

}

