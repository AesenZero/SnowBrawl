using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    [SerializeField] float lifetime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroying", lifetime);
    }


    void Destroying()
    {
        Destroy(gameObject);
    }

}
