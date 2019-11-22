using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float floatSpeed = 5;
    public float lifeTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Animation anim in GetComponentsInChildren<Animation>())
        {
            anim.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * floatSpeed);
    }
}
