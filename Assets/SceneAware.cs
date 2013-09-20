using UnityEngine;
using System;
using System.Collections;

public abstract class SceneAware<T> : MonoBehaviour where T : Scene {

	protected T scene;
	protected Game game;
	public bool isLerping = false;

	virtual protected void Start() {
		scene = FindObjectOfType(typeof(T)) as T;
		game = scene.game;
	}

	public IEnumerator LerpPosition(Vector3 end, float time, Action whenDone = null) {
		float i = 0f;
		Vector3 start = transform.position;
		isLerping = true;
		while (i < time) {
			transform.position = Mathfx.Coserp(start, end, i / time);
			i += Time.deltaTime;
			yield return null;
		}
		isLerping = false;
		if (whenDone != null) {
			whenDone();
		}
	}

	public IEnumerator LerpRotation(Vector3 end, float time, Action whenDone = null) {
		float i = 0f;
		Vector3 start = transform.localRotation.eulerAngles;
		isLerping = true;
		while (i < time) {
			transform.localRotation = Quaternion.Euler(Mathfx.Coserp(start, end, i / time));
			i += Time.deltaTime;
			yield return null;
		}
		isLerping = false;
		if (whenDone != null) {
			whenDone();
		}
	}

	public IEnumerator LerpScale(Vector3 end, float time, Action whenDone = null) {
		float i = 0f;
		Vector3 start = transform.localScale;
		isLerping = true;
		while (i < time) {
			transform.localScale = Mathfx.Coserp(start, end, i / time);
			i += Time.deltaTime;
			yield return null;
		}
		isLerping = false;
		if (whenDone != null) {
			whenDone();
		}
	}

}