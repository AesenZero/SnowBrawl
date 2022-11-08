using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Text txt;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Rigidbody Kin;
    [SerializeField] protected int shootingSpeed;
    //public Bullet bullets;
    public KeyCode shootingButton;
    [SerializeField] protected Transform shootingPosition;
    [SerializeField] protected Transform shootingStart;
    [SerializeField] protected int Bullets;
    [SerializeField] protected int BulletMax = 4;
    protected Vector3 sv;
    [SerializeField] protected BulletSkill ReloadSys;
    [SerializeField] protected SkillChange skChange;
    [SerializeField] protected int KeyOfBall;
    [SerializeField] protected AudioSource AS;

    [SerializeField] protected int killCount = 0;

    [SerializeField] protected AudioClip BulletSound;
    [SerializeField] protected AudioClip IceSound;
    [SerializeField] protected AudioClip RocketSound;
    bool godmod = false;
    // Use this for initialization
    virtual protected void Start ()
    {
        sv = (shootingPosition.position-shootingStart.position).normalized;
        if(skChange==null) skChange = GetComponent<SkillChange>();
        AS = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    virtual protected void Update ()
    {

        ChangeSkillInput();
        if (Input.GetKeyDown(shootingButton) && Bullets > 0)
        {
            AS.clip = selectShootingSound(KeyOfBall);
            AS.Play();
            CmdShoot();
        }
        
        
    }

    protected void CmdShoot()
    {
        sv = (shootingPosition.position - shootingStart.position).normalized;
        projectile = Instantiate(projectilePrefab, shootingPosition.position, transform.rotation);
        if (skChange.GetSkills()[KeyOfBall].IsConsumable())
        {
            skChange.GetSkills()[KeyOfBall].Disable();
            if(skChange.GetSkills()[KeyOfBall].GetAmount() == 0) skChange.CmdNext();
        }
        Kin = projectile.GetComponent<Rigidbody>();
        ReloadSys = projectile.GetComponent<BulletSkill>();
        GameStateManager.Manager.GetShotSkills().Add(ReloadSys);
        ReloadSys.SetInstigator(gameObject);
        ReloadSys.SetParentForward(gameObject.transform.forward);
        Kin.velocity = shootingSpeed * (sv);
        ReloadSys.SetParent(gameObject.GetComponent<ShootingSystem>());
        projectile = null;
        if(!godmod) Bullets--;



    }

    public int GetKillCount()
    {
        return killCount;
    }
    
    public void IncreaseKillCount()
    {
        killCount++;
    }

    protected AudioClip selectShootingSound(int i)
    {
        switch(i)
        {
            case 0:
                return BulletSound;
            case 1:
                return BulletSound;
            case 2:
                return IceSound;
            case 3:
                return RocketSound;
            default:
                return null;
        }


    }

    private void ChangeSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) skChange.ChangeSkillPlayer(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) skChange.ChangeSkillPlayer(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) skChange.ChangeSkillPlayer(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) skChange.ChangeSkillPlayer(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) skChange.ChangeSkillPlayer(4);
    }

    public void GainGodPowers()
    {
        skChange = GetComponent<SkillChange>();
        godmod = true;
        Bullets = 4;
        for (int i = 0; i < skChange.GetSkills().Length; i++)
        {
            skChange.GetSkills()[i].Activate();
            skChange.GetSkills()[i].SetAmount(999);           
        }
    }

    public int GetKeyOfBall()
    {
        return KeyOfBall;
    }

    public void SetKeyOfBall(int i)
    {
        KeyOfBall = i;
    }

    public int GetBullets()
    {
        return Bullets;
    }

    public void IncreaseBullets()
    {
        Bullets++;
    }

    public int GetBulletMax()
    {
        return BulletMax;
    }

    public void SetBullets(int i)
    {
        Bullets = i;
    }    

    public void SetProjectilePrefab(GameObject g)
    {
        projectilePrefab = g;
    }
}
