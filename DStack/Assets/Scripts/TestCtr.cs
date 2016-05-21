using UnityEngine;
using System.Collections;

public class TestCtr : MonoBehaviour
{

	static TestCtr g_Instance;

	public Material baseMtl;
	public Material moveMtl;
	public GameObject rangeObj;

	public float rangeScale=5f;

	public PlayPanelCtr playPanelCtr;
	public GameResultCtr gameResultCtr;

	int score=0;
	bool failed=false;

	public static TestCtr Instance ()
	{
		return g_Instance;
	}

	void Awake ()
	{
		g_Instance = this;
	}


	// Use this for initialization
	void Start ()
	{
		Init ();
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(failed) return;
		if (move)
			UpdateMoveCube ();
		UpdateFallDown ();
	}

	GameObject baseObj, moveObj;
	Vector2 baseSize;

	void Init ()
	{
		Vector3 rs=rangeObj.transform.localScale;
		rangeObj.transform.localScale=new Vector3(rangeScale,rs.y,rangeScale);

		baseObj = GameObject.CreatePrimitive (PrimitiveType.Cube) as GameObject;
		moveObj = GameObject.CreatePrimitive (PrimitiveType.Cube) as GameObject;

		baseObj.GetComponent<Renderer>().material=baseMtl;
		moveObj.GetComponent<Renderer>().material=moveMtl;

		Reset();


	}

	void Reset(){
		curScaleX=curScaleZ=1f;
		baseObj.transform.localScale = new Vector3 (1, 1, 1);
		moveObj.transform.localScale = new Vector3 (1, 1, 1);
		baseObj.transform.position = Vector3.zero;
		moveObj.transform.position = new Vector3 (0, 1f, 0);
		minX = -5;
		maxX = 5;
		minZ = -5;
		maxZ = 5;
		 moveByX = true;
		 down = true;
		 move = true;
		 falldown = false;
		score=0;
		playPanelCtr.UpdateScore(score);
		failed=false;
		moveV=3f;
	}

	void UpdateMoveCube ()
	{
		Vector3 pot ;
		if (moveByX) {
		pot= moveObj.transform.position;
			if (down) {
				if (pot.x > minX) {
					pot.x -= Time.deltaTime * moveV;
				} else {
					down = false;
				}
			} else {
				if (pot.x < maxX) {
					pot.x += Time.deltaTime * moveV;
				} else {
					down = true;
				}
			}

		}else{

			 pot = moveObj.transform.position;
			if (down) {
				if (pot.z > minZ) {
					pot.z -= Time.deltaTime * moveV;
				} else {
					down = false;
				}
			} else {
				if (pot.z < maxZ) {
					pot.z += Time.deltaTime * moveV;
				} else {
					down = true;
				}
			}
		}
		moveObj.transform.position = pot;
	}



	float minX = -5, maxX = 5;
	float minZ = -5, maxZ = 5;
	bool moveByX = true;
	bool down = true;
	bool move = true;
	bool falldown = false;
	float moveV=2f;

	void CreateCube ()
	{

	}

	void Play ()
	{



	}

	Vector3 basePot;
	Vector3 movePot;
	float curScaleX = 1f;
	float curScaleZ = 1f;

//	void Combine ()
//	{
//		basePot = baseObj.transform.position;
//		movePot = moveObj.transform.position;
//		float dx = basePot.x - movePot.x;
//		float dz = basePot.z - movePot.z;
//
//		float eS=dx*dx+dz*dz;
//		if (eS< 0.01f) {
//			Debug.Log (" prefect");
//			moveV+=1f;
//		}else if(eS<1f){
//			moveV+=0.5f;
//		}else if(eS>=1f){
//			moveV=3f;
//		}
//
//
//		Vector3 centerPot = (basePot + movePot) * 0.5f;
//		centerPot.y = 0;
//
//		float minx = basePot.x < movePot.x ? basePot.x : movePot.x;
//		float maxx = basePot.x < movePot.x ? movePot.x : basePot.x;
//		float minz = basePot.z < movePot.z ? basePot.z : movePot.z;
//		float maxz = basePot.z < movePot.z ? movePot.z : basePot.z;
//
//		curScaleX = maxx - minx + curScaleX;
//		curScaleZ = maxz - minz + curScaleZ;
//
//		baseObj.transform.position = centerPot;
//		baseObj.transform.localScale = new Vector3 (curScaleX, 1f, curScaleZ);
//
//		move = true;
//		moveByX=!moveByX;
//		centerPot.y=1f;
//		if(moveByX){
//			this.minX=Mathf.Clamp( -curScaleX*2f,-rangeScale*0.6f,0);
//			this.maxX=Mathf.Clamp(curScaleX*2f,0,rangeScale*0.6f);
//			centerPot.x=this.minX;
//		}else{
//			this.minZ=Mathf.Clamp( -curScaleZ*2f,-rangeScale*0.6f,0);
//			this.maxZ=Mathf.Clamp(curScaleZ*2f,0,rangeScale*0.6f);
//			centerPot.z=this.minZ;
//		}
//		moveObj.transform.position = centerPot;
//		moveObj.transform.localScale = new Vector3 (curScaleX, 1f, curScaleZ);
//	
//		CheckFail();
//	}

