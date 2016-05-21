using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaySceneCtr : MonoBehaviour
{
	
	static PlaySceneCtr g_Instance;

	public Camera camera;

	public Material baseMtl;
	public Material moveMtl;
	public GameObject rangeObj;
	
	public float rangeScale=5f;
	
	public PlayPanelCtr playPanelCtr;
	public GameResultCtr gameResultCtr;
	
	int score=0;
	bool failed=false;
	
	public static PlaySceneCtr Instance ()
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
	Vector3 camInitPot;
	Vector3 rangeObjInitPot;
	
	void Init ()
	{
		Vector3 rs=rangeObj.transform.localScale;
		rangeObj.transform.localScale=new Vector3(rangeScale,rs.y,rangeScale);
		
		baseObj = GameObject.CreatePrimitive (PrimitiveType.Cube) as GameObject;
	
		baseObj.GetComponent<Renderer>().material=baseMtl;
		
		camInitPot=camera.transform.position;
		rangeObjInitPot=rangeObj.transform.position;

		Reset();
		
	}
	
	void Reset(){
		_Start();

		baseObj.transform.localScale = Vector3.one;
		moveObj.transform.localScale = Vector3.one;
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
		camera.transform.position=camInitPot;
		newCamPot=camInitPot;
		rangeObj.transform.position=rangeObjInitPot;
		moveVScale=1f;
		prefectCount=0;
		combineExtend=Vector3.one;
		combineCenterPot=Vector3.zero;
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
	Vector3 combineCenterPot;
	Vector3 combineExtend=Vector3.one;


	void Combine ()
	{
		basePot = combineCenterPot;
		movePot = moveObj.transform.position;
		Vector3 dPot=movePot-basePot;
		dPot.y=0;
		float ss=dPot.sqrMagnitude;
		if (ss< 0.1f) {
			Debug.Log (" prefect");
			prefectCount++;
			moveV+=1f;
		}else if(ss<1f){
			moveV+=0.5f;
			prefectCount=0;
		}else if(ss>=1f){
			moveV=3f*moveVScale;
			prefectCount=0;
		}
		if(Mathf.Abs(dPot.x)>combineExtend.x ||Mathf.Abs(dPot.z)>combineExtend.z) {
			Fail();
			return;
		}
		Debug.Log(" prefectCount="+prefectCount);
		// new pot and extend
		Vector3 old_CombineExtend=combineExtend;
		combineExtend.x+=Mathf.Abs(dPot.x);
		combineExtend.z+=Mathf.Abs(dPot.z);
		combineExtend.y=1;
		combineCenterPot = (basePot + movePot) * 0.5f;

		//dice pot and extend
		Vector3 diceCenterPot=combineCenterPot;
		Vector3 diceExtend= old_CombineExtend-new Vector3(Mathf.Abs(dPot.x),0,Mathf.Abs(dPot.z));
		GameObject diceObj=CreateDicObj();
		diceCenterPot.y=1;
		diceObj.transform.position=diceCenterPot;
		diceObj.transform.localScale=diceExtend;
		Rigidbody rg=diceObj.AddComponent<Rigidbody>();
		Vector3 tmp=diceCenterPot;tmp.y-=1;

		rg.AddForce(Vector3.up*10,ForceMode.VelocityChange);
		if(Random.Range(0,2)==1) rg.AddForce(Vector3.right*10,ForceMode.VelocityChange);
		else rg.AddForce(Vector3.left*10,ForceMode.VelocityChange);
		rg.angularVelocity=Vector3.up*1;

		// move pot and extend
		Vector3 moveExtend=old_CombineExtend-diceExtend;
		if(dPot.x==0) moveExtend.x=old_CombineExtend.x;
		else if(dPot.z==0) moveExtend.z=old_CombineExtend.z;
		moveExtend.y=1;

		Vector3 moveCenterPot=movePot+mul(diceExtend*0.5f,dPot.normalized);
		moveObj.transform.position=moveCenterPot;
		moveObj.transform.localScale=moveExtend;


		
		if(prefectCount>=3){
			ScaleAll(0.9f);
		}



		move = true;
		moveByX=!moveByX;
		Vector3 newPot=combineCenterPot;
		newPot.y=1f;
		if(moveByX){
			this.minX=Mathf.Clamp( -combineExtend.x*2f,-rangeScale*0.6f,0);
			this.maxX=Mathf.Clamp(combineExtend.x*2f,0,rangeScale*0.6f);
			newPot.x=this.minX;
		}else{
			this.minZ=Mathf.Clamp( -combineExtend.z*2f,-rangeScale*0.6f,0);
			this.maxZ=Mathf.Clamp(combineExtend.z*2f,0,rangeScale*0.6f);
			newPot.z=this.minZ;
		}



		CreateMoveObj();
		moveObj.transform.position = newPot;
		moveObj.transform.localScale = combineExtend;
		
		CheckFail();

		move=true;


		float maxScale=Mathf.Max( combineExtend.x,combineExtend.z);

		newCamPot=camInitPot;
		newCamPot.x=combineCenterPot.x;
		newCamPot.z=combineCenterPot.z;
		newCamPot.y=camInitPot.y+ maxScale;


		Vector3 rangObjNewPot=rangeObj.transform.position;
		rangObjNewPot.x=combineCenterPot.x;
		rangObjNewPot.z=combineCenterPot.z;
		rangeObj.transform.position=rangObjNewPot;

		moveVScale=(2*maxScale+7)/9f;
	


	
	}


	int prefectCount=0;
	public GameObject scaleCenter;

void ScaleAll(float scale){
		scaleCenter.transform.localScale=Vector3.one;
		scaleCenter.transform.position=combineCenterPot;
		for(int i=0;i<moveObjs.Count;i++){
			if(moveObjs[i]!=null ){
				moveObjs[i].transform.parent=scaleCenter.transform;
			}
		}
		baseObj.transform.parent=scaleCenter.transform;
		Vector3 s=Vector3.one*scale;s.y=1;
		scaleCenter.transform.localScale=s;
		for(int i=0;i<moveObjs.Count;i++){
			if(moveObjs[i]!=null ){
				moveObjs[i].transform.parent=null;
			}
		}
		baseObj.transform.parent=null;

		combineExtend*=scale;

}

	float moveVScale=1f;
	Vector3 newCamPot;

	void LateUpdate(){
		Vector3 camPot=Vector3.Lerp(camera.transform.position,newCamPot,Time.deltaTime);
		
		camera.transform.position=camPot;
		for(int i=0;i<diceObjs.Count;i++){
			if(diceObjs[i]!=null && diceObjs[i].transform.position.y<-200) diceObjs[i].SetActive(false);
		}
	}



	Vector3 mul(Vector3 v1,Vector3 v2){
		return new Vector3(v1.x*v2.x,v1.y*v2.y,v1.z*v2.z);
	}

	List<GameObject> moveObjs=new List<GameObject>();
	List<GameObject> diceObjs=new List<GameObject>();


	void _Start(){
		Clear();
		CreateMoveObj();
		combineCenterPot=baseObj.transform.position;
		baseObj.transform.localScale=Vector3.one;
		combineExtend=baseObj.transform.localScale;
	}

	Color curMoveObjColor;
	void CreateMoveObj(){
		moveObj = GameObject.CreatePrimitive (PrimitiveType.Cube) as GameObject;
		moveObj.GetComponent<Renderer>().material=moveMtl;
		moveObjs.Add(moveObj);
		curMoveObjColor=new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0.5f,1f));
		moveObj.GetComponent<Renderer>().material.SetColor("_Color",curMoveObjColor);
		moveObj.GetComponent<BoxCollider>().isTrigger=true;
	}

	GameObject CreateDicObj(){
		GameObject 	diceObj = GameObject.CreatePrimitive (PrimitiveType.Cube) as GameObject;
		diceObj.GetComponent<Renderer>().material=moveMtl;
		diceObj.GetComponent<Renderer>().material.SetColor("_Color",curMoveObjColor);
		diceObjs.Add(diceObj);
		return diceObj;
	}

	void Clear(){
		for(int i=0;i<moveObjs.Count;i++){
			GameObject.DestroyImmediate(moveObjs[i]);
				moveObjs[i]=null;
		}

		for(int i=0;i<diceObjs.Count;i++){
			GameObject.DestroyImmediate(diceObjs[i]);
			diceObjs[i]=null;
		}
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
			vy += Time.deltaTime * 19.8f;
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
		float minx=basePot.x-combineExtend.x*0.5f;
		float maxx=basePot.x+combineExtend.x*0.5f;
		float minz=basePot.z-combineExtend.z*0.5f;
		float maxz=basePot.z+combineExtend.z*0.5f;
		float re=rangeScale*0.5f;
		
//		if(minx<-re || maxx>re || minz<-re || maxz >re){
		if(combineExtend.x>rangeScale || combineExtend.z>rangeScale){
			Debug.Log(" on fail");
			Fail();
		}else{
			score++;
			playPanelCtr.UpdateScore(score);
		}
	}
	
	
	public void OnRestart(){
		Reset();
	}

	void Fail(){
		gameResultCtr.gameObject.SetActive(true);
		failed=true;
	}
	
}
