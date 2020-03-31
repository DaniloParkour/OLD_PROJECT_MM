using UnityEngine;
using System.Collections;

public class NumberGeneration : MonoBehaviour {

	public GameObject[] numbers;
	public GameObject minus;

	private bool negative;

	// Use this for initialization
	void Start () {

		//Remover DEPOIS ....................................................................................
		/*GameObject[] nums = getNumbers (119037);
		for(int i = 0; i < nums.Length; i++){
			nums[i].transform.position = new Vector3((i - nums.Length/2)*0.25f,0,0);
			SpriteRenderer sr = nums[i].GetComponent<SpriteRenderer>();
			if(sr != null)
				sr.sortingLayerName = "GUI";
		}*/
		//....................................................................................................

	}

	public GameObject[] numToObjects(int number){
		if (number < 0) {
			number *= -1;
			negative = true;
		} else {
			negative = false;
		}
		return getNumbers (number);
	}

	private GameObject[] getNumbers(int number) {

		if(number / 10 < 1){
			GameObject[] retornoFinal;

			if(!negative) {
				retornoFinal = new GameObject[1];
				retornoFinal[0] = getNumObj(number);
			} else {
				retornoFinal = new GameObject[2];
				retornoFinal[0] = Instantiate (minus);
				retornoFinal[1] = getNumObj(number);
			}
			return retornoFinal;
		}

		int valueNum = number % 10;
		GameObject[] retorno = new GameObject[1];
		retorno[0] = getNumObj(valueNum);

		GameObject[] otherNums = getNumbers (number / 10);
		GameObject[] accNums = new GameObject[otherNums.Length + 1];

		for(int i = 0; i < accNums.Length; i++){
			if(i+1 == accNums.Length){
				accNums[i] = retorno[0];
			} else {
				accNums[i] = otherNums[i];
			}
		}

		return accNums;
	}

	private GameObject getNumObj(int value){
		for (int i = 0; i < 10; i++) {
			if (value == i)
				return Instantiate (numbers [i]);
		}
		return null;
	}

	//Getters and setters

}
