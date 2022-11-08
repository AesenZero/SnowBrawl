using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingSystem : ShootingSystem
{
    [SerializeField] EnemyAI myAI;
    [SerializeField] float curShootingDelay;
    [SerializeField] float shootingDelay;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public void Shoot()
    {
        if (curShootingDelay <= 0)
        {
            if (KeyOfBall == 0 || KeyOfBall == 3) AS.Play();
            CmdShoot();
            curShootingDelay = shootingDelay;
        }else curShootingDelay -= Time.deltaTime;
    }



}
