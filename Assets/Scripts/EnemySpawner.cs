using UnityEngine.Networking;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour {

	public GameObject EnemyPrefab;

	public int NumberOfEnemy;
	// Use this for initialization

	public override void OnStartServer() {
		for (int i = 0; i < NumberOfEnemy; i++) {
			float x = Random.Range(-8.0f, 8.0f), z = Random.Range(-8.0f, 8.0f);
			float y = Random.Range(0, 180);
			var Pos = new Vector3(x, 0f, z);
			var Rot = Quaternion.Euler(0f, y, 0f);
			var Enemy = Instantiate(EnemyPrefab, Pos, Rot);
			NetworkServer.Spawn(Enemy);
		}
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
