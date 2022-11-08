using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Character_Movement : MonoBehaviour
{
    [SerializeField] protected Transform characterTransform;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected Rigidbody RB;
    [SerializeField] KeyCode Forward;
    [SerializeField] KeyCode Back;
    [SerializeField] KeyCode RotateRight;
    [SerializeField] KeyCode RotateLeft;
    [SerializeField] protected bool freezeDebuff = false;
    [SerializeField] protected bool isMoving = false;
    [SerializeField] protected AudioSource AS;
 
    // Use this for initialization
    protected virtual void Start()
    {
        RB = GetComponent<Rigidbody>();
        transform.LookAt(GameStateManager.Manager.GetCenterOfStage().transform);
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (freezeDebuff == false) Move();
        else isMoving = false;

        if (!isMoving) AS.Stop();
    }

    protected virtual void Move()
    {
        isMoving = false;
        if (Input.GetKey(Forward))
        {
            characterTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
            isMoving = true;
            if (!AS.isPlaying) AS.Play();
        }

        if (Input.GetKey(Back))
        {
            characterTransform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);
            isMoving = true;
            if (!AS.isPlaying) AS.Play();
        }

        if (Input.GetKey(RotateRight))
        {
            characterTransform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
            isMoving = true;
        }

        if (Input.GetKey(RotateLeft))
        {
            characterTransform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime, Space.Self);
            isMoving = true;
        }

        
    }

    public void SetIsMoving(bool b)
    {
        isMoving = b;
    }

    public bool IsItMoving()
    {
        return isMoving;
    }

    public void SetFreezeDebuff(bool b)
    {
        freezeDebuff = b;
    }

    public bool GetFreezeDebuff()
    {
        return freezeDebuff;
    }

}
