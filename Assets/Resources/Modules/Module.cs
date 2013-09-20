using UnityEngine;
using System.Collections;

abstract public class Module : SceneAware<Scene>, Damageable {

	public ModuleStats stats;
	public bool isActive = true;

	[HideInInspector]
	public Ship ship;

	public GameObject visibleObj;
	public GameObject electricityObj;

	public static readonly EffectLabel[] explosions = {
		EffectLabel.Explode1,
		EffectLabel.Explode2,
		EffectLabel.Explode3,
		EffectLabel.Explode4,
		EffectLabel.Explode5,
		EffectLabel.Explode6,
		EffectLabel.Explode7,
		EffectLabel.Explode8,
		EffectLabel.Explode9,
		EffectLabel.Explode10
	};

	[HideInInspector]
	public float hitPointsLeft;

	public ModuleType GetModuleType() {
		return (ModuleType) GetType().GetField("type").GetRawConstantValue();
	}

	override protected void Start() {
		base.Start();
		hitPointsLeft = stats.hitPoints;
	}

	virtual protected void Update() {
		float hpPerc = hitPointsLeft / stats.hitPoints;

		// electricity
		if (electricityObj != null) {
			if (hpPerc < 0.25f) {
				electricityObj.SetActive(true);
			}
			else {
				electricityObj.SetActive(false);
			}
		}

		// boken animation
		if (visibleObj != null) {
			if (hpPerc < 0.5f) {
				visibleObj.animation.Play();
			}
			else {
				visibleObj.animation.Stop();
			}
		}

	}

	public float TakeDamage(float damage) {
//		Debug.Log(this + " taking damage: " + damage + " / " + hitPointsLeft);
		if (damage >= hitPointsLeft) {
			damage = hitPointsLeft;
			hitPointsLeft = 0f;
			Die();
		}
		else {
			hitPointsLeft -= damage;
		}
		return damage;
	}

	public float Repair(float toRepair) {
		float leftOver = toRepair - (stats.hitPoints - hitPointsLeft);
		if (leftOver > 0f) {
			hitPointsLeft = stats.hitPoints;
			return leftOver;
		}
		else {
			hitPointsLeft += toRepair;
			return 0f;
		}
	}

	protected void DeActivate() {
		isActive = false;
		visibleObj.SetActive(false);
		collider.enabled = false;
	}

	virtual protected void OnDestroy() {

	}

	public void Die() {

		// get random explosion prefab
		Effect prefab = game.GetEffectPrefab(Module.explosions[Random.Range(1, 10)]);

		// instantiate the effect
		Effect effect = Instantiate(prefab, transform.position + new Vector3(0f, 0f, -.51f), transform.rotation) as Effect;
		effect.transform.parent = transform;

		// deactivate the module
		DeActivate();

		// completely destroy the module after some time
		Destroy(gameObject, 3f);
	}

	public void SetContents(float contents) {
		if (this is BatteryConstructionModule) {
			((BatteryConstructionModule)this).energyLeft = contents;
		}
		else if (this is BatteryCombatModule) {
			((BatteryCombatModule)this).energyLeft = contents;
		}
		else if (this is CrewConstructionModule) {
			((CrewConstructionModule)this).crewLeft = (int)contents;
		}
		else if (this is CrewCombatModule) {
			((CrewCombatModule)this).crewLeft = (int)contents;
		}
		else if (this is FuelConstructionModule) {
			((FuelConstructionModule)this).fuelLeft = contents;
		}
		else if (this is FuelCombatModule) {
			((FuelCombatModule)this).fuelLeft = contents;
		}
	}

	public float GetContents() {
		if (this is BatteryConstructionModule) {
			return ((BatteryConstructionModule)this).energyLeft;
		}
		else if (this is BatteryCombatModule) {
			return ((BatteryCombatModule)this).energyLeft;
		}
		else if (this is CrewConstructionModule) {
			return ((CrewConstructionModule)this).crewLeft;
		}
		else if (this is CrewCombatModule) {
			return ((CrewCombatModule)this).crewLeft;
		}
		else if (this is FuelConstructionModule) {
			return ((FuelConstructionModule)this).fuelLeft;
		}
		else if (this is FuelCombatModule) {
			return ((FuelCombatModule)this).fuelLeft;
		}
		return 0f;
	}
	
}
