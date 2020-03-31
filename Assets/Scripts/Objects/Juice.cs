using UnityEngine;
using System.Collections;

public class Juice : MonoBehaviour {

	private AuxiliarAnim anim;
	private float timeToReload;
	private Vector3 initialPosition;
	private Vector3 initialScale;

	// Use this for initialization
	void Start () {
		anim = GetComponent<AuxiliarAnim> ();
		timeToReload = -10;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToReload > 0)
			timeToReload -= Time.deltaTime;
		else if (timeToReload > -5) {
			timeToReload = -10;
			transform.position = initialPosition;
			transform.localScale = initialScale;
			gameObject.SetActive(false);
		}
	}

	public void goToCounter(Vector3 posCounter){
		initialPosition = transform.position;
		initialScale = transform.localScale;
		anim.moveTo (posCounter, 0.5f);
		anim.scaleTo (new Vector3(0.4f, 0.4f, 0.4f), 0.5f);
		timeToReload = 0.8f;
	}
}
