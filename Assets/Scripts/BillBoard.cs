using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour {

	// Use this for initialization
	public Transform c;
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(c);
	}
}
