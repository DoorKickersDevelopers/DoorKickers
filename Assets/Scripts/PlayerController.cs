using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerController : NetworkBehaviour {

	public float RotateSpeed = 150f;
	public float MoveSpeed = 3f;
	public GameObject BulletPrefab;
	public GameObject GrenadePrefab;
	public Transform BulletSpawn;
	public bool IsLocalObject=false;
	// Use this for initialization
	void Start () {
		// TODO Add something in the near future	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			PlayerMove();
			if (Input.GetKeyDown(KeyCode.Space)) {
				CmdShoot();
			}

			if (Input.GetKeyDown(KeyCode.E)) {
				CmdThrow();
			}
		}
	}
	[Command]
	void CmdShoot() {
		var Bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
		Bullet.GetComponent<Rigidbody>().velocity = Bullet.transform.forward * 15;
		NetworkServer.Spawn(Bullet);
		Destroy(Bullet, 1.0f);
	}

	[Command]
	void CmdThrow() {
		var grenade = Instantiate(GrenadePrefab, BulletSpawn.position, BulletSpawn.rotation);
		grenade.GetComponent<Rigidbody>().velocity = grenade.transform.forward * 10;
		NetworkServer.Spawn(grenade);
	}
	void PlayerMove() {
		float x, z;
		x = Input.GetAxis("Horizontal") * Time.deltaTime * RotateSpeed;
		z = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;
		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
		
	}

	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}
}
