using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBall : BulletSkill
{
    [SerializeField] float detectionRadius;
    [SerializeField] int BulletSpeed;
    [SerializeField] bool isDetected;
    [SerializeField] GameObject target;
    [SerializeField] GameObject fireEffect;
    [SerializeField] AudioClip fireSound;
    [SerializeField] GameObject explodeSound;

    protected override void Effect()
    {
        if(!isDetected)
        {
            //gameObject.transform.Translate(ParentForward * BulletSpeed * Time.deltaTime, Space.World);
            foreach (var item in GameStateManager.Manager.GetPlayers())
            {
                if (item == Instigator || item == null) continue;
                Character_Movement CM = item.GetComponent<Character_Movement>();
                if (!CM.IsItMoving()) continue;
                if(Vector3.Distance(item.transform.position,transform.position)<=detectionRadius)
                {
                    target = item;
                    BulletSpeed *= 2;
                    fireEffect.SetActive(true);
                    AS.Play();
                    Kin.velocity *= 0;
                    isDetected = true;
                }
            }
        }else
        {
            gameObject.transform.LookAt(target.transform);
            gameObject.transform.Translate(transform.forward * BulletSpeed * Time.deltaTime, Space.World);
        }

        //Kin.velocity = Vector3.forward * Time.deltaTime * BulletSpeed;


    }

    private void OnTriggerEnter(Collider ot)
    {
        if (ot.gameObject.tag == "Player" && ot.gameObject != Instigator)
        {
            AS.Stop();
            Explode(ot.gameObject);

        }
    }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        key = 3;
        isStatic = false;
        AS.clip = fireSound;
        Destroying();
    }

    // Update is called once per frame
    void Update()
    {
        Effect();
    }

    void Explode(GameObject g)
    {
        Instantiate(explodeSound, g.transform.position, Quaternion.identity);
        Rigidbody r = g.GetComponent<Rigidbody>();
        r.AddForce((g.transform.position-new Vector3(transform.position.x,g.transform.position.y,transform.position.z)).normalized*20f,ForceMode.Impulse);
        Destroy(gameObject);
    }

    protected void Destroying()
    {
        Invoke("Death", 5f);
    }

}
