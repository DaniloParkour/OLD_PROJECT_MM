using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour {

	public Sprite front;
	public Sprite back;
	public Fruit fruit;

	private bool selected;
	private bool mouseDown;
	private bool spriteFront;
	private bool reseted;
	private SpriteRenderer sr;
	private SpriteRenderer sr_fruit;
	private Transform t;
	
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private float initialScale;
	private string initialLayer;
	private int initialNumLayer;
	private Vector3 moveTo;
	private float toScaleValue;
	private float toScaleTime;

	private float velFlip = 200;
	private float velSize = 0.2f;
	private float cardVelocity = 6;

	/*Variables to control the animations and actions*/
	private bool cardShow; //The card is on flip to show animation.
	private bool cardHide;
	private bool cardMoving;
	private bool onToScale;
	private bool enableClick;
	private bool onZoom;
	private float timeToClickZoom;
	private float anteriorSize;
	private Vector3 anteriorPosition;
	private string anteriorSortingLayer;
	private int anteriorSortingOrder;
	private float timeToRemoveMe;

	private int cardValue;

	void Awake(){
		sr = GetComponent<SpriteRenderer> ();
		sr.sprite = back;
		sr_fruit = fruit.GetComponent<SpriteRenderer> ();
		t = transform;
		toScaleValue = 1;
	}

	// Use this for initialization
	void Start () {
		spriteFront = false;
		cardShow = false;
		cardHide = false;
		initialPosition = new Vector3(t.localPosition.x, t.localPosition.y, t.localPosition.z);
		initialRotation = new Quaternion (t.localRotation.x, t.localRotation.y, t.localRotation.z, t.localRotation.w);
		enableClick = false;
		onZoom = false;
		timeToClickZoom = -10;
		reseted = false;
		timeToRemoveMe = -10;
		mouseDown = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (timeToRemoveMe > 0)
			timeToRemoveMe -= Time.deltaTime;

		//if (timeToRemoveMe <= 0 && timeToRemoveMe > -5)
		//	Destroy (this.gameObject);

		//if (timeToClickZoom > 0 && !onZoom)
		if (timeToClickZoom > 0)
			timeToClickZoom -= Time.deltaTime;

		if (cardShow)
			showCard ();
		if (cardHide)
			hideCard ();
		if (cardMoving)
			moveCard();
		if (onToScale)
			goToScale ();

		//if(!cardMoving)
		//	Debug.Log ("VEIO AQUI! 10 = "+sr.sortingOrder+" : "+t.localPosition.y +" = "+ initialPosition.y+".");

		if (sr.sortingOrder == 10 && !cardMoving && initialPosition.Equals(Vector3.zero)) {
			//Debug.Log ("E NAO VEIO AQUI!!!");
			sr.sortingOrder = 0;
			sr_fruit.sortingOrder = 0;
		}

		//if ((Input.GetMouseButtonDown (0)) && onZoom && t.localScale.x == 4) {
		//		clickToZoom();
		//}

		if (onZoom && t.localPosition == anteriorPosition && t.localScale.x == anteriorSize) {
			onZoom = false;
			GameSettings.cardOnZoom = false;
			sr.sortingLayerName = anteriorSortingLayer;
			sr.sortingOrder = anteriorSortingOrder;
			sr_fruit.sortingLayerName = anteriorSortingLayer;
			sr_fruit.sortingOrder = anteriorSortingOrder;
		}

		//Changes sprite when necessary.
		if(t.eulerAngles.y >= 90 && t.eulerAngles.y <= 270 && !spriteFront){
			sr.sprite = front;
			spriteFront = true;
			t.transform.localScale = new Vector3(t.localScale.x,t.localScale.y,1);
		} else if((t.eulerAngles.y > 270 || t.eulerAngles.y < 90) && spriteFront){
			sr.sprite = back;
			spriteFront = false;
			t.transform.localScale = new Vector3(t.localScale.x,t.localScale.y,1);
		}

		if(reseted && t.localPosition == initialPosition && t.localScale.x == initialScale){
			sr.sortingLayerName = initialLayer;
			sr.sortingOrder = initialNumLayer;
			sr_fruit.sortingLayerName = initialLayer;
			sr_fruit.sortingOrder = initialNumLayer;
			enableClick = true;
			reseted = false;
		}

		if (mouseDown && !Input.GetMouseButton (0)) {
			mouseDown = false;
		}
	
	}

	public void toScale(float scale, float time){
		toScaleValue = scale;
		toScaleTime = time;
		onToScale = true;
	}

	public bool selectCard(){
		if (!cardMoving && !onToScale && enableClick && mouseDown && !GameSettings.checkingOperation) {
			mouseDown = false;
			selected = true;
			cardShow = true;
			cardHide = false;
			return true;
		}

		return false;
	}

	public bool selectLearnCard(){
		if (!cardMoving) {
			mouseDown = false;
			selected = true;
			cardShow = true;
			cardHide = false;
			enableClick = true;
			return true;
		}
		
		return false;
	}

	public void hideThisCard(){
		mouseDown = false;
		selected = false;
		cardShow = false;
		cardHide = true;

	}

	public void moveCardTo(Vector3 position){
		if (sr.sortingOrder < 10) {
			sr.sortingOrder = 10;
			sr_fruit.sortingOrder = 10;
		}
		mouseDown = false;
		moveTo = position;
		cardMoving = true;
	}

	private void showCard(){
		if (cardMoving || onToScale || !enableClick)
			return;
		if ((t.eulerAngles.y < 180)) {
			t.Rotate (new Vector3 (0, 1, 0), velFlip * Time.deltaTime);
		} else {
			t.localRotation = new Quaternion(0,-1,0,0);
			cardShow = false;
		}
	}

	private void hideCard(){
		if ((t.eulerAngles.y < 360 && t.eulerAngles.y >= 180)) {
			t.Rotate (new Vector3 (0, 1, 0), velFlip * Time.deltaTime);
		} else {
			t.localRotation = new Quaternion(0,0,0,1);
			cardHide = false;
		}
	}

	private void moveCard(){

		if (t.localPosition.x == moveTo.x && t.localPosition.y == moveTo.y)
			cardMoving = false;

		float moveX = 0;
		float moveY = 0;

		moveX = t.localPosition.x - moveTo.x;
		moveY = t.localPosition.y - moveTo.y;

		Quaternion q = t.localRotation;

		t.localRotation = new Quaternion (0,0,0,1);

		if (moveX > 0.2f)
			t.Translate (-cardVelocity * Time.deltaTime, 0, 0);
		else if (moveX < -0.2f)
			t.Translate (cardVelocity * Time.deltaTime, 0, 0);
		else if (moveX != 0 && moveX >= -0.2f && moveX <= 0.2f)
			t.localPosition = new Vector3 (moveTo.x, t.localPosition.y, t.localPosition.z);

		if (moveY > 0.2f)
			t.Translate (0, -cardVelocity * Time.deltaTime, 0);
		else if (moveY < -0.2f)
			t.Translate (0, cardVelocity * Time.deltaTime, 0);
		else if (moveY != 0 && moveY >= -0.2f && moveY <= 0.2f)
			t.localPosition = new Vector3 (t.localPosition.x, moveTo.y, t.localPosition.z);

		t.localRotation = q;
	}

	private void goToScale(){
		float value = (toScaleValue - t.localScale.x)*(Time.deltaTime/toScaleTime);
		if (t.localScale.x != toScaleValue) {
			t.localScale = new Vector3 (t.localScale.x + value, t.localScale.y + value, 1);
		}

		if(Mathf.Abs(t.localScale.x - toScaleValue) <= 0.05f){
			t.localScale = new Vector3(toScaleValue, toScaleValue, 1);
			onToScale = false;
		}
	}

	public void clickToZoom(){

		if (cardMoving || onToScale || !enableClick)
			return;

		if (timeToClickZoom >= 0 && !onZoom && !GameSettings.cardOnZoom && t.localRotation.y == -1 && enableClick) {

			anteriorSize = t.localScale.x;
			anteriorPosition = new Vector3(t.localPosition.x, t.localPosition.y, t.localPosition.z);
			anteriorSortingLayer = sr.sortingLayerName;
			anteriorSortingOrder = sr.sortingOrder;

			sr.sortingLayerName = "GUI";
			sr.sortingOrder = 100;
			sr_fruit.sortingLayerName = "GUI";
			sr_fruit.sortingOrder = 100;
			moveCardTo(new Vector3(0,0,0));
			toScale(4,0.25f);
			onZoom = true;
			GameSettings.cardOnZoom = true;
			return;
		}

		if (t.localScale.x == 4 && timeToClickZoom >= 0) {
			moveCardTo (anteriorPosition);
			toScale(anteriorSize, 0.25f);
			timeToClickZoom = -10;
		}

		if (timeToClickZoom < 0)
			timeToClickZoom = 0.2f;
	}

	public void resetCard(){
		moveCardTo (initialPosition);
		toScale (initialScale, 0.5f);
		cardHide = true;
		selected = false;
		reseted = true;
	}

	public void removeMeFromScene(){
		enableClick = false;
		toScale (2, 1f);
		timeToRemoveMe = 2f;
	}

	public void removeMeFromLearnScene(){
		enableClick = false;
		toScale (4, 0.5f);
		timeToRemoveMe = 0.5f;
	}

	public void clickDown(){
		mouseDown = true;
	}

	/*Controller and managers use the function "initTurn()" to reset configuration to first or new turn.
	 The game manager will call this functions on diferents objects on scene.*/
	public void initTurn(){
		transform.localPosition = new Vector3 (initialPosition.x, initialPosition.y, initialPosition.z);
		transform.localRotation = new Quaternion (initialRotation.x, initialRotation.y, initialRotation.z, initialRotation.w);
		sr = GetComponent<SpriteRenderer> ();
		sr.sprite = back;
		t = transform;
		spriteFront = false;
		cardShow = false;
	}

	public void attInitialValues(){
		initialPosition = new Vector3(t.localPosition.x, t.localPosition.y, t.localPosition.z);
		initialRotation = new Quaternion (t.localRotation.x, t.localRotation.y, t.localRotation.z, t.localRotation.w);
		initialScale = t.localScale.x;
		initialLayer = sr.sortingLayerName;
		initialNumLayer = sr.sortingOrder;
	}

	public void toCardsLayer(){
		if(sr != null)
			sr.sortingLayerName = "Cards";
		if(fruit != null)
			fruit.GetComponent<SpriteRenderer> ().sortingLayerName = "Cards";
	}

	public void toOperationLayer(){
		if(sr != null)
			sr.sortingLayerName = "Operation";
		if(fruit != null)
			fruit.GetComponent<SpriteRenderer> ().sortingLayerName = "Operation";
	}

	//Getters and setters
	public bool isSelected(){
		return selected;
	}

	public bool onFlipCardShow(){
		return cardShow;
	}

	public bool onMoveCard(){
		return cardMoving;
	}

	public bool isOnToScale(){
		return onToScale;
	}

	public void setEnableClick(bool enable){
		enableClick = enable;
	}

	public bool isEnableClick(){
		return enableClick;
	}

	public bool isCardHide(){
		return cardHide;
	}

	public bool isOnZoom(){
		return onZoom;
	}

	public float getTimeToClickZoom(){
		return timeToClickZoom;
	}

	public int getCardValue(){
		return cardValue;
	}

	public void setCardValue(int value, Sprite newFruit){
		cardValue  = value;
		fruit.GetComponent<SpriteRenderer>().sprite = newFruit;
	}

	public Fruit getFruit(){
		return fruit;
	}

	public float getTimeToRemoveMe(){
		return timeToRemoveMe;
	}

}
