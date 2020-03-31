using UnityEngine;
using System.Collections;

public class Cash : MonoBehaviour {

	public GameManager gameManager;

	private AuxiliarAnim anim;
	private Transform t;
	private float timeToAnimOnReceive;
	private bool onReceiveCoin;

	// Use this for initialization
	void Start () {
		t = transform;
		timeToAnimOnReceive = -10;
		anim = GetComponent<AuxiliarAnim> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeToAnimOnReceive -= Time.deltaTime;

		if(timeToAnimOnReceive <= 0 && timeToAnimOnReceive > -5){
			animOnReceive();
		}
	}

	public void receiveCoin(){
		gameManager.playAudioCoinClap ();
		t.localScale = new Vector3 (1,1,1);
		timeToAnimOnReceive = 0;
		gameManager.addToScore(1);
	}

	private void animOnReceive(){
		if (!onReceiveCoin && t.localScale.x == 1 && t.localScale.y == 1) {
			onReceiveCoin = true;
			anim.scaleTo (new Vector3 (1.2f, 1.2f, 1), 0.1f);
			timeToAnimOnReceive = 0.15f;
		} else if (onReceiveCoin && t.localScale.x == 1.2f && t.localScale.y == 1.2f) {
			anim.scaleTo (new Vector3 (1, 1, 1), 0.1f);
			timeToAnimOnReceive = 0.15f;
		} else if (onReceiveCoin && t.localScale.x == 1 && t.localScale.y == 1) {
			onReceiveCoin = false;
			timeToAnimOnReceive = -10;
		}
	}
}
