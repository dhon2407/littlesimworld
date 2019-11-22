using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject BubbleSpawnerGO;
    public ParticleSystem particlesystem;
    public GameObject CurrentBubbleSpawnerGO;
    public static BubbleSpawner Instance;
    public float MaximumLifetimeOfEmitter = 5;
    // Start is called before the first frame update
    private void Awake()
    {
      
    }
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*public void SpawnBubble(Vector3 WhereToSpawnFrom)
    {
       

            //CurrentBubbleSpawnerGO = Instantiate(BubbleSpawnerGO, WhereToSpawnFrom, Quaternion.Euler(Vector3.zero));
            particlesystem.Play();
            //Destroy(CurrentBubbleSpawnerGO, MaximumLifetimeOfEmitter);
        
        
    }*/
    public static void SpawnBubble()
    {


        //CurrentBubbleSpawnerGO = Instantiate(BubbleSpawnerGO, this.transform.position, Quaternion.Euler(Vector3.zero), this.transform);
        if (BubbleSpawner.Instance.particlesystem.isPlaying == false)
        {

            BubbleSpawner.Instance.particlesystem.Play();
           // Debug.Log("Spawned XP bubble" + "ID: " + BubbleSpawner.Instance.GetInstanceID());
        }
           
           // Destroy(CurrentBubbleSpawnerGO, MaximumLifetimeOfEmitter);
       
    }

}
