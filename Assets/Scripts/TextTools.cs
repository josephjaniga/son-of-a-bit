using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class TextTools {

	public static GameObject sct;

	public static string WordFinder2(int requestedLength){
		UnityEngine.Random rnd = new UnityEngine.Random();
		string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
		string[] vowels = { "a", "e", "i", "o", "u" };
		string word = "";
		if (requestedLength == 1){
			word = GetRandomLetter(rnd, vowels);
		} else {
			for (int i = 0; i < requestedLength; i+=2){
				word += GetRandomLetter(rnd, consonants) + GetRandomLetter(rnd, vowels);
			}
			word = word.Replace("q", "qu").Substring(0, requestedLength); // We may generate a string longer than requested length, but it doesn't matter if cut off the excess.
		}
		return char.ToUpper(word[0]) + word.Substring(1);
	}
	
	public static string GetRandomLetter(UnityEngine.Random rnd, string[] letters){
		return letters[Mathf.RoundToInt(UnityEngine.Random.Range(0, letters.Length - 1))];
	}

	public static void clearAlerts(){
		// clear all the alerts
		foreach (Transform child in GameObject.Find("Alerts").transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	public static void createAlert(string Message){

		if ( sct == null ){
			sct = Resources.Load("Prefabs/UI/SCT") as GameObject;
			sct.GetComponent<SCT>().isEnabled = false;
		}

		GameObject alert = GameObject.Instantiate(sct, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
		alert.GetComponent<Text>().text = Message;
		alert.transform.SetParent(GameObject.Find("Alerts").transform);
		alert.transform.localPosition = new Vector3(0f, -80f, 0f);
		alert.GetComponent<Text>().fontSize = 12;
		alert.GetComponent<SCT>().Timer = 6;
		alert.GetComponent<SCT>().Timeout = 6;

	}

}
