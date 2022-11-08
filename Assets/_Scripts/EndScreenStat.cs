using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreenStat : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] List<PlayerStatPanel> PlayerStats;
    [SerializeField] GameObject playerStatPref;
    [SerializeField] Text winner;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        PlayerStats = new List<PlayerStatPanel>();
        GameStateManager.Manager.GetStats().Sort((s1, s2) => s2.killCount.CompareTo(s1.killCount));
        for (int i = 0; i < GameStateManager.Manager.GetStats().Count; i++)
        {
            GameObject temp = Instantiate(playerStatPref,content.transform);
            RectTransform RT = temp.GetComponent<RectTransform>();
            RT.sizeDelta = new Vector2(518, 60);
            PlayerStats.Add(temp.GetComponent<PlayerStatPanel>());
            PlayerStats[i].SetText(GameStateManager.Manager.GetStats()[i].name, GameStateManager.Manager.GetStats()[i].killCount.ToString(), GameStateManager.Manager.GetStats()[i].killer);
        }
        winner.text = "Winner: "+ GameStateManager.Manager.GetWinnerName();
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
