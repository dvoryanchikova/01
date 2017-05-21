using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	public bool eat;

	// Update is called once per frame
	void Update () {
		if(eat)
		{
			transform.position = new Vector3(Random.Range(-18,18),Random.Range(-5,5),0);
			gameObject.GetComponent<SpriteRenderer> ().enabled = true;

			eat = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{	
		if(other.tag == "Pray")
		{
			eat = true;
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;

		}
	}


}
