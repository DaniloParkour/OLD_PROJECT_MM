using UnityEngine;
using System.Collections;

public class Order : MonoBehaviour {

	private bool choosed;
	private Vector3 initialPosition;
	private Vector3 initialScale;
	private Transform t;
	private AuxiliarAnim anim;
	private SpriteRenderer sr;
	private int orderValue;

	private bool onDesapear;
	private bool onShowMe;
	private bool onZoom;
	private bool numsInitiateds;
	
	void Awake () {
		t = transform;
		initialPosition = t.position;
		initialScale = t.localScale;
		anim = GetComponent<AuxiliarAnim> ();
		choosed = false;
		onDesapear = false;
		onShowMe = false;
		onZoom = false;
		sr = GetComponent<SpriteRenderer> ();
		orderValue = 0;
	}

	// Use this for initialization
	void Start(){

	}
	
	// Update is called once per frame
	void Update () {
		if(!numsInitiateds && sr.material.color.a >= 0.9f){
			for(int i = 0; i < transform.childCount; i++){
				SpriteRenderer sr_c = transform.GetChild(i).GetComponent<SpriteRenderer>();
				if(sr_c != null){
					sr_c.material.color = new Color(1,1,1,1);
				}
				transform.GetChild(i).transform.localScale = new Vector3(1,1,1);
				transform.GetChild(i).transform.localPosition = new Vector3(((i-transform.childCount/2)*0.25f),0,0);
				if(transform.childCount % 2 == 0)
					transform.GetChild(i).transform.Translate(0.125f,0,0);
			}
		}
	}

	public void backToInit(){
		anim.moveTo (initialPosition, 0.5f);
		anim.scaleTo (initialScale, 0.5f);
	}

	public void goToOperation(Vector3 pos){
		anim.moveTo (pos, 0.5f);
		anim.scaleTo (new Vector3(1.3f,1,1), 0.5f);
	}

	public void goToOperation(Vector3 pos, Vector3 size){
		anim.moveTo (pos, 0.5f);
		anim.scaleTo (size, 0.5f);
	}

	public void zoom(){

		/*if (t.localScale.x == 1.3f && t.position.x == initialPosition.x && !onZoom) {
			onZoom = true;
			anim.scaleTo(new Vector3(2.6f, 2, 1), 0.2f);
			anim.moveTo(new Vector3(0,t.position.y-1.5f,0), 0.2f);
		}*/
	}

	public void backZoom(){
		if (t.position.x == 0 && t.position.y == (initialPosition.y-1.5f) && onZoom) {
			onZoom = false;
			anim.scaleTo (new Vector3 (1.3f, 1f, 1), 0.2f);
			anim.moveTo(new Vector3(initialPosition.x,initialPosition.y,initialPosition.z), 0.2f);
		}
	}

	public void desapear(){
		onDesapear = true;
		onShowMe = false;
		anim.scaleTo (new Vector3 (0f, 0f, 1), 0.6f);
		anim.fadeTo (0f,0.6f);

		for(int i = transform.childCount-1; i >= 0; i--){
			Destroy (transform.GetChild(i).gameObject);
		}
	}

	public void showMe(Vector3 pos, int value){
		orderValue = value;
		onDesapear = false;
		onShowMe = true;
		t.localScale = new Vector3 (0f, 0f, 1);
		t.position = pos;
		sr.material.color = new Color (sr.material.color.r, sr.material.color.g, sr.material.color.b, 0);
		anim.scaleTo (new Vector3 (1.3f, 1f, 1), 0.6f);
		anim.fadeTo (1f,0.6f);
		numsInitiateds = false;
	}

	public void showMeLearn(Vector3 pos, int value, Vector3 size){
		orderValue = value;
		onDesapear = false;
		onShowMe = true;
		t.localScale = new Vector3 (0f, 0f, 1);
		t.position = pos;
		sr.material.color = new Color (sr.material.color.r, sr.material.color.g, sr.material.color.b, 0);
		anim.scaleTo (size, 0.6f);
		anim.fadeTo (1f,0.6f);
		numsInitiateds = false;
	}

	public void resetMe(){
		anim.moveTo (initialPosition, 0.5f);
		anim.scaleTo (initialScale, 0.5f);
		for(int i = transform.childCount-1; i >= 0; i--){
			Destroy (transform.GetChild(i).gameObject);
		}
	}

	public int getOrderValue(){
		return orderValue;
	}

	//Getters and setters
	public bool isOnZoom(){
		return onZoom;
	}
	public bool isOnShowMe(){
		return onShowMe;
	}
	public Vector3 getInitialPosition(){
		return initialPosition;
	}

}
