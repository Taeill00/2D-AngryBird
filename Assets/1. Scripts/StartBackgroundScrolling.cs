using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBackgroundScrolling : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if(transform.position.x <= -20.48f)
        {
            transform.position = new Vector3(20.48f, 0, 0);
        }
    }
}
