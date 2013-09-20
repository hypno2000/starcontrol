using UnityEngine;
using System.Collections;

public class SetThemeColor : MonoBehaviour 
{

	public void SetR(float val)
	{
		GetThemeColor.COLOR_R = (byte)Mathf.RoundToInt(255*val);
	}
	
	public void SetG(float val)
	{
		GetThemeColor.COLOR_G = (byte)Mathf.RoundToInt(255*val);
	}
	
	public void SetB(float val)
	{
		GetThemeColor.COLOR_B = (byte)Mathf.RoundToInt(255*val);
	}
	
}
