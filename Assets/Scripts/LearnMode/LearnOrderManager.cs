using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LearnOrderManager : MonoBehaviour {

	public Order[] orders;
	public LearnGameManager gameManager;
	
	private float timeToInitOrders;
	private Order choosedOrder;
	private bool closeOrders;
	private int[] orderValues;
	private NumberGeneration genNums;
	private bool blockToBack;
	private bool onInitOrders;
	private Vector3 initialScale;

	// Use this for initialization
	void Start () {
		timeToInitOrders = -10;
		closeOrders = false;
		orderValues = new int[orders.Length];
		genNums = GetComponent<NumberGeneration> ();
		onInitOrders = false;
		initialScale = orders[0].transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToInitOrders > -5)
			timeToInitOrders -= Time.deltaTime;
		
		if (timeToInitOrders < 0 && timeToInitOrders > -1) {
			
			for(int i = 0; i < orders.Length; i++){
				
				if(!closeOrders && !orders[i].gameObject.activeSelf){
					orders[i].gameObject.SetActive(true);
				}
				
				if(!closeOrders && !orders[i].isOnShowMe() && onInitOrders){
					//orders[i].showMe(orders[i].transform.position, orderValues[i]);
					orders[i].showMeLearn(orders[i].transform.position, orderValues[i],initialScale);
					GameObject[] nums = genNums.numToObjects(orderValues[i]);
					for(int posI = 0; posI < nums.Length; posI++){
						if(nums[posI] != null) {
							nums[posI].gameObject.transform.SetParent(orders[i].transform);
							nums[posI].gameObject.SetActive(true);
							SpriteRenderer sr_n = nums[posI].GetComponent<SpriteRenderer>();
							sr_n.material.color = new Color(1,1,1,1);
							sr_n.color = new Color(1,1,1,1);
							sr_n.sortingLayerName = "Operation";
							sr_n.sortingOrder = 10;
						}
					}
					
					if(i+1 == orders.Length){
						timeToInitOrders = -10;
						onInitOrders = false;
					}
					break;
					
				} else if(closeOrders && orders[i].isOnShowMe()){
					Color cor = orders[i].GetComponent<SpriteRenderer>().material.color;
					orders[i].desapear();
					if(i+1 == orders.Length){
						timeToInitOrders = -10;
						onInitOrders = false;
					}
				}
			}
			if(timeToInitOrders != -10)
				timeToInitOrders = 0.5f;
		}
		
		//Verify click on asny order
		if ((Input.GetMouseButtonUp (0)) && !GameSettings.panelOpened ) {
			
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 10);
			
			if(hit.collider != null){
				Order order = hit.collider.gameObject.GetComponent<Order>();
				//if(order != null && order.GetComponent<SpriteRenderer>().material.color.a == 1){
				if(order != null && !closeOrders){
					gameManager.playAudioCheck_press();
					gameManager.chooseOrder(order);
				}
			}
		}
	}

	public void initOrders(int value){
		
		if (onInitOrders)
			return;
		
		blockToBack = false;
		
		int randomValue = Random.Range (0, orders.Length);
		int min, max;
		
		if (value >= 0) {
			min = value - 5;
			max = value + 5;
			if(min < 0){
				max -= min;
				min = 0;
			}
		} else {
			min = value - 5;
			max = value + 5;
			if(max > 0){
				min += max;
				max = 0;
			}
		}
		
		for (int i = 0; i < orderValues.Length; i++)
			orderValues [i] = -99999;
		
		for(int i = 0; i < orderValues.Length; i++){
			bool jaTem;
			do {
				int newValue = Random.Range(min, max);
				jaTem = isOnOrders(newValue);
				orderValues[i] = newValue;
			} while((orderValues[i] == value) || jaTem);
		}
		
		orderValues[randomValue] = value;
		
		closeOrders = false;
		onInitOrders = true;
		timeToInitOrders = 0.5f;
		
	}
	
	private bool isOnOrders(int value){
		bool retorno = false;
		
		for (int i = 0; i < orderValues.Length; i++)
		if (value == (orderValues[i])) {
			retorno = true;
		}
		
		return retorno;
	}
	
	public void removeOrders () {
		closeOrders = true;
		timeToInitOrders = 0.2f;
		onInitOrders = false;
	}
	
	public Order getChoosedOrder(){
		return choosedOrder;
	}
	
	public void initTurn(){
		removeOrders ();
		for (int i = 0; i < orders.Length; i++)
			if (orders [i] != null && orders [i].gameObject.activeSelf) {
				orders [i].resetMe ();
			}
		choosedOrder = null;
	}
	
	public bool isOnInitOrders(){
		return onInitOrders;
	}
}