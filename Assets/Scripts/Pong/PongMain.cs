using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PongMain : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;

	public GameObject ball;

	public int p1_score = 0;
	public int p2_score = 0;

	public GameObject Player1Score;
	public GameObject Player2Score;

	// Use this for initialization
	void Start () {

		GameObject.Find("Backdrop").renderer.material.color = new Color(0.1f, 0.1f, 0.1f);

	}
	
	// Update is called once per frame
	void Update () {
	
		Player1Score.GetComponent<Text>().text = ""+p1_score;
		Player2Score.GetComponent<Text>().text = ""+p2_score;

	}

	public void updateScore(int p1, int p2){
		p1_score = p1;
		p2_score = p2;
	}

	public void reset(){
		player1.GetComponent<PlayerOne>().reset();
		player2.GetComponent<PlayerTwo>().reset();
		ball.GetComponent<Ball>().reset();
	}

}