	void Combine ()
	{
		basePot = baseObj.transform.position;
		movePot = moveObj.transform.position;
		float dx = basePot.x - movePot.x;
		float dz = basePot.z - movePot.z;
		
		float eS=dx*dx+dz*dz;
		if (eS< 0.01f) {
			Debug.Log (" prefect");
			moveV+=1f;
		}else if(eS<1f){
			moveV+=0.5f;
		}else if(eS>=1f){
			moveV=3f;
		}
		
		
		Vector3 centerPot = (basePot + movePot) * 0.5f;
		centerPot.y = 0;
		
		float minx = basePot.x < movePot.x ? basePot.x : movePot.x;
		float maxx = basePot.x < movePot.x ? movePot.x : basePot.x;
		float minz = basePot.z < movePot.z ? basePot.z : movePot.z;
		float maxz = basePot.z < movePot.z ? movePot.z : basePot.z;
		
		curScaleX = maxx - minx + curScaleX;
		curScaleZ = maxz - minz + curScaleZ;
		
		baseObj.transform.position = centerPot;
		baseObj.transform.localScale = new Vector3 (curScaleX, 1f, curScaleZ);
		
		move = true;
		moveByX=!moveByX;
		centerPot.y=1f;
		if(moveByX){
			this.minX=Mathf.Clamp( -curScaleX*2f,-rangeScale*0.6f,0);
			this.maxX=Mathf.Clamp(curScaleX*2f,0,rangeScale*0.6f);
			centerPot.x=this.minX;
		}else{
			this.minZ=Mathf.Clamp( -curScaleZ*2f,-rangeScale*0.6f,0);
			this.maxZ=Mathf.Clamp(curScaleZ*2f,0,rangeScale*0.6f);
			centerPot.z=this.minZ;
		}
		moveObj.transform.position = centerPot;
		moveObj.transform.localScale = new Vector3 (curScaleX, 1f, curScaleZ);
		
		CheckFail();
	}


	public void MoveCubeFallDown ()
	{
		falldown = true;
		move = false;
		vy = 0;
	}

	float vy = 0;

	void UpdateFallDown ()
	{
		if (falldown) {
			Vector3 pot = moveObj.transform.position;
			vy += Time.deltaTime * 9.8f;
			pot.y -= vy * Time.deltaTime;
			if (pot.y <= 0) {
				falldown = false;
				pot.y = 0;

			}
			moveObj.transform.position = pot;

			if (!falldown)
				Combine ();

		}
	}

	public void CheckFail(){

		basePot=baseObj.transform.position;
		float minx=basePot.x-curScaleX*0.5f;
		float maxx=basePot.x+curScaleX*0.5f;
		float minz=basePot.z-curScaleZ*0.5f;
		float maxz=basePot.z+curScaleZ*0.5f;
		float re=rangeScale*0.5f;

		if(minx<-re || maxx>re || minz<-re || maxz >re){
			Debug.Log(" on fail");
			gameResultCtr.gameObject.SetActive(true);
			failed=true;
		}else{
			score++;
			playPanelCtr.UpdateScore(score);
		}
	}


	public void OnRestart(){
		Reset();
	}

}
