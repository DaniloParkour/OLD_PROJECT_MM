using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public Transform cashTarget;

	private float y_pos;
	private Vector3 throwDirection;
	private bool onThrow;
	private bool onCash;
	private Transform t;
	private float timeToGoCash;
	private float gravity = 3.5f;
	private AuxiliarAnim anim;
	private float timeToDestroyMe;
	
	void Awake () {
		y_pos = 0;
		t = transform;
		timeToGoCash = -10;
		anim = GetComponent<AuxiliarAnim> ();
		onThrow = false;
		timeToDestroyMe = -10;
	}
	
	// Update is called once per frame
	void Update () {

		if (timeToDestroyMe <= 0 && timeToDestroyMe > -5)
			Destroy (this.gameObject);

		if (timeToDestroyMe > 0)
			timeToDestroyMe -= Time.deltaTime;

		if (onThrow)
			throwAnim ();
		else if (timeToGoCash > 0)
			timeToGoCash -= Time.deltaTime;
		else if (timeToGoCash > -5 && !onCash) {
			timeToGoCash = -10;
			goToCash();
		}

		if (t.position == cashTarget.position && !anim.isOnMove ()) {
			cashTarget.gameObject.GetComponent<Cash>().receiveCoin();
			desapear ();
		}

	}

	public void goToCash(){
		if(!anim.isOnMove())
			anim.moveTo (cashTarget.position, Random.Range(0.5f, 2f));
		timeToGoCash = -10;
	}

	private void throwAnim(){
		if (t.localPosition.y > y_pos) {
			t.Translate(throwDirection.x*Time.deltaTime, throwDirection.y*Time.deltaTime , throwDirection.z*Time.deltaTime);
			throwDirection.y -= gravity*gravity*Time.deltaTime;
		} else {
			onThrow = false;
			timeToGoCash = 2;
		}
	}

	public void throwMe(Vector3 direction, float yMin){
		timeToGoCash = -10;
		y_pos = yMin;
		throwDirection = direction;
		onThrow = true;
	}

	public void throwRandom(){
		float fx = Random.Range (-2f,2f);
		float fy = Random.Range (3.5f,5.0f);
		throwMe (new Vector3(fx,fy,0), Random.Range(-2f, -1f));
	}

	public void loseMe(){
		float moveX = Random.Range (0.0f, 1.0f);
		anim.moveTo (new Vector3 (t.position.x + moveX, t.position.y - 1, t.position.z), 1);
		anim.fadeTo (0, 1);
		timeToDestroyMe = 1;
	}

	private void desapear(){
		t.position = new Vector3 (-10, 0, 0);
		gameObject.SetActive (false);
	}
}
