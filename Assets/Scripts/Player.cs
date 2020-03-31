using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
	
	private int totalScore;
	private int bestScore;
	private int totalCoins;
	private int totalRight;
	private int totalUnrihgt;
	
	private bool defaultCashier;
	private bool machineCashier;
	private bool pigCashier;
	private bool watermellonCashier;

	//Default, Pineapple, BR_flag e Snooker_ball.
	private bool defaultCard;
	private bool pineappleCard;
	private bool br_flagCard;
	private bool snookerBallCard;

	private Sprite sprtCashier;
	private Sprite sprtCard;

	public static Player instance = null;

	public Player(int totalScorePlayer, int bestScorePlayer, int totalCoinsPlayer){
		if (instance == null) {
			instance = new Player();
			instance.totalScore = totalScorePlayer;
			instance.totalCoins = totalCoinsPlayer;
			instance.bestScore = bestScorePlayer;

			defaultCard = true;
			defaultCashier = true;

			machineCashier = false;
			pigCashier = false;
			watermellonCashier = false;

			pineappleCard = false;
			br_flagCard = false;
			snookerBallCard = false;

			sprtCashier = null;
			sprtCard = null;

		}
	}

	private Player(){

	}

	//Absolute value grants that no remove coins on addCoins.
	public void addCoins(int quant){
		totalCoins += Mathf.Abs (quant);
	}
	public void removeCoins(int quant){
		totalCoins -= Mathf.Abs (quant);
	}
	
	public void addTotalScore(int quant){
		totalScore += Mathf.Abs (quant);
	}

	public void addOneUnright(){
		totalUnrihgt++;
	}

	public void addOneRight(){
		totalRight++;
	}

	public void initPlayer(){
		instance.totalRight = 0;
		instance.totalUnrihgt = 0;
	}

	public void buyCashier(GameSettings.itensCashier newCashier){
		if(newCashier.Equals(GameSettings.itensCashier.MACHINE))
			machineCashier = true;
		else if(newCashier.Equals(GameSettings.itensCashier.PIG))
			pigCashier = true;
		else if(newCashier.Equals(GameSettings.itensCashier.WATERMELON))
			watermellonCashier = true;
	}

	public void buyCard(GameSettings.itensCards newCard){
		if(newCard.Equals(GameSettings.itensCards.PINEAPPLE))
			pineappleCard = true;
		else if(newCard.Equals(GameSettings.itensCards.SNOOKER_BALL))
			snookerBallCard = true;
		else if(newCard.Equals(GameSettings.itensCards.BR_FLAG))
			br_flagCard = true;
	}

	public string getCards(){
		string retorno = " - InitialCard;";

		if (pineappleCard)
			retorno += " - Pineapple;";
		if(snookerBallCard)
			retorno += " - SnookerBall;";
		if(br_flagCard)
			retorno += " - BR_Flag;";

		return retorno;
	}

	public string getCashiers(){
		string retorno = " - InitialCashier;";
		
		if (pigCashier)
			retorno += " - Pig;";
		if(machineCashier)
			retorno += " - Machine;";
		if(watermellonCashier)
			retorno += " - Watermellon;";
		
		return retorno;
	}

	//Get / Set
	public int getTotalScore(){
		return totalScore;
	}
	public int getBestScore(){
		return bestScore;
	}
	public void setBestScore(int newValue){
		if (newValue > bestScore)
			bestScore = newValue;
	}
	public int getTotalCoins(){
		return totalCoins;
	}
	public int getTotalUnright(){
		return totalUnrihgt;
	}
	public int getTotalRight(){
		return totalRight;
	}
	public void setSprtCashier(Sprite sprtCash){
		if (Application.loadedLevelName.Equals ("TitleScreen")) {
			sprtCashier = sprtCash;
		}
	}
	public Sprite getSprtCashier(){
		return sprtCashier;
	}
	public void setSprtCard(Sprite sprtCard){
		if (Application.loadedLevelName.Equals ("TitleScreen")) {
			this.sprtCard = sprtCard;
		}
	}
	public Sprite getSprtCard(){
		return sprtCard;
	}
}
