using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    public float Health;
    public float MoveSpeed;
    public UnityEvent RangedAttack = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        //RangedAttack += Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot()
    {

    }
}
