using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Cube : MonoBehaviour {
	public ParticleSystem xanh;

	AudioSource[] sources;
	private float m_TransitionIn;
	private float m_TransitionOut;
	private float m_QuarterNote;
	public AudioSource run;
	public GameObject scoreObject;
	int score=0;
	int positionNewPlane = -2000;
	int nextPlanePositionX = -4000;
	int positionNewCube = 1500;
	int nextCubePositionX = 0;
	float Cube_Z=0;
	Color red = new Color (1, 0, 0, 1);
	Color yellow = new Color (1, 0.92f, 0.016f, 1);
	Color blue = new Color (0, 0, 1, 1);
	Color playerColor;
	float moveSpeed = 600f;// tốc độ theo chiều doc
	float runSpeed = 1000f; // tốc độ theo chiều ngang
	float jumpHeight = 120f;
	public GameObject player;
	public ParticleSystem  vang;
	public ParticleSystem red1;
	float baseSpeed = 1000f;
	void Start () {
		
		//effect = GetComponent<ParticleSystem> ();
	//	print (effect);	
//		effect.Play ();

		//m_QuarterNote = 60 / bpm;
		//m_TransitionIn = m_QuarterNote;
		//m_TransitionOut = m_QuarterNote * 32;
		player = GameObject.Find("Cube");
		sources = GameObject.FindSceneObjectsOfType(typeof(AudioSource)) as AudioSource[];
		print (sources.Length);
		playerColor = player.GetComponent<Renderer> ().material.color;

	}

	// Update is called once per frame
	void Update () {
		
		Vector3 current = transform.position;
		if (current.y <= 25) {
			sources [2].Play ();
			transform.Translate (-moveSpeed * Time.deltaTime, jumpHeight * Input.GetAxis ("Jump"), runSpeed * Input.GetAxis ("Vertical") * Time.deltaTime);

		} ;
		print (current.y);
		if (current.y > 25) {

			transform.Translate (-moveSpeed * Time.deltaTime, -2f, runSpeed * Input.GetAxis ("Vertical") * Time.deltaTime);

		} ;
		if (current.x < positionNewPlane+2000) {
			GameObject plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
			plane.GetComponent<MeshCollider> ().enabled = false; 
			plane.transform.position = new Vector3 (nextPlanePositionX, 0, 0);
			plane.transform.localScale = new Vector3 (500, 20, 50);
			plane.GetComponent<Renderer> ().material.color = new Color (0.234375f, 0.421875f, 0.79296875f, 1);
			positionNewPlane -= 4000;
			nextPlanePositionX -= 4000;
		}
		if (current.x < (positionNewCube + 3000)) {
			int flag = Random.Range (0, 10);
			print ("them");
			var cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.position = new Vector3 (nextCubePositionX, 0, Cube_Z);
			cube.transform.localScale = new Vector3 (10, 120, 200);
			positionNewCube -= 500;
			nextCubePositionX -= 500;
			Cube_Z = Random.Range (-150f, 148f);

			int distanceCube_X = Random.Range (300, 400);
			int flagColor=Random.Range(0,3);
			if (flagColor == 0) {
				cube.name = "red_cube";
				cube.GetComponent<Renderer> ().material.color = red;
			}  else if (flagColor == 1) {
				cube.name = "blue_cube";
				cube.GetComponent<Renderer> ().material.color = blue;
			}  else {
				cube.name = "yellow_cube";
				cube.GetComponent<Renderer>().material.color = yellow;
			}

			//them Sphere
			int flagSphere=Random.Range(1,5);



			if (flagSphere == 1) {
				var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphere.transform.position = new Vector3 (nextCubePositionX+Random.Range(200,300), Random.Range(0,100), Cube_Z+Random.Range(-50,50));
				if (flagColor == 0) {
					sphere.name = "red_sphere";
					sphere.GetComponent<Renderer> ().material.color = red;
				}  else if (flagColor == 1) {
					sphere.name = "blue_sphere";
					sphere.GetComponent<Renderer> ().material.color = blue;
				}  else {
					sphere.name = "yellow_sphere";
					sphere.GetComponent<Renderer>().material.color = yellow;
				}

				var sphereCollider = (BoxCollider)sphere.AddComponent<BoxCollider>();
				sphereCollider.center = new Vector3(0,0,0);
				sphere.transform.localScale = new Vector3 (80, 80, 80);
			}
		}

		if (current.z < -280 || current.z > 280) {
			
			print ("out of land => dead");
			runSpeed = 0;
			moveSpeed = 0;
			jumpHeight = 0;
		}
	}
	void OnCollisionEnter(Collision collision)
	{ 
		
		if (collision.transform.name == "blue_sphere" ) {
			ParticleSystem sc = Instantiate(xanh, collision.transform.position, Quaternion.identity) as ParticleSystem;
//			ParticleSystem.Play ();
			//Destroy(collision.gameObject);
			player.GetComponent<Renderer> ().material.color = blue;
			playerColor = blue;
			score+=15;
			sources [0].Play ();
			print ("change to blue");
			scoreObject.GetComponent<Text> ().text = score.ToString();	
		}  else if (collision.transform.name == "red_sphere") {
			ParticleSystem sc = Instantiate(red1, collision.transform.position, Quaternion.identity) as ParticleSystem;
			//Destroy(collision.gameObject);
			player.GetComponent<Renderer> ().material.color = red;
			playerColor = red;
			score+=15;
			sources [0].Play ();
			scoreObject.GetComponent<Text> ().text = score.ToString();
			print ("change to red");
		}  else if (collision.transform.name == "yellow_sphere" ) {
			ParticleSystem sc = Instantiate(vang, collision.transform.position, Quaternion.identity) as ParticleSystem;
			//Destroy(collision.gameObject);
			player.GetComponent<Renderer> ().material.color = yellow;
			playerColor = yellow;
			score+=15;
			sources [0].Play ();
			scoreObject.GetComponent<Text> ().text = score.ToString();
			print ("change to yellow");
		}  else if (collision.transform.name == "blue_cube" && playerColor == blue) {
			ParticleSystem sc = Instantiate(xanh, collision.transform.position, Quaternion.identity) as ParticleSystem;
			Destroy(collision.gameObject);
			score+=5;
			sources [0].Play ();
		scoreObject.GetComponent<Text> ().text = score.ToString();
		}  else if (collision.transform.name == "red_cube" && playerColor == red) {
			ParticleSystem sc = Instantiate(red1, collision.transform.position, Quaternion.identity) as ParticleSystem;
			Destroy(collision.gameObject);
			score+=5;
			sources [0].Play ();
 			scoreObject.GetComponent<Text> ().text = score.ToString();
		}  else if (collision.transform.name == "yellow_cube" && playerColor == yellow) {
			ParticleSystem sc = Instantiate(vang, collision.transform.position, Quaternion.identity) as ParticleSystem;
			Destroy(collision.gameObject);
			score+=5;
			scoreObject.GetComponent<Text> ().text = score.ToString();
		}  else {
			sources [1].Play ();
			print ("dead");
			moveSpeed = 0;
			runSpeed = 0;
			jumpHeight = 0;
		}
		runSpeed = baseSpeed + Mathf.Round (score / 1000) * 50;
	}
	void OnCollisionExit(Collision collisionInfo) {
		print("Exit cube " + collisionInfo.transform.name);


	}

}

