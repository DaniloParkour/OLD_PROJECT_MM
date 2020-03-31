using UnityEngine;
using System.Collections;

public class OperationManager : MonoBehaviour {

	public GameManager gameManager;
	public ResponsePanelController responsePanel;

	//The 4 operators will be on scene all time.
	public Operator sum, sub, mul, div;
	public AuxiliarAnim equals;
	public GameObject bu_cancel;
	public GameObject bu_accept;

	public ResultController result;

	//The same juice will be user all game time. Is a script with the Juice animations and throw coins.
	public JuiceController juice;

	private CardController cardA;
	private CardController cardB;
	private bool onOperation;
	private bool onOperators;
	private bool operationChoosed;
	private bool waitingForResult;
	private Order resultValue;
	private Operator op; //is a choosed operation

	private Transform t;
	private bool buttonPressed;
	private int totalOperators;
	
	// Use this for initialization
	void Start () {

		//Remover depois
		//responsePanel.showPanel ("10 X 10 = 100");

		t = transform;
		onOperation = false;
		onOperators = false;

		sum.setUseThis (true);
		sum.gameObject.transform.position = new Vector3 (0, 0, 0);
		sum.gameObject.SetActive (false);

		sub.setUseThis (true);
		sub.gameObject.transform.position = new Vector3 (0, 0, 0);
		sub.gameObject.SetActive (false);

		div.setUseThis (true);
		div.gameObject.transform.position = new Vector3 (0, 0, 0);
		div.gameObject.SetActive (false);

		mul.setUseThis (true);
		mul.gameObject.transform.position = new Vector3 (0, 0, 0);
		mul.gameObject.SetActive (false);

		operationChoosed = false;
		waitingForResult = false;

		bu_cancel.SetActive (true);
		bu_cancel.transform.position = new Vector3 (0,-10,0);

		bu_accept.SetActive (true);
		bu_accept.transform.position = new Vector3 (0,-10,0);

		equals.GetComponent<SpriteRenderer> ().material.color = new Color (1,1,1,0);

	}
	
