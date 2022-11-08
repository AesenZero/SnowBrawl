using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersCountHolder : MonoBehaviour
{

    [SerializeField] int playersCount;
    [SerializeField] bool GodMode;

    static PlayersCountHolder me;
    // Start is called before the first frame update
    void Awake()
    {
        if (me == null) me = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }


    public void SetPlayersCount(int i)
    {
        playersCount = i;
    }

    public int GetPlayersCount()
    {
        return playersCount;
    }

    public void SetGodMode(bool b)
    {
        GodMode = b;
    }

    public bool GetGodMode()
    {
        return GodMode;
    }
}
