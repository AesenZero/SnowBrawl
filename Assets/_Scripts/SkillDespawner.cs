using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDespawner : MonoBehaviour
{
    [SerializeField] float MinHeight = -15f;

    void Update()
    {
        if( gameObject.transform.position.y < MinHeight)
        {
            Destroy(gameObject);
        }
    }
}
