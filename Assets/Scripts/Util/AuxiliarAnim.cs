using UnityEngine;
using System.Collections;

public class AuxiliarAnim : MonoBehaviour {

	private bool onMove;
	private bool onScale;
	private bool onFade;
	private bool onColorChange;

	private Vector3 toPosition;
	private Vector3 toScale;
	private float toAlpha;
	private Color toColor;
	private Transform t;
	private float timeToMove;
	private float timeToScale;
	private float timeToFade;
	private float timeToColorChange;
	private SpriteRenderer sr;

	private Vector3 anteriorPosition;
	private Vector3 anteriorScale;
	private Color anteriorColor;
	private float anteriorAlpha;

	void Awake(){
		onMove = false;
		onScale = false;
		onFade = false;
		toPosition = new Vector3 (0, 0, 0);
		t = transform;
		timeToMove = 1;
		timeToScale = 1;
		timeToFade = 1;
		timeToColorChange = 1;
		sr = GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (onMove)
			move ();
		if (onScale)
			scale ();
		if (onFade)
			fade ();
		if (onColorChange)
			colorChange ();

	}

	private void move(){

		if (t.position.x == toPosition.x && t.position.y == toPosition.y && t.position.z == toPosition.z) {
			onMove = false;
			return;
		}
		
		float moveX = 0;
		float moveY = 0;
		float moveZ = 0;

		Quaternion q = t.rotation;
		
		t.rotation = new Quaternion (0,0,0,1);
		
		if(t.position.x != toPosition.x)
			moveX = (toPosition.x - anteriorPosition.x)*Time.deltaTime/timeToMove;
		if(t.position.y != toPosition.y)
			moveY = (toPosition.y - anteriorPosition.y)*Time.deltaTime/timeToMove;
		if(t.position.z != toPosition.z)
			moveZ = (toPosition.z - anteriorPosition.z)*Time.deltaTime/timeToMove;

		if((moveX > 0 && t.position.x > toPosition.x) || (moveX < 0 && t.position.x < toPosition.x)){
			t.position = new Vector3(toPosition.x, t.position.y, t.position.z);
			moveX = 0;
		}

		if((moveY > 0 && t.position.y > toPosition.y) || (moveY < 0 && t.position.y < toPosition.y)){
			t.position = new Vector3(t.position.x, toPosition.y, t.position.z);
			moveY = 0;
		}

		if((moveZ > 0 && t.position.z > toPosition.z) || (moveZ < 0 && t.position.z < toPosition.z)){
			t.position = new Vector3(t.position.x, t.position.y, toPosition.z);
			moveZ = 0;
		}

		t.Translate (moveX, moveY, moveZ);
		
		t.rotation = q;
	}

	private void scale(){

		if (t.localScale.x == toScale.x && t.localScale.y == toScale.y && t.localScale.z == toScale.z) {
			onScale = false;
		}
		
		float scaleX = 0;
		float scaleY = 0;
		float scaleZ = 0;

		if(t.localScale.x != toScale.x)
		scaleX = (toScale.x - anteriorScale.x)*Time.deltaTime/timeToScale;
		if(t.localScale.y != toScale.y)
		scaleY = (toScale.y - anteriorScale.y)*Time.deltaTime/timeToScale;
		if(t.localScale.z != toScale.z)
		scaleZ = (toScale.z - anteriorScale.z)*Time.deltaTime/timeToScale;
		
		if((scaleX > 0 && t.localScale.x > toScale.x) || (scaleX < 0 && t.localScale.x < toScale.x)){
			t.localScale = new Vector3(toScale.x, t.localScale.y, t.localScale.z);
			scaleX = 0;
		}
		
		if((scaleY > 0 && t.localScale.y > toScale.y) || (scaleY < 0 && t.localScale.y < toScale.y)){
			t.localScale = new Vector3(t.localScale.x, toScale.y, t.localScale.z);
			scaleY = 0;
		}
		
		if((scaleZ > 0 && t.localScale.z > toScale.z) || (scaleZ < 0 && t.localScale.z < toScale.z)){
			t.localScale = new Vector3(t.localScale.x, t.localScale.y, toScale.z);
			scaleZ = 0;
		}
		
		t.localScale = new Vector3 (t.localScale.x+scaleX, t.localScale.y+scaleY, t.localScale.z+scaleZ);

	}

	private void fade(){
		if (sr.material.color.a == toAlpha)
			onFade = false;
		
		float fadeValue = 0;

		if(sr.material.color.a != toAlpha)
			fadeValue = (toAlpha - anteriorAlpha)*Time.deltaTime/timeToFade;
		
		if((fadeValue > 0 && sr.material.color.a > toAlpha) || (fadeValue < 0 && sr.material.color.a < toAlpha)){
			sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, toAlpha);
			fadeValue = 0;
		}
		
		sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, sr.material.color.a + fadeValue);
	}

	private void colorChange(){
		
	}

	public void moveTo(Vector3 pos, float time){
		toPosition = pos;
		onMove = true;
		timeToMove = Mathf.Abs(time);
		anteriorPosition = t.position;
	}

	public void scaleTo(Vector3 scale, float time){
		toScale = scale;
		onScale = true;
		timeToScale = Mathf.Abs(time);
		anteriorScale = t.localScale;
	}

	public void fadeTo(float alpha, float time){
		toAlpha = alpha;
		onFade = true;
		timeToFade = Mathf.Abs(time);
		anteriorAlpha = sr.material.color.a;
	}

	public void colorTo(Color color, float time){
		toColor = color;
		onColorChange = true;
		timeToColorChange = Mathf.Abs(time);
		anteriorColor = sr.material.color;
	}

	//Gettes and setters
	public bool isOnMove(){
		return onMove;
	}
	public bool isOnScale(){
		return onScale;
	}
	public bool isOnFade(){
		return onFade;
	}
	public bool isOnColorChange(){
		return onColorChange;
	}

}
