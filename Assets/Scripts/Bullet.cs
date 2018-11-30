using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public const int Damage = 20;

	private void OnCollisionEnter(Collision collision) {
		Debug.Log("Get You!");
		var HitObject = collision.gameObject;
		var health = HitObject.GetComponent<Health>();
		if (health != null) {
			health.GetDamage(Damage);
		}
		Destroy(gameObject);
	}
}
