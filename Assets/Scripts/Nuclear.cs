using UnityEngine;
using UnityEngine.Networking;

public class Nuclear : NetworkBehaviour {
	private Vector3 OriginPos;
	private Quaternion OriginRot;
	private Transform Human;

	private const float BackDistance = 0.0f;

	private const float UpDistance = 2.5f;
	
	// Use this for initialization
	void Start () {
		OriginPos = transform.position;
		OriginRot = transform.rotation;
		Human = null;
	}

	void LateUpdate() {
		if (Human == null) {
			transform.position = OriginPos;
			transform.rotation = OriginRot;
			return;
		}
		else {
			Vector3 offset = -Human.forward * BackDistance + Human.up * UpDistance;
			transform.position = Human.position + offset;
			transform.rotation = Human.rotation;
		}
	}

	private void OnCollisionEnter (Collision other) {
		Debug.Log("A Collider happened!");
		if (Human != null) {
			return;
		}
		if (other.gameObject.CompareTag("Human")) {
			Human = other.transform;
		}
	}


	// Update is called once per frame
	void Update () {
		
	}
}
