using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    public int damageToGive;

    private PlayerStats plrStats;

    public int expToGiveVit;
    // Start is called before the first frame update
    void Start()
    {
        plrStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().PlayerHurt(damageToGive);

            //plrStats.AddExpVit(expToGiveVit);
        }
        
    }
}
