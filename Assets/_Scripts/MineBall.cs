using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBall : BulletSkill
{
    [SerializeField] GameObject explodeSound;
    [SerializeField] GameObject MinePlace;

    new void Start()
    {
        base.Start();
        Kin.velocity *= 0;
        Vector3 temp = (-Instigator.transform.forward - Instigator.transform.position).normalized+ Instigator.transform.position;
        transform.position = temp;
        if (Instigator.name != "Player") BulletLocation = BulletPlace;
        else BulletLocation = MinePlace;
        gameObject.name = Instigator.name + " Mine";
        isStatic = true;
        key = 4;



    }

    void Explode(GameObject g)
    {
        Instantiate(explodeSound, g.transform.position, Quaternion.identity);
        Rigidbody r = g.GetComponent<Rigidbody>();
        r.AddForce((g.transform.position - new Vector3(transform.position.x, g.transform.position.y, transform.position.z)).normalized * 20f, ForceMode.Impulse);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision coll)
    {
        FoolStop(coll.gameObject);
        if (coll.gameObject.tag == "Player" && coll.gameObject != Instigator)
        {
            Explode(coll.gameObject);
        }
    }


}
