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

    protected override void CmdShoot()
    {
        sv = (shootingPosition.position - shootingStart.position).normalized;
        projectile = Instantiate(projectilePrefab, shootingPosition.position, transform.rotation);
        if (skChange.GetSkills()[KeyOfBall].IsConsumable())
        {
            skChange.GetSkills()[KeyOfBall].Disable();
            if (skChange.GetSkills()[KeyOfBall].GetAmount() == 0) skChange.CmdNext();
        }
        Kin = projectile.GetComponent<Rigidbody>();
        ReloadSys = projectile.GetComponent<BulletSkill>();
        GameStateManager.Manager.GetShotSkills().Add(ReloadSys);
        ReloadSys.SetInstigator(gameObject);
        ReloadSys.SetParentForward(gameObject.transform.forward);
        Kin.velocity = shootingSpeed * (sv);
        ReloadSys.SetParent(gameObject.GetComponent<ShootingSystem>());
        projectile = null;
        Bullets--;


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
