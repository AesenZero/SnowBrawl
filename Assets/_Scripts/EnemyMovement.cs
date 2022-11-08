using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Character_Movement
{
    bool stopper = false;
    float stopSeconds = 0f;
    // Start is called before the first frame update


    // Update is called once per frame
    protected override void Update()
    {
        isMoving = false;
        if(stopSeconds>0f)
        {
            stopSeconds -= Time.deltaTime;
            stopper = true;
        }
        else stopper = false;
        if (RB.velocity != Vector3.zero) isMoving = true;

    }

    private void LateUpdate()
    {
        if (!isMoving) AS.Stop();
    }

    public void Rotate(float i)
    {
        if (freezeDebuff == true || stopper == true) return;
        isMoving = true;
        characterTransform.Rotate(i*Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }

    public void Move(Vector3 dir)
    {
        if (freezeDebuff == true || stopper == true) return;
        isMoving = true;
        if (!AS.isPlaying) AS.Play();
        characterTransform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.Self);
    }

    protected override void Move()
    {
        
    }

    public void ChangeStopper(float s)
    {
        stopSeconds = s;
    }
}
