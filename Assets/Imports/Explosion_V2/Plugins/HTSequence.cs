// HTSequence v1.0 (Aout 2012)
// HTSequence.cs library is copyright (c) of Hedgehog Team
// Please send feedback or bug reports to the.hedgehog.team@gmail.com
using UnityEngine;
using System.Collections;

[System.Serializable]
public class HTSequence {
	
	[SerializeField]
	public GameObject spriteSheet;
	[SerializeField]
	public Vector3 offset;
	[SerializeField]
	public float waittingTime;
	[SerializeField]
	public bool foldOut=true;
	[SerializeField]
	public Color color;
	[SerializeField]
	public GameObject spriteSheetRef;
	[SerializeField]
	public GameObject oldSpriteSheetRef;
	[SerializeField]
	public bool play=false;

}
