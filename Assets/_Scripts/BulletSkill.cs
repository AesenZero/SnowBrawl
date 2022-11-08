using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class BulletSkill : MonoBehaviour
{
    [SerializeField] protected Rigidbody Kin;
    [SerializeField] protected GameObject Instigator;
    [SerializeField] protected ShootingSystem Parent;
    [SerializeField] protected GameObject BulletLocation;
    [SerializeField] protected bool bCanWeTakeIt = false;
    [SerializeField] protected bool isStatic;
    [SerializeField] protected Vector3 ParentForward;
    [SerializeField] protected int key;
    [SerializeField] protected AudioSource AS;
    [SerializeField] protected GameObject BulletPlace;
    [SerializeField] protected bool isDouble = false;
    // Use this for initialization
    protected void Start ()
    {
        Kin = GetComponent<Rigidbody>();
        AS = GetComponent<AudioSource>();
        BulletLocation = BulletPlace;
        
    }
	
	// Update is called once per frame
	void Update ()
    {

    }


    public bool CanWeTakeIt()
    {
        return bCanWeTakeIt;
    }

    public void TakeThis()
    {
        bCanWeTakeIt = false;
        BulletLocation.SetActive(false);
    }

    protected virtual void Effect()
    {
        return;
    }

    protected virtual void Effect(GameObject player)
    {
        return;
    }

    protected void Death()
    {
        Destroy(gameObject);
    }


    protected void FoolStop(GameObject gg)
    {
        if (gg.tag == "Stage")
        {
            Kin.velocity *= 0;
            GameStateManager.Manager.GetSpawner().GetSkillBalls().Add(gameObject);
            BulletLocation.SetActive(true);
        }
    }


    protected void OnDestroy()
    {
        GameStateManager.Manager.GetShotSkills().Remove(this);
    }

    public GameObject GetInstigator()
    {
        return Instigator;
    }

    public void SetInstigator(GameObject g)
    {
        Instigator = g;
    }



    public bool IsItDouble()
    {
        return isDouble;
    }

    public void SetParent(ShootingSystem g)
    {
        Parent = g;
    }

    public ShootingSystem GetParent()
    {
        return Parent;
    }

    public bool IsItStatic()
    {
        return isStatic;
    }

    public void SetIsStatic(bool b)
    {
        isStatic = b;
    }

    public Vector3 GetParentForward()
    {
        return ParentForward;
    }

    public void SetParentForward(Vector3 v)
    {
        ParentForward = v;
    }

    public int GetKey()
    {
        return key;
    }    

}
