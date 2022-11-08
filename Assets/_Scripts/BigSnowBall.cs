using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BigSnowBall : BulletSkill
{
    [SerializeField] int BulletSpeed;
    private Vector3 Scaling;
    [SerializeField] float scaleSpeed;


    void CmdEffect()
    {
        gameObject.transform.Translate(ParentForward * BulletSpeed * Time.deltaTime, Space.World);
    }

    // Use this for initialization
    new void Start ()
    {
        base.Start();
        Kin.velocity *= 0;
        Scaling = transform.localScale;
        isStatic = false;
        key = 1;
        Destroying();


        
    }
	

    

	// Update is called once per frame
	void Update ()
    {

        CmdEffect();
        Scale();
	}

    void Scale()
    {
        Scaling.x += scaleSpeed * Time.deltaTime;
        Scaling.y += scaleSpeed * Time.deltaTime;
        Scaling.z += scaleSpeed * Time.deltaTime;
        transform.localScale = Scaling;
    }


    private void OnTriggerStay(Collider ot)
    {
        if(ot.gameObject.tag == "Player" && ot.gameObject!=Instigator)
        {
            ot.gameObject.transform.position = gameObject.transform.position;
        }
    }

    protected void Destroying()
    {
        Invoke("Death", 13f);
    }
}
