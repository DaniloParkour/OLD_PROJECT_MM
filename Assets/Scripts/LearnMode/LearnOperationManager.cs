using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LearnOperationManager : MonoBehaviour {

	public AuxiliarAnim sum;
	public AuxiliarAnim sub;
	public AuxiliarAnim mul;
	public AuxiliarAnim eq;
	public AuxiliarAnim bu_verify;
	public AuxiliarAnim bu_end;
	public LearnGameManager gameManager;

	private CardController cardA;
	private CardController cardB;
	private AuxiliarAnim choosedOp;
	private Order choosedOrder;
	private float timeToNewTurn;

	private bool onOperation;

	// Use this for initialization
	void Start () {
		if (GameSettings.useSum)
			choosedOp = sum;
		else if (GameSettings.useSub)
			choosedOp = sub;
		else if (GameSettings.useMul)
			choosedOp = mul;

		choosedOp.gameObject.SetActive (true);
		choosedOp.gameObject.GetComponent<SpriteRenderer> ().material.color = new Color (0,1,0,0);
		eq.gameObject.GetComponent<SpriteRenderer> ().material.color = new Color (0,1,0,0);
		bu_verify.gameObject.GetComponent<SpriteRenderer> ().material.color = new Color (1,1,1,0);

		onOperation = false;

		timeToNewTurn = -10;

	}
	
	// Update is called once per frame
	void Update () {
		if (cardA != null && !cardA.isSelected()) {
			cardA.selectLearnCard();
		}
		if (cardB != null && !cardB.isSelected()) {
			cardB.selectLearnCard();
		}

		if(cardA != null && cardB != null && cardA.isSelected() && cardB.isSelected() && !onOperation){
			initOperation();
		}

		if ((Input.GetMouseButtonUp (0)) && !GameSettings.panelOpened) {
			
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 5);

			if(hit.collider != null && (hit.collider.gameObject.Equals(bu_verify.gameObject))){
				if(bu_verify.GetComponent<SpriteRenderer>().material.color.a == 1){
					verifyOperation();
				}
			}

			if(hit.collider != null && (hit.collider.gameObject.Equals(bu_end.gameObject))){
				if(bu_verify.GetComponent<SpriteRenderer>().material.color.a == 0){
					gameManager.playAudioBu_press();
					gameManager.openPause();
				}
			}

		}

		if (timeToNewTurn >= 0)
			timeToNewTurn -= Time.deltaTime;

		if(timeToNewTurn <= 0 && timeToNewTurn > -5 && cardA != null && cardB != null){
			Destroy(cardA.gameObject);
			Destroy(cardB.gameObject);
			cardA = null;
			cardB = null;
			gameManager.initTurn();
			timeToNewTurn = -10;
		}

	}

	public void addOrder(Order order){
		if (timeToNewTurn > 0)
			return;
		if (choosedOrder != null)
			choosedOrder.backToInit ();
		choosedOrder = order;
		choosedOrder.goToOperation (new Vector3 (5, -0.5f, 0), new Vector3 (5, 4, 1));
		bu_verify.fadeTo (1, 0.5f);
		bu_end.fadeTo (0, 0.2f);
	}
	
	public bool initOperation(){
		if (cardA == null || cardB == null || choosedOp == null)
			return false;

		choosedOp.fadeTo (1, 1.5f);
		eq.fadeTo (1, 1.5f);

		int value = cardA.getCardValue() + cardB.getCardValue();

		if(choosedOp.gameObject.name.Equals("Subtracao"))
			value = cardA.getCardValue() - cardB.getCardValue();
		if(choosedOp.gameObject.name.Equals("Multiplicacao"))
			value = cardA.getCardValue() * cardB.getCardValue();
		if(choosedOp.gameObject.name.Equals("Divisao"))
			value = cardA.getCardValue() / cardB.getCardValue();

		gameManager.initOrders (value);

		onOperation = true;
		return true;
	}

	public void setCardA(CardController newCardA){
		cardA = newCardA;
	}

	public void setCardB(CardController newCardB){
		cardB = newCardB;
	}

	public bool isOnOperation(){
		return onOperation;
	}

	public void initTurn(){
		onOperation = false;
	}

	private void verifyOperation(){

		int value = cardA.getCardValue() + cardB.getCardValue();
		if(choosedOp.gameObject.name.Equals("Subtracao"))
			value = cardA.getCardValue() - cardB.getCardValue();
		if(choosedOp.gameObject.name.Equals("Multiplicacao"))
			value = cardA.getCardValue() * cardB.getCardValue();
		if(choosedOp.gameObject.name.Equals("Divisao"))
			value = cardA.getCardValue() / cardB.getCardValue();

		if (choosedOrder.getOrderValue () == value) {
			gameManager.addRightPoint ();
			cardA.removeMeFromLearnScene ();
			cardB.removeMeFromLearnScene ();
			bu_verify.fadeTo (0, 0.5f);
			bu_end.fadeTo(1, 0.2f);
			choosedOp.fadeTo (0, 0.5f);
			eq.fadeTo (0, 0.5f);
			timeToNewTurn = 0.75f;
		} else {
			gameManager.addUnrightPoint ();
			//bu_verify.GetComponent<SpriteRenderer>().material.color = new Color(1,1,1,0);
			bu_verify.fadeTo(0, 0.5f);
			choosedOrder.backToInit();
			choosedOrder = null;
		}
	}
}
