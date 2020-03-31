using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LearnCardsManager : MonoBehaviour {

	public GameObject[] numbers;
	public LearnGameManager gameManager;
	public CardController card;

	private float timeToCreateCards;
	private List<string> asks;

	//Audios
	private AudioSource audioFlipCard;
	private AudioSource audioEndGame;

	// Use this for initialization
	void Start () {
		timeToCreateCards = 2;

		AudioSource[] audios = GetComponents<AudioSource> ();
		foreach (AudioSource au in audios) {
			if(au.clip.name.Equals("dealing-card")){
				//audioDealingCard = au;
			} else if(au.clip.name.Equals("card-flip")){
				audioFlipCard = au;
			} else if(au.clip.name.Equals("piglevelwin2")){
				audioEndGame = au;
			}
		}

		asks = new List<string> ();

		if (GameSettings.useSum || GameSettings.useMul) {
			for (int i = 1; i < 10; i++) {
				if(!GameSettings.numbersToLearn.Contains(" "+(i)))
					continue;
				for (int j = 1; j < 10; j++) {
					asks.Add (i + " " + j);
				}
			}
		} else if (GameSettings.useSub) {
			for (int i = 1; i < 10; i++) {
				if(!GameSettings.numbersToLearn.Contains(" "+(i)))
					continue;
				for (int j = 1; j < 10; j++) {
					if (i >= j) {
						asks.Add (i + " " + j);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log (asks.Count);

		if(timeToCreateCards > 0)
			timeToCreateCards -= Time.deltaTime;

		//if (asks != null)
		//	Debug.Log ("> "+asks.Count);

		if (timeToCreateCards <= 0 && timeToCreateCards > -10 && asks != null && asks.Count >= 0) {

			timeToCreateCards = -10;

			if(asks.Count == 0){
				audioEndGame.Play();
				gameManager.openEndGame();
			} else {
				string ask = asks[Random.Range(0, asks.Count)];
				asks.Remove(ask);

				int a = valueA(ask);
				int b = valueB(ask);

				CardController cardA = Instantiate(card, transform.position, Quaternion.identity)  as CardController;
				cardA.setCardValue(a, numbers[a].GetComponent<SpriteRenderer>().sprite);
				cardA.moveCardTo(new Vector3(-5.5f, -0.5f, 0));
				cardA.transform.Find("fruit").localScale = new Vector3(-3,3,1);
				cardA.toScale(2,0.5f);

				CardController cardB = Instantiate(card, transform.position, Quaternion.identity) as CardController;
				cardB.setCardValue(b, numbers[b].GetComponent<SpriteRenderer>().sprite);
				cardB.moveCardTo(new Vector3(-0.5f, -0.5f, 0));
				cardB.transform.Find("fruit").localScale = new Vector3(-3,3,1);
				cardB.toScale(2,0.5f);

				audioFlipCard.Play ();

				gameManager.addCardA(cardA);
				gameManager.addCardB(cardB);
			}
		}

	}

	private int valueA(string str){
		for (int i = 0; i < 10; i++) {
			if (str.StartsWith (""+i))
				return i;
		}
		return -9999;
	}

	private int valueB(string str){
		for (int i = 0; i < 10; i++) {
			if (str.EndsWith (""+i))
				return i;
		}
		return -9999;
	}

	public void initTurn(){
		timeToCreateCards = 0.5f;
	}
}
