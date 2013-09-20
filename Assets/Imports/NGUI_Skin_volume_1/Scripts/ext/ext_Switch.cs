using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ext_Switch : MonoBehaviour 
{
	
	public string leftText = "Off";
	public string rightText = "On";
	public bool state=false;
	
	public UILabel lbl_left;
	public UILabel lbl_right;
	public UISprite thumb;
	
	
	// Use this for initialization
	void Start () 
	{
		
		this.thumb = (UISprite)this.gameObject.GetComponentInChildren<UISprite>();
		this.thumb.gameObject.GetComponent<ext_SwitchButtonEvent>().SetState(this.state);	
	}
	
	
	// Update is called once per frame
	void Update () 
	{
#if UNITY_EDITOR
		this.lbl_left.text = leftText;
		this.lbl_right.text = rightText;
		this.thumb.gameObject.GetComponent<ext_SwitchButtonEvent>().SetState(this.state);	
#endif	
	}
	
}
