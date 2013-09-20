using UnityEngine;
using System.Collections;

public class SampleGUI : MonoBehaviour {

	public GUISkin skinBlackGloss;

	// Black window
	private Rect blackWinRect = new Rect(10.0f, 10.0f, 760.0f, 600.0f);
	private string blackTextField = "Sample Text Field";
	private string blackTextArea = "Sample Text Area\nSample Text Area\nSample Text Area";
	private bool blackButtonToggle = false;
	private bool blackButtonToggleLeft = false;
	private bool blackButtonToggleMid = false;
	private bool blackButtonToggleRight = false;
	private bool blackToggleRadio = false;
	private bool blackToggleChkBox = false;
	private float blackHorizontalSlide = 0.5f;
	private float blackVerticalSlide = 0.5f;
	private Vector2 blackScroll = Vector2.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		// Black Gloss
		GUI.skin = skinBlackGloss;
		blackWinRect = GUI.Window(100000, blackWinRect, winBlack, GUIContent.none, "Window");
	}

	// Sample Black Gloss GUI
	void winBlack(int id) {
		GUI.DragWindow(new Rect(0.0f, 0.0f, 760.0f, 40.0f));

		GUILayout.BeginArea(new Rect(20.0f, 20.0f, 720.0f, 560.0f));
			GUILayout.BeginHorizontal();

			GUILayout.BeginVertical();
				GUILayout.Label("Sample Window",  new GUILayoutOption[] { GUILayout.Width(180.0f), GUILayout.Height(24.0f) });
				GUILayout.Label("Sample Label First Line\nSample Label Second Line\nSample Label Third Line",  new GUILayoutOption[] { GUILayout.Width(180.0f) });
				GUILayout.Space(8.0f);
				blackTextField = GUILayout.TextField(blackTextField, new GUILayoutOption[] { GUILayout.Width(240.0f), GUILayout.Height(32.0f) });
				GUILayout.Space(8.0f);
				blackTextArea = GUILayout.TextArea(blackTextArea, new GUILayoutOption[] { GUILayout.Width(350.0f), GUILayout.Height(160.0f) });
				GUILayout.Space(8.0f);
				GUILayout.Label("Sample Button",  new GUILayoutOption[] { GUILayout.Width(180.0f) });
				GUILayout.Space(8.0f);
				GUILayout.BeginHorizontal();
					GUILayout.Button("Single", new GUILayoutOption[] { GUILayout.Width(100.0f), GUILayout.Height(32.0f) });
					GUILayout.Space(8.0f);
					GUILayout.Button("Left", "ButtonLeft", new GUILayoutOption[] { GUILayout.Width(80.0f), GUILayout.Height(32.0f) });
					GUILayout.Button("Middle", "ButtonMid", new GUILayoutOption[] { GUILayout.Width(80.0f), GUILayout.Height(32.0f) });
					GUILayout.Button("Right", "ButtonRight", new GUILayoutOption[] { GUILayout.Width(80.0f), GUILayout.Height(32.0f) });
				GUILayout.EndHorizontal();
				GUILayout.Space(8.0f);
				GUILayout.Label("Sample Toggle Button",  new GUILayoutOption[] { GUILayout.Width(180.0f) });
				GUILayout.Space(8.0f);
				GUILayout.BeginHorizontal();
					blackButtonToggle = GUILayout.Toggle(blackButtonToggle, "Single", "ButtonToggle", new GUILayoutOption[] { GUILayout.Width(100.0f), GUILayout.Height(32.0f) });
					GUILayout.Space(8.0f);
					blackButtonToggleLeft = GUILayout.Toggle(blackButtonToggleLeft, "Left", "ButtonToggleLeft", new GUILayoutOption[] { GUILayout.Width(80.0f), GUILayout.Height(32.0f) });
					blackButtonToggleMid = GUILayout.Toggle(blackButtonToggleMid, "Middle", "ButtonToggleMid", new GUILayoutOption[] { GUILayout.Width(80.0f), GUILayout.Height(32.0f) });
					blackButtonToggleRight = GUILayout.Toggle(blackButtonToggleRight, "Right", "ButtonToggleRight", new GUILayoutOption[] { GUILayout.Width(80.0f), GUILayout.Height(32.0f) });
				GUILayout.EndHorizontal();
				GUILayout.Space(8.0f);
				blackToggleChkBox = GUILayout.Toggle(blackToggleChkBox, "Sample Toggle Checkbox Style",  new GUILayoutOption[] { GUILayout.Width(240.0f), GUILayout.Height(28.0f) });
				blackToggleRadio = GUILayout.Toggle(blackToggleRadio, "Sample Toggle Radio Style", "ToggleRadio",  new GUILayoutOption[] { GUILayout.Width(240.0f), GUILayout.Height(28.0f) });
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
				GUILayout.Space(20.0f);
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
				GUILayout.Space(30.0f);
				GUILayout.Box("Sample Box Area\nSample Box Area\nSample Box Area", new GUILayoutOption[] { GUILayout.Width(350.0f), GUILayout.Height(120.0f) });
				GUILayout.Space(8.0f);
				GUILayout.Label("Sample Horizontal Slider",  new GUILayoutOption[] { GUILayout.Width(180.0f) });
				GUILayout.Space(8.0f);
				blackHorizontalSlide = GUILayout.HorizontalSlider(blackHorizontalSlide, 0.0f, 1.0f, new GUILayoutOption[] { GUILayout.Width(350.0f), GUILayout.Height(10.0f) });
				GUILayout.Space(8.0f);
				GUILayout.Label("Sample Vertical Slider",  new GUILayoutOption[] { GUILayout.Width(180.0f) });
				GUILayout.Space(8.0f);
				GUILayout.BeginHorizontal();
					GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(10.0f) });
					GUILayout.Space(10.0f);
					GUILayout.EndVertical();
					GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(300.0f) });
					blackVerticalSlide = GUILayout.VerticalSlider(blackVerticalSlide, 0.0f, 1.0f, new GUILayoutOption[] { GUILayout.Width(10.0f), GUILayout.Height(120.0f) });
					GUILayout.EndVertical();
				GUILayout.EndHorizontal();

				GUILayout.Space(24.0f);
				blackScroll = GUILayout.BeginScrollView(blackScroll, true, true, new GUILayoutOption[] { GUILayout.Width(320.0f), GUILayout.Height(180.0f) });
					GUILayout.Label("Sample Scroll View Area...", new GUILayoutOption[] { GUILayout.Width(420.0f) });
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
					GUILayout.Label("Sample Scroll View Area...");
				GUILayout.EndScrollView();

			GUILayout.EndVertical();
			
			GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}