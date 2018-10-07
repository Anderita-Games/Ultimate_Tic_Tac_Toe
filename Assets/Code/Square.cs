using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
	public UnityEngine.UI.Text Text;
	public int Section;
	public int Block;
	public string Possesor;
	public int Level = 0;
	GameMaster Canvas;
	public UnityEngine.UI.RawImage Background;

	void Start () {
		Canvas = GameObject.Find("Canvas").GetComponent<GameMaster>();
	}

	void Update () {
		if ((Canvas.Active_Section == Section || Canvas.Active_Section == 0) && Level != Canvas.Levels - 1 && Level == 0 && Canvas.Section_Max[Section] != 9) {
			Background.color = Color.gray;
		}else {
			Background.color = Color.black;
		}
	}

	public void OnClick () {
		if (Text.text == "" && (Canvas.Active_Section == Section || Canvas.Active_Section == 0 && Level == 0)) {
			Text.text = Canvas.Current_Player;
			Possesor = Text.text;
			Canvas.Squares_Taken++;
			Canvas.Section_Max[Section]++;
			if (Canvas.Current_Player == "X") {
				Canvas.Current_Player = "O";
			}else {
				Canvas.Current_Player = "X";
			}
			if (GameObject.Find("Level " + Level + "/Square " + Block + " | 1") && Canvas.Section_Max[Block] != 9) {
				Canvas.Active_Section = Block;
			}else {
				Canvas.Active_Section = 0;
			}
			Check();
		}
	}

	public void Check () { //Checking for complete ownership
		string Square_Preface = "Level " + Level + "/Square " + Section + " | ";
		for (int a = 1; a <= 3; a++) { //Vertical Check
			if (GameObject.Find(Square_Preface + a) && GameObject.Find(Square_Preface + (a + 3)) && GameObject.Find(Square_Preface + (a + 6))) {
				string Temp_Possesor = GameObject.Find(Square_Preface + a).GetComponent<Square>().Possesor;
				if (Temp_Possesor != "" && Temp_Possesor != null) {
					if (GameObject.Find(Square_Preface + (a + 3)).GetComponent<Square>().Possesor == Temp_Possesor) {
						if (GameObject.Find(Square_Preface + (a + 6)).GetComponent<Square>().Possesor == Temp_Possesor) {
							if (this.GetComponent<Square>().Level == Canvas.Levels - 1) {
								Win();
							}else {
								Enlarge(Temp_Possesor);
							}
						}
					}
				}
			}
		}
		for (int a = 1; a <= 7; a += 3) { //Horizontal Check
			if (GameObject.Find(Square_Preface + a) && GameObject.Find(Square_Preface + (a + 1)) && GameObject.Find(Square_Preface + (a + 2))) {
				string Temp_Possesor = GameObject.Find(Square_Preface + a).GetComponent<Square>().Possesor;
				if (Temp_Possesor != "" && Temp_Possesor != null) {
					if (GameObject.Find(Square_Preface + (a + 1)).GetComponent<Square>().Possesor == Temp_Possesor) {
						if (GameObject.Find(Square_Preface + (a + 2)).GetComponent<Square>().Possesor == Temp_Possesor) {
							if (this.GetComponent<Square>().Level == Canvas.Levels - 1) {
								Win();
							}else {
								Enlarge(Temp_Possesor);
							}
						}
					}
				}
			}
		}
		if (GameObject.Find(Square_Preface + "5")) {
			string Temp_Possesor2 = GameObject.Find(Square_Preface + "5").GetComponent<Square>().Possesor;
			if (Temp_Possesor2 != "") { //45° Lines
				if (GameObject.Find(Square_Preface + "1") && GameObject.Find(Square_Preface + "9")) {
					if (GameObject.Find(Square_Preface + "1").GetComponent<Square>().Possesor == Temp_Possesor2) {
						if (GameObject.Find(Square_Preface + "9").GetComponent<Square>().Possesor == Temp_Possesor2) {
							if (this.GetComponent<Square>().Level == Canvas.Levels - 1) {
								Win();
							}else {
								Enlarge(Temp_Possesor2);
							}
						}
					}
				}
				if (GameObject.Find(Square_Preface + "3") && GameObject.Find(Square_Preface + "7")) {
					if (GameObject.Find(Square_Preface + "3").GetComponent<Square>().Possesor == Temp_Possesor2) {
						if (GameObject.Find(Square_Preface + "7").GetComponent<Square>().Possesor == Temp_Possesor2) {
							if (this.GetComponent<Square>().Level == Canvas.Levels - 1) {
								Win();
							}else {
								Enlarge(Temp_Possesor2);
							}
						}
					}
				}
			}
		}
	}

	void Enlarge (string possesor) {
		GameObject The_Future = GameObject.Find("Level " + Level + "/Square " + Section + " | 5");
		for (int i = 1; i <= 9; i++) {
			if (i != 5 && i != Block) { //Gameobject.find shouldnt need to exist
				Destroy(GameObject.Find("Level " + Level + "/Square " + Section + " | " + i));
			}
		}
		Canvas.Squares_Taken = Canvas.Squares_Taken - Canvas.Section_Max[Section] + 9;
		The_Future.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = possesor;
		The_Future.GetComponent<Square>().Possesor = possesor;
		The_Future.GetComponent<Square>().Block = Section;
		The_Future.GetComponent<Square>().Level += 1;
		The_Future.transform.localScale = new Vector3(The_Future.transform.localScale.x * 3, The_Future.transform.localScale.y * 3, 1);
		The_Future.name = "Square " + The_Future.GetComponent<Square>().Level + " | " + Section;
		The_Future.GetComponent<Square>().Section = 1;
		The_Future.transform.SetParent(GameObject.Find("Level " + The_Future.GetComponent<Square>().Level).transform);
		if (Block == Section) {
			Canvas.Active_Section = 0;
		}
		The_Future.GetComponent<Square>().Check();
		if (The_Future != this.gameObject) {
			Destroy(this.gameObject);
		}
	}

	void Win () {
		Canvas.Restart = true;
		Canvas.Winner = Possesor;
		if (Possesor == "X") {
			Canvas.Score_X.text = (int.Parse(Canvas.Score_X.text) + 1).ToString();
		}else if (Possesor == "O") {
			Canvas.Score_Y.text = (int.Parse(Canvas.Score_Y.text) + 1).ToString();
		}
		Canvas.Active_Section = -1;
	}
}
