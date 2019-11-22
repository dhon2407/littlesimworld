using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;
using UnityEditor;
using System.Linq;

public class JobsPopUp : MonoBehaviour

    
{
    public GameObject PopUpInstance;
    public Image ImageOfWork;
    public Image WorkProgressBar;
    public float currentJobProgres = 0;
    public float currentJobTime = 0;
    public float requiredJobTime = 10;
    public int MoneyGain = 5;
    public KeyCode KeyToWork = KeyCode.E;
    [Space]
    public float healthDrain = 0.1f;
    public float energyDrain = 0.1f;
    public float moodDrain = 0.1f;

    public static Jobs CurrentJob;

    public Animator anim;

    public static JobsPopUp Instance;


    void Start()
    {
        Instance = this;
    }


    void Update()
    {   
        WorkProgressBar.fillAmount = currentJobProgres;
        if(CurrentJob != null && CurrentJob.WorkingDays.Contains(DayNightCycle.Instance.WeekDay) && DayNightCycle.Instance.time >= CurrentJob.JobStartingTime && !CurrentJob.WorkedToday&&
            !JobsPopUp.Instance.anim.GetBool("CarCalled"))
        {
            JobsPopUp.Instance.anim.SetBool("CarCalled", true);
            CurrentJob.WorkedToday = true;
            GameLibOfMethods.AddChatMessege("Car near your home will take you to your job.");
        }
    }

    public void LoadPlayerInTheCar()
    {
        if (!anim.GetBool("PlayerInCar") && anim.GetBool("CarCalled"))
        {
            anim.SetBool("PlayerInCar", true);
            GameLibOfMethods.cantMove = true;
            //GameLibOfMethods.TempPos = GameLibOfMethods.player.transform.position;

            //Camera.main.GetComponent<CameraFollow>().target = Camera.main.transform;

            //GameLibOfMethods.player.GetComponent<Rigidbody2D>().simulated = false;

            //GameLibOfMethods.player.transform.SetParent(gameObject.transform);
            //GameLibOfMethods.player.transform.position = gameObject.transform.position;
            //GameLibOfMethods.player.gameObject.SetActive(false);
            CurrentJob.WorkedToday = true;
        }
        



        
    }

    public void GoFromRightToHome()
    {
        if (CurrentJob != null)
        { 

            if (anim.GetBool("PlayerInCar"))
            {
                GameLibOfMethods.cantMove = false;

                GameLibOfMethods.doingSomething = false;

                //GameLibOfMethods.player.transform.SetParent(null);

                //GameLibOfMethods.player.transform.localScale = Vector3.one;

                //Camera.main.GetComponent<CameraFollow>().target = GameLibOfMethods.player.transform;

                //GameLibOfMethods.player.transform.position = GameLibOfMethods.TempPos;
                //GameLibOfMethods.player.GetComponent<Rigidbody2D>().simulated = true;

                anim.SetBool("PlayerInCar", false);
                anim.SetBool("FinishedWork", false);
                //GameLibOfMethods.player.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(WaitForPlayer());
            }

        }
        else
        {
            GameLibOfMethods.AddChatMessege("You have to job");
        }
    }

    public void TurnOffJobThings()
    {
        ImageOfWork.gameObject.SetActive(false);
        WorkProgressBar.gameObject.SetActive(false);
    }

    public void CalledForCar()
    {
        anim.SetBool("CarCalled", true);
        StartCoroutine(WaitForPlayer());
    }
    public IEnumerator WaitForPlayer()
    {
        


            float waitingTime = 0;
            while (true)
            {
                waitingTime += Time.deltaTime;

                if (anim.GetBool("PlayerInCar"))
                {
                    yield break;
                }
                if (waitingTime >= CurrentJob.MaxCarWaitTime)
                {
                    anim.SetBool("FinishedWork", false);
                    anim.SetBool("CarCalled", false);
                    CurrentJob.Penalize();
                    yield break;
                }
                yield return null;


            }
        
    }

