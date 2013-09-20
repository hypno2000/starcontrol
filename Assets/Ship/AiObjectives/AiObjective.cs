using UnityEngine;
using System;

abstract public class AiObjective : SceneAware<CombatScene> {

	public AiShip ship;
	public Ship opponentShip;

	private float? forwardVelocity;
	private float? leftTurnVelocity;
	private float? rightTurnVelocity;
	private float? backVelocity;
	private float? forwardVelocityTime;
	private float? leftTurnVelocityTime;
	private float? rightTurnVelocityTime;
	private float? backVelocityTime;

	protected float forwardAcceleration = .6f;
	protected float leftTurnAcceleration = .6f;
	protected float rightTurnAcceleration = .6f;
	protected float backAcceleration = .6f;

	public float? angleToOpponent = null;
	public float? distanceToOpponent = null;
	public bool accelerate = false;
	public bool turnLeft = false;
	public bool turnRight = false;
	public bool moveBack = false;
	public bool fire = false;

	virtual protected void Awake() {

	}

	override protected void Start() {
		base.Start();
	}

	virtual protected void OnCollisionEnter() {
		CalcAccelerations();
		ResetAccelerationCalc();
	}

	void ResetAccelerationCalc() {
		forwardVelocity = null;
		forwardVelocityTime = null;
		leftTurnVelocity = null;
		leftTurnVelocityTime = null;
		rightTurnVelocity = null;
		rightTurnVelocityTime = null;
	}

	void CalcAccelerations() {

		// calc forward acceleration
		float timeElapsed;
		if (accelerate && forwardVelocity == null) {
			forwardVelocity = ship.rigidbody.velocity.magnitude;
			forwardVelocityTime = Time.realtimeSinceStartup;
		}
		else if (!accelerate || (timeElapsed = Time.realtimeSinceStartup - (float)forwardVelocityTime) > 1f) {
			if (forwardVelocity != null && timeElapsed > 1f) {
				forwardAcceleration = (ship.rigidbody.velocity.magnitude - (float)forwardVelocity) / timeElapsed;
			}
			forwardVelocity = null;
			forwardVelocityTime = null;
		}

		// calc left turning acceleration
		if (turnLeft && leftTurnVelocity == null) {
			leftTurnVelocity = ship.rigidbody.angularVelocity.z;
			leftTurnVelocityTime = Time.realtimeSinceStartup;
		}
		else if (!turnLeft || (timeElapsed = Time.realtimeSinceStartup - (float)leftTurnVelocityTime) > 1f) {
			if (leftTurnVelocity != null && timeElapsed > 1f) {
				leftTurnAcceleration = (ship.transform.TransformDirection(ship.rigidbody.angularVelocity).z - (float)leftTurnVelocity) / timeElapsed;
			}
			leftTurnVelocity = null;
			leftTurnVelocityTime = null;
		}

		// calc right turning acceleration
		if (turnRight && rightTurnVelocity == null) {
			rightTurnVelocity = -ship.transform.TransformDirection(ship.rigidbody.angularVelocity).z;
			rightTurnVelocityTime = Time.realtimeSinceStartup;
		}
		else if (!turnRight || (timeElapsed = Time.realtimeSinceStartup - (float)rightTurnVelocityTime) > 1f) {
			if (rightTurnVelocity != null && timeElapsed > 1f) {
				rightTurnAcceleration = (-ship.transform.TransformDirection(ship.rigidbody.angularVelocity).z - (float)rightTurnVelocity) / timeElapsed;
			}
			rightTurnVelocity = null;
			rightTurnVelocityTime = null;
		}


	}

	virtual protected void Update() {
		return;
		// fire weapons
		if (fire) {
			if (ship.weaponSystem.Fire()) {
				ResetAccelerationCalc();
			}
		}

		CalcAccelerations();

		// accelerating
		if (accelerate) {
			ship.propulsionSystem.Thrust();
		}
		else {
			ship.propulsionSystem.StopThrusters();
		}

		// turning left
		if (turnLeft) {
			ship.propulsionSystem.ManeuverLeft();
		}
		else {
			ship.propulsionSystem.StopLeftManeuverers();
		}

		// turning right
		if (turnRight) {
			ship.propulsionSystem.ManeuverRight();
		}
		else {
			ship.propulsionSystem.StopRightManeuverers();
		}

		// back
		if (moveBack) {
			ship.propulsionSystem.ManeuverBack();
		}
		else {
			ship.propulsionSystem.StopBackManeuverers();
		}

		// clean up
		accelerate = false;
		turnLeft = false;
		turnRight = false;
		moveBack = false;
		fire = false;
		angleToOpponent = null;
		distanceToOpponent = null;
	}

	protected float GetAngleToOppenent() {
		if (angleToOpponent != null) {
			return (float)angleToOpponent;
		}

		Quaternion initial = ship.transform.localRotation;
		ship.transform.LookAt(
			opponentShip.transform.position,
			-Vector3.forward
		);

		// get a "right vector" for each rotation
		Vector3 forwardA = initial * Vector2.right;
		Vector3 forwardB = ship.transform.localRotation * Vector2.right;

		// get a numeric angle for each vector, on the X-Y plane (relative to world right)
		float angleA = Mathf.Atan2(forwardA.x, forwardA.y) * Mathf.Rad2Deg;
		float angleB = Mathf.Atan2(forwardB.x, forwardB.y) * Mathf.Rad2Deg;

		// get the signed difference in these angles
		float angle = Mathf.DeltaAngle(angleA, angleB);

		ship.transform.localRotation = initial;
		angleToOpponent = angle;
		return angle;
	}

	protected float GetDistanceToOpponent() {
		if (distanceToOpponent != null) {
			return (float)distanceToOpponent;
		}
		float distance = Vector3.Distance(ship.transform.position, opponentShip.transform.position);
		distanceToOpponent = distance;
		return distance;
	}

}