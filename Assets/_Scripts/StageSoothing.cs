using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StageSoothing : MonoBehaviour
{
    [SerializeField] private Vector3 stageScale;
    public GameStateManager Man;

    [SerializeField] private Vector3 stageStartScale;
	// Use this for initialization
	void Start ()
    {
        Man = GameStateManager.Manager;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        Soothing();
    }

    private float GetScaleKoef()
    {
        return Man.GetTimeToEnd() / Man.GetFullGameTime();
    }
    
    void Soothing()
    {
        if (Man.GetTimeToEnd() > 0)
        {
            stageScale = Vector3.Lerp(Vector3.zero, stageStartScale, GetScaleKoef());
            stageScale.y = stageStartScale.y;
            transform.localScale = stageScale;
        }
    }

    public void Init()
    {
        stageStartScale = Man.playersCoef * Man.GetPlayerBunch() * 10f * Vector3.one;
        stageStartScale.y = 0.15f;
        stageScale = stageStartScale;
        transform.localScale = stageScale;
    }
}
