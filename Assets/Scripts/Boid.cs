using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {


	public float sight = 75f;
	public float sight_to_enemy = 1f;
	public float space = 3f;

	public Boid prey;
	public GameObject[] food;

	public bool enemy;
	public bool yes;

	public Vector3 center, v,v1,v2,v3,v4,v5,v7;

	public Boid[] Boids;
	public List<Boid> preys;
	public List<Boid> enemies;


	void Start () {

		if (gameObject.tag == "Enemy")
			enemy = true;
		else
			enemy = false;
		

		Boids = FindObjectsOfType<Boid> ();

		foreach(var boid in Boids)
		{
			if (boid.enemy == false)
				preys.Add (boid);
			else
				enemies.Add (boid);
		}
		//для ручной установки обновления кадров
		//InvokeRepeating ("MoveSwarm", 0f, 0.1f);
	}

	
	void Update () {

		MoveSwarm ();
		if(!enemy) 
		{
			//Debug.DrawLine (transform.position, v, Color.black);
		}
		else
		{
			//Debug.DrawLine(v5, transform.position, Color.red);
		}
	  /*Debug.DrawLine(transform.position, center, Color.gray);
		Debug.DrawLine(transform.position, v1, Color.red);
		Debug.DrawLine(transform.position, food [0].transform.position, Color.green);
		Debug.DrawLine(transform.position, v4, Color.red);
		if(prey != null)
			Debug.DrawLine(transform.position, prey.transform.position, Color.red); */

		transform.position += new Vector3 (v.x, v.y, 0)*Time.deltaTime;	

	}
		
	void MoveSwarm()
	{
		if (!enemy)
		PrayMove (preys);
		
	else
		EnemyMove (enemies);

	Rule4 (Boids);
	Rule5 (Boids);

		v =  3*v1 + 0.5F*v2 + 2 * v3 + v7 + 5*v4 + 5*v5;

	Rule8 (Boids);
	Rule6 (Boids);

	}

	void PrayMove(List<Boid> boids)
	{
		Rule1 (boids);
		Rule2 (boids);
		Rule3 (boids);
		Rule7 (boids);
	}

	void EnemyMove(List<Boid> boids)
	{
		Rule1 (boids);
		Rule2 (boids);
		Rule3 (boids);
	}

	void Rule1 (List<Boid> boids)
	{
		v1 = Vector3.zero;
		foreach(var boid in boids)
		{
			if(boid.gameObject != gameObject 
				&& (boid.transform.position - gameObject.transform.position).magnitude < space) 
			{
				v1 += (gameObject.transform.position - boid.transform.position) / (transform.position - boid.transform.position).magnitude;
			}
		}
	}

	void Rule2(List<Boid> boids)
	{

		v2 = Vector3.zero;
		center = Vector3.zero;
		foreach(var boid in boids)
		{
			if (boid.gameObject != gameObject && (boid.transform.position - gameObject.transform.position).magnitude < space*2)
			{
				center += boid.transform.position;
			}
			center /= boids.Count;
			v2 = center - gameObject.transform.position;
		}       
	}

	void Rule3(List<Boid> boids)
	{
		v3 = Vector3.zero;
		foreach (var boid in boids)
		{
			if(boid.gameObject!= gameObject && (boid.transform.position - gameObject.transform.position).magnitude < sight)
				v3 += boid.v; 
		}
		v3 = v3 / 8f;
	}

	void Rule4(Boid[] boids)
	{
		v4 = Vector3.zero;
		v5 = Vector3.zero;
		prey = null;
		foreach(var boid in boids)
		{
			if (boid.enemy && (enemy == false) && (boid.transform.position - gameObject.transform.position).magnitude < sight_to_enemy)
			{
				v4 += transform.position - boid.transform.position;
				v7 = Vector3.zero;
			} 
		}
		
	}
	void Rule5(Boid[] boids)
	{
		v5 = Vector3.zero;
		prey = null;
		foreach (var boid in boids)
		{
			if( boid.gameObject != gameObject && (boid.enemy!=true) && enemy && (boid.transform.position - gameObject.transform.position).magnitude < sight)
			{
				prey = boid;
			}
			if(enemy && (boid.transform.position - gameObject.transform.position).magnitude > sight)
			{
				prey = null;
				boid.gameObject.GetComponent<Boid> ().space = 9f;
				
			}	
			if( prey != null)
			{
				v5 += prey.transform.position - transform.position;
				boid.GetComponent<Boid> ().space = 2f;
			}
		}
	}
		

	void Rule6(Boid[] boids)
	{
		foreach(var boid in boids)
		{
			if (!enemy)
				boid.v = Vector3.ClampMagnitude (boid.v, 10);
			else
				boid.v = Vector3.ClampMagnitude (boid.v, 4);
		}
	}

	void Rule7(List<Boid> boids)
	{
		v7 = Vector3.zero;
		foreach(var boid in boids)
		{
			if( (boid.gameObject.transform.position - food[0].transform.position).magnitude < sight)
			{
				v7 += food [0].transform.position - boid.transform.position;
			}

		}
	}

	void Rule8(Boid[] boids)
	{
		foreach (var boid in boids) 
		{
			if (boid.transform.position.magnitude > 15)
				v += -transform.position.normalized;
		}
	}
}
