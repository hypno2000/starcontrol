using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatCamera : MonoBehaviour
{

	private Ship[] _ships;
	public float MinDistance;

	void Start()
	{
		_ships = FindObjectsOfType(typeof(Ship)) as Ship[];
	}

	void Update()
	{
		// focus camera on middle of the two ships
		var middlePoint = (_ships[0].transform.position + _ships[1].transform.position) / 2;
		gameObject.transform.position = new Vector3(
			middlePoint.x,
			middlePoint.y,
			gameObject.transform.position.z
		);

		// zoom in/out based on ships distance
		var distance = Vector3.Distance(_ships[0].transform.position, _ships[1].transform.position) / 1.2f;
		camera.orthographicSize = Mathf.Max((_ships[0].maxLength + _ships[1].maxLength) / 2f, distance);

	}

}
