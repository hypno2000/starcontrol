using UnityEngine;
using System;

public class Gauge : MonoBehaviour {

	private UISlider progressBar;
	private UILabel valueLabel;
	private float max = 10f;
	private float current = 2f;

	void Start() {
		progressBar = GetComponentInChildren<UISlider>() as UISlider;
		var labels = GetComponentsInChildren<UILabel>();
		if (labels.Length > 1) {
			valueLabel = GetComponentsInChildren<UILabel>()[1] as UILabel;
		}
	}

	public void SetCurrent(float val) {
		current = val;
		UpdateProgressBar();
		UpdateLabel();
	}

	public void SetMax(float val) {
		max = val;
		UpdateProgressBar();
		UpdateLabel();
	}

	void UpdateProgressBar() {
		if (max == 0f) {
			progressBar.sliderValue = 0f;
		}
		else {
			progressBar.sliderValue = current / max;
		}
	}

	void UpdateLabel() {
		if (valueLabel == null) {
			return;
		}
		valueLabel.text = "[00ff00]" + ((int)current) + " [ffffff]/[00ff00] " + (int)max;
	}

}

