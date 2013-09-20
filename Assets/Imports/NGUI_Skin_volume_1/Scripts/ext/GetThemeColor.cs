using UnityEngine;
using System.Collections;


public class GetThemeColor : MonoBehaviour 
{
	
	public bool realTime = true;
	
	public static byte COLOR_R = 255;
	public static byte COLOR_G = 255;
	public static byte COLOR_B = 255;
	
	private UISprite spr;
	private UISlicedSprite sspr;
	
	// Use this for initialization
	void Start () 
	{
		this.spr = (UISprite)this.gameObject.GetComponent<UISprite>();
		this.sspr = (UISlicedSprite)this.gameObject.GetComponent<UISlicedSprite>();
		
		if (this.spr!=null) this.spr.color = new Color32(GetThemeColor.COLOR_R,GetThemeColor.COLOR_G,GetThemeColor.COLOR_B,255);
		if (this.sspr!=null) this.sspr.color = new Color32(GetThemeColor.COLOR_R,GetThemeColor.COLOR_G,GetThemeColor.COLOR_B,255);
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.realTime)
		{
			if (this.spr!=null) this.spr.color = new Color32(GetThemeColor.COLOR_R,GetThemeColor.COLOR_G,GetThemeColor.COLOR_B,255);
			if (this.sspr!=null) this.sspr.color = new Color32(GetThemeColor.COLOR_R,GetThemeColor.COLOR_G,GetThemeColor.COLOR_B,255);
		}
	}
	
	
	
}
