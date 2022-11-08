using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.Manager.GetPause()) Close();
	}

	public void MainMenu()
    {
		GameStateManager.Manager.UnpauseGame();
		SceneManager.LoadScene("StartMenu");
    }

	public void Retry()
    {
		GameStateManager.Manager.UnpauseGame();
		SceneManager.LoadScene("GameScene");
    }

	public void Close()
    {
		GameStateManager.Manager.UnpauseGame();
		gameObject.SetActive(false);
    }
}
