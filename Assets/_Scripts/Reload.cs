using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Reload : MonoBehaviour
{
    [SerializeField] protected ShootingSystem shootSys;
    [SerializeField] protected Character_Movement CM;
    [SerializeField] protected SkillChange skChange;
    public KeyCode TakeUP;

    [SerializeField] protected float bulletCreationTime;
    [SerializeField] protected float bulletCreationThreshold;
    [SerializeField] protected float tempCreating = 0f;


    protected virtual void Start()
    {
        shootSys = GetComponent<ShootingSystem>();
        CM = GetComponent<Character_Movement>();
        skChange = GetComponent<SkillChange>();
    }
    protected virtual void Update()
    {
       
    }

    protected virtual void OnTriggerStay(Collider ot)
    {
        if (ot.gameObject.tag == "Bullet")
        {
            CmdTakingUp(ot.gameObject);
        }

        if (ot.gameObject.tag == "SkillBall")
        {
            CmdGetSkill(ot.gameObject);
        }
    }

    protected virtual void CmdTakingUp(GameObject ot)
    {
        if (ot == null)
            return;

        BulletSkill bull = ot.GetComponent<BulletSkill>();
        if (bull.CanWeTakeIt() && shootSys.GetBullets() < shootSys.GetBulletMax())
        {
            shootSys.IncreaseBullets();
            if (bull.IsItDouble())
            {
                shootSys.IncreaseBullets();
                shootSys.SetBullets(Mathf.Min(shootSys.GetBullets(), shootSys.GetBulletMax()));
            }
            bull.TakeThis();
            GameStateManager.Manager.GetSpawner().GetSkillBalls().Remove(ot);
            Destroy(ot);
        }
        else if (bull.CanWeTakeIt())
        {
            bull.TakeThis();
            GameStateManager.Manager.GetSpawner().GetSkillBalls().Remove(ot);
            Destroy(ot);
        }
    }


    protected virtual void CmdGetSkill(GameObject ot)
    {
        if (ot == null)
            return;

        SkillBall skBall = ot.GetComponent<SkillBall>();
        if (skBall.CanWeTakeIt())
        {
            int temp = skBall.GetSkillNumber();
            skChange.GetSkills()[temp].Activate();
            GameStateManager.Manager.GetSpawner().GetSkillBalls().Remove(ot);
            Destroy(ot);
        }
    }
}
