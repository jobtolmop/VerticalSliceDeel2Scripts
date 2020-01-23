using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlinkMovement : MonoBehaviour
{
    public float speed = 1;
    public float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        transform.position += Vector3.up * speed * Time.deltaTime;

        if (timer > 1)
        {
            speed = -speed;
            timer = 0;
        }
    }
}
