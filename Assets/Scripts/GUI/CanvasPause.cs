using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasPause : MonoBehaviour {

	public GameObject pauseWindow;
	public GameObject endGameWindow;

	// Use this for initialization
	void Start () {

		pauseWindow.SetActive (true);
		endGameWindow.SetActive (true);

		Button[] bs = pauseWindow.GetComponentsInChildren<Button> ();

		for(int v = 0; v < 2; v++){

			foreach(Button b in bs){
				Text t = b.GetComponentInChildren<Text>();
				if(t == null)
					continue;
				string tex = new GameSettings().getText(b.gameObject.name.ToUpper());
				if(!tex.Equals("")){
					t.text = tex;
				}
			}
			bs = endGameWindow.GetComponentsInChildren<Button> ();
		}

		pauseWindow.SetActive (false);
		endGameWindow.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
