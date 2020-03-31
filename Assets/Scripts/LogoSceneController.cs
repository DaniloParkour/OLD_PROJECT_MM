using UnityEngine;
using System.Collections;

public class LogoSceneController : MonoBehaviour {

	public GameObject[] quads;
	public AuxiliarAnim name;
	public AuxiliarAnim fadePlane;

	private int currentSituation;
	private float timeToWait;

	// Use this for initialization
	void Start () {
		currentSituation = 0;
		timeToWait = 2;
		name.GetComponent<SpriteRenderer> ().material.color = new Color (1,1,1,0);
		fadePlane.GetComponent<SpriteRenderer> ().material.color = new Color (1,1,1,0);
	}
	
	// Update is called once per frame
	void Update () {

		if (currentSituation <= 11) {
			if (timeToWait >= 0)
				timeToWait -= Time.deltaTime;
			else {

				if(currentSituation == 10){
					fadePlane.fadeTo(1, 1f);
					timeToWait = 1;
				}

				if(currentSituation == 11)
					Application.LoadLevel("TitleScreen");

				if(currentSituation < 9){
					quads[currentSituation].SetActive(true);
					//Adicionar som de puc/plim.
					if(currentSituation == 6)
						name.fadeTo(1, 1f);
				}

				if (currentSituation < 6)
					timeToWait = 0.3f;
				else if (currentSituation < 9)
					timeToWait = 0.1f;
				else if (currentSituation == 9)
					timeToWait = 2;

				currentSituation++;
			}
		}
	
	}
}
