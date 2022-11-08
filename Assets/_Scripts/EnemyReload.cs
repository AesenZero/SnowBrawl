using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReload : Reload
{

    [SerializeField] EnemyMovement EM;

    [SerializeField] int enoughSkillValue;
    [SerializeField] int skillValue = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        shootSys = GetComponent<ShootingSystem>();
        skChange = GetComponent<SkillChange>();
        EM = GetComponent<EnemyMovement>();

    }



    protected override void CmdGetSkill(GameObject ot)
    {
        if (ot == null)
            return;

        SkillBall skBall = ot.GetComponent<SkillBall>();
        if (skBall.CanWeTakeIt())
        {
            int temp = skBall.GetSkillNumber();
            skChange.GetSkills()[temp].Activate();
            skillValue += skChange.GetSkills()[temp].GetValue();
            GameStateManager.Manager.GetSpawner().GetSkillBalls().Remove(ot);
            Destroy(ot);
        }
    }

    protected override void CmdTakingUp(GameObject ot)
    {
        if (ot == null)
            return;

        BulletSkill bull = ot.GetComponent<BulletSkill>();
        if (bull.CanWeTakeIt())
        {
            if(shootSys.GetBullets() < shootSys.GetBulletMax())
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
            else 
            {
                bull.TakeThis();
                GameStateManager.Manager.GetSpawner().GetSkillBalls().Remove(ot);
                Destroy(ot);
            }

        }
    }

    public bool IsSkillValueEnough()
    {
        return (skillValue>=enoughSkillValue);
    }

    public void ReduceSkillValue(int i)
    {
        skillValue -= i;
    }
}
