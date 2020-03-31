using UnityEngine;
using System.Collections;

public class FadePlane : MonoBehaviour {

	private Transform t_load;
	private float timeToAnim;
	private AuxiliarAnim anim;
	private float timeToDestroy;

	// Use this for initialization
	void Start () {
		if(t_load == null)
			t_load = transform.Find ("Loading").transform;
		timeToAnim = 0.1f;
		anim = GetComponent<AuxiliarAnim> ();
		timeToDestroy = -10;
	}
	
	// Update is called once per frame
	void Update () {

		if(timeToDestroy > -5)
			timeToDestroy -= Time.deltaTime;
		else {

		}

		timeToAnim -= Time.deltaTime;

		if (timeToAnim <= 0) {
			t_load.Rotate(new Vector3(0,0,-1), 45);
			timeToAnim = 0.1f;
		}

		if (Application.GetStreamProgressForLevel("MainScene") == 1) {
			transform.rotation = new Quaternion (0, 0, 0, 1);
			Destroy(this.gameObject);
		} else {

			Debug.Log ("Value: " + Application.GetStreamProgressForLevel ("MainScene"));

		}

	}

	public void fadeIn(float time){
		anim.fadeTo (1, time);
	}
	public void fadeOut(float time){
		anim.fadeTo (1, time);
	}
}
