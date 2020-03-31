using UnityEngine;
using System.Collections;

public class Fruit : MonoBehaviour {

	private AuxiliarAnim anim;
	private float timeToDestroy;

	// Use this for initialization
	void Start () {
		anim = GetComponent<AuxiliarAnim> ();
		timeToDestroy = -10;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToDestroy <= 0 && timeToDestroy > -5)
			Destroy (this.gameObject);
	}

	public void goToResult(float x, float y){
		anim.moveTo (new Vector3(x,y,0), 0.5f);
		inToBlender ();
	}

	private void inToBlender(){
		anim.scaleTo (new Vector3(0,0,0), 0.5f);
		timeToDestroy = 0.8f;
	}
}
