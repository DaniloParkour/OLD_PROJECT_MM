using UnityEngine;
using System.Collections;

public class FadePlaneController : MonoBehaviour {

	private SpriteRenderer sr;
	private bool onFade;
	private bool onUnrightResult;
	private float fadeValue;
	private float totalTime;
	private AuxiliarAnim anim;
	private string anteriorLayer;
	private int anteriorOrderLayer;
	private float anteriorAlpha;
	private float timeToBackOnUnright;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		sr.material.color = new Color (0, 0, 0, 0);
		onFade = false;
		totalTime = 1;
		onUnrightResult = false;
		anim = GetComponent<AuxiliarAnim> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (onFade)
			fadePlane ();

		if (onUnrightResult) {
			if (timeToBackOnUnright < 0) {
				onUnrightResult = false;
				sr.material.color = new Color (0, 0, 0, anteriorAlpha);
				sr.sortingLayerName = anteriorLayer;
				sr.sortingOrder = anteriorOrderLayer;
			}
			timeToBackOnUnright -= Time.deltaTime;
		}

	}

	public void fadeTo(float value, float secondsTime){
		fadeValue = value;
		totalTime = secondsTime;
		onFade = true;
	}

	private void fadePlane(){
		if (sr.material.color.a < fadeValue)
			sr.material.color = new Color (0, 0, 0, (sr.material.color.a + Time.deltaTime * (1/totalTime)));
		else
			sr.material.color = new Color (0, 0, 0, (sr.material.color.a - Time.deltaTime * (1/totalTime)));

		if(Mathf.Abs(sr.material.color.a - fadeValue) <= 0.05f){
			onFade = false;
			sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, fadeValue);
		}
	}

	public void unrightResult(){
		onUnrightResult = true;
		anteriorAlpha = sr.material.color.a;
		anteriorLayer = sr.sortingLayerName;
		anteriorOrderLayer = sr.sortingOrder;
		sr.material.color = new Color (1,0,0, anteriorAlpha);
		sr.sortingLayerName = "GUI";
		sr.sortingOrder = 100;
		timeToBackOnUnright = 0.1f;
	}
}
