using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkillBall : BulletSkill
{
    
    SkillSpawn skSpawn;
    [SerializeField] private int skillsAmount;
    [SerializeField] private int skillNumber;
    [SerializeField] GameObject SkillPlace;
    

	// Use this for initialization
	new void Start ()
    {
        base.Start();
        skSpawn = GameStateManager.Manager.GetStage().GetComponent<SkillSpawn>();
        skillsAmount = skSpawn.GetAmountOfSkills();
        skillNumber = GetSkillNumber(skillsAmount);
        if (skillNumber == 0)
        {
            tag = "Bullet";
            isDouble = true;
            BulletLocation = BulletPlace;
        }
        else BulletLocation = SkillPlace;
        
        
        
	}
	

    int GetSkillNumber(int skillsAmount)
    {
        return Random.Range(0, skillsAmount);
    }


    private void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "Stage")
        {

            Effect();


        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        FoolStop(coll.gameObject);
    }

    protected override void Effect()
    {
        Kin.velocity *= 0;
        bCanWeTakeIt = true;
    }

    public int GetSkillNumber()
    {
        return skillNumber;
    }

    public void SetSkillSpawn(SkillSpawn sSpawn)
    {
        skSpawn = sSpawn;
    }





}
