using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pijltje : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        transform.position += Vector3.right * speed * Time.deltaTime;

        if (timer > 0.3f)
        {
            speed = -speed;
            timer = 0;
        }

        Vector3 selectedPos = EventSystem.current.currentSelectedGameObject.transform.position;

        transform.position = new Vector3(transform.position.x, selectedPos.y, transform.position.z);
    }
}
