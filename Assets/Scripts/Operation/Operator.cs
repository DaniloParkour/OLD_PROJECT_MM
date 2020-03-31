using UnityEngine;
using System.Collections;

public class Operator : MonoBehaviour {

	private Vector3 position;
	private Vector3 initialPos;
	private Transform t;
	private bool useThis;
	private bool selected;
	private bool initOnScene; //The object is get in on scene.
	private bool onBright;
	private float timeToAnim;
	private float timeToBright;
	private float currentTime;
	private float vel;
	private SpriteRenderer sr;
	private bool translateMe;

	// Use this for initialization
	void Start () {
		vel = 5f;
		initOnScene = false;
		t = transform;
		translateMe = true;
		timeToBright = -10;
		onBright = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		if (initOnScene) {

			if (translateMe && !onBright)
				openOperator ();
			if(onBright)
				brightOperator();

			if(timeToAnim > -5 && timeToAnim <= 0) {
			
				sr.material.color = new Color (1, 1, 1, 1);
				t.localScale = new Vector3 (1.5f, 1.5f, 1);
				timeToAnim = -10;
				currentTime = 0.05f;
				translateMe = false;

				if(position == initialPos) {
					initOnScene = false;
					gameObject.SetActive(false);
				}
			}

		}
	}

	public void initOn(Vector3 pos){
		if (!useThis)
			return;

		position = pos;
		initOnScene = true;
		timeToAnim = 0.5f;
		currentTime = 0.05f;
		translateMe = true;

		sr = GetComponent<SpriteRenderer> ();

		sr.material.color = new Color (1, 1, 1, 0);
		t.localScale = new Vector3 (0,0,1);

	}

	private void operatorGoToPosition(){
		float dx, dy;
		float value = Time.deltaTime * vel;

		if (t.position.x <= (position.x - 0.1f))
			dx = 1;
		else if (t.position.x >= (position.x + 0.1f))
			dx = -1;
		else
			dx = 0;
		
		if (t.position.y < (position.y - 0.1f))
			dy = 1;
		else if (t.position.y > (position.y + 0.1f))
			dy = -1;
		else
			dy = 0;

		t.Translate (new Vector3(dx*value,dy*value,0));
		
		if(dx == 0 && dy == 0){
			t.position = new Vector3(position.x, position.y, position.z);
		}
	}

	public void openOperator(){

		operatorGoToPosition ();

		if (timeToAnim > -5) {
			currentTime -= Time.deltaTime;

			if (currentTime <= 0) {
				if(position != initialPos){
					sr.material.color = new Color (1, 1, 1, sr.material.color.a + 0.1f);
					t.localScale = new Vector3 (t.localScale.x + 0.2f, t.localScale.y + 0.2f, 1);
				} else {
					sr.material.color = new Color (1, 1, 1, sr.material.color.a - 0.1f);
					t.localScale = new Vector3 (t.localScale.x - 0.2f, t.localScale.y - 0.2f, 1);
				}
					timeToAnim -= 0.05f;
					currentTime = 0.05f;
			}

		}



	}

	private void brightOperator(){
		if (timeToBright <= -10) {
			timeToBright = 0.2f;
			sr.material.color = new Color (2, 2, 2, 1);
			onBright = true;
		} else if (timeToBright >= 0) {
			timeToBright -= Time.deltaTime;
		} else {
			timeToBright = -10;
			onBright = false;
		}

		if (timeToBright <= 0.1f && sr.material.color.r > 1) {
			sr.material.color = new Color (1, 1, 1, 1);
		}
	}

	public void animOnChoose(Vector3 toPosition){
		position = toPosition;
		selected = true;
		translateMe = true;
		brightOperator ();
	}

	public void goToOperation(Vector3 pos){
		initOn (pos);
	}

	public void removeMeFromScene(){
		position = initialPos;
		timeToAnim = 0.5f;
		currentTime = 0.05f;
		translateMe = true;
		timeToBright = -10;
		onBright = false;
	}



	/*Controller and managers use the function "initTurn()" to reset configuration to first or new turn.
	 The game manager will call this functions on diferents objects on scene.*/
	public void initTurn(){
		initialPos = new Vector3 (0, 0, 0);
		gameObject.SetActive (false);
	}

	//getters and setters
	public bool isInitOnScene(){
		return initOnScene;
	}

	public void setInitialPos(Vector3 pos){
		initialPos = pos;
	}

	public void setUseThis(bool useThis){
		this.useThis = useThis;
	}

	public bool isUseThis(){
		return useThis;
	}

	public bool isSelected(){
		return selected;
	}

}
