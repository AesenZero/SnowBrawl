using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public struct Skill 
{
    [SerializeField] string name;
    [SerializeField] GameObject Ball;
    [SerializeField] int Amount;
    [SerializeField] int key;
    [SerializeField]
    private bool isActive;
    [SerializeField] Sprite icon;
    [SerializeField]
    private bool consumable;
    [SerializeField] private int skillValue;
    public readonly bool isStatic;

    public void Disable()
    {
        Amount--;
        if(Amount <=0) isActive = false;
    }

    public void Activate()
    {
        Amount++;
        isActive = true;
    }

    public bool IsActive()
    {

        return isActive;
    }

    public bool IsConsumable()
    {
        return consumable;
    }

    public int GetValue()
    {
        return skillValue;
    }

    public string GetName()
    {
        return name;
    }

    public GameObject GetBall()
    {
        return Ball;
    }

    public int GetAmount()
    {
        return Amount;
    }

    public void SetAmount(int i)
    {
        Amount = i;
    }

    public int GetKey()
    {
        return key;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

   
}


public class SkillChange : MonoBehaviour
{

    [SerializeField] protected ShootingSystem MySS;

    [SerializeField] protected int K;
    [SerializeField] protected Skill[] Balls = new Skill[1];
    [SerializeField] KeyCode ChangeKey;


	// Use this for initialization
	void Start ()
    {
        MySS = GetComponent<ShootingSystem>();
        K = 0;
        Balls[K].Activate();
        
	}
	
    public Skill[] GetSkills()
    {
        return Balls;
    }

	// Update is called once per frame
	protected virtual void Update ()
    {

        if (Input.GetKeyDown(ChangeKey)) CmdNext();
	}

    
    protected virtual void Change(Skill Ball)
    {
      MySS.SetProjectilePrefab(Ball.GetBall());
      MySS.SetKeyOfBall(Ball.GetKey());
      IconChange(Ball.GetKey());


    }

    public void CmdNext()
    {
        int oldK = K;
        do
        {
            K = (K + 1) % Balls.Length;
            if (Balls[K].IsActive())
            {
                Change(Balls[K]);
                return;
            }

        }
        while (oldK!=K);
    }

    public void ChangeSkillPlayer(int i)
    {
        if (Balls[i].IsActive()) Change(Balls[i]);
    }

    public virtual void IconChange(int K)
    {
        UImanager.UIman.ChangeSkillIcon(GetIcon(K));
        UImanager.UIman.ChangeSkillAmount(Balls[K]);
    }

    public Sprite GetIcon(int i)
    {
        return Balls[i].GetIcon();
    }
    
}
