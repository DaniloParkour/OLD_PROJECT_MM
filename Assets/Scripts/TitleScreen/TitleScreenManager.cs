using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenManager : MonoBehaviour {

	public GameObject panelChooseOperators;
	public GameObject panelShop;
	public GameObject loadPlane;

	public Sprite[] flags;

	private string usingCashier;
	private string usingCard;
	private Sprite sprtCashUse;
	private Sprite sprtCardUse;
	private bool[] activedNumbers;

	//Audios
	private AudioSource audioBu_press;
	private AudioSource audioCheck_press;

	// Use this for initialization
	void Start () {

		Screen.orientation = ScreenOrientation.Portrait;

		AudioSource[] audios = GetComponents<AudioSource> ();
		foreach (AudioSource au in audios) {
			if(au.clip.name.Equals("camera-click-nikon")){
				audioBu_press = au;
			} else if(au.clip.name.Equals("coinclap")){
				audioCheck_press = au;
			}
		}

		//Assim enquanto não tiver um loadData().
		if (Player.instance == null) {
			new Player (0, 0, 0);
			Player.instance.addCoins (200);
		}

		//Player.instance.buyCard (GameSettings.itensCards.SNOOKER_BALL);
		//Player.instance.buyCashier (GameSettings.itensCashier.PIG);

		activedNumbers = new bool[9];
		for (int i = 0; i < activedNumbers.Length; i++)
			activedNumbers [i] = true;

		usingCashier = "InitialCashier";
		usingCard = "InitialCard";

		sprtCashUse = panelShop.transform.Find ("panelShop").transform.Find ("CashiersPanel").transform.Find ("bu_cash1").GetChild (0).GetComponent<Image> ().sprite;
		sprtCashUse = panelShop.transform.Find ("panelShop").transform.Find ("CardsPanel").transform.Find ("bu_card1").GetChild (0).GetComponent<Image> ().sprite;

		attTexts ();
		attIcons ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void openChooseOperatorsWindow(string buttonName){



		if (GameSettings.panelOpened)
			return;

		if (!panelChooseOperators.activeSelf && !GameSettings.panelOpened) {
			panelChooseOperators.SetActive (true);
			GameSettings.panelOpened = true;

			GameObject panelPlay = panelChooseOperators.transform.Find("panelChooseOps").gameObject;
			GameObject panelLearn = panelChooseOperators.transform.Find("panelOpsLearn").gameObject;
			GameSettings gs = new GameSettings();

			if (buttonName.Equals ("bu_play")) {
				panelPlay.SetActive(true);
				panelLearn.SetActive(false);
				panelChooseOperators.transform.Find("Title").GetComponent<Text> ().text = gs.getText (GameSettings.texts.GAME_MODE);
				panelChooseOperators.transform.Find("panelChooseOps").Find("PlayMode").GetComponent<Text> ().text = gs.getText (GameSettings.texts.NORMAL_MODE);
				panelChooseOperators.transform.Find("panelChooseOps").Find("TimeMode").GetComponent<Text> ().text = gs.getText (GameSettings.texts.TIME_MODE);
			} else if (buttonName.Equals ("bu_learn")) {
				panelPlay.SetActive(false);
				panelLearn.SetActive(true);
				panelChooseOperators.transform.Find("Title").GetComponent<Text> ().text = gs.getText (GameSettings.texts.LEARN);
			}

		}
	}

	public void closeChooseOperatorsWindow(){
		if (panelChooseOperators.activeSelf) {
			panelChooseOperators.SetActive (false);
			GameSettings.panelOpened = false;
		}
	}

	public void activeShopWindow(bool active){
		if (active && GameSettings.panelOpened)
			return;

		if (!active && (panelShop.transform.Find ("BuyItem").gameObject.activeSelf || panelShop.transform.Find ("NeedMoreCoins").gameObject.activeSelf))
			return;

		if (panelShop.activeSelf != active) {
			panelShop.SetActive (active);
			panelShop.transform.Find("panelShop").Find("CashiersPanel").gameObject.SetActive(true);
			panelShop.transform.Find("panelShop").Find("CardsPanel").gameObject.SetActive(false);
			if(!active)
				GameSettings.panelOpened = false;
			else
				GameSettings.panelOpened = true;
		}

	}

	public void changeLanguage(){
		if (GameSettings.language.Equals ("EN"))
			GameSettings.language = "PT";
		else 
			GameSettings.language = "EN";
		attTexts ();
		attIcons ();
	}

	public void attTexts(){

		bool act_chooseOps = panelChooseOperators.activeSelf;
		bool act_shop = panelShop.activeSelf;

		panelChooseOperators.SetActive (true);
		panelShop.SetActive (true);

		GameSettings gs = new GameSettings ();

		Button[] buttons = FindObjectsOfType<Button>();
		for(int i = 0; i < buttons.Length; i++){
			if(buttons[i].gameObject.name.Equals("bu_play"))
				buttons[i].transform.Find("Text").GetComponent<Text>().text = gs.getText(GameSettings.texts.BU_PLAY);
			if(buttons[i].gameObject.name.Equals("bu_learn"))
				buttons[i].transform.Find("Text").GetComponent<Text>().text = gs.getText(GameSettings.texts.LEARN);

			//else if(buttons[i].gameObject.name.Equals("bu_exit"))
			//	buttons[i].transform.FindChild("Text").GetComponent<Text>().text = gs.getText(GameSettings.texts.BU_EXIT);
		}

		if(panelShop.activeSelf && panelShop.transform.Find ("panelShop").transform.Find("CashiersPanel").gameObject.activeSelf)
			panelShop.transform.Find ("panelShop").transform.Find("Title").GetComponent<Text> ().text = gs.getText (GameSettings.texts.CASH);
		if(panelShop.activeSelf && panelShop.transform.Find ("panelShop").transform.Find("CardsPanel").gameObject.activeSelf)
			panelChooseOperators.transform.Find("Title").GetComponent<Text> ().text = gs.getText (GameSettings.texts.CARDS);

		Text[] texts = FindObjectsOfType<Text> ();
		foreach(Text t in texts){
			if(t.gameObject.name.Equals("TotalGold")){
				t.text = Player.instance.getTotalCoins()+"";
			}
		}

		//Retornar as ativações iniciais
		panelChooseOperators.SetActive (act_chooseOps);
		panelShop.SetActive (act_shop);
	}

	public void playAudioBu_press(){
		audioBu_press.Play ();
	}

	public void playAudioCheck_press(){
		audioCheck_press.Play ();
	}

	public void attIcons() {
		string playerCashiers = Player.instance.getCashiers ();
		string playerCards = Player.instance.getCards ();

		GameObject panelCashiers = panelShop.transform.Find ("panelShop").transform.Find ("CashiersPanel").gameObject;
		GameObject panelCards = panelShop.transform.Find ("panelShop").transform.Find ("CardsPanel").gameObject;

		bool shopActive = panelShop.activeSelf;
		bool cashiersActived = panelCashiers.gameObject.activeSelf;
		bool cardsActived = panelCards.gameObject.activeSelf;

		panelShop.SetActive (true);
		panelCards.SetActive (true);
		panelCashiers.SetActive (true);


		Button[] buttons = panelShop.transform.GetComponentsInChildren<Button>();

		foreach (Button b in buttons) {
			if(b.gameObject.name.Contains("bu_cash") || b.gameObject.name.Contains("bu_card")){
				Image img = b.transform.GetChild(0).gameObject.GetComponent<Image>();

				if(img.gameObject.name.Equals(usingCard) || img.gameObject.name.Equals(usingCashier)){
					b.gameObject.GetComponent<Image>().color = Color.green;
					img.color = Color.white;

					if(b.gameObject.name.Contains("bu_cash"))
						sprtCashUse = img.sprite;
					else if(b.gameObject.name.Contains("bu_card"))
						sprtCardUse = img.sprite;

				} else if (playerCards.Contains(img.gameObject.name) || playerCashiers.Contains(img.gameObject.name)){
					b.gameObject.GetComponent<Image>().color = new Color(1,0.427f,0);
					img.color = Color.white;
					//Debug.Log("Comprado pelo Player: "+img.name);
				} else {
					b.gameObject.GetComponent<Image>().color = new Color(1,0.427f,0);
					img.color = Color.red;
					//Debug.Log(b.name+" - Falta comprar: "+img.name);
				}
			}
		}

		if (cashiersActived) {
			panelCashiers.SetActive (true);
			panelCards.SetActive (false);
		} else if (cardsActived) {
			panelCards.SetActive (true);
			panelCashiers.SetActive (false);
		}

		panelShop.SetActive (shopActive);

		//Set language icon
		foreach(Canvas c in FindObjectsOfType<Canvas>()){
			if(c.name.Equals("Canvas World_Space")){
				if (GameSettings.language.Equals ("EN")) {
					c.transform.Find("bu_language").Find("Panel").GetComponent<Image>().sprite = flags[1];
				} else if (GameSettings.language.Equals ("PT")) {
					c.transform.Find("bu_language").Find("Panel").GetComponent<Image>().sprite = flags[0];
				}
			}
		}
	}

	public void nextPageShop(){
		GameObject panelCashiers = panelShop.transform.Find ("panelShop").transform.Find ("CashiersPanel").gameObject;
		GameObject panelCards = panelShop.transform.Find ("panelShop").transform.Find ("CardsPanel").gameObject;

		bool cashiersActived = panelCashiers.activeSelf;
		bool cardsActived = panelCards.activeSelf;

		Text title = panelShop.transform.Find ("panelShop").transform.Find ("Title").GetComponent<Text> ();

		if(cashiersActived){
			panelCashiers.SetActive(false);
			panelCards.SetActive(true);
			title.text = new GameSettings().getText (GameSettings.texts.CARDS);

		} else if(cardsActived){
			panelCards.SetActive(false);
			panelCashiers.SetActive(true);
			title.text = new GameSettings().getText (GameSettings.texts.CASH);
		}

	}

	public void previewPageShop(){
		GameObject panelCashiers = panelShop.transform.Find ("panelShop").transform.Find ("CashiersPanel").gameObject;
		GameObject panelCards = panelShop.transform.Find ("panelShop").transform.Find ("CardsPanel").gameObject;
		
		bool cashiersActived = panelCashiers.activeSelf;
		bool cardsActived = panelCards.activeSelf;

		Text title = panelShop.transform.Find ("panelShop").transform.Find ("Title").GetComponent<Text> ();
		
		if(cashiersActived){
			panelCashiers.SetActive(false);
			panelCards.SetActive(true);
			title.text = new GameSettings().getText (GameSettings.texts.CARDS);
		} else if(cardsActived){
			panelCards.SetActive(false);
			panelCashiers.SetActive(true);
			title.text = new GameSettings().getText (GameSettings.texts.CASH);
		}
		
	}

	public void selectItemShop(Button item){

		string itensPlayer = Player.instance.getCashiers ();
		itensPlayer += Player.instance.getCards ();
		Image imgItem = item.transform.GetChild (0).GetComponent<Image> ();

		if (itensPlayer.Contains ("- " + imgItem.name + ";")) {
			if (item.name.Contains ("bu_cash"))
				usingCashier = imgItem.name;
			else if (item.name.Contains ("bu_card"))
				usingCard = imgItem.name;
		} else {
			int valuePrice = 0;

			if(imgItem.name.Equals("Machine"))
				valuePrice = (int)GameSettings.prices.MachinePrice;
			if(imgItem.name.Equals("Pig"))
				valuePrice = (int)GameSettings.prices.PigPrice;
			if(imgItem.name.Equals("Watermellon"))
				valuePrice = (int)GameSettings.prices.WatermellonPrice;
			if(imgItem.name.Equals("Pineapple"))
				valuePrice = (int)GameSettings.prices.PineapplePrice;
			if(imgItem.name.Equals("BR_Flag"))
				valuePrice = (int)GameSettings.prices.BR_FlagPrice;
			if(imgItem.name.Equals("SnookerBall"))
				valuePrice = (int)GameSettings.prices.SnookerBallPrice;

			panelShop.transform.Find("BuyItem").gameObject.SetActive(true);
			GameObject buyWindow = panelShop.transform.Find("BuyItem").transform.Find("BuyItemWindow").gameObject;
			buyWindow.transform.Find("Text").GetComponent<Text>().text = new GameSettings().getText(GameSettings.texts.BUY_ITEM);
			buyWindow.transform.Find("BuyItemPanel").transform.Find("ItemImage").GetComponent<Image>().sprite = imgItem.sprite;
			buyWindow.transform.Find("BuyItemPanel").transform.Find("Text").GetComponent<Text>().text = new GameSettings().getText(GameSettings.texts.PRICE)+": "+valuePrice;
		}

		attIcons ();
		attTexts ();
	}

	public void closeMe(GameObject go){
		go.SetActive (false);
	}

	public void buyItem(Image img){

		if (img.sprite == null)
			return;

		Sprite sprt = img.sprite;
		bool needMoreCoins = false;

		if(sprt.name.Equals("CashierTwo")){ //Pig
			if((int)GameSettings.prices.PigPrice <= Player.instance.getTotalCoins()){
				Player.instance.removeCoins((int)GameSettings.prices.PigPrice);
				Player.instance.buyCashier(GameSettings.itensCashier.PIG);
			} else
				needMoreCoins = true;

		} else if(sprt.name.Equals("CashierThree")){ //Machine
			if((int)GameSettings.prices.MachinePrice <= Player.instance.getTotalCoins()){
				Player.instance.removeCoins((int)GameSettings.prices.MachinePrice);
				Player.instance.buyCashier(GameSettings.itensCashier.MACHINE);
			} else
				needMoreCoins = true;

		} else if(sprt.name.Equals("Melancia")){ 
			if((int)GameSettings.prices.WatermellonPrice <= Player.instance.getTotalCoins()){
				Player.instance.removeCoins((int)GameSettings.prices.WatermellonPrice);
				Player.instance.buyCashier(GameSettings.itensCashier.WATERMELON);
			} else
				needMoreCoins = true;

		} else if(sprt.name.Equals("CardBackTwo")){ //Pineapple
			if((int)GameSettings.prices.PineapplePrice <= Player.instance.getTotalCoins()){
				Player.instance.removeCoins((int)GameSettings.prices.PineapplePrice);
				Player.instance.buyCard(GameSettings.itensCards.PINEAPPLE);
			} else
				needMoreCoins = true;

		} else if(sprt.name.Equals("CardBackThree")){ //Flag
			if((int)GameSettings.prices.BR_FlagPrice <= Player.instance.getTotalCoins()){
				Player.instance.removeCoins((int)GameSettings.prices.BR_FlagPrice);
				Player.instance.buyCard(GameSettings.itensCards.BR_FLAG);
			} else
				needMoreCoins = true;

		} else if(sprt.name.Equals("CardBackFour")){ //SnookerBall
			if((int)GameSettings.prices.SnookerBallPrice <= Player.instance.getTotalCoins()){
				Player.instance.removeCoins((int)GameSettings.prices.SnookerBallPrice);
				Player.instance.buyCard(GameSettings.itensCards.SNOOKER_BALL);
			} else
				needMoreCoins = true;
		}

		if(needMoreCoins){
			panelShop.transform.Find("NeedMoreCoins").Find("NeedCoins").Find("BuyItemPanel").Find("Text")
				.GetComponent<Text>().text = (new GameSettings()).getText (GameSettings.texts.NEED_COINS);
			panelShop.transform.Find("NeedMoreCoins").gameObject.SetActive(true);
		}

		attIcons ();
		attTexts ();
	}

	public void exitGame(){
		new GameSettings ().exitGame ();
	}

	public void addNumber(Toggle num){

		if (!num.isOn) {
			int quant = 0;
			for(int i = 0; i < activedNumbers.Length; i++)
				if(activedNumbers[i])
					quant++;

			if(quant == 1){
				num.Select();
				num.isOn = true;
				return;
			}
		}

		if (num.gameObject.name.Equals ("One"))
			activedNumbers [0] = num.isOn;
		else if (num.gameObject.name.Equals ("Two"))
			activedNumbers [1] = num.isOn;
		else if (num.gameObject.name.Equals ("Three"))
			activedNumbers [2] = num.isOn;
		else if (num.gameObject.name.Equals ("Four"))
			activedNumbers [3] = num.isOn;
		else if (num.gameObject.name.Equals ("Five"))
			activedNumbers [4] = num.isOn;
		else if (num.gameObject.name.Equals ("Six"))
			activedNumbers [5] = num.isOn;
		else if (num.gameObject.name.Equals ("Seven"))
			activedNumbers [6] = num.isOn;
		else if (num.gameObject.name.Equals ("Eight"))
			activedNumbers [7] = num.isOn;
		else if (num.gameObject.name.Equals ("Nine"))
			activedNumbers [8] = num.isOn;

		if (num.isOn) {
			num.transform.Find("Background").GetComponent<Image>().color = Color.white;
			num.transform.Find("Label").GetComponent<Text>().color = Color.black;
		} else {
			num.transform.Find("Background").GetComponent<Image>().color = Color.grey;
			num.transform.Find("Label").GetComponent<Text>().color = new Color(0,0,0,0.5f);
		}
	}

	//String is "+", "x", "+ -", "+ - x /"...
	public void playGame(string operators){

		int modeGame = 1;

		if (operators.Contains ("Mode=2"))
			modeGame = 2;
		else if (operators.Contains ("Mode=3"))
			modeGame = 3;

		Debug.Log ("Game mode is "+modeGame+". -> 1) Normal. 2) Learn. 3) Time.");

		loadPlane.SetActive (true);

		GameSettings.useSum = false;
		GameSettings.useSub = false;
		GameSettings.useMul = false;
		GameSettings.useDiv = false;

		if (operators.Contains ("+"))
			GameSettings.useSum = true;
		if (operators.Contains ("-"))
			GameSettings.useSub = true;
		if (operators.Contains ("x"))
			GameSettings.useMul = true;
		if (operators.Contains ("/"))
			GameSettings.useDiv = true;

		if (!GameSettings.useSum && !GameSettings.useSub && !GameSettings.useMul && !GameSettings.useDiv)
			GameSettings.useSum = true;

		GameSettings.panelOpened = false;

		new Player (0, 0, 0);

		Player.instance.setSprtCashier (sprtCashUse);
		Player.instance.setSprtCard (sprtCardUse);

		if(modeGame==1)
			Application.LoadLevel("MainScene");
		if (modeGame == 2) {
			GameSettings.numbersToLearn = "";
			for(int i = 0; i < activedNumbers.Length; i++){
				if(activedNumbers[i])
					GameSettings.numbersToLearn += " "+(i+1);
			}
			if(GameSettings.numbersToLearn.Equals(""))
				GameSettings.numbersToLearn = "1 2 3 4 5 6 7 8 9";

			Application.LoadLevel ("LearnScene");
		}

	}
}
