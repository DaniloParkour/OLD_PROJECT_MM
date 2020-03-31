using UnityEngine;
using System.Collections;

public class Blender : MonoBehaviour {

	private SpriteRenderer sr;
	private AuxiliarAnim anim;
	private float timeToWait;
	private float rotateValue;
	private Transform t;
	
	void Awake () {
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<AuxiliarAnim> ();
		rotateValue = 220f;
		t = transform;
	}

	void Start() {

	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0) && timeToWait > 0 && t.localScale.Equals(new Vector3(1,1,1))){
			timeToWait = 0;
		}

		blendAnimation ();

		if(timeToWait > 0){
			timeToWait -= Time.deltaTime;
		}

		if(timeToWait <= 0){
			if(t.localScale.x == 1){
				timeToWait = 0.5f;
				anim.scaleTo(new Vector3(0,0,0), 0.5f);
			} else
				gameObject.SetActive(false);
		}
	}

	private void blendAnimation() {



		if (rotateValue > 0) {
			if (t.localRotation.z > -0.1f)
				t.Rotate (0, 0, -rotateValue * Time.deltaTime);
			else
				rotateValue *= -1;
		}  else {
			if (t.localRotation.z < 0.1f)
				t.Rotate (0, 0, -rotateValue * Time.deltaTime);
			else
				rotateValue *= -1;
		}

	}

	public void initBlender(float time) {
		transform.localScale = new Vector3 (0,0,0);
		anim.scaleTo (new Vector3(1,1,1), 0.3f);
		timeToWait = time;

	}
}
