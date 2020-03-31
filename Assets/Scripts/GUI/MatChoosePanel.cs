using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatChoosePanel : MonoBehaviour {

	private bool openPanel;
	private bool closePanel;
	private float timeToWait;

	// Use this for initialization
	void Start () {

		foreach (Text t in GetComponentsInChildren<Text>()) {
			if(t.gameObject.name.Equals("TitleWindow")){
				t.text = new GameSettings().getText(GameSettings.texts.TITLE_CHOOSE_PANEL);
				if(GameSettings.language.Equals("EN"))
					t.fontSize = 18;
				if(GameSettings.language.Equals("PT"))
					t.fontSize = 14;
			}
		}

		timeToWait = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (timeToWait > 0)
			timeToWait -= Time.deltaTime;

		if ((openPanel || closePanel ) && timeToWait <= 0) {
			toCurrentSize ();
		}

	}

	private void toCurrentSize (){

		float scale = Time.deltaTime * 0.02f;
		if (closePanel && transform.localScale.y >= 0.001f)
			scale *= -1;

		if ((openPanel && transform.localScale.x < 0.02f) || (closePanel && transform.localScale.x > 0.001f))
			transform.localScale = new Vector3 (transform.localScale.x + scale, transform.localScale.y, 1);
		else if ((openPanel && transform.localScale.y < 0.018f) || (closePanel && transform.localScale.y > 0.001f))
			transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y + scale, 1);
		else {
			if(openPanel){
				transform.localScale = new Vector3(0.02f, 0.018f,1);
				openPanel = false;
			} else if(closePanel) {
				closePanel = false;
				gameObject.SetActive(false);
			}
		}

		if (transform.localScale.x > 1)
			transform.localScale = new Vector3 (1, transform.localScale.y, 1);
		if (transform.localScale.y > 1)
			transform.localScale = new Vector3 (transform.localScale.x, 1, 1);
	}

	public void open(){
		transform.localScale = new Vector3 (0.001f,0.001f,1);
		openPanel = true;
	}

	public void close(){
		if (openPanel)
			return;
		closePanel = true;
		timeToWait = 0.5f;
	}

	public bool isOpenPanel(){
		return openPanel;
	}
}
