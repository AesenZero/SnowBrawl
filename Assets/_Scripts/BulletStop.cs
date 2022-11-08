using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletStop : BulletSkill
{


    // Use this for initialization
    new void Start()
    {
        base.Start();
        key = 0;
        isStatic = false;
#if UNITY_EDITOR
        name = name + Random.Range(0000, 99999);
#endif

    }

    // Update is called once per frame
    void Update()
    {
        isStatic = bCanWeTakeIt;
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
        if(coll.gameObject.tag == "Player" && coll.gameObject != Instigator && !bCanWeTakeIt)
        {
            AS.Play();
        }
        FoolStop(coll.gameObject);
    }

    protected override void Effect()
    {
        Kin.velocity *= 0;
        bCanWeTakeIt = true;
        Instigator = null;

    }
}
