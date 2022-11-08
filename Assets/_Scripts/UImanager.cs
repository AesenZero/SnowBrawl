using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UImanager : MonoBehaviour
{
    [SerializeField] List<Image> bulletIcons = new List<Image>();
    public static UImanager UIman;
    [SerializeField] Text txt;
    [SerializeField] Text Time_txt;
    [SerializeField] int Time;
    [SerializeField] ShootingSystem ShootingSys;
    [SerializeField] Image SkillButtonImg;
    [SerializeField] Text SkillAmount;
    const int transparent = 100;
    [SerializeField] Color fullColor;
    [SerializeField] Color transparColor;
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject Ending;
    [SerializeField] Text EndingText;
    [SerializeField] Text PlayersLeft;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject target;
    private void Awake()
    {
        
        UIman = this; 
    }


    // Update is called once per frame
    void Update ()
    {
        if (!GameStateManager.Manager.IsGameStarted()) return;
        if (Input.GetKeyDown(KeyCode.Escape) && !GameStateManager.Manager.GetPause()) OpenPauseMenu();
        foreach (Image img in bulletIcons)
        {
            img.color = transparColor;
        }
        for (int i = 0; i < ShootingSys.GetBullets(); i++)
        {          
            bulletIcons[i].color = fullColor;
        }
        Time = (int)GameStateManager.Manager.GetTimeToEnd();
        Time_txt.text = Time.ToString();
        PlayersLeft.text = GameStateManager.Manager.GetPlayers().Count.ToString();
    }

    
    public void ChangeSkillIcon(Sprite S)
    {
        SkillButtonImg.sprite = S;
        
    }

    public void DisableTarget()
    {
        target.SetActive(false);
    }

    public void ChangeSkillAmount(Skill b)
    {
        if(b.IsConsumable())
        {
            SkillAmount.text = b.GetAmount().ToString();
        }
        else
        {
            SkillAmount.text = "∞";
        }
    }

    public void Init()
    {
        ShootingSys = GameStateManager.Manager.GetPlayerShootingSystem();
        target.SetActive(true);
        fullColor = new Color32(255, 255, 255, 255);
        transparColor = new Color32(255, 255, 255, 100);
        Ending = GameObject.FindGameObjectWithTag("EndingUI");
        EndingText = Ending.GetComponentInChildren<Text>();
        Ending.SetActive(false);
    }

    public void ActivateEndingPanel()
    {
        Ending.SetActive(true);
    }

    public void ChangeEndingText(string s)
    {
        EndingText.text = s;
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        GameStateManager.Manager.PauseGame();
    }

}
