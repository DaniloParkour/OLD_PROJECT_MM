using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour {

	public GameManager gameManager;
	public CardController cardPrefab;
	public Sprite[] fruits;

	private int rows, collumns;
	private List<CardController> cards;
	private CardController cardA;
	private float timeToAddCard;
	private int currentCardToAdd;
	private CardController cardB;
	private bool cardsInitied;
	private float timeToChooseCards;

	// Use this for initialization
	void Start () {
		cardA = null;
		cardB = null;
		rows = 5;
		collumns = 4;
		cards = new List<CardController>();
		timeToAddCard = -10;
		cardsInitied = false;
		timeToChooseCards = -10;
	}
	
	// Update is called once per frame
	void Update () {

		verifyAllCards ();

		if (timeToAddCard > 0) {
			timeToAddCard -= Time.deltaTime;
		} else if (timeToAddCard > -5 && currentCardToAdd < cards.Count) {
			gameManager.playAudioFlipCard();
			cards [currentCardToAdd].gameObject.SetActive (true);
			//Vector3 pos = cards [currentCardToAdd].transform.position;
			Vector3 pos = cards [currentCardToAdd].transform.localPosition;
			cards [currentCardToAdd].transform.localPosition = new Vector3 (0, 0, 0);
			cards [currentCardToAdd].moveCardTo (pos);
			cards [currentCardToAdd].GetComponent<SpriteRenderer> ().material.color = new Color (0.7f, 0.7f, 0.7f, 1);
			cards [currentCardToAdd].setEnableClick (false);
			cards [currentCardToAdd].transform.Rotate (0, 180, 0);
			timeToAddCard = 0.25f;
			if(cards.Count <= 8)
				timeToAddCard = 0.4f;
			currentCardToAdd++;
		} else if (currentCardToAdd == cards.Count && cards.Count > 1 && !cards [cards.Count - 1].onMoveCard ()) {
			for (int i = 0; i < cards.Count; i++) {
				cards [i].hideThisCard ();
			}
			cardsInitied = true;
			currentCardToAdd++;
		} else if (currentCardToAdd == cards.Count+1 && cards.Count > 1 && !cards [cards.Count - 1].isCardHide()) {
			for (int i = 0; i < cards.Count; i++) {
				cards [i].GetComponent<SpriteRenderer> ().material.color = new Color (1, 1, 1, 1);
				cards [i].setEnableClick(true);
				cards [i].attInitialValues();
			}
			gameManager.initTime();
			currentCardToAdd++;
		}

		if (timeToChooseCards <= 0) {

			/*Se ao clicar o botao as cartas já foram adicionadas no campo e todas terminaram de virar para esconder (.isCardHide())*/
			if ((Input.GetMouseButtonUp (0)) && !GameSettings.panelOpened && !GameSettings.checkingOperation && !((cardA != null && cardA.isOnZoom ()) || (cardB != null && cardB.isOnZoom ()))) {

				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 8); //The layer 8 is the Cards layer.

				if (hit.collider != null) {
					CardController card = hit.collider.gameObject.GetComponent<CardController> ();
					if ((cardA == null || cardB == null) && !card.isSelected ()) {
						if (card.selectCard ()) {
							gameManager.playAudioDealingCard();
							if (cardA == null)
								cardA = card;
							else
								cardB = card;
						}
					}

				}
			}

			if ((Input.GetMouseButtonDown (0)) && !GameSettings.panelOpened && !GameSettings.checkingOperation) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 8); //The layer 8 is the Cards layer.
				
				if (hit.collider != null) {
					CardController card = hit.collider.gameObject.GetComponent<CardController> ();
					card.clickDown ();
					if (cardA != null && card == cardA) {
						cardA.clickToZoom ();
					} else if (cardB != null && card == cardB) {
						cardB.clickToZoom ();
					}
				}
			}
		} else {
			timeToChooseCards -= Time.deltaTime;
		}
	}

	private void verifyAllCards(){
		if (cards == null)
			return;
		for (int i = cards.Count; i > 0; i--) {
			if(!cards[i-1].gameObject.activeSelf)
				continue;
			if (cards[i-1].getTimeToRemoveMe() <= 0 && cards[i-1].getTimeToRemoveMe() > -5){
				CardController c = cards[i-1];
				cards.Remove(c);
				Destroy(c.gameObject);
			}
		}

		if (cardsInitied && cards.Count == 0) {
			gameManager.initEndGameScreen();
		}
	}

	public void createCamp(){
		float currentPosX = 0;
		float currentPosY = 0;
		float dx = 0;
		float dy = 0;
		float size = 10;

		if (rows == 5) {
			if(size > 1)
				size = 1;
			currentPosY = 3.2f;
			dy = -1.8f;
		} else if (rows == 4) {
			if(size > 1.2)
				size = 1.2f;
			currentPosY = 3;
			dy = -2.2f;
		} else if (rows == 3) {
			if(size > 1.2)
				size = 1.2f;
			currentPosY = 2.4f;
			dy = -2.6f;
		} else if (rows == 2) {
			if(size > 1.8)
				size = 1.8f;
			currentPosY = 1.2f;
			dy = -3.2f;
		}

		if (collumns == 4) {
			if(size > 1)
				size = 1;
			currentPosX = -2.25f;
			dx = 1.5f;
		} else if (collumns == 3) {
			if(size > 1.2)
				size = 1.2f;
			currentPosX = -2f;
			dx = 2f;
		} else if (collumns == 2) {
			if(size > 1.8)
				size = 1.8f;
			currentPosX = -1.5f;
			dx = 3f;
		}

		for(int i = 0; i < collumns; i++){
			for(int j = 0; j < rows; j++){

				if(!(((collumns * rows) % 2 != 0) && i == (int)collumns/2 && j == (int)rows/2)){
					CardController newCard = (Instantiate(cardPrefab) as CardController);
					int cardNum = Random.Range(1,11);
					newCard.gameObject.transform.SetParent(this.gameObject.transform);
					newCard.gameObject.transform.localScale = new Vector3 (size,size,0);
					newCard.toCardsLayer();
					newCard.gameObject.transform.position = new Vector3(currentPosX, currentPosY, 0);
					newCard.setCardValue(cardNum, fruits[cardNum-1]);
					newCard.gameObject.SetActive(false);
					cards.Add (newCard);
					timeToAddCard = 2;
					currentCardToAdd = 0;
				}

				if(j < rows-1)
					currentPosY += dy;
				else {
					currentPosX += dx;
					currentPosY -= dy*(rows-1);
				}

			}
		}
	}

	/*Controller and managers use the function "initTurn()" to reset configuration to first or new turn.
	 The game manager will call this functions on diferents objects on scene.*/
	public void initTurn(){
		cardA = null;
		cardB = null;
		timeToChooseCards = 2;
	}

	//Getters and setters
	public CardController getCardA(){
		return cardA;
	}
	public CardController getCardB(){
		return cardB;
	}
	public int getCollumns(){
		return collumns;
	}
	public int getRows(){
		return rows;
	}
	public void setCollumns(int numCols){
		collumns = numCols;
	}
	public void setRows(int numRows){
		rows = numRows;
	}
	public List<int> getCardsValues(){
		List<int> values = new List<int>();

		return values;
	}
}
