using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerFreezing : MonoBehaviour
{
    [SerializeField] GameObject FreezeObj;
    [SerializeField] Character_Movement MyMove;

	// Use this for initialization
	void Start ()
    {
        MyMove = GetComponent<Character_Movement>();
	}
	


    public void Freezing()
    {
        FreezeObj.SetActive(true);
        MyMove.SetFreezeDebuff(true);
        Invoke("AntiFreeze", 4f);
    }

    public void AntiFreeze()
    {
        FreezeObj.SetActive(false);
        MyMove.SetFreezeDebuff(false);
    }

    
}