	// Update is called once per frame
	void Update () {
		if (onOperation) {
			if(bu_cancel.transform.position.y == -10){
				bu_cancel.transform.position = new Vector3 (0,0,0);
				bu_cancel.transform.localScale = new Vector3 (0,0,0);
				AuxiliarAnim bu_a = bu_cancel.GetComponent<AuxiliarAnim>();
				bu_a.moveTo (new Vector3(-1.5f,-2.5f,0), 0.5f);
				bu_a.scaleTo(new Vector3 (1,1,1), 0.5f);
			}
			if (!onOperators && ((Mathf.Abs (cardB.transform.position.x - (t.localPosition.x + 2)) < 0.05f && cardB.transform.position.y == t.localPosition.y) || totalOperators == 1)) {
				initOperators ();
				onOperators = true;
			}

			if (((Input.GetMouseButtonUp (0)) || totalOperators == 1) && !GameSettings.panelOpened && !GameSettings.cardOnZoom) {

				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 9);

				if((hit.collider != null) || totalOperators == 1 && !operationChoosed){
					operationChoosed = true;
					Operator[] ops;
					if(totalOperators == 1){
						ops = getAllOperators();
						for(int i = 0; i < ops.Length; i++){
							if(ops[i].isUseThis())
								op = ops[i];
						}
					}else {
						op = hit.collider.gameObject.GetComponent<Operator>();
						op.animOnChoose(new Vector3(-0.9f,0,0));
					}

					ops = getAllOperators();
					for(int i = 0; i < ops.Length; i++){
						if(!ops[i].gameObject.name.Equals(op.gameObject.name))
							ops[i].removeMeFromScene();
					}

					if(op == sub && cardA.getCardValue() < cardB.getCardValue()){
						CardController c_aux = cardA;
						cardA = cardB;
						cardB = c_aux;
						cardA.moveCardTo(cardB.transform.position);
					}

					if(totalOperators > 1)
						cardB.moveCardTo(new Vector3(0.2f,0,0));

					cardA.toScale (1f, 0.25f);
					cardB.toScale (1f, 0.25f);

					waitingForResult = true;
					equals.fadeTo(1,0.5f);

					int value = -99999;
					int a = cardA.getCardValue();
					int b = cardB.getCardValue();

					if(op == sum)
						value = a + b;
					else if(op == sub)
						value = a - b;
					else if(op == mul)
						value = a * b;
					else
						value = a / b;

					if(totalOperators > 1)
						gameManager.playAudioBu_press();

					gameManager.initOrders(value);
				}

				if(Input.GetMouseButtonUp(0)) {
					hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 5); //9 is the UI Layer

					if(hit.collider != null && !buttonPressed){
						if(hit.collider.gameObject.name.Equals("bu_cancel")) {

							AuxiliarAnim bu_an = hit.collider.gameObject.GetComponent<AuxiliarAnim>();
							if(!bu_an.isOnMove() && !bu_an.isOnScale()){
								gameManager.playAudioBu_press();
								Debug.Log("BU PRESSED DE CANCEL OPERATION!");
								cancelOperation();
								buttonPressed = true;
							}

						} else if(hit.collider.gameObject.name.Equals("bu_verify")) {

							AuxiliarAnim bu_an = hit.collider.gameObject.GetComponent<AuxiliarAnim>();
							if(!bu_an.isOnMove() && !bu_an.isOnScale()){
								cardA.setEnableClick(false);
								cardB.setEnableClick(false);
								verifyOperation();
								buttonPressed = true;
							}

						}
					}
				}
			}
		}
	}

	private void initOperators(){

		Vector3[] positions = {new Vector3 (-1, 0, 0), new Vector3 (1, 0, 0), new Vector3 (0, -1, 0), new Vector3 (0, 1, 0)};
		int currentOperatorPosition = 0;

		if(totalOperators == 3) {
			positions = new Vector3[3];
			positions[0] = new Vector3 (-1, -1, 0);
			positions[1] = new Vector3 (1, -1, 0);
			positions[2] = new Vector3 (0, 1, 0);
		} else if(totalOperators == 2) {
			positions = new Vector3[2];
			positions[0] = new Vector3 (-1, 0, 0);
			positions[1] = new Vector3 (1, 0, 0);
		} else if(totalOperators == 1) {
			positions = new Vector3[1];
			positions[0] = new Vector3 (-0.9f,0,0);

			cardB.moveCardTo(new Vector3(0.2f,0,0));
			cardA.toScale (1f, 0.25f);
			cardB.toScale (1f, 0.25f);

		}

		if (sum.isUseThis ()) {
			sum.gameObject.SetActive (true);
			sum.goToOperation (positions[currentOperatorPosition]);
			currentOperatorPosition++;
		} else
			sum.gameObject.SetActive (false);

		if (sub.isUseThis ()) {
			sub.gameObject.SetActive (true);
			sub.goToOperation (positions[currentOperatorPosition]);
			currentOperatorPosition++;
		} else
			sub.gameObject.SetActive (false);

		if (div.isUseThis ()) {
			div.gameObject.SetActive (true);
			div.goToOperation (positions[currentOperatorPosition]);
			currentOperatorPosition++;
		} else
			div.gameObject.SetActive (false);

		if (mul.isUseThis ()) {
			mul.gameObject.SetActive (true);
			mul.goToOperation (positions[currentOperatorPosition]);
		} else
			mul.gameObject.SetActive (false);

	}

	/*Tha X values of two cards are -2 and 2 on initOperation. The operaton will ask for the opetator
	 and after player choose an operator, the X values will be -2 and 0.*/
	public void initOperation(){

		if (onOperation)
			return;

		totalOperators = 4;
		
		if (!sum.isUseThis ())
			totalOperators --;
		if (!sub.isUseThis ())
			totalOperators --;
		if (!mul.isUseThis ())
			totalOperators --;
		if (!div.isUseThis ())
			totalOperators --;

		buttonPressed = false;
		cardA.toOperationLayer ();
		cardB.toOperationLayer ();
		cardA.moveCardTo (new Vector3((t.localPosition.x-2), t.localPosition.y, t.localPosition.z));

		if (totalOperators > 1)
			cardB.moveCardTo (new Vector3 ((t.localPosition.x + 2), t.localPosition.y, t.localPosition.z));
		else {
			cardB.moveCardTo (new Vector3 ((t.localPosition.x - 0.2f), t.localPosition.y, t.localPosition.z));
		}

		cardA.toScale (1.5f, 0.25f);
		cardB.toScale (1.5f, 0.25f);

		onOperation = true;
	}

	/*Recebe o pedido (resultado) e aceita-o se estiver na hora certa. Se não, retorna false*/
	public bool addResultOrder(Order order){
		if (waitingForResult) {
			gameManager.playAudioCoinClap();
			resultValue = order;
			order.goToOperation(new Vector3(2.2f,0,0));
			waitingForResult = false;

			bu_accept.transform.position = new Vector3 (0,0,0);
			bu_accept.transform.localScale = new Vector3 (0,0,0);
			AuxiliarAnim bu_a = bu_accept.GetComponent<AuxiliarAnim>();
			bu_a.moveTo (new Vector3(1.5f,-2.5f,0), 0.5f);
			bu_a.scaleTo(new Vector3 (1,1,1), 0.5f);

			return true;
		}

		if(order == resultValue){
			order.backToInit();
			waitingForResult = true;
			bu_accept.transform.position = new Vector3(0,-10,0);
		}

		return false;
	}

	public void animResult(){
		//Nao implementado
	}

	public void enableOperation(){
		//Nao implementado
	}

	//Anable the button thats verify the operation is right.
	public void enableBuVerify(){
		//Nao implementado
	}

	public void endTurn(){
		//Nao implementado
	}

	public bool isComplete(){
		//Nao implementado
		return false;
	}

	public void cancelOperation(){
		equals.fadeTo (0, 0.5f);
		if(cardA != null)
			cardA.resetCard ();
		if(cardB != null)
			cardB.resetCard ();
		if (sum.gameObject.activeSelf)
			sum.removeMeFromScene ();
		if (sub.gameObject.activeSelf)
			sub.removeMeFromScene ();
		if (mul.gameObject.activeSelf)
			mul.removeMeFromScene ();
		if (div.gameObject.activeSelf)
			div.removeMeFromScene ();
		gameManager.initTurn ();
	}

	private void verifyOperation() {

		string c_op = "";
		int value = -9999;
		int rightValue = -9999;

		if (op == null)
			return;
		else if (op == sum) {
			c_op = "+";
			value = cardA.getCardValue() + cardB.getCardValue();
		} else if (op == sub) {
			c_op = "-";
			value = cardA.getCardValue() - cardB.getCardValue();
		} else if (op == mul) {
			c_op = "x";
			value = cardA.getCardValue() * cardB.getCardValue();
		} else if (op == div) {
			c_op = "÷";
			value = cardA.getCardValue() / cardB.getCardValue();
		}

		int a = cardA.getCardValue ();
		int b = cardB.getCardValue ();

		if (value == resultValue.getOrderValue ()) {
			gameManager.playAudioBu_press();
			Debug.Log("Open Result!");
			gameManager.openResult (true, a + " " + c_op + " " + b + " = " + value, resultValue.getInitialPosition());
		} else {
			gameManager.playAudioError();
			gameManager.openResult (false, a + " " + c_op + " " + b + " = " + value, new Vector3(0,0,0));
			responsePanel.showPanel(a + " " + c_op + " " + b + " = " + value);
		}

	}

	/*Controller and managers use the function "initTurn()" to reset configuration to first or new turn.
	The game manager will call this functions on diferents objects on scene.*/
	public void initTurn(){
		onOperation = false;
		waitingForResult = false;
		operationChoosed = false;
		onOperators = false;
		bu_cancel.transform.position = new Vector3 (0,-10,0);
		bu_accept.transform.position = new Vector3 (0, -10, 0);
		resultValue = null;
	}

	//Getter and setter
	public CardController getCardA(){
		return cardA;
	}
	public void setCardA(CardController newCardA){
		if(newCardA == null)
			cardA = newCardA;
		else if (!onOperation)
			cardA = newCardA;
	}

	public CardController getCardB(){
		return cardB;
	}
	public void setCardB(CardController newCardB){
		if(newCardB == null)
			cardB = newCardB;
		else if (!onOperation)
			cardB = newCardB;
	}
	public bool isOnOperation(){
		return onOperation;
	}
	public bool getOnOperators(){
		return onOperators;
	}
	public void useSum(bool active){
		sum.setUseThis (active);
	}
	public void useSub(bool active){
		sub.setUseThis (active);
	}
	public void useMul(bool active){
		mul.setUseThis (active);
	}
	public void useDiv(bool active){
		div.setUseThis (active);
	}
	public bool isButtonPressed() {
		return buttonPressed;
	}

	private Operator[] getAllOperators(){
		Operator[] ops = {sum, sub, mul, div};
		return ops;
	}
}
