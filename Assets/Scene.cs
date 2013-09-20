using UnityEngine;
using System.Collections.Generic;

abstract public class Scene : MonoBehaviour {

	public Game gamePrefab;
	[HideInInspector]
	public Game game;
	[HideInInspector]
	public Dictionary<Vector3, Constellation> constellations;

	virtual protected void Awake() {
		constellations = new Dictionary<Vector3, Constellation>();

		// disable test
		GameObject test = GameObject.Find("Test");
		if (test != null) {
			test.SetActive(false);
		}

		game = FindObjectOfType(typeof(Game)) as Game;

		if (game == null) {
			game = Instantiate(gamePrefab) as Game;
		}

	}

	abstract protected Constellation Construct(ConstellationManifest conMf);

	public Constellation GetConstellation(Vector3 pos) {
		return Construct(game.universeGenerator.GetConstellation(pos));
	}

	public Bounds GetCameraBounds() {
		float width = Camera.main.orthographicSize * Camera.main.aspect;
		float height = Camera.main.orthographicSize;
		Vector3 pos = Camera.main.transform.position;
		return new Bounds(new Vector3(pos.x, pos.y, 0f), new Vector3(width * 2f, height * 2f, 1f));
	}

}
