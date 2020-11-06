using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Game : MonoBehaviour
{
    public Text playerDisplay;
    public Text scoreDisplay;

	private void Awake()
	{
		if (!DBManager.LoggedIn)
		{
			SceneManager.LoadScene(0);
		}
		playerDisplay.text = "Player: " + DBManager.username;
		scoreDisplay.text = "Score: " + DBManager.score;
	}

	public void CallSaveData()
	{
		StartCoroutine(SavePlayerData());

	}

	IEnumerator SavePlayerData()
	{
		WWWForm form = new WWWForm();
		form.AddField("name", DBManager.username);
		form.AddField("score", DBManager.score);

		UnityWebRequest request = UnityWebRequest.Post("http://localhost/sqlconnect/savedata.php", form);
		yield return request.SendWebRequest();

		if (request.downloadHandler.text == "0")
		{
			Debug.Log("Game Saved");
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
		else
		{
			Debug.Log("Save Failed. Error #" + request.downloadHandler.text);
		}

		DBManager.LogOut();
	}

	public void IncreaseScore()
	{
		DBManager.score++;
		scoreDisplay.text = "Score: " + DBManager.score;
	}
}
