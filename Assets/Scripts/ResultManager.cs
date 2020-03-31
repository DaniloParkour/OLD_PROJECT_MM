using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultManager : MonoBehaviour {

	public Coin coin;
	public Blender blender;
	public Juice juice;
	public OperationManager operationManager;

	private List<Coin> coins;
	private CardController cardA;
	private CardController cardB;
	private Vector3 juiceTo;

	/* -> 0: Fruits goes to result.
	 * -> 1: Show blender.
	 * -> 2: Show Glass of juice and throw coins and desapear Cards.
	 * -> 3: New turn */
	private int animationStage;

	private float timeToNextStage;
	private Transform t;

	// Use this for initialization
	void Start () {

		t = transform;
		timeToNextStage = -10;

		coins = new List<Coin>();
		while (coins.Count < 8){
			Coin newCoin = Instantiate(coin) as Coin;
			newCoin.gameObject.transform.SetParent(this.gameObject.transform);
			newCoin.transform.localScale = new Vector3(0.5f, 0.5f, 1);
			newCoin.transform.localPosition = new Vector3(-10,-10,0);
			coins.Add(newCoin);
		}
	}

	// Update is called once per frame
	void Update () {

		if(timeToNextStage > -5)
			timeToNextStage -= Time.deltaTime;

		if (timeToNextStage <= 0 && timeToNextStage > -5) {
			animationStage ++;

			if(animationStage == 1)
				openBlender();
			if(animationStage == 2)
				openJuice();
			if(animationStage == 3){
				juice.goToCounter (juiceTo);
				newTurnBeRight();
			}

		}

		if (animationStage == 1 && timeToNextStage > 0 && !blender.gameObject.activeSelf)
			timeToNextStage = 0;

		if (Input.GetMouseButtonDown (0) && animationStage == 2 && timeToNextStage > 0)
			timeToNextStage = 0;

	}

	public void initRightResult(CardController a, CardController b, Vector3 juiceGoTo) {
		cardA = a;
		cardB = b;
		animationStage = 0;
		timeToNextStage = 0.5f;
		cardA.getFruit ().goToResult (t.position.x, t.position.y);
		cardB.getFruit ().goToResult (t.position.x, t.position.y);
		juiceTo = juiceGoTo;
		GameSettings.checkingOperation = true;
	}

	public void throwCoinsNow(){
		for (int i = 0; i < coins.Count; i++) {
			coins[i].gameObject.SetActive(true);
			coins[i].transform.localPosition = new Vector3(0,0,0);
			coins[i].throwRandom();
		}
		timeToNextStage = -10;
	}

	private void resetCoins(){
		for(int i = 0; i < coins.Count; i++){
			coins[i].transform.position = new Vector3(-10,0,0);
			coins[i].transform.localScale = new Vector3(0.5f,0.5f,1);
		}
	}

	private void openBlender() {
		blender.gameObject.SetActive (true);
		blender.initBlender (3);
		timeToNextStage = 3.5f;
	}

	private void openJuice() {
		juice.gameObject.SetActive (true);
		throwCoinsNow ();
		timeToNextStage = 2f;
	}

	public void newTurnBeRight() {
		//juice.gameObject.SetActive (false);
		cardA.removeMeFromScene ();
		cardB.removeMeFromScene ();
		operationManager.setCardA (null);
		operationManager.setCardB (null);
		operationManager.cancelOperation ();
		GameSettings.checkingOperation = false;
	}

	public void loseCoins(int quant){
		for(int i = 0; i < quant; i++){
			Coin loseCoin = Instantiate(coin);
			loseCoin.transform.position = new Vector3(-2.5f, 4.6f, 0);
			SpriteRenderer sr = loseCoin.GetComponent<SpriteRenderer>();
			sr.material.color = Color.red;
			sr.sortingLayerName = "Operation";
			loseCoin.loseMe();
		}
	}
}
