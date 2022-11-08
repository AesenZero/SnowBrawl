using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Death : MonoBehaviour
{

    [SerializeField] protected bool gotHit;
    [SerializeField] protected GameObject Instigator;
    [SerializeField] protected float gotHitDuration = 5f;
    [SerializeField] protected float gotHitChecker = 0f;
    [SerializeField] protected int MinHeight = -15;
    [SerializeField] private Camera DeathCam;
    
    // Update is called once per frame
    protected virtual void Update ()
    {
		if(IDontFeelSoGood())
        {

            Instantiate(DeathCam);
            CmdDead();
        }
        if (gotHitChecker > 0f)
        {
            gotHitChecker -= Time.deltaTime;
        }
        else
        {
            gotHit = false;
            Instigator = null;
        }

    }

   protected bool IDontFeelSoGood()
   {
        return gameObject.transform.position.y < MinHeight;
   }

    protected void CmdDead()
    {
        ShootingSystem mySS = GetComponent<ShootingSystem>();
        PlayersStat myStat;
        if (Instigator != null)
        {
            ShootingSystem SS = Instigator.GetComponent<ShootingSystem>();
            print(Instigator.name + " killed " + gameObject.name);
            SS.IncreaseKillCount();
            myStat = new PlayersStat(gameObject.name, mySS.GetKillCount(), false, Instigator.name);
        }
        else
        {
            myStat = new PlayersStat(gameObject.name, mySS.GetKillCount());
        }

        GameStateManager.Manager.GetStats().Add(myStat);
        GameStateManager.Manager.GetPlayers().Remove(gameObject);
        if(gameObject.name == "Player") GameStateManager.Manager.DisableTarget();
        Destroy(gameObject);
    }

    protected void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "SkillBall" || coll.gameObject.tag == "Bullet")
        {
            BulletSkill bs = coll.gameObject.GetComponent<BulletSkill>();
            Instigator = bs.GetInstigator();
            if (Instigator == GameStateManager.Manager.gameObject) return;
            gotHit = true;
            gotHitChecker = gotHitDuration;
        }
    }

    protected void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "SkillBall" || coll.gameObject.tag == "Bullet")
        {
            BulletSkill bs = coll.gameObject.GetComponent<BulletSkill>();          
            Instigator = bs.GetInstigator();
            if (Instigator == GameStateManager.Manager.gameObject) return;
            gotHit = true;
            gotHitChecker = gotHitDuration;
        }
    }
}
