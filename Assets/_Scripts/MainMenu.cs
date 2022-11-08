using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] InputField input;
    [SerializeField] PlayersCountHolder holder;
    [SerializeField] Toggle GodModeToggle;
    // Start is called before the first frame update

    private void Start()
    {
        if (holder == null) holder = GameObject.FindGameObjectWithTag("PlayersCount").GetComponent<PlayersCountHolder>();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Tutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        ChangePlayersAI();
    }

    public void ChangePlayersAI()
    {
        if (holder == null) holder = GameObject.FindGameObjectWithTag("PlayersCount").GetComponent<PlayersCountHolder>();
        holder.SetGodMode(GodModeToggle.isOn);
        try
        {
            holder.SetPlayersCount(int.Parse(input.text));
        }
        catch(Exception e)
        {
            holder.SetPlayersCount(0);
        }
        
    }
}
