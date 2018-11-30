using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	public const int MaxHp = 100;
	[SyncVar(hook = "OnChangeHp")]
	private int Hp = MaxHp;

	public int Ident;
	public RectTransform HealthBar;
//	private NetworkStartPosition[] SpawnPoints;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GetDamage(int amount) {
		if (!isServer)
			return;
		Hp -= amount;
		//HealthBar.sizeDelta = new Vector2(Hp, HealthBar.sizeDelta.y);
		if (Hp <= 0) {
			Debug.Log("Dead");
			Destroy(gameObject);
		/*	if (Ident == 1) {
				Hp = MaxHp;
				RpcRespawn();	
			}
			else {
				Destroy(gameObject);
			}*/
			
		}
		
	}

	void OnChangeHp(int hp) {
		HealthBar.sizeDelta = new Vector2(hp, HealthBar.sizeDelta.y);
	}
	
/*	[ClientRpc]
	void RpcRespawn(){
	    if(isLocalPlayer) {
		    Vector3 SpawnPoint = Vector3.zero;
		    if (SpawnPoints != null && SpawnPoints.Length > 0) {
			    SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
		    }
	        transform.position = SpawnPoint;
	    }
	}*/
}
