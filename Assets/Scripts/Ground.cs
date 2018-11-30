using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;

public class Ground : NetworkBehaviour {

	// Use this for initialization
	public GameObject NuclearPrefab;
	public GameObject WallPrefab;

	public int EdgeWidth;
	//场地周围一圈空地的宽度
	
	public int DoorWidth;
	//门的宽度

	public int MinRoom;
	//房间的最小宽度

	public int space;
	public float p;
	//以概率p将space * space 大小一块区域中所有墙移除
	
	
	private int width;
	private int height;
	
	private bool[,] square;
	private Vector3 offset;	
	
	
	public override void OnStartServer() {

		float size_x = gameObject.GetComponent<Collider>().bounds.size.x;
		float scale_x = gameObject.GetComponent<Transform>().localScale.x;
		float size_z = gameObject.GetComponent<Collider>().bounds.size.z;
		float scale_z = gameObject.GetComponent<Transform>().localScale.z;
		
		width = (int)(size_x * scale_x);
		height = (int) (size_z * scale_z);
		offset = transform.position;
		width -= 2 * EdgeWidth;
		height -= 2 * EdgeWidth;
		offset += new Vector3(EdgeWidth,0,EdgeWidth);
		
		square = new bool[width+1, height+1];
		DrawWall();
		
		
		DrawLine(1, 1, width - 1, height - 1);
		
		for(int i = 1; i <= width - space; i++){
			for(int j = 1; j <= height - space; j++){
				if(Random.value < p){
					for(int k = i; k <= i+space-1; k++){
						for(int l = j; l <= j+space-1; l++){
							square[k, l] = false;
						}
					}
				}
			}
		}
		int x = Random.Range(1, width );
		int y = Random.Range(1, height );
		while(square[x, y]){
			x = Random.Range(1, width );
			y = Random.Range(1, height );
		}
		var Nuclear = Instantiate(NuclearPrefab, (new Vector3(x,0,y))+offset, Quaternion.Euler(0f,0f,0f));
		Debug.Log(Nuclear.transform.position);
		NetworkServer.Spawn(Nuclear);
		for(int i = 0; i <= width; i++){
			for(int j = 0; j <= height; j++){
				if(square[i, j]) {
					var Wall = Instantiate(WallPrefab, (new Vector3(i, 0, j)) + offset,
						new Quaternion(0, 0, 0, 0));
					NetworkServer.Spawn(Wall);
				}
			}
		}
	}
	
	void DrawWall(){
		for(int i = 0; i <= width; i++){
			for(int j = 0; j <= height; j++){
				if(i == 0 || j == 0 || i == width || j == height){
					square[i, j] = true;
				}
				else
					square[i, j] = false;
			}
		}

		int d = Random.Range(1, width - DoorWidth + 1);
		for (int i = d; i <= d + DoorWidth - 1; i++)
			square[i, 0] = false;
		d = Random.Range(1, width - DoorWidth + 1);
		for (int i = d; i <= d + DoorWidth - 1; i++)
			square[i, height] = false;
		d = Random.Range(1, height - DoorWidth + 1);
		for (int j = d; j <= d + DoorWidth - 1; j++)
			square[0, j] = false;
		d = Random.Range(1, height - DoorWidth + 1);
		for (int j = d; j <= d + DoorWidth - 1; j++)
			square[width, j] = false;

	}
	
	void DrawLine(int x1, int y1, int x2, int y2){
		int w = x2 - x1 + 1;
		int h = y2 - y1 + 1;
		if(w > h){
			if(w > 2 * MinRoom){
				int x = Random.Range(x1 + MinRoom, x2 - MinRoom+1);
				for(int i = y1; i <= y2; i++){
					square[x, i] = true;
				}

				int d = Random.Range(y1, y2 + 1 - DoorWidth);

				for (int i = d; i <= d + DoorWidth; i++) {
					square[x, i] = true;
				}
				DrawLine(x1, y1, x-1, y2);
				DrawLine(x+1, y1, x2, y2);
			}
		}
		else{
			if(h > 2 * MinRoom){
				int y = Random.Range(y1 + MinRoom, y2 - MinRoom+1);
				for(int i = x1; i <= x2; i++){
					square[i, y] = true;
				}

				int d = Random.Range(x1, x2 + 1 - DoorWidth);
				for (int i = d; i <= d + DoorWidth; i++) {
					square[i, y] = true;
				}
				DrawLine(x1, y1, x2, y-1);
				DrawLine(x1, y+1, x2, y2);
			}
		}
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}