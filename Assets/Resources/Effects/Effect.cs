using UnityEngine;

public class Effect : MonoBehaviour {

	public float destroyTime = 5;

	public EffectLabel GetEffectType() {
		return (EffectLabel) GetType().GetField("type").GetRawConstantValue();
	}

	void Start() {
		Destroy(gameObject, destroyTime);
	}

	public void Fade(float time) {
		NcCurveAnimation.NcInfoCurve curve = new NcCurveAnimation.NcInfoCurve();
		curve.m_ApplyType = NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MATERIAL_COLOR;
		curve.m_ToColor = new Vector4(0f, 0f, 0f, 0f);
		curve.m_bRecursively = true;
		curve.m_bApplyOption = new bool[] {false, false, false, true};
		curve.m_AniCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		var anim = gameObject.AddComponent<NcCurveAnimation>();
		anim.AddCurveInfo(curve);
		anim.m_fDurationTime = time;
	}

	public void SetRange(float range) {
		transform.localScale = new Vector3(transform.localScale.x, range, transform.localScale.y);
	}

}