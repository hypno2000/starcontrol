using UnityEngine;
using System.Collections;


[AddComponentMenu("NGUI/Widget/Event/Switch Activate")]
public class ext_SwitchActivateEvent : MonoBehaviour 
{

	public GameObject target;
    

    void OnClick () 
	{ 
		
		if (target != null) 
		{
			NGUITools.SetActive(target, this.transform.parent.gameObject.GetComponent<ext_Switch>().state); 
		}
	}

	
}
