using UnityEngine;
using System.Collections;


public class ext_SwitchButtonEvent : MonoBehaviour 
{
	public float xOffset = 25;
	[HideInInspector] public bool state = false;
	[HideInInspector] public TweenPosition tweenPos;
	
//	private Vector3 left_pos;
//	private Vector3 right_pos;
	
	void Start()
	{
		this.tweenPos  = this.gameObject.GetComponent<TweenPosition>();	
		this.tweenPos.Reset();
	
//		this.left_pos = new Vector3(-xOffset,0f,0f);
//		this.right_pos = new Vector3(xOffset,0f,0f);
		
		//this.tweenPos.from = this.left_pos;
		//this.tweenPos.to = this.right_pos;
	}
	
	// Use this for initialization
	void OnClick () 
	{
		this.state = !this.state;		
		this.tweenPos.Play(state);
		
		this.transform.parent.gameObject.GetComponent<ext_Switch>().state = this.state;
	}
	
	
	public void SetState(bool s)
	{
		this.state = s;	
		this.tweenPos.Play(state);
	}
	
	
	
}
