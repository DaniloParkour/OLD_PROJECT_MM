using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResponsePanelController : MonoBehaviour {

	private string response;
	private bool showResponsePanel;

	public Text panelText;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void closePanel(){
		gameObject.SetActive (false);
		GameSettings.panelOpened = false;
	}

	public void showPanel(string text){
		gameObject.SetActive(true);
		panelText.text = text;
		GameSettings.panelOpened = true;
	}

	//Gettes and setters
	public bool isShowResponsePanel(){
		return showResponsePanel;
	}
}
