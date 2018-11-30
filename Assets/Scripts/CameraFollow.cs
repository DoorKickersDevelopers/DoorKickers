using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour {

	public GameObject c;

	public override void OnStartLocalPlayer() {
		c.SetActive(true);
	}
}
