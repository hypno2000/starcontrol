using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

abstract public class Planet : SceneAware<Scene> {

	public int index;
	public Star star;

	override protected void Start() {
		base.Start();
		SolarSystemScene scene = (SolarSystemScene)this.scene;
		if (scene.playerShip.manifest.planet == index) {
			StartCoroutine(LerpEnter(30f, .7f, () => {
				scene.playerShip.rigidbody.velocity = scene.playerShip.manifest.velocity;
				scene.playerShip.rigidbody.angularVelocity = scene.playerShip.manifest.angularVelocity;
			}));
		}
	}

	public PlanetType GetPlanetType() {
		return (PlanetType) GetType().GetField("type").GetRawConstantValue();
	}

	void OnTriggerEnter(Collider other) {
		CombatModule mod = CombatModule.FindModule(other.gameObject);
		if (mod == null || mod.ship.isLerping) {
			return;
		}
		Ship ship = mod.ship;
		if (ship.planet == null) {
			StartCoroutine(LerpEnter(30f, .7f));
			ship.planet = this;
		}
	}

	public IEnumerator LerpEnter(float expandFactor, float time, Action whenDone = null) {
		List<Vector3> vStart = new List<Vector3>();
		List<Vector3> vEnd = new List<Vector3>();
		List<float> fStart = new List<float>();
		List<float> fEnd = new List<float>();

		// lights
		Light starLight = new List<Light>(star.GetComponentsInChildren<Light>()).Find(x => x.flare == null);
		fStart.Add(starLight.range);
		fEnd.Add(starLight.range * expandFactor);

		// this planet scale
		float scaleFactor = expandFactor * 10f;
		vStart.Add(transform.localScale);
		vEnd.Add(transform.localScale * expandFactor / 10f);

		// all planets
		foreach (Planet planet in star.planets) {
			vStart.Add(planet.transform.position);
			vEnd.Add(planet.transform.position * expandFactor);
		}

		// orbit lines
		BuildCircleMesh[] lines = FindObjectsOfType(typeof(BuildCircleMesh)) as BuildCircleMesh[];
		foreach (BuildCircleMesh line in lines) {
			line.gameObject.renderer.enabled = false;
			fStart.Add(line.innerRadius);
			fEnd.Add((line.innerRadius + line.circleWidth / 2f) * expandFactor - line.circleWidth / 2f);
		}

		// camera
		vStart.Add(Camera.main.transform.position);
		vEnd.Add(new Vector3(
			transform.position.x * expandFactor,
			transform.position.y * expandFactor,
			Camera.main.transform.position.z
		));
		fStart.Add(Camera.main.orthographicSize);
		fEnd.Add(Camera.main.orthographicSize / expandFactor * 20f);

		// player ship
		Camera.main.transform.position = vEnd[vEnd.Count - 1];
		Camera.main.orthographicSize = fEnd[fEnd.Count - 1];
		Bounds endCameraBounds = scene.GetCameraBounds();
		Camera.main.transform.position = vStart[vStart.Count - 1];
		Camera.main.orthographicSize = fStart[fStart.Count - 1];

		Ship ship = ((SolarSystemScene)scene).playerShip;
		if (ship.planet == null) {
			vStart.Add(ship.transform.position);
			float maxDistance = Mathf.Min(endCameraBounds.extents.x, endCameraBounds.extents.y) - ship.maxLength;
			vEnd.Add((ship.transform.position - transform.position).normalized * maxDistance + transform.position * expandFactor);
		}
		else {
			vStart.Add(transform.position + (ship.manifest.position - transform.position * expandFactor).normalized * (ship.maxLength + .5f + transform.lossyScale.x / scaleFactor));
			vEnd.Add(ship.manifest.position);
		}

		// lerp
		return LerpZoom(vStart, vEnd, fStart, fEnd, time, whenDone);
	}

