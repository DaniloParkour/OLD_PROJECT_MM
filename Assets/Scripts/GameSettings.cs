using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

	public static bool pause;
	public static bool panelOpened;
	public static bool blockCardsSelector;
	public static bool cardOnZoom;
	public static bool useSum;
	public static bool useSub;
	public static bool useMul;
	public static bool useDiv;
	public static bool checkingOperation;
	public static string numbersToLearn;

	public enum prices {
		MachinePrice = 150,
		PigPrice = 200,
		WatermellonPrice = 300,
		PineapplePrice = 150,
		BR_FlagPrice = 300,
		SnookerBallPrice = 450
	};

	public static string language = "EN";
	public enum texts {BU_PLAY, BU_EXIT, CASH, CARDS, OPS, BU_CONTINUE, BU_RETRY, BU_TITLE, TITLE_CHOOSE_PANEL, BUY_ITEM, PRICE, LEARN, GAME_MODE, NORMAL_MODE, TIME_MODE, NEED_COINS};
	public enum itensCashier {INITIAL, PIG, MACHINE, WATERMELON};
	public enum itensCards {INITIAL, SNOOKER_BALL, BR_FLAG, PINEAPPLE};

	// Use this for initialization
	void Start () {
		pause = false;
		panelOpened = false;
		blockCardsSelector = true;
		cardOnZoom = false;
		useSum = true;
		useSub = false;
		useMul = false;
		useDiv = false;
		checkingOperation = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public string getText(string enumName){

		string retorno = "";

		if(enumName.ToUpper().Equals("BU_PLAY")){
			retorno = getText(texts.BU_PLAY);
		} else if(enumName.ToUpper().Equals("BU_EXIT")){
			retorno = getText(texts.BU_EXIT);
		} else if(enumName.ToUpper().Equals("CASH")){
			retorno = getText(texts.CASH);
		} else if(enumName.ToUpper().Equals("OPS")){
			retorno = getText(texts.OPS);
		} else if(enumName.ToUpper().Equals("BU_CONTINUE")){
			retorno = getText(texts.BU_CONTINUE);
		} else if(enumName.ToUpper().Equals("BU_RETRY")){
			retorno = getText(texts.BU_RETRY);
		} else if(enumName.ToUpper().Equals("BU_TITLE")){
			retorno = getText(texts.BU_TITLE);
		} else if(enumName.ToUpper().Equals("CARDS")){
			retorno = getText(texts.CARDS);
		} else if(enumName.ToUpper().Equals("CASH")){
			retorno = getText(texts.CASH);
		} else if(enumName.ToUpper().Equals("BUY_ITEM")){
			retorno = getText(texts.BUY_ITEM);
		} else if(enumName.ToUpper().Equals("PRICE")){
			retorno = getText(texts.PRICE);
		} else if(enumName.ToUpper().Equals("LEARN")){
			retorno = getText(texts.LEARN);
		} else if(enumName.ToUpper().Equals("GAME_MODE")){
			retorno = getText(texts.GAME_MODE);
		} else if(enumName.ToUpper().Equals("NORMAL_MODE")){
			retorno = getText(texts.NORMAL_MODE);
		} else if(enumName.ToUpper().Equals("TIME_MODE")){
			retorno = getText(texts.TIME_MODE);
		} else if(enumName.ToUpper().Equals("NEED_COINS")){
			retorno = getText(texts.NEED_COINS);
		}

		return retorno;
	}

	public string getText(texts enumText){

		if(language.Equals("EN")){

			if(enumText == texts.BU_PLAY)
				return "Play";
			else if(enumText == texts.BU_EXIT)
				return "Exit";
			else if(enumText == texts.CASH)
				return "Cashiers";
			else if(enumText == texts.CARDS)
				return "Cards";
			else if(enumText == texts.OPS)
				return "Operators";
			else if(enumText == texts.BU_CONTINUE)
				return "Continue";
			else if(enumText == texts.BU_RETRY)
				return "Retry";
			else if(enumText == texts.BU_TITLE)
				return "Title";
			else if(enumText == texts.TITLE_CHOOSE_PANEL)
				return "Choose a cards camp";
			else if(enumText == texts.BUY_ITEM)
				return "Buy item";
			else if(enumText == texts.PRICE)
				return "Price";
			else if(enumText == texts.LEARN)
				return "Learn";
			else if(enumText == texts.GAME_MODE)
				return "Game mode";
			else if(enumText == texts.NORMAL_MODE)
				return "Free mode";
			else if(enumText == texts.TIME_MODE)
				return "Time mode";
			else if(enumText == texts.NEED_COINS)
				return "Need more coins!";

		} else if(language.Equals("PT")){

			if(enumText == texts.BU_PLAY)
				return "Jogar";
			else if(enumText == texts.BU_EXIT)
				return "Sair";
			else if(enumText == texts.CASH)
				return "Caixa";
			else if(enumText == texts.CARDS)
				return "Cartas";
			else if(enumText == texts.OPS)
				return "Operadores";
			else if(enumText == texts.BU_CONTINUE)
				return "Continuar";
			else if(enumText == texts.BU_RETRY)
				return "Reiniciar";
			else if(enumText == texts.BU_TITLE)
				return "Início";
			else if(enumText == texts.BUY_ITEM)
				return "Comprar item";
			else if(enumText == texts.PRICE)
				return "Preço";
			else if(enumText == texts.LEARN)
				return "Aprender";
			else if(enumText == texts.GAME_MODE)
				return "Modo jogo";
			else if(enumText == texts.NORMAL_MODE)
				return "Jogo livre";
			else if(enumText == texts.TIME_MODE)
				return "Modo tempo";
			else if(enumText == texts.NEED_COINS)
				return "Moedas insuficiente!";

		}

		return "";
	}

	public void exitGame(){
		Application.Quit ();
	}

}
