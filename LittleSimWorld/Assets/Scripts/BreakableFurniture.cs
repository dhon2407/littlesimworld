using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BreakableFurniture : MonoBehaviour
{
    public float breakChancePerSecond = 50;
    public bool isBroken = false;
    public float RepairProgress = 0;
    public float repairSpeed = 100;
    public Image Progress;
    public Transform CharacterPosition;
    public AudioSource SoundSource;
    public float MaxVolume = 0.25f;
    public AudioClip EnteringSound, LoopingSound, ExitSound;

    public ParticleSystem LoopingParticleSystem;
    public ParticleSystem.EmissionModule Emission;
    [Header("Status bar gain amounts")]
    public float XpGainGetHour = 0;
    public float EnergyGainPerHour = 4;
    public float FoodGainPerHour = 4;
    public float MoodGainPerHour = 4;
    public float BladderGainPerHour = 4;
    public float ThirstGainPerHour = 4;
    public float HealthGainPerHour = 4;
    public float HygieneGainPerHour = 4;
    [Header("Status bar drain amounts")]
    public float EnergyDrainPerHour = 4;
    public float FoodDrainPerHour = 4;
    public float MoodDrainPerHour = 4;
    public float BladderDrainPerHour = 4;
    public float ThirstDrainPerHour = 4;
    public float HealthDrainPerHour = 4;
    public float HygieneDrainPerHour = 4;

    public ParticleSystem BrokenParticleSystem;

    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    protected virtual void Start()
    {
        if (SoundSource)
        {
            OptionsManager.Instance.SoundSources.Add(SoundSource);
        }
        if (LoopingParticleSystem)
        {
            Emission = LoopingParticleSystem.emission;
            Emission.enabled = false;
            SoundSource.Stop();
        }
        
      
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Fix()
    {
        yield return new WaitForEndOfFrame();
        if (GameLibOfMethods.canInteract && !GameLibOfMethods.doingSomething && isBroken)
        {
            yield return new WaitForEndOfFrame();
            GameLibOfMethods.cantMove = true;
            GameLibOfMethods.canInteract = false;
            GameLibOfMethods.doingSomething = true;
            GameLibOfMethods.animator.SetBool("Fixing", true);
            Progress.transform.parent.gameObject.SetActive(true);
            Break();
            yield return new WaitForEndOfFrame();
            while (!Input.GetKey(KeyCode.E))
            {
                RepairProgress += (Time.deltaTime * repairSpeed) * PlayerStatsManager.Instance.RepairSpeed;
                Progress.fillAmount = RepairProgress / 100;
                if(RepairProgress >= 100)
                {
                    OnFix();
                    yield break;
                }
                
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSecondsRealtime(0.1f);

            


            GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            InteractionChecker.Instance.ResetPlayer(false);
            yield return null;
        }
        else
        {
            yield break;
        }
    }
   
    public void Break()
    {
        
        BrokenParticleSystem.Play();
    }
    public void OnFix()
    {
        BrokenParticleSystem.Stop();
        Progress.transform.parent.gameObject.SetActive(false);
        isBroken = false;
        RepairProgress = 0;
        Progress.fillAmount = RepairProgress;
        InteractionChecker.Instance.ResetPlayer(false);
        PlayerStatsManager.Repair.Instance.AddXP(10);
    }

    public void PlayEnterAndLoopSound()
    {
        StartCoroutine(PlayEnterSound());   
    }

    private IEnumerator PlayEnterSound()
    {
        if (SoundSource)
        {
            SoundSource.Stop();
            SoundSource.loop = false;
        }
        if (SoundSource && EnteringSound)
        {
           
            SoundSource.clip = EnteringSound;
            SoundSource.Play();
            Debug.Log("playing enter sound");
        }
        if (SoundSource)
        {
            
            while (SoundSource.isPlaying)
            {

                yield return new WaitForFixedUpdate();
            }
        }
       
        if (SoundSource && LoopingSound)
        {
            Debug.Log("playing loop sound");
            SoundSource.loop = true;
            SoundSource.clip = LoopingSound;
            SoundSource.Play();
        }
        yield return null;
    }
    public void PlayExitSound()
    {
        if (SoundSource)
        {
            SoundSource.Stop();
            SoundSource.loop = false;
        }
      
        if (SoundSource && ExitSound)
        {
            Debug.Log("playing exit sound");
            SoundSource.clip = ExitSound;

           SoundSource.Play();
            //SoundSource.PlayOneShot(ExitSound);
           // SoundSource.PlayDelayed(1);

        }
       
    }
}
