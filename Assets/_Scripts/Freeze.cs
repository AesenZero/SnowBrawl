using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Freeze : BulletSkill
{
    
    [SerializeField] protected int BulletSpeed;

	// Use this for initialization
	new void Start ()
    {
        base.Start();
        key = 2;
        isStatic = false;
        Kin.velocity = ParentForward * BulletSpeed;
    }
	

    private void OnTriggerEnter(Collider ot)
    {


        if (ot.gameObject.tag == "Player" && ot.gameObject != Instigator)
        {
            
            RpcEffect(ot.gameObject);

        }
    }

    protected void RpcEffect(GameObject player)
    {
        PlayerFreezing pFreeze = player.GetComponent<PlayerFreezing>();

        pFreeze.Freezing();
        Destroy(gameObject);
    }

    protected void Destroying()
    {
        Invoke("Death", 10f);
    }
}