	public IEnumerator LerpLeave(float expandFactor, float time, Action whenDone = null) {
		List<Vector3> vStart = new List<Vector3>();
		List<Vector3> vEnd = new List<Vector3>();
		List<float> fStart = new List<float>();
		List<float> fEnd = new List<float>();

		// lights
		Light starLight = new List<Light>(star.GetComponentsInChildren<Light>()).Find(x => x.flare == null);
		fStart.Add(starLight.range);
		fEnd.Add(starLight.range / expandFactor);

		// this planet scale
		float scaleFactor = expandFactor * 10f;
		vStart.Add(transform.localScale);
		vEnd.Add(transform.localScale / expandFactor * 10f);

		// all planets
		foreach (Planet planet in star.planets) {
			vStart.Add(planet.transform.position);
			vEnd.Add(planet.transform.position / expandFactor);
		}

		// orbit lines
		BuildCircleMesh[] lines = FindObjectsOfType(typeof(BuildCircleMesh)) as BuildCircleMesh[];
		foreach (BuildCircleMesh line in lines) {
			line.gameObject.renderer.enabled = false;
			fStart.Add(line.innerRadius);
			fEnd.Add((line.innerRadius + line.circleWidth / 2f) / expandFactor - line.circleWidth / 2f);
		}

		// camera
		vStart.Add(Camera.main.transform.position);
		vEnd.Add(new Vector3(
			star.transform.position.x / expandFactor,
			star.transform.position.y / expandFactor,
			Camera.main.transform.position.z
		));
		fStart.Add(Camera.main.orthographicSize);
		fEnd.Add(Camera.main.orthographicSize * expandFactor / 20f);

		// player ship
		Camera.main.transform.position = vEnd[vEnd.Count - 1];
		Camera.main.orthographicSize = fEnd[fEnd.Count - 1];
		Camera.main.transform.position = vStart[vStart.Count - 1];
		Camera.main.orthographicSize = fStart[fStart.Count - 1];

		Ship ship = ((SolarSystemScene)scene).playerShip;
		vStart.Add(ship.transform.position);
		vEnd.Add((transform.position / expandFactor) + (ship.transform.position - transform.position).normalized * (ship.maxLength + .5f + transform.lossyScale.x / scaleFactor));

		// lerp
		return LerpZoom(vStart, vEnd, fStart, fEnd, time, whenDone);
	}

	public IEnumerator LerpZoom(List<Vector3> vStart, List<Vector3> vEnd, List<float> fStart, List<float> fEnd, float time, Action whenDone = null) {
		float elapsed = 0f;
		isLerping = true;

		// orbit lines
		BuildCircleMesh[] lines = FindObjectsOfType(typeof(BuildCircleMesh)) as BuildCircleMesh[];

		// lights
		Light starLight = new List<Light>(star.GetComponentsInChildren<Light>()).Find(x => x.flare == null);

		// player ship
		Ship ship = ((SolarSystemScene)scene).playerShip;

		int v;
		int f;
		float progress = 0f;
		while (time > 0f && progress < 1f) {
			v = 0;
			f = 0;
			progress = elapsed / time;

			// lights
			starLight.range = Mathfx.Hermite(fStart[f], fEnd[f], progress);
			f++;

			// this planet scale
			transform.localScale = Mathfx.Hermite(vStart[v], vEnd[v], progress);
			v++;

			// all planets
			foreach (Planet planet in star.planets) {
				planet.transform.position = Mathfx.Hermite(vStart[v], vEnd[v], progress);
				v++;
			}

			// orbit lines
			foreach (BuildCircleMesh line in lines) {
				line.innerRadius = Mathfx.Hermite(fStart[f], fEnd[f], progress);
				f++;
			}

			// camera
			Camera.main.transform.position = Mathfx.Hermite(vStart[v], vEnd[v], progress);
			v++;
			Camera.main.orthographicSize = Mathfx.Hermite(fStart[f], fEnd[f], progress);
			f++;

			// player ship
			ship.rigidbody.velocity = Vector3.zero;
			ship.rigidbody.angularVelocity = Vector3.zero;
			ship.transform.position = Mathfx.Hermite(vStart[v], vEnd[v], progress);
			v++;

			elapsed += Time.deltaTime;
			yield return null;
		}

		// make sure the end is exact
		v = 0;
		f = 0;

		// lights
		starLight.range = fEnd[f];
		f++;

		// this planet scale
		transform.localScale = vEnd[v];
		v++;

		// all planets
		foreach (Planet planet in star.planets) {
			planet.transform.position = vEnd[v];
			v++;
		}

		// orbit lines
		foreach (BuildCircleMesh line in lines) {
			line.innerRadius = fEnd[f];
			f++;
		}

		// camera
		Camera.main.transform.position = vEnd[v];
		v++;
		Camera.main.orthographicSize = fEnd[f];
		f++;

		// player ship
		ship.rigidbody.velocity = Vector3.zero;
		ship.rigidbody.angularVelocity = Vector3.zero;
		ship.transform.position = Mathfx.Coserp(vStart[v], vEnd[v], progress);
		v++;

		// orbit lines
		foreach (BuildCircleMesh line in lines) {
			line.gameObject.renderer.enabled = true;
		}

		isLerping = false;
		if (whenDone != null) {
			whenDone();
		}
	}

}