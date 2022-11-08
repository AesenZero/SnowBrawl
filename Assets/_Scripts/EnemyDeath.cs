using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : Death
{

    // Update is called once per frame
    protected override void Update()
    {
        if (IDontFeelSoGood())
        {
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


}
