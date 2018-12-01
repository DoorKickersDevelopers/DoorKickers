using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Experimental.PlayerLoop;
using Random = UnityEngine.Random;

public class Ground : NetworkBehaviour {

	// Use this for initialization
	public GameObject NuclearPrefab;
	public GameObject WallPrefab;

	public int EdgeWidth;
	//场地周围一圈空地的宽度

	public int CenterIntensity;
	//生成的核弹在地图中心的强度，使用该值表示地图离边缘该宽度处不会产生核弹
	
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
	private bool[,] color;
	
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
		
		square = new bool[width + 1, height + 1];
		color = new bool[width + 1, height + 1]; 
		
		BuildMap();
		while (!LegalMap()) BuildMap();
		
		int x = Random.Range(CenterIntensity, width+1-CenterIntensity);
		int y = Random.Range(CenterIntensity, height+1-CenterIntensity);
		while(square[x, y]){
			x = Random.Range(CenterIntensity, width+1-CenterIntensity);
			y = Random.Range(CenterIntensity, height+1-CenterIntensity);
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

	void dfs(int x, int y) {
		color[x,y] = true;
		if (x > 0 && !color[x - 1, y]) dfs(x - 1, y);
		if (x < width && !color[x + 1, y]) dfs(x + 1, y);
		if (y > 0 && !color[x, y - 1]) dfs(x, y - 1);
		if (y < height && !color[x, y + 1]) dfs(x, y + 1);
	}
	
	bool LegalMap() {
		for(int i = 0; i <= width; i++)
			for (int j = 0; j <= height; j++)
				color[i, j] = false;
		for(int i=0;i<=width;i++)
			if (!square[i, 0]) {
				dfs(i,0);
				break;
			}
		for(int i=0;i<=width;i++)
			for(int j=0;j<=height;j++)
				if (!square[i, j] && !color[i, j])
					return false;
		return true;
	}
	
	void BuildMap() {
		Clear();
		
		
		DrawLine(0, 0, width , height );
		
		for(int i = 0; i <= width - space + 1; i++){
			for(int j = 0; j <= height - space + 1; j++){
				if(Random.value < p){
					for(int k = i; k <= i+space-1; k++){
						for(int l = j; l <= j+space-1; l++){
							square[k, l] = false;
						}
					}
				}
			}
		}
	}
	
	void Clear(){
		for(int i = 0; i <= width; i++)
			for(int j = 0; j <= height; j++)
					square[i, j] = false;
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