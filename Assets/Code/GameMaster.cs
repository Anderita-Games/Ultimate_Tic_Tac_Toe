using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
	public GameObject Square_Prefab;
	int Buffer = 3;
	public int Levels = 2;
	float Size;

	//UI Shit TODO: Make this better
	public UnityEngine.UI.Text Score_X;
	public UnityEngine.UI.Text Score_Y;
	public UnityEngine.UI.Text Score_Tie;
	public UnityEngine.UI.Text Title_X;
	public UnityEngine.UI.Text Title_Y;
	public UnityEngine.UI.Text Title_Tie;

	//GAME VARS
	public string Current_Player;
	public int Active_Section = 0;
	public int Squares_Taken = 0;
	public int[] Section_Max;
	public string Winner;
	public bool Restart = false;

	void Start () {
		Section_Max = new int[10];
		Size = this.gameObject.GetComponent<RectTransform>().sizeDelta.x / 9.0f - Buffer;
		Score_X.text = PlayerPrefs.GetInt("X_Score").ToString();
		Score_Y.text = PlayerPrefs.GetInt("Y_Score").ToString();
		Score_Tie.text = PlayerPrefs.GetInt("Tie_Score").ToString();
		Starting();
	}

	void Update () {
		if (Squares_Taken == 81 && Restart == false) {
			Restart = true;
			Active_Section = -1;
			Score_Tie.text = (int.Parse(Score_Tie.text) + 1).ToString();
		}
		if (Restart == true) {
			Score_X.color = Color.white;
			Score_Y.color = Color.white;
			Score_Tie.color = Color.white;
			Title_X.color = Color.white;
			Title_Y.color = Color.white;
			Title_Tie.color = Color.white;
			if (Input.GetMouseButtonDown(0)) {
				PlayerPrefs.SetInt("X_Score", int.Parse(Score_X.text));
				PlayerPrefs.SetInt("Y_Score", int.Parse(Score_Y.text));
				PlayerPrefs.SetInt("Tie_Score", int.Parse(Score_Tie.text));
				SceneManager.LoadScene("MainMenu");
			}
		}else {
			Score_Tie.color = Color.grey;
			Title_Tie.color = Color.grey;
			if (Current_Player == "X") {
				Score_X.color = Color.white;
				Title_X.color = Color.white;
				Score_Y.color = Color.grey;
				Title_Y.color = Color.grey;
			}else if (Current_Player == "O") {
				Score_Y.color = Color.white;
				Title_Y.color = Color.white;
				Score_X.color = Color.grey;
				Title_X.color = Color.grey;
			}
		}
	}

	void Starting () {
		PlayerPrefs.SetInt("X_Score", 0);
		PlayerPrefs.SetInt("Y_Score", 0);
		PlayerPrefs.SetInt("Tie_Score", 0);
		Current_Player = "X"; //TODO: Smart change
		for (int a = 0; a < Levels; a++) {
			GameObject Container = new GameObject();
			Container.transform.SetParent(GameObject.Find("Middle UI").transform);
			Container.name = "Level " + a;
		}
		for (int x = -4; x <= 4; x++) {
			for (int y = -4; y <= 4; y++) {
				GameObject Square = Instantiate(Square_Prefab, GameObject.Find("Middle UI").transform);
				Square.GetComponent<RectTransform>().anchoredPosition = new Vector3(x * (Size / 2) + (x * Buffer), y * (Size / 2) + (y * Buffer));
				Square.GetComponent<RectTransform>().sizeDelta = new Vector2 (Size, Size);
				Square.GetComponent<Square>().Section = Mathf.CeilToInt((x + 5) / 3.0f) + (Mathf.CeilToInt((y - 2) / -3.0f) * 3);
				Square.GetComponent<Square>().Block = (x + 5 - (Mathf.CeilToInt((x + 2) / 3f) * 3)) + (((y - 4) * -3) - (Mathf.CeilToInt((y - 2) / -3.0f) * 9));
				Square.name = "Square " + Square.GetComponent<Square>().Section + " | " + Square.GetComponent<Square>().Block;
				Square.transform.SetParent(GameObject.Find("Level 0").transform);
			}
		}
	}
}
