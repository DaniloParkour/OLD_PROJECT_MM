using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LearnGameManager : MonoBehaviour {

	public LearnOperationManager operationManager;
	public LearnCardsManager cardsManager;
	public LearnOrderManager ordersManager;
	public GameObject fadePlane;
	public GameObject pauseWindow;

	public Text numRight;
	public Text numUnright;

	private int totalRight;
	private int totalUnright;
	private float timeToCloseFadePlane;

	//Audios
	private AudioSource musicScene;
	private AudioSource audioBu_press;
	private AudioSource audioCheck_press;
	private AudioSource audioError;
	private AudioSource audioFlipCard;

	// Use this for initialization
	void Start () {

		Screen.orientation = ScreenOrientation.Landscape;

		AudioSource[] audios = GetComponents<AudioSource> ();
		foreach (AudioSource au in audios) {
			if(au.clip.name.Equals("camera-click-nikon")){
				audioBu_press = au;
			} else if(au.clip.name.Equals("coinclap")){
				audioCheck_press = au;
			} else if(au.clip.name.Equals("error")){
				audioError = au;
			} else if(au.clip.name.Equals("card-flip")){
				audioFlipCard = au;
			} else if (au.clip.name.Equals("harp")){
				musicScene = au;
			}
		}

		//Apenas para testes (deletar depois).
		if(!GameSettings.useDiv && !GameSettings.useSub && !GameSettings.useSum && !GameSettings.useMul)
			GameSettings.useMul = true;

		totalRight = 0;
		totalUnright = 0;

		timeToCloseFadePlane = -10;

	}
	
	// Update is called once per frame
	void Update () {
		if (timeToCloseFadePlane >= 0)
			timeToCloseFadePlane -= Time.deltaTime;
		if (timeToCloseFadePlane > -5 && timeToCloseFadePlane < 0) {
			fadePlane.SetActive (false);
			timeToCloseFadePlane = -10;
		}
	}

	public void playAudioBu_press(){
		audioBu_press.Play ();
	}

	public void playAudioCheck_press(){
		audioCheck_press.Play ();
	}

	public void playAudioError(){
		audioError.Play ();
	}

	public void openPause(){
		pauseWindow.transform.Find("PanelEndGame").Find("ImageRight").Find("TotalRight").GetComponent<Text>().text = ""+totalRight;
		pauseWindow.transform.Find("PanelEndGame").Find("ImageUnright").Find("TotalUnright").GetComponent<Text>().text = ""+totalUnright;
		pauseWindow.SetActive (true);
		GameSettings.panelOpened = true;
		Time.timeScale = 0;
	}

	public void openEndGame(){
		musicScene.Stop ();
		pauseWindow.transform.Find("PanelEndGame").Find("ImageRight").Find("TotalRight").GetComponent<Text>().text = ""+totalRight;
		pauseWindow.transform.Find("PanelEndGame").Find("ImageUnright").Find("TotalUnright").GetComponent<Text>().text = ""+totalUnright;
		pauseWindow.SetActive (true);
		GameSettings.panelOpened = true;
		pauseWindow.transform.Find ("PanelEndGame").Find ("bu_exit").gameObject.SetActive(false);
	}

	public void addRightPoint(){
		totalRight++;
		numRight.text = totalRight + "";
		playAudioBu_press ();
	}

	public void addUnrightPoint(){
		totalUnright++;
		numUnright.text = totalUnright + "";
		fadePlane.SetActive (true);
		timeToCloseFadePlane = 0.2f;
		playAudioError ();
	}

	public void addCardA(CardController cardA){
		operationManager.setCardA (cardA);
	}
	public void addCardB(CardController cardB){
		operationManager.setCardB (cardB);
	}

	public void chooseOrder(Order order){
		operationManager.addOrder (order);
	}

	public void initOrders(int value){
		ordersManager.initOrders (value);
	}

	//Buttons actions
	public void closeMe(GameObject obj){
		obj.SetActive (false);
		if (obj.Equals (pauseWindow)) {
			GameSettings.panelOpened = false;
			Time.timeScale = 1;
		}
	}

	public void retry(){
		Time.timeScale = 1;
		GameSettings.panelOpened = false;
		Application.LoadLevel (2); //2 in Build Settings is the LearnScene.
	}

	public void home(){
		Time.timeScale = 1;
		GameSettings.panelOpened = false;
		Application.LoadLevel ("TitleScreen");
	}

	//InitTurn
	public void initTurn(){
		operationManager.initTurn ();
		cardsManager.initTurn ();
		ordersManager.initTurn ();
	}
}
