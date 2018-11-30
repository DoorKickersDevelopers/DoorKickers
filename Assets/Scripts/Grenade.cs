using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	private const int Damage = 80;
	private const float LifeSpan = 1.5f;
	private const float Range = 7f;
	// Use this for initialization
	void Start () {
		Invoke("Bomb",LifeSpan);
		Destroy(gameObject,LifeSpan);
	}
		
	void Bomb() {

		Collider[] others = Physics.OverlapSphere(transform.position, Range);

		foreach (Collider c in others) {
			var health = c.gameObject.GetComponent<Health>();
			if (health != null) {
				health.GetDamage(Damage);
			}
		}
	}
}
