using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public struct PlayersStat
{
    public string name;
    public int killCount;
    public string killer;
    public bool isWinner;

    public PlayersStat(string n, int k, bool b = false, string ki = "none")
    {
        name = n;
        killCount = k;
        killer = ki;
        isWinner = b;
    }
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Manager;

    [SerializeField] float TimeToEnd;
    [SerializeField] float FullGameTime;
    [SerializeField] GameObject stage;
    [SerializeField] GameObject centerOfTheStage;
    [SerializeField] ShootingSystem playerShootSys;
    [SerializeField] List<GameObject> players;
    [SerializeField] List<BulletSkill> shotSkills;
    [SerializeField] SkillSpawn spawner;
    [SerializeField] StageSoothing stageS;
    public readonly int playersCoef = 4;
    [SerializeField] int amountOfPlayersAI = 0;
    [SerializeField] int playersBunch { get { return (int)((players.Count) / playersCoef - 0.01f)+1; } }
    [SerializeField] int stageCoef { get { return(int)((amountOfPlayersAI+1)/playersCoef-0.01f)+1; } }
    [SerializeField] int amountOfSkillsPerSpawn;
    [SerializeField] bool gameStarted = false;
    [SerializeField] bool gameEnded = false;
    [SerializeField] List<PlayersStat> stats = new List<PlayersStat>();
    [SerializeField] UImanager UIm;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerAIPrefab;
    [SerializeField] string winnerName;
    [SerializeField] bool GodMode;
    [SerializeField] bool isPaused;
    private void Awake()
    {
        if (Manager != null)
        {
            Debug.LogError("Singletone eto ploho, pomogite v menegere");
        }
        Manager = this;
        UIm = GetComponent<UImanager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted && !isPaused)
        {
            TimeToEnd -= (Time.deltaTime);
            if(players.Count == 1 && !gameEnded)
            {
                ShootingSystem SS = players[0].GetComponent<ShootingSystem>();
                winnerName = players[0].name;
                PlayersStat winner = new PlayersStat(winnerName, SS.GetKillCount(), true);
                stats.Add(winner);
                UIm.ActivateEndingPanel();
                UIm.ChangeEndingText(players[0].name + " Win");
                Invoke("EndScreen", 5f);
                gameEnded = true;
            }
        }
    }

    public float GetTimeToEnd()
    {
        return TimeToEnd;
    }

    public float GetFullGameTime()
    {
        return FullGameTime;
    }    

    public float GetStageRad()
    {
        return stage.transform.localScale.x / 2f;
    }

    public GameObject GetStage()
    {
        return stage;
    }

    public GameObject GetCenterOfStage()
    {
        return centerOfTheStage;
    }

    public ShootingSystem GetPlayerShootingSystem()
    {
        return playerShootSys;
    }

    public List<GameObject> GetPlayers()
    {
        return players;
    }

    public List<BulletSkill> GetShotSkills()
    {
        return shotSkills;
    }

    public SkillSpawn GetSpawner()
    {
        return spawner;
    }

    public StageSoothing GetStageSoothing()
    {
        return stageS;
    }
    
    public int GetPlayerBunch()
    {
        return playersBunch;
    }

    public string GetWinnerName()
    {
        return winnerName;
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    public List<PlayersStat> GetStats()
    {
        return stats;
    }

    public void InitGame()
    {
        PlayersCountHolder PCH = GameObject.FindGameObjectWithTag("PlayersCount").GetComponent<PlayersCountHolder>();
        SetAmountOfPlayersAI(PCH.GetPlayersCount());
        GodMode = PCH.GetGodMode();
        TimeToEnd = FullGameTime;    
        float temp = 360 / (amountOfPlayersAI + 1);
        GameObject g = Instantiate(playerPrefab, -centerOfTheStage.transform.forward.normalized * stageCoef*20 * 0.9f, Quaternion.identity);

        playerShootSys = g.GetComponent<ShootingSystem>();
        if (GodMode) playerShootSys.GainGodPowers();
        g.name = "Player";
        for (int i = 0; i < amountOfPlayersAI; i++)
        {
            g = Instantiate(playerAIPrefab, Quaternion.Euler(0, temp * (i + 1), 0) * -centerOfTheStage.transform.forward.normalized * stageCoef * 20 * 0.9f, Quaternion.identity);
            g.name = "Enemy "+ i.ToString();
        }
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        amountOfSkillsPerSpawn = playersBunch;
        spawner = stage.GetComponent<SkillSpawn>();
        spawner.SetSkillPerSpawn(amountOfSkillsPerSpawn);
        stageS = stage.GetComponent<StageSoothing>();
        stageS.Man = this;
        stageS.Init();

        UIm.Init();
        gameStarted = true;
    }

    void EndScreen()
    {
        SceneManager.LoadScene("EndScene");
    }

    private void SetAmountOfPlayersAI(int i)
    {
        amountOfPlayersAI = i;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public bool GetPause()
    {
        return isPaused;
    }

    public void DisableTarget()
    {
        UIm.DisableTarget();
    }
}
