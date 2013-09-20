using UnityEngine;
using System;

public class SeekAndDestroyObjective : AiObjective {

	override protected void Update() {
		base.Update();

		if (ship.propulsionSystem == null) {
			return;
		}

		float dist, acc, speed, time, timeToStop, diff;

		// left braking
		dist = GetAngleToOppenent();
		speed = -ship.transform.TransformDirection(ship.rigidbody.angularVelocity).z;
		if (dist > 0f && speed > 0.1f) {
			time = dist / speed;
			acc = leftTurnAcceleration / 6f;
			timeToStop = Mathf.Sqrt(2f) * Mathf.Sqrt(dist) / acc;
			diff = time - timeToStop;
			if (diff <= 0f && (dist < 40f || timeToStop / time < .5f)) {
//				Debug.Log("Left Braking!! Angle: " + dist + " Speed: " + speed);
				turnLeft = true;
				turnRight = false;
			}
//			Debug.Log(
//				"left) acc: " +  acc +
//				" speed: " + speed +
//				" dist: " + dist +
//				" time: " + time +
//				" timeToStop: " + timeToStop +
//				" time - timeToStop: " + (time - timeToStop)
//			);
		}

		// right braking
		dist = -GetAngleToOppenent();
		speed = ship.transform.TransformDirection(ship.rigidbody.angularVelocity).z;
		if (dist > 0f && speed > 0.1f) {
			time = dist / speed;
			acc = rightTurnAcceleration / 6f;
			timeToStop = Mathf.Sqrt(2f) * Mathf.Sqrt(dist) / acc;
			diff = time - timeToStop;
			if (diff <= 0f && (dist < 40f || timeToStop / time < .5f)) {
//				Debug.Log("Right Braking!! Angle: " + dist + " Speed: " + speed);
				turnRight = true;
				turnLeft = false;
			}
//			Debug.Log(
//				"right) acc: " +  acc +
//				" speed: " + speed +
//				" dist: " + dist +
//				" time: " + time +
//				" timeToStop: " + timeToStop +
//				" time - timeToStop: " + (time - timeToStop)
//			);
		}

		float angle = GetAngleToOppenent();
		float distance = GetDistanceToOpponent();
		float detectWindow = 140f / GetDistanceToOpponent();

		// accelerate if far enough and angle and speed is ok
		if (distance > 6f && speed < .2f && angle > -40f && angle < 40f) {
			accelerate = true;
		}

		// normal turning if no brakes anganged
		if (!turnLeft && !turnRight) {
			if (GetAngleToOppenent() > 10f) {
//				Debug.Log("Right normal turn");
				turnRight = true;
			}
			else if (GetAngleToOppenent() < -10f) {
//				Debug.Log("Left normal turn");
				turnLeft = true;
			}
		}

		// fire when angle is good
		if (angle > -detectWindow && angle < detectWindow) {
			fire = true;
		}

	}

}