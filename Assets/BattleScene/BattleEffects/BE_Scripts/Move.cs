using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float moveSpeed = 0.01f;
    [Tooltip("If not random, then default is up")]
    public bool randomDirection;

    float x = 0;
    float y = 1;
    float z = 0;

    void Start()
    {
        if (randomDirection)
        {
            x = Random.Range(-1f, 1f);
            y = Random.Range(-1f, 1f);
            z = Random.Range(-1f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(x,y,z) * moveSpeed;
    }
}