      public IEnumerator DoJob(string AnimationToPlay)
  {


      if (!GameLibOfMethods.doingSomething && anim.GetBool("PlayerInCar"))
      {
          anim.SetBool("FinishedWork", false);
          anim.SetBool("CarCalled", false);
          ImageOfWork.gameObject.SetActive(true);
          WorkProgressBar.gameObject.SetActive(true);
          GameLibOfMethods.doingSomething = true;
          GameLibOfMethods.cantMove = true;
            DayNightCycle.Instance.ChangeSpeedToSupaFast();
            while (true)
          {


              if (currentJobProgres < 1)
              {
                  PlayerStatsManager.Instance.Heal(-healthDrain);
                  PlayerStatsManager.Instance.AddMood(-moodDrain);
                  PlayerStatsManager.Instance.AddEnergy(-energyDrain);
                  currentJobTime += Time.deltaTime;
                  currentJobProgres = currentJobTime / requiredJobTime;


              }
              else
              {

                  PlayerStatsManager.Instance.AddMoney(MoneyGain);
                  currentJobTime = 0;
                  currentJobProgres = 0;
                  GameLibOfMethods.doingSomething = false;
                  anim.SetBool("FinishedWork", true);
                  TurnOffJobThings();
                    DayNightCycle.Instance.ChangeSpeedToNormal();
                    CurrentJob.Finish();

                  yield break;
              }
              if (Input.GetKeyDown(KeyToWork))
              {

              }
              if (GameLibOfMethods.passedOut || Input.GetKeyDown(KeyToWork))
              {

                  TurnOffJobThings();
                  GoFromRightToHome();
                    DayNightCycle.Instance.ChangeSpeedToNormal();
                    yield break;
              }




              yield return new WaitForFixedUpdate();

          }
      }
  }
    /*public IEnumerator DoJob(string AnimationToPlay)
    {

        Debug.Log("Working");


        DayNightCycle.Instance.ChangeSpeedToFaster();
        GameLibOfMethods.doingSomething = true;
        GameLibOfMethods.cantMove = true;
        

        yield return new WaitForEndOfFrame();
        while (!Input.GetKey(KeyToWork) && 
            !PlayerStatsManager.Instance.passingOut)

        {


            if (currentJobProgres < 1)
            {
                PlayerStatsManager.Instance.Heal(-healthDrain);
                PlayerStatsManager.Instance.AddMood(-moodDrain);
                PlayerStatsManager.Instance.AddEnergy(-energyDrain);
                currentJobTime += Time.deltaTime;
                currentJobProgres = currentJobTime / requiredJobTime;
                GameLibOfMethods.progress = currentJobProgres;
                GameLibOfMethods.animator.SetBool(AnimationToPlay, true);

            }
            else
            {

                PlayerStatsManager.Instance.AddMoney(MoneyGain);
                currentJobTime = 0;
                currentJobProgres = 0;
                GameLibOfMethods.progress = 0;
                CurrentJob.Finish();
                Debug.Log("MoneyGiven");


            }


            yield return new WaitForFixedUpdate();

        }
        GameLibOfMethods.doingSomething = false;
        GameLibOfMethods.progress = 0;
        TurnOffJobThings();
        DayNightCycle.Instance.ChangeSpeedToNormal();
        StartCoroutine(InteractionChecker.Instance.JumpTo(GameLibOfMethods.TempPos));

    }*/
    public void SetJobToCooker()
    {
        CurrentJob = CookingJob.Instance;
        //GameLibOfMethods.AddChatMessege("You assigned to scientist job. Wait for car to arrive everyday, it will wait for you for an hour. Payment per " + CurrentJob.RequiredSkill.SkillName + ": " + CurrentJob.MoneyPerSkillLevel);
    }
    [System.Serializable]
    public class Jobs
    {
        virtual public string JobName
        { get; set; }

        virtual public PlayerStatsManager.Skills RequiredSkill {get;set;}
        public int MoneyPerSkillLevel = 50;
        public static Jobs Instance = new Jobs();
        virtual public float XPbonus
        {
            get;
            set;
        }

        virtual public float JobStartingTime
        { get; set; }

        public int MaxPenalizeDays = 3;
        public int currentPenalizedDays;

        virtual public List<int> WorkingDays
        { get; set; }

        public float MaxCarWaitTime = 60;

        public bool WorkedToday = false;

        public virtual void Penalize()
        {
            CurrentJob.currentPenalizedDays += 1;
            GameLibOfMethods.AddChatMessege("You got penalized for not showing up on work today. Dont go to work another " +
                    (CurrentJob.MaxPenalizeDays - CurrentJob.currentPenalizedDays) + " days and you will be fired.");

            if (CurrentJob.currentPenalizedDays >= CurrentJob.MaxPenalizeDays)
            {
                
                CurrentJob.FireFromJob();
                CurrentJob.Reset();
            }
        }
        public virtual void Reset()
        {
            currentPenalizedDays = 0;
        }
        public virtual void FireFromJob()
        {
            GameLibOfMethods.AddChatMessege("You got fired from your job.");
            CurrentJob = null;
        }
        public virtual void Finish()
        {


            Debug.Log("No job specified.");
          
        }
    }
    [System.Serializable]
    public class CookingJob : JobsPopUp.Jobs
    {
        new public static JobsPopUp.Jobs Instance = new CookingJob();
        new public int MoneyPerSkillLevel = 50;
        override public PlayerStatsManager.Skills RequiredSkill { get { return PlayerStatsManager.Cooking.Instance; } }
        override public string JobName
        {
            get { return JobName = "cooking"; }
        }
        public override float XPbonus { get { return XPbonus = 10; } }
        new public int MaxPenalizeDays = 3;
        new public int currentPenalizedDays = 0;
        override public List<int> WorkingDays
        {
            get { return WorkingDays = new List<int> { 0, 1, 2, 3, 4 }; ; }
        }

        override public float JobStartingTime
        {
            get { return Instance.JobStartingTime = 36000; ; }
        }


        public override void Finish()
        {


            PlayerStatsManager.Instance.AddMoney(MoneyPerSkillLevel * RequiredSkill.Level);
            RequiredSkill.AddXP(XPbonus);
            GameLibOfMethods.AddChatMessege("You got " + (MoneyPerSkillLevel * RequiredSkill.Level) + " dollars and " + XPbonus + " XP from your " + JobName + " tier " + RequiredSkill.Level+ " job. Level up " + RequiredSkill.SkillName + " to earn more money." );

        }
        


    }


}
