using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public CardManager cardsManager;
	public OperationManager operationManager;
	public MatChoosePanel matChoosePanel;
	public FadePlaneController fadePlane;
	public OrderController ordersController;
	public ResultManager resultManager;
	//public NumberGeneration numberGeneration;
	public GameObject scorePanel;
	public GameObject pausePanel;
	public GameObject endGamePanel;
	public GameObject prefabCard;
	public GameObject cashier;

	/*Time to wait for things the no rum at same time. This time is to whait for panels open or close for example*/
	private float timeToWait;
	//private float timeToFadePlane;
	private bool matCampInitied;
	private SpriteRenderer fp_sr; //Sprite Renderer of Fade Plane.
	private float timeToCancelOperation;
	private float timeToActiveScore;
	private bool activeScore;
	private int score;
	private int currentScore;
	private float timeToAddScore;
	private Text textScore;
	private float timeScoreToWhite;
	private float timeToOpenEndGameWindow;
	private float totalTime;

	//Audios
	private AudioSource musicScene;
	private AudioSource audioPressButton;
	private AudioSource audioDealingCard;
	private AudioSource audioFlipCard;
	private AudioSource audioError;
	private AudioSource audioCoinClap;
	private AudioSource endGameAudio;

	// Use this for initialization
	void Start () {

		Screen.orientation = ScreenOrientation.Portrait;

		AudioSource[] audios = GetComponents<AudioSource> ();
		foreach (AudioSource au in audios) {
			if(au.clip.name.Equals("camera-click-nikon")){
				audioPressButton = au;
			} else if(au.clip.name.Equals("dealing-card")){
				audioDealingCard = au;
			} else if(au.clip.name.Equals("card-flip")){
				audioFlipCard = au;
			} else if(au.clip.name.Equals("error")){
				audioError = au;
			} else if(au.clip.name.Equals("coinclap")){
				audioCoinClap = au;
			} else if(au.clip.name.Equals("cheap-flash-game-tune")){
				musicScene = au;
			} else if(au.clip.name.Equals("piglevelwin2")){
				endGameAudio = au;
			}
		}
			
		timeToWait = 1;
		timeToCancelOperation = -10;
		matChoosePanel.gameObject.SetActive (true);
		matChoosePanel.open ();
		matCampInitied = false;
		fadePlane.gameObject.SetActive (true);
		operationManager.gameObject.SetActive (true);
		activeScore = true;
		timeToActiveScore = -10;
		score = 0;
		currentScore = 0;
		timeToAddScore = 0.2f;
		textScore = scorePanel.transform.Find ("Score").gameObject.GetComponent<Text>();
		timeScoreToWhite = 0;
		timeToOpenEndGameWindow = -10;
		totalTime = 0;

		if (Player.instance == null)
			new Player (0, 0, 0);

		Player.instance.initPlayer ();
		if (Player.instance.getSprtCashier () != null)
			cashier.GetComponent<SpriteRenderer> ().sprite = Player.instance.getSprtCashier ();
		if (Player.instance.getSprtCard () != null) {
			CardController pf_card = prefabCard.GetComponent<CardController> ();
			prefabCard.GetComponent<SpriteRenderer> ().sprite = Player.instance.getSprtCard ();
			pf_card.back = Player.instance.getSprtCard ();
		}

		totalTime = -10;
	}
	
	// Update is called once per frame
	void Update () {

		if(totalTime >= 0)
			totalTime += Time.deltaTime;

		if (timeToOpenEndGameWindow > 0) {
			timeToOpenEndGameWindow -= Time.deltaTime;

			if(timeToOpenEndGameWindow <= 2)
				musicScene.volume -= Time.deltaTime/2;
		}

		if (timeToOpenEndGameWindow <= 0 && timeToOpenEndGameWindow > -5) {
			endGamePanel.gameObject.SetActive(true);
			endGameAudio.Play();
			Text[] texts = FindObjectsOfType<Text>();
			foreach (Text t in texts){
				if(t.gameObject.name.Equals("EndGameScore")){
					t.text = score+"";
					Player.instance.addCoins(score);
				} else if(t.gameObject.name.Equals("TotalRight"))
					t.text = Player.instance.getTotalRight()+"";
				else if(t.gameObject.name.Equals("TotalUnright"))
					t.text = Player.instance.getTotalUnright()+"";
				else if(t.gameObject.name.Equals("TotalTime")){
					int mins = (int) (totalTime/60);
					int secs = (int) (totalTime - (mins*60));

					string zero = "";
					string zeroMin = "";
					if(secs < 10)
						zero = "0";
					if(mins < 10)
						zeroMin = "0";

					if(mins > 0)
						t.text = zeroMin+mins+":"+zero+secs;
					else
						t.text = zero+secs+"s";
				}
			}
			timeToOpenEndGameWindow = -10;
		}

		verifyCoinClick ();

		if (timeScoreToWhite >= 0 && currentScore == score)
			timeScoreToWhite -= Time.deltaTime;


		if (currentScore != score) {
			timeScoreToWhite = 0.5f;
			timeToAddScore -= Time.deltaTime;
			if (timeToAddScore <= 0) {
				if (currentScore < score){
					if(!textScore.color.Equals(Color.grey))
						textScore.color = Color.green;
					currentScore++;
				}
				if (currentScore > score) {
					if(!textScore.color.Equals(Color.red))
						textScore.color = Color.red;
					currentScore --;
					resultManager.loseCoins(1);
				}
				timeToAddScore = 0.1f;
				textScore.text = currentScore + "";
			}
		} else if(!textScore.color.Equals(Color.white) && timeScoreToWhite <= 0) {
			textScore.color = Color.white;
		}

		if (timeToActiveScore >= 0)
			timeToActiveScore -= Time.deltaTime;

		if(timeToActiveScore <= 0 && timeToActiveScore >= -5){
			scorePanel.SetActive(activeScore);
			timeToActiveScore = -10;
		}

		if (timeToWait > 0) {
			timeToWait -= Time.deltaTime;
			return;
		}

		if (timeToCancelOperation <= 0 && timeToCancelOperation > -5) {
			cancelOperation ();
			timeToCancelOperation = - 10;
		}

		if (timeToCancelOperation > 0)
			timeToCancelOperation -= Time.deltaTime;

		/*Se A e B estao selecionados, ficar verificando B para iniciar montar equancao quando terminar a
		 animacao de mostrar imagem.*/
		if(cardsManager.getCardA() != null && cardsManager.getCardB() != null && !cardsManager.getCardB().onFlipCardShow() && !operationManager.isOnOperation()){
			operationManager.setCardA(cardsManager.getCardA());
			operationManager.setCardB(cardsManager.getCardB());

			if(!GameSettings.useSum && !GameSettings.useSub && !GameSettings.useMul && !GameSettings.useDiv)
				GameSettings.useSum = true;

			operationManager.useSum(GameSettings.useSum);
			operationManager.useSub(GameSettings.useSub);
			operationManager.useDiv(GameSettings.useDiv);
			operationManager.useMul(GameSettings.useMul);

			operationManager.initOperation();
			fadePlane.fadeTo(0.6f, 2);

		}
	
	}

	public void playAudioBu_press(){
		audioPressButton.Play ();
	}
	public void playAudioDealingCard(){
		audioDealingCard.Play ();
	}
	public void playAudioFlipCard(){
		audioFlipCard.Play ();
	}
	public void playAudioError(){
		audioError.Play ();
	}
	public void playAudioCoinClap(){
		audioCoinClap.Play ();
	}

	public void initEndGameScreen(){
		if (timeToOpenEndGameWindow == -10 && !endGamePanel.activeSelf) {
			timeToOpenEndGameWindow = 4;
		}
	}

	public void initTime(){
		totalTime = 0;
	}

	private void verifyCoinClick (){
		if (Input.GetMouseButton (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 11); //Coin Layer
			
			if (hit.collider != null) {
				Coin coin = hit.collider.gameObject.GetComponent<Coin> ();
				if (coin != null) {
					coin.goToCash();
				}
			}
		}
	}

	public void chargeCamp(string type){
		if (matCampInitied || matChoosePanel.isOpenPanel())
			return;
		else
			matCampInitied = true;

		int rows = 5;
		int collumns = 4;

		if (type.StartsWith ("2"))
			rows = 2;
		else if (type.StartsWith ("3"))
			rows = 3;
		else if (type.StartsWith ("4"))
			rows = 4;
		else if (type.StartsWith ("5"))
			rows = 5;

		if (type.EndsWith ("2"))
			collumns = 2;
		else if (type.EndsWith ("3"))
			collumns = 3;
		else if (type.EndsWith ("4"))
			collumns = 4;

		cardsManager.setRows (rows);
		cardsManager.setCollumns (collumns);
		cardsManager.createCamp ();
	}

	/*Controller and managers use the function "initTurn()" to reset configuration to first or new turn.
	 The game manager will call this functions on diferents objects on scene.*/
	public void initTurn(){
		cardsManager.initTurn ();
		operationManager.initTurn ();
		ordersController.initTurn ();
		fadePlane.fadeTo (0, 0.5f);
		//removeOrders ();
	}

	public void cancelOperation(){
		ordersController.removeOrders ();
		operationManager.cancelOperation ();
	}

	public void initOrders(int value){
		ordersController.initOrders (value);
	}

	public void removeOrders(){
		ordersController.removeOrders();
	}

	public bool chooseOrder(Order order){
		if (operationManager.isButtonPressed ())
			return false;
		return operationManager.addResultOrder(order);
	}

	/*RightBoxResult serve para definir se o resultado será aberto em uma janela de resultado correto ou não.*/
	public void openResult(bool rightBoxResult, string description, Vector3 juiceGoTo){
		if (rightBoxResult) {
			resultManager.initRightResult(operationManager.getCardA(), operationManager.getCardB(), juiceGoTo);
			Player.instance.addOneRight();
		} else {
			fadePlane.unrightResult();
			timeToCancelOperation = 0.2f;
			addToScore(-4);
			timeToAddScore = 1;
			Player.instance.addOneUnright();
		}
	}

	public void activePanelScore(float time, bool active){
		timeToActiveScore = time;
		activeScore = active;
	}

	//Para remover score basta passar números negativos por parâmetro.
	public int addToScore(int quant){
		score += quant;
		if (score < 0)
			score = 0;
		return score;
	}

	public void pauseGame(){
		if (!pausePanel.activeSelf) {
			pausePanel.SetActive (true);
			cardsManager.gameObject.SetActive(false);
			Time.timeScale = 0;
		} else {
			pausePanel.SetActive(false);
			cardsManager.gameObject.SetActive(true);
			Time.timeScale = 1;
		}
	}

	public void toTitle(){
		Time.timeScale = 1;
		Application.LoadLevel("TitleScreen");
	}

	public void retry(){
		Time.timeScale = 1;
		Application.LoadLevel("MainScene");
	}

	public void exitGame(){
		new GameSettings ().exitGame ();
	}

}
