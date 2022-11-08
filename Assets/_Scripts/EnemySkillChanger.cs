using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillChanger : SkillChange
{
    // Start is called before the first frame update
    void Start()
    {
        MySS = GetComponent<EnemyShootingSystem>();
        K = 0;
        Balls[K].Activate();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    protected override void Change(Skill Ball)
    {
        MySS.SetProjectilePrefab(Ball.GetBall());
        MySS.SetKeyOfBall(Ball.GetKey());
        K = Ball.GetKey();   
    }

    public void ChangeSkill(int key)
    {
        Change(Balls[key]);
    }
}
