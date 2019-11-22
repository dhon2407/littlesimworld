using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractionChecker : MonoBehaviour
{
    public KeyCode KeyToInteract;
    public Actions invActions;
    public GameObject Weights;
    public GameObject FryingPan;
    public static InteractionChecker Instance;
    public AnimationCurve jumpCurve;
    private int currentFrames;
    public GameObject lastHighlightedObject;
    public float JumpSpeed = 0.8f;
    public float BladderGainAmount = 0.027777778f;
    public float HygieneGainAmount = 0.027777778f;
    public float EnergyGainSpeedWhenPassedOut = 10;
    public GameObject Piss;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyToInteract) && !GameLibOfMethods.isSleeping && GameLibOfMethods.canInteract && !GameLibOfMethods.doingSomething)
        {
            DayNightCycle.Instance.ChangeSpeedToNormal();
            GameObject interactableObject = GameLibOfMethods.CheckInteractable();
            GameLibOfMethods.lastInteractable = interactableObject;

            if (interactableObject)
            {
                if (interactableObject.GetComponent<BreakableFurniture>() && !interactableObject.GetComponent<BreakableFurniture>().isBroken && !GameLibOfMethods.doingSomething)
                {
                    interactableObject.GetComponent<BreakableFurniture>().PlayEnterAndLoopSound();
                }
                if (interactableObject.GetComponent<BreakableFurniture>() && interactableObject.GetComponent<BreakableFurniture>().isBroken)
                {
                    StartCoroutine(interactableObject.GetComponent<BreakableFurniture>().Fix());
                    return;
                }

                if (interactableObject.GetComponent<Chair>() && !GameLibOfMethods.sitting)
                {
                    SitDown(gameObject, interactableObject);
                    return;
                }

                if (interactableObject.GetComponent<Door>())
                {
                    InteractWithDoor(interactableObject);
                }
                if (interactableObject.GetComponent<Bed>())
                {
                    if (!GameLibOfMethods.doingSomething && PlayerStatsManager.Instance.Food > PlayerStatsManager.Instance.MaxFood * 0.1f &&
            PlayerStatsManager.Instance.Thirst > PlayerStatsManager.Instance.MaxThirst * 0.1f &&
            PlayerStatsManager.Instance.Bladder > PlayerStatsManager.Instance.MaxBladder * 0.1f &&
            PlayerStatsManager.Instance.Hygiene > PlayerStatsManager.Instance.MaxHygiene * 0.1f)
                    {
                        //StartCoroutine(SlowlyApproachToSleep());
                        //GameLibOfMethods.animator.SetBool("Jumping", true);
                        GameLibOfMethods.animator.SetTrigger("PrepToSleep");
                        GameLibOfMethods.canInteract = false;
                        GameLibOfMethods.cantMove = true;
                        
                    }

                }
                if (interactableObject.GetComponent<WorkoutPlace>())
                {
                    StartCoroutine(SlowlyApproachToLift());
                }
                /*if (interactableObject.GetComponent<HomeComputer>())
                {
                    StartCoroutine(GameLibOfMethods.DoAction(invActions.Study, 10f));
                }*/
                if (interactableObject.GetComponent<JobsPopUp>())
                {
                    interactableObject.GetComponent<JobsPopUp>().LoadPlayerInTheCar();
                }
                if (interactableObject.GetComponent<Treadmill>())
                {
                    StartCoroutine(ApproachAndRunOnTreadmill());
                }
                if (interactableObject.GetComponent<TelephoneBooth>())
                {
                    if (JobsPopUp.CurrentJob != null)
                    {
                        /*JobsPopUp.Instance.anim.SetBool("CarCalled", true);
                        GameLibOfMethods.AddChatMessege("Called car to work.");*/
                    }

                }
                if (interactableObject.GetComponent<Shower>())
                {
                    StartCoroutine(SlowlyApproachToTakeShower());
                }
                if (interactableObject.GetComponent<Toilet>())
                {
                    StartCoroutine(SlowlyApproachToTakeADump());
                }
                if (interactableObject.GetComponent<Bank>())
                {
                    interactableObject.GetComponent<Bank>().SwitchBankUI();
                }
                if (interactableObject.GetComponent<AtommItem>() || interactableObject.GetComponent<AtommContainer>())
                {
                    AtommInventory.Instance.CheckRaycast();
                }
                if (interactableObject.GetComponent<Shop>())
                {
                    AtommInventory.Instance.CheckRaycast();
                }
                if (interactableObject.GetComponent<UpgradesShop>())
                {
                    AtommInventory.Instance.CheckRaycast();
                }
                if (interactableObject.GetComponent<CookingStove>())
                {
                    interactableObject.GetComponent<CookingStove>().Switch();
                }
            }
        }

        currentFrames += 1;
        if (currentFrames >= 20)
        {
            GameObject tempObject = GameLibOfMethods.CheckInteractable();
            if (tempObject &&
                tempObject.GetComponent<Outline>() &&
                tempObject.GetComponent<Outline>().RendererOfOutlinedObject &&
                GameLibOfMethods.player &&
                !GameLibOfMethods.doingSomething &&
                GameLibOfMethods.canInteract)
            {
                if (lastHighlightedObject && tempObject && lastHighlightedObject != tempObject)
                    lastHighlightedObject.GetComponent<Outline>().DisableOutline();
                lastHighlightedObject = tempObject;
                tempObject.GetComponent<Outline>().EnableOutline();
            }
            else
            {
                if (lastHighlightedObject)
                {
                    lastHighlightedObject.GetComponent<Outline>().DisableOutline();
                }
                lastHighlightedObject = null;
            }
            currentFrames = 0;
        }
    }

    public void TurnOnAnimator()
    {
        GameLibOfMethods.animator.enabled = true;
    }
    public void SpawnPiss()
    {
        Instantiate(Piss, GameLibOfMethods.player.transform.position, GameLibOfMethods.player.transform.rotation, null);
    }
    public void ResetPlayer()
    {
        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<CookingStove>())
            GameLibOfMethods.lastInteractable.GetComponent<CookingStove>().FryingPan.SetActive(true);

        GameLibOfMethods.canInteract = true;
        GameLibOfMethods.doingSomething = false;
        Weights.SetActive(false);
        GameLibOfMethods.player.transform.rotation = Quaternion.Euler(Vector3.zero);

        if (GameLibOfMethods.lastInteractable)
            Physics2D.IgnoreCollision(GameLibOfMethods.player.GetComponent<Collider2D>(), GameLibOfMethods.lastInteractable.GetComponent<Collider2D>(), false);

        SpriteControler.Instance.LeftHand.position = SpriteControler.Instance.LeftHandLeft.transform.position;
        SpriteControler.Instance.LeftHand.GetComponent<SpriteRenderer>().sortingOrder = 6;
        SpriteControler.Instance.RightHand.position = SpriteControler.Instance.RightHandRight.transform.position;
        SpriteControler.Instance.RightHand.GetComponent<SpriteRenderer>().sortingOrder = 6;

        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<WorkoutPlace>())
            GameLibOfMethods.lastInteractable.GetComponent<WorkoutPlace>().Weights.SetActive(true);

        ResetAnimations();
        GameLibOfMethods.sitting = false;

        Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, false);


        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<Shower>())
            GameLibOfMethods.lastInteractable.GetComponent<Shower>().Emission.enabled = false;
        DayNightCycle.Instance.currentTimeSpeedMultiplier = 1;
        GameLibOfMethods.cantMove = false;
        GameLibOfMethods.canInteract = true;
        GameLibOfMethods.doingSomething = false;
        GameLibOfMethods.player.transform.rotation = Quaternion.Euler(Vector3.zero);
        GameLibOfMethods.animator.enabled = true;

        if (GameLibOfMethods.concecutiveSleepTime >= GameLibOfMethods.neededConcecutiveSleepTimeForPositiveBuff && DayNightCycle.Instance.time < 36000)
        {
            Instantiate(Resources.Load<GameObject>("Buffs/Slept Well"), Buffs.Instance.transform);
        }

        GameLibOfMethods.isSleeping = false;
        GameLibOfMethods.cantMove = false;
        GameLibOfMethods.canInteract = true;

        GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        GameLibOfMethods.concecutiveSleepTime = 0;

        GameLibOfMethods.player.transform.rotation = Quaternion.Euler(Vector3.zero);
        GameLibOfMethods.player.GetComponent<Animator>().enabled = true;
        GameLibOfMethods.player.GetComponent<Animator>().SetBool("Sleeping", false);
        GameLibOfMethods.player.GetComponent<Animator>().SetBool("Fixing", false);
        
        Debug.Log("Reset Player");
        PlayerStatsManager.Instance.passingOut = false;
    }
    public void ResetPlayer(bool movePlayerAfterAction)
    {
        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<CookingStove>())
            GameLibOfMethods.lastInteractable.GetComponent<CookingStove>().FryingPan.SetActive(true);
        if (movePlayerAfterAction)
            GameLibOfMethods.player.transform.position = GameLibOfMethods.TempPos;

        GameLibOfMethods.canInteract = true;
        GameLibOfMethods.doingSomething = false;
        Weights.SetActive(false);
        GameLibOfMethods.player.transform.rotation = Quaternion.Euler(Vector3.zero);

        if (GameLibOfMethods.lastInteractable)
            Physics2D.IgnoreCollision(GameLibOfMethods.player.GetComponent<Collider2D>(), GameLibOfMethods.lastInteractable.GetComponent<Collider2D>(), false);

        SpriteControler.Instance.LeftHand.position = SpriteControler.Instance.LeftHandLeft.transform.position;
        SpriteControler.Instance.LeftHand.GetComponent<SpriteRenderer>().sortingOrder = 6;
        SpriteControler.Instance.RightHand.position = SpriteControler.Instance.RightHandRight.transform.position;
        SpriteControler.Instance.RightHand.GetComponent<SpriteRenderer>().sortingOrder = 6;

        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<WorkoutPlace>())
            GameLibOfMethods.lastInteractable.GetComponent<WorkoutPlace>().Weights.SetActive(true);

        ResetAnimations();
        GameLibOfMethods.sitting = false;

        Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, false);


        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<Shower>())
            GameLibOfMethods.lastInteractable.GetComponent<Shower>().Emission.enabled = false;
        DayNightCycle.Instance.currentTimeSpeedMultiplier = 1;
        GameLibOfMethods.cantMove = false;
        GameLibOfMethods.canInteract = true;
        GameLibOfMethods.doingSomething = false;
        GameLibOfMethods.player.transform.rotation = Quaternion.Euler(Vector3.zero);
        GameLibOfMethods.animator.enabled = true;

        if (GameLibOfMethods.concecutiveSleepTime >= GameLibOfMethods.neededConcecutiveSleepTimeForPositiveBuff && DayNightCycle.Instance.time < 36000)
        {
            Instantiate(Resources.Load<GameObject>("Buffs/Slept Well"), Buffs.Instance.transform);
        }

        GameLibOfMethods.isSleeping = false;
        GameLibOfMethods.cantMove = false;
        GameLibOfMethods.canInteract = true;

        GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        GameLibOfMethods.concecutiveSleepTime = 0;

        GameLibOfMethods.player.transform.rotation = Quaternion.Euler(Vector3.zero);
        GameLibOfMethods.player.GetComponent<Animator>().enabled = true;
        GameLibOfMethods.player.GetComponent<Animator>().SetBool("Sleeping", false);
        GameLibOfMethods.player.GetComponent<Animator>().SetBool("Fixing", false);
        PlayerStatsManager.Instance.passingOut = false;
    }
   


    public void SitDown(GameObject character, GameObject chair)
    {
        GameLibOfMethods.lastChair = chair;
        SpriteRenderer charSprite = character.GetComponent<SpriteRenderer>();
        SpriteRenderer chairSprite = chair.GetComponent<SpriteRenderer>();

        GameLibOfMethods.InitialSortingLayerCharacter = charSprite.sortingOrder;
        GameLibOfMethods.InitialSortingLayerChair = chairSprite.sortingOrder;

        GameLibOfMethods.TempPos = character.transform.position;



        Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, true);

        character.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StartCoroutine(SitDown());

        /*if (!chairScript.IsFacingDown)
        {
            charSprite.sortingOrder = (chairSprite.sortingOrder - 1);
        }*/
        /*else
        {
            charSprite.sortingOrder = (chairSprite.sortingOrder + 1);
        }*/

    }
    public IEnumerator SitDown()
    {
        if (!GameLibOfMethods.doingSomething)
        {
            GameLibOfMethods.doingSomething = true;
            //GameLibOfMethods.canInteract = false;
            //float percentage = 0;
            Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, true);
            GameLibOfMethods.TempPos = GameLibOfMethods.player.transform.position;



            // GameLibOfMethods.cantMove = true;
            GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;





            //GameLibOfMethods.animator.SetBool("Jumping", false);

            //StartCoroutine(WalkTo(GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().CharacterPosition.position, delegate { StartCoroutine(BrowsingInternet()); }));
            StartCoroutine(WalkTo(GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().CharacterPosition.position, delegate {
                StartSitting();
            GameLibOfMethods.lastInteractable.GetComponent<Chair>().ActivateChoices();}));
            Debug.Log("sat");
            yield return null;

            /*while (true)
            {
               
                percentage += 0.04f;
                Debug.Log(percentage);
                
                GameLibOfMethods.player.transform.position = Vector2.Lerp(GameLibOfMethods.TempPos, GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().CharacterPosition.position, percentage);
                GameLibOfMethods.player.transform.localScale = new Vector3(jumpCurve.Evaluate(percentage), jumpCurve.Evaluate(percentage), 1);

                //GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
//
               // foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
               // {
               //     sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
               // }


                if (percentage >= 1)
                {
                    GameLibOfMethods.sitting = true;
                    GameLibOfMethods.cantMove = false;
                    GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //StartCoroutine(GameLibOfMethods.DoAction(invActions.TakeShower, 10,true,true));

                    GameLibOfMethods.animator.SetBool("Jumping", false);
                    GameLibOfMethods.animator.SetBool("Sitting", true);

                    percentage = 0;
                    yield return new WaitForEndOfFrame();
                    StartCoroutine(BrowsingInternet());

                    yield break;

                }

                yield return new WaitForFixedUpdate();
            }*/

        }

    }

    public void StartSitting()
    {
        StartCoroutine(Sitting());
    }

    private IEnumerator Sitting()
    {
        GameLibOfMethods.sitting = true;
        GameLibOfMethods.cantMove = true;
        GameLibOfMethods.lastInteractable.GetComponent<Chair>().ActivateChoices();
        while (!Input.GetKey(InteractionChecker.Instance.KeyToInteract) && !PlayerStatsManager.Instance.passingOut)
        {

            /*GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            //blackScreen.CrossFadeAlpha(0, 1, false);

            GameLibOfMethods.facingDir = Vector2.left;*/





            yield return new WaitForFixedUpdate();
        }
        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>())
        {
            Debug.Log("Playing exit sound");
            GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().PlayExitSound();
        }
        //GameLibOfMethods.animator.SetBool("Lifting", false);
        GameLibOfMethods.sitting = false;
        yield return new WaitForEndOfFrame();

        StartCoroutine(WalkTo(GameLibOfMethods.TempPos,GameLibOfMethods.lastInteractable.GetComponent<Chair>().DisableChoices ));

        }
    public void StandUp(GameObject character)
    {
        GameLibOfMethods.lastChair.layer = 10;

        Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, false);
        character.GetComponent<SpriteRenderer>().sortingOrder = GameLibOfMethods.InitialSortingLayerCharacter;
        Debug.Log("stand up");
    }
    public void InteractWithDoor(GameObject door)
    {
        door.GetComponent<Door>().InteractWithDoor();
    }


    public void CheckInteractableType(string InteractableName)
    {
        /*if(InteractableName == "Jelly")
        {
            StartCoroutine(invActions.EatJelly());
        }
        if (InteractableName == "Physics")
        {
            //StartCoroutine(invActions.Study());
        }*/
    }

    public IEnumerator SlowlyApproachToLift()
    {
        if (!GameLibOfMethods.doingSomething)
        {

            float percentage = 0;
            GameLibOfMethods.canInteract = false;
            GameLibOfMethods.cantMove = true;
            Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, true);
            GameLibOfMethods.TempPos = GameLibOfMethods.player.transform.position;



            GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameLibOfMethods.animator.SetBool("Jumping", true);
            GameLibOfMethods.doingSomething = true;



            while (true)
            {
                percentage += JumpSpeed;
                Debug.Log(percentage);

                GameLibOfMethods.player.transform.position = Vector2.Lerp(GameLibOfMethods.TempPos, GameLibOfMethods.lastInteractable.GetComponent<WorkoutPlace>().CharacterPosition.position, percentage);
                GameLibOfMethods.player.transform.localScale = new Vector3(jumpCurve.Evaluate(percentage), jumpCurve.Evaluate(percentage), 1);

                /*GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));

                foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
                }*/


                if (percentage >= 1)
                {


                    GameLibOfMethods.player.GetComponent<SpriteControler>().FaceRIGHT();
                    GameLibOfMethods.lastInteractable.GetComponent<WorkoutPlace>().Weights.SetActive(false);

                    Weights.SetActive(true);

                    GameLibOfMethods.cantMove = true;
                    GameLibOfMethods.Walking = false;
                    //StartCoroutine(GameLibOfMethods.DoAction(invActions.TakeShower, 10,true,true));

                    GameLibOfMethods.animator.SetBool("Jumping", false);
                    GameLibOfMethods.animator.SetBool("Lifting", true);

                    percentage = 0;
                    yield return new WaitForEndOfFrame();
                    StartCoroutine(Lifting());


                    yield break;

                }

                yield return new WaitForFixedUpdate();
            }

        }

    }
    public static IEnumerator Lifting()
    {
        SpriteControler.Instance.FaceRIGHT();
        while (!Input.GetKey(InteractionChecker.Instance.KeyToInteract) && !PlayerStatsManager.Instance.passingOut)
        {


            PlayerStatsManager.Strength.Instance.AddXP(0.027777778f);
            PlayerStatsManager.Instance.SubstractEnergy(0.027777778f);








            /*GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            //blackScreen.CrossFadeAlpha(0, 1, false);

            GameLibOfMethods.facingDir = Vector2.left;*/





            yield return new WaitForFixedUpdate();
        }
        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>())
        {
            Debug.Log("Playing exit sound");
            GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().PlayExitSound();
        }

        GameLibOfMethods.animator.SetBool("Lifting", false);
        yield return new WaitForEndOfFrame();

        AtommInventory.StaticCoroutine.Start(InteractionChecker.Instance.JumpOff());

    }
    public IEnumerator SlowlyApproachToTakeADump()
    {
        if (!GameLibOfMethods.doingSomething)
        {

            float percentage = 0;

            GameLibOfMethods.canInteract = false;
            GameLibOfMethods.cantMove = true;

            Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, true);

            GameLibOfMethods.TempPos = GameLibOfMethods.player.transform.position;

            GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            GameLibOfMethods.animator.SetBool("Jumping", true);
            GameLibOfMethods.doingSomething = true;




            while (true)
            {
                percentage += JumpSpeed;
                // Debug.Log(percentage);

                GameLibOfMethods.player.transform.position = Vector2.Lerp(GameLibOfMethods.TempPos, GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().CharacterPosition.position, percentage);
                GameLibOfMethods.player.transform.localScale = new Vector3(jumpCurve.Evaluate(percentage), jumpCurve.Evaluate(percentage), 1);
                /*GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
                
                foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
                }*/


                if (percentage >= 1)
                {
                    GameLibOfMethods.player.transform.position = GameLibOfMethods.lastInteractable.GetComponent<Toilet>().CharacterPosition.position;
                    //StartCoroutine(FadeIn());

                    GameLibOfMethods.cantMove = true;
                    SpriteControler.Instance.FaceLEFT();
                    GameLibOfMethods.animator.SetBool("Jumping", false);
                    GameLibOfMethods.animator.SetBool("TakingADump", true);


                    percentage = 0;
                    yield return new WaitForEndOfFrame();
                    SpriteControler.Instance.FaceLEFT();
                    StartCoroutine(TakingADump());
                    GameLibOfMethods.canInteract = true;


                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }

        }

    }
    public IEnumerator TakingADump()
    {
        SpriteControler.Instance.FaceLEFT();
        float timeWithFullBar = 0;
        while (!Input.GetKey(InteractionChecker.Instance.KeyToInteract) && !PlayerStatsManager.Instance.passingOut)
        {



            PlayerStatsManager.Instance.AddBladder(BladderGainAmount);


            if (PlayerStatsManager.Instance.Bladder >= PlayerStatsManager.Instance.MaxBladder)
            {
                timeWithFullBar += Time.deltaTime;

                if (timeWithFullBar >= 2)
                {
                    GameLibOfMethods.CreateFloatingText("You have no juice left in your body.", 2);
                    break;
                }

            }





            /*GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            //blackScreen.CrossFadeAlpha(0, 1, false);

            GameLibOfMethods.facingDir = Vector2.left;*/



            yield return new WaitForFixedUpdate();
        }
        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>())
        {
            GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().PlayExitSound();
        }

        GameLibOfMethods.animator.SetBool("TakingADump", false);
        yield return new WaitForEndOfFrame();

        AtommInventory.StaticCoroutine.Start(InteractionChecker.Instance.JumpOff());
    }
    public IEnumerator ApproachAndRunOnTreadmill()
    {
        if (!GameLibOfMethods.doingSomething)
        {

            float percentage = 0;

            GameLibOfMethods.canInteract = false;
            GameLibOfMethods.cantMove = true;

            Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, true);

            GameLibOfMethods.TempPos = GameLibOfMethods.player.transform.position;

            GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            GameLibOfMethods.animator.SetBool("Jumping", true);
            GameLibOfMethods.doingSomething = true;

            while (true)
            {
                percentage += JumpSpeed;
                Debug.Log(percentage);

                GameLibOfMethods.player.transform.position = Vector2.Lerp(GameLibOfMethods.TempPos, GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().CharacterPosition.position, percentage);
                GameLibOfMethods.player.transform.localScale = new Vector3(jumpCurve.Evaluate(percentage), jumpCurve.Evaluate(percentage), 1);
                /*GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
                
                foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
                }*/


                if (percentage >= 1)
                {
                    GameLibOfMethods.player.transform.position = GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().CharacterPosition.position;
                    //StartCoroutine(FadeIn());

                    GameLibOfMethods.cantMove = true;
                    SpriteControler.Instance.FaceLEFT();
                    GameLibOfMethods.animator.SetBool("Jumping", false);
                    GameLibOfMethods.animator.SetBool("Walking", true);


                    percentage = 0;
                    yield return new WaitForEndOfFrame();
                    SpriteControler.Instance.FaceLEFT();
                    StartCoroutine(RunningOnTreadmill());
                    GameLibOfMethods.canInteract = true;


                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }

        }

    }
    public static IEnumerator RunningOnTreadmill()
    {
        SpriteControler.Instance.FaceLEFT();
        while (!Input.GetKey(InteractionChecker.Instance.KeyToInteract) && !PlayerStatsManager.Instance.passingOut)
        {
            PlayerStatsManager.Fitness.Instance.AddXP(0.027777778f);
            PlayerStatsManager.Instance.SubstractEnergy(0.027777778f);
            GameLibOfMethods.animator.SetBool("Walking", true);

            /*GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            //blackScreen.CrossFadeAlpha(0, 1, false);

            GameLibOfMethods.facingDir = Vector2.left;*/

            yield return new WaitForFixedUpdate();

        }

        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>())
        {
            Debug.Log("Playing exit sound");
            GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().PlayExitSound();
        }

        GameLibOfMethods.animator.SetBool("Walking", false);
        yield return new WaitForEndOfFrame();

        AtommInventory.StaticCoroutine.Start(InteractionChecker.Instance.JumpOff());
    }

    public IEnumerator SlowlyApproachToTakeShower()
    {
        yield return new WaitForEndOfFrame();

        if (!GameLibOfMethods.doingSomething)
        {
            GameLibOfMethods.doingSomething = true;
            float percentage = 0;
            GameLibOfMethods.canInteract = false;
            GameLibOfMethods.cantMove = true;
            GameLibOfMethods.Walking = true;
            Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, true);

            GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().Emission.enabled = true;
            GameLibOfMethods.TempPos = GameLibOfMethods.player.transform.position;
            GameLibOfMethods.animator.SetBool("Jumping", true);

            while (true)
            {

                percentage += JumpSpeed;

                GameLibOfMethods.player.transform.position = Vector2.Lerp(GameLibOfMethods.TempPos, GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().CharacterPosition.position, percentage);
                GameLibOfMethods.player.transform.localScale = new Vector3(jumpCurve.Evaluate(percentage), jumpCurve.Evaluate(percentage), 1);

                /*GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));

                foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
                }*/


                if (percentage >= 1)
                {
                    //GameLibOfMethods.player.transform.position = GameLibOfMethods.lastInteractable.GetComponent<Shower>().StandingPoint.position;
                    //StartCoroutine(FadeIn());
                    SpriteControler.Instance.FaceDOWN();
                    GameLibOfMethods.cantMove = true;
                    GameLibOfMethods.Walking = false;
                    //StartCoroutine(GameLibOfMethods.DoAction(invActions.TakeShower, 10,true,true));

                    GameLibOfMethods.animator.SetBool("Jumping", false);
                    GameLibOfMethods.animator.SetBool("TakingShower", true);

                    percentage = 0;
                    yield return new WaitForEndOfFrame();
                    StartCoroutine(TakingShower());
                    GameLibOfMethods.canInteract = true;
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
    public IEnumerator TakingShower()
    {
        if (!GameLibOfMethods.canInteract)
        {
            yield return new WaitForEndOfFrame();
            float timeWithFullBar = 0;
            while (!Input.GetKey(InteractionChecker.Instance.KeyToInteract) && !PlayerStatsManager.Instance.passingOut && !GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().isBroken)
            {


                PlayerStatsManager.Instance.AddHygiene(HygieneGainAmount);

                float chance = Random.Range(0f, 100f);
                if (chance <= GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().breakChancePerSecond / 60)
                {
                    GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().isBroken = true;
                    GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().Break();
                }


                if (PlayerStatsManager.Instance.Hygiene >=  PlayerStatsManager.Instance.MaxHygiene)
                {
                    timeWithFullBar += Time.deltaTime;

                    if (timeWithFullBar >= 2)
                    {
                        GameLibOfMethods.CreateFloatingText("You are too clean for this.", 2);
                        break;
                    }
                }

                /*GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                //blackScreen.CrossFadeAlpha(0, 1, false);

                GameLibOfMethods.facingDir = Vector2.left;*/
                yield return new WaitForFixedUpdate();
            }

            if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>())
            {
                Debug.Log("Playing exit sound");
                GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().PlayExitSound();
            }


            GameLibOfMethods.animator.SetBool("TakingShower", false);
            yield return new WaitForEndOfFrame();

            AtommInventory.StaticCoroutine.Start(InteractionChecker.Instance.JumpOff());
        }
    }
    public void StartBrowsingInternet()
    {
        StartCoroutine(BrowsingInternet());
    }
    private IEnumerator BrowsingInternet()
    {
        if (!GameLibOfMethods.doingSomething)
        {

        
        GameLibOfMethods.cantMove = true;
        GameLibOfMethods.canInteract = false;
        GameLibOfMethods.animator.SetBool("Sitting", true);
        GameLibOfMethods.doingSomething = true;

        float moodDrainSpeed = GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().MoodDrainPerHour;
        float energyDrainSpeed = GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().EnergyDrainPerHour;
        float xpGainSpeed = GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().XpGainGetHour;
        yield return new WaitForEndOfFrame();
        Debug.Log("started Browsing internet");
        while (!Input.GetKey(InteractionChecker.Instance.KeyToInteract) && !PlayerStatsManager.Instance.passingOut)
        {

            GameLibOfMethods.animator.SetBool("Learning", true);
            PlayerStatsManager.Intelligence.Instance.AddXP(((xpGainSpeed) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
            PlayerStatsManager.Instance.Energy -=(((energyDrainSpeed) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
            PlayerStatsManager.Instance.Mood -=(((moodDrainSpeed) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);

            /*GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            //blackScreen.CrossFadeAlpha(0, 1, false);

            GameLibOfMethods.facingDir = Vector2.left;*/

            yield return new WaitForEndOfFrame();
        }

        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>())
        {
            Debug.Log("Playing exit sound");
            GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().PlayExitSound();
        }


        GameLibOfMethods.animator.SetBool("Learning", false);
        yield return new WaitForEndOfFrame();

        AtommInventory.StaticCoroutine.Start(WalkTo(GameLibOfMethods.TempPos, null));

            //Debug.Log("cant browse, busy doing something else");
        }
    }
    public IEnumerator DoActionOnInteractableObject(string animationName)
    {
        GameLibOfMethods.cantMove = true;
        GameLibOfMethods.canInteract = false;
        GameLibOfMethods.animator.SetBool(animationName, true);
        GameLibOfMethods.doingSomething = true;

        float moodDrainSpeed = GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().MoodDrainPerHour;
        float energyDrainSpeed = GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().EnergyDrainPerHour;
        float xpGainSpeed = GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().XpGainGetHour;
        yield return new WaitForEndOfFrame();
        //Debug.Log("started Browsing internet");
        while (!Input.GetKey(InteractionChecker.Instance.KeyToInteract) && !PlayerStatsManager.Instance.passingOut)
        {
            GameLibOfMethods.animator.SetBool(animationName, false);
            GameLibOfMethods.animator.SetBool("Learning", true);
            PlayerStatsManager.Intelligence.Instance.AddXP(((xpGainSpeed) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
            PlayerStatsManager.Instance.Energy -= (((energyDrainSpeed) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
            PlayerStatsManager.Instance.Mood -= (((moodDrainSpeed) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);

            /*GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            //blackScreen.CrossFadeAlpha(0, 1, false);

            GameLibOfMethods.facingDir = Vector2.left;*/

            yield return new WaitForEndOfFrame();
        }

        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>())
        {
            Debug.Log("Playing exit sound");
            GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().PlayExitSound();
        }


        GameLibOfMethods.animator.SetBool("Learning", false);
        yield return new WaitForEndOfFrame();

        AtommInventory.StaticCoroutine.Start(WalkTo(GameLibOfMethods.TempPos, null));

        //Debug.Log("cant browse, busy doing something else");

    }

    public void _SlowlyApproachToSleep()
    {
        StartCoroutine(SlowlyApproachToSleep());
        //GameLibOfMethods.animator.SetBool("Jumping", true);
    }

    IEnumerator SlowlyApproachToSleep()
    {
        if (!GameLibOfMethods.doingSomething && PlayerStatsManager.Instance.Food > PlayerStatsManager.Instance.MaxFood * 0.1f &&
            PlayerStatsManager.Instance.Thirst > PlayerStatsManager.Instance.MaxThirst * 0.1f &&
            PlayerStatsManager.Instance.Bladder > PlayerStatsManager.Instance.MaxBladder * 0.1f &&
            PlayerStatsManager.Instance.Hygiene > PlayerStatsManager.Instance.MaxHygiene * 0.1f)
        {
            float percentage = 0;
            GameLibOfMethods.canInteract = false;
            GameLibOfMethods.cantMove = true;
            Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, true);
            GameLibOfMethods.TempPos = GameLibOfMethods.player.transform.position;

            GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //GameLibOfMethods.animator.SetBool("Jumping", true);

            while (true)
            {
                percentage += JumpSpeed;

                /*GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));

                foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
                }*/
                GameLibOfMethods.player.transform.position = Vector2.Lerp(GameLibOfMethods.TempPos, GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().CharacterPosition.position, percentage);
                GameLibOfMethods.player.transform.localScale = new Vector3(jumpCurve.Evaluate(percentage), jumpCurve.Evaluate(percentage), 1);

                yield return new WaitForFixedUpdate();

                if (percentage >= 1)
                {
                    //GameLibOfMethods.animator.SetBool("Jumping", false);
                    GameLibOfMethods.player.GetComponent<Animator>().SetBool("Sleeping", true);
                    
                    GameLibOfMethods.isSleeping = true;
                    GameLibOfMethods.cantMove = true;

                    Physics2D.IgnoreLayerCollision(GameLibOfMethods.player.layer, 10, true);

                    GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                    //blackScreen.CrossFadeAlpha(1, 1, false);

                    //PlayerStatsManager.SetStamina(100);
                    StartCoroutine(Sleeping());
                    GameLibOfMethods.AddChatMessege("Went to sleep.");


                    percentage = 0;
                    yield return new WaitForEndOfFrame();

                    yield break;
                }
            }
        }
    }
    public IEnumerator Sleeping()
    {
        yield return new WaitForEndOfFrame();
        float timeWithFullBar = 0;
        DayNightCycle.Instance.ChangeSpeedToSleepingSpeed();
        while (!Input.GetKey(InteractionChecker.Instance.KeyToInteract) && !PlayerStatsManager.Instance.passingOut &&
            !GameLibOfMethods.doingSomething && PlayerStatsManager.Instance.Food > PlayerStatsManager.Instance.MaxFood * 0.1f &&
            PlayerStatsManager.Instance.Thirst > PlayerStatsManager.Instance.MaxThirst * 0.1f &&
            PlayerStatsManager.Instance.Bladder > PlayerStatsManager.Instance.MaxBladder * 0.1f &&
            PlayerStatsManager.Instance.Hygiene > PlayerStatsManager.Instance.MaxHygiene * 0.1f)
        {
            //Debug.Log(Time.deltaTime);

            GameLibOfMethods.concecutiveSleepTime += (Time.deltaTime * DayNightCycle.Instance.currentTimeSpeedMultiplier) * DayNightCycle.Instance.speed;
            PlayerStatsManager.Instance.AddEnergy(((GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().EnergyGainPerHour) 
                * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
            PlayerStatsManager.Instance.AddMood(((GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().MoodGainPerHour)
                * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
            PlayerStatsManager.Instance.Heal(((GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().HealthGainPerHour)
               * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);


            if (PlayerStatsManager.Instance.Energy >= PlayerStatsManager.Instance.MaxEnergy)
            {
                timeWithFullBar += Time.deltaTime;

                if (timeWithFullBar >= 2)
                {
                    GameLibOfMethods.CreateFloatingText("Can't sleep more", 2);
                    break;
                }
            }
            yield return new WaitForEndOfFrame();


        }

        if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>())
        {
            Debug.Log("Playing exit sound");
            GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().PlayExitSound();
        }


        yield return new WaitForEndOfFrame();
        GameLibOfMethods.player.GetComponent<Animator>().SetBool("Sleeping", false);
        //StartCoroutine(JumpOff());
        DayNightCycle.Instance.ChangeSpeedToNormal();
        GameManager.Instance.SaveGame();
    }
    public IEnumerator SleepingOnFloor()
    {
        Debug.Log(PlayerStatsManager.Instance.passingOut);
        if (!PlayerStatsManager.Instance.passingOut)
        {
            Debug.Log(PlayerStatsManager.Instance.passingOut);
            PlayerStatsManager.Instance.passingOut = true;
            yield return new WaitForEndOfFrame();
            DayNightCycle.Instance.ChangeSpeedToSleepingSpeed();
            while (PlayerStatsManager.Instance.Energy < 50)
            {
                //Debug.Log(Time.deltaTime);


                PlayerStatsManager.Instance.AddEnergy(((EnergyGainSpeedWhenPassedOut)
                    * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
                PlayerStatsManager.Instance.Health -= (((EnergyGainSpeedWhenPassedOut * 0.5f)
                  * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);



                yield return new WaitForEndOfFrame();


            }

            if (GameLibOfMethods.lastInteractable && GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>())
            {
                Debug.Log("Playing exit sound");
                GameLibOfMethods.lastInteractable.GetComponent<BreakableFurniture>().PlayExitSound();
            }


            yield return new WaitForEndOfFrame();
            GameLibOfMethods.player.GetComponent<Animator>().SetBool("PassOutToSleep", false);
            //StartCoroutine(JumpOff());
            DayNightCycle.Instance.ChangeSpeedToNormal();
            PlayerStatsManager.Instance.passingOut = false;
            ResetPlayer();
            //GameManager.Instance.SaveGame();
        }
    }

    public void JumpOffBed()
    {
        StartCoroutine(JumpOff());
    }
    public void ParalizePlayer()
    {
        GameLibOfMethods.doingSomething = true;
        GameLibOfMethods.canInteract = false;
        GameLibOfMethods.cantMove = true;
    }

    public IEnumerator FinishAnimation()
    {
        float percentage = 0;
        bool secondPhase = false;
        while (!secondPhase)
        {
            percentage += 0.02f;
            GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));

            foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
            }
            if (percentage >= 1f)
            {
                percentage = 0;
                secondPhase = true;
            }
            yield return new WaitForFixedUpdate();
        }

        while (secondPhase)
        {
            percentage += 0.02f;
            GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0, 1, percentage));

            foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0, 1, percentage));
            }
            if (percentage >= 1f)
            {

                yield break;
            }
            yield return new WaitForFixedUpdate();
        }


    }

    public IEnumerator FadeOut()
    {

        float percentage = 0;
        while (true)
        {
            percentage += 0.02f;
            GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));

            foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, percentage));
            }
            if (percentage >= 1f)
            {
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator FadeIn()
    {
        float percentage = 0;
        while (true)
        {
            percentage += 0.02f;
            GameLibOfMethods.player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0, 1, percentage));

            foreach (SpriteRenderer sprite in GameLibOfMethods.player.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0, 1, percentage));
            }
            if (percentage >= 1f)
            {
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator JumpOff()
    {
        ResetAnimations();

        float percentage = 0;
        GameLibOfMethods.animator.SetBool("Jumping", true);
        Vector2 temp = GameLibOfMethods.player.transform.position;
        while (true)
        {
            percentage += JumpSpeed;

            GameLibOfMethods.player.transform.position = Vector2.Lerp(temp, GameLibOfMethods.TempPos, percentage);
            GameLibOfMethods.player.transform.localScale = new Vector3(jumpCurve.Evaluate(percentage), jumpCurve.Evaluate(percentage), 1);
            if (percentage >= 1f)
            {

                ResetPlayer(false);
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator JumpTo(Vector2 JumpTo, UnityAction WhatToDoAfterJump)
    {
        ResetAnimations();

        float percentage = 0;
        GameLibOfMethods.animator.SetBool("Jumping", true);
        Vector2 temp = GameLibOfMethods.player.transform.position;
        while (true)
        {
            percentage += JumpSpeed;

            GameLibOfMethods.player.transform.position = Vector2.Lerp(temp, JumpTo, percentage);
            GameLibOfMethods.player.transform.localScale = new Vector3(jumpCurve.Evaluate(percentage), jumpCurve.Evaluate(percentage), 1);
            if (percentage >= 1f)
            {

                ResetPlayer(false);
                WhatToDoAfterJump();
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator JumpTo(Vector2 JumpTo)
    {
        ResetAnimations();

        float percentage = 0;
        GameLibOfMethods.animator.SetBool("Jumping", true);
        Vector2 temp = GameLibOfMethods.player.transform.position;
        while (true)
        {
            percentage += JumpSpeed;

            GameLibOfMethods.player.transform.position = Vector2.Lerp(temp, JumpTo, percentage);
            GameLibOfMethods.player.transform.localScale = new Vector3(jumpCurve.Evaluate(percentage), jumpCurve.Evaluate(percentage), 1);
            if (percentage >= 1f)
            {

                ResetPlayer(false);
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public void SleepOnFloor()
    {
        StartCoroutine(SleepingOnFloor());
    }
    public void ResetAnimations()
    {
        GameLibOfMethods.animator.SetBool("Lifting", false);
        GameLibOfMethods.animator.SetBool("TakingADump", false);
        GameLibOfMethods.animator.SetBool("TakingShower", false);
        GameLibOfMethods.animator.SetBool("Cooking", false);
        GameLibOfMethods.animator.SetBool("Sleeping", false);
        GameLibOfMethods.animator.SetBool("Jumping", false);
        GameLibOfMethods.animator.SetBool("Eating", false);
        GameLibOfMethods.animator.SetBool("Learning", false);
        GameLibOfMethods.animator.SetBool("Sitting", false);
        GameLibOfMethods.animator.SetBool("Drinking", false);
        GameLibOfMethods.animator.SetBool("Fixing", false);
        GameLibOfMethods.animator.SetBool("PissingInPants", false);
        GameLibOfMethods.animator.SetBool("PassOutToSleep", false);
    }
    public IEnumerator WalkTo(Vector2 WalkTo, UnityAction WhatToDoAfter)
    {
        ResetAnimations();
        GameLibOfMethods.doingSomething = true;
        GameLibOfMethods.canInteract = false;
        float percentage = 0;

        Vector2 temp = GameLibOfMethods.player.transform.position;
        while (true)
        {
            percentage += 0.04f;
            if (Mathf.Abs((WalkTo - temp).normalized.x) < Mathf.Abs((WalkTo - temp).normalized.y))
            {
                GameLibOfMethods.animator.SetFloat("Vertical", (WalkTo - temp).normalized.y);
            }
            else
            {
                GameLibOfMethods.animator.SetFloat("Horizontal", (WalkTo - temp).normalized.x);
            }

            GameLibOfMethods.animator.SetBool("Walking", true);
            GameLibOfMethods.player.transform.position = Vector2.Lerp(temp, WalkTo, percentage);

            if (GameLibOfMethods.animator.GetFloat("Vertical") < 0)
            {
                SpriteControler.Instance.FaceDOWN();
            }

            if (GameLibOfMethods.animator.GetFloat("Vertical") > 0)
            {
                SpriteControler.Instance.FaceUP();
            }
            if (GameLibOfMethods.animator.GetFloat("Horizontal") < 0)
            {
                SpriteControler.Instance.FaceLEFT();
            }

            if (GameLibOfMethods.animator.GetFloat("Horizontal") > 0)
            {
                SpriteControler.Instance.FaceRIGHT();
            }
            if (percentage >= 1f)
            {

                ResetPlayer(false);
                if (WhatToDoAfter != null)
                    WhatToDoAfter();
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public static IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }

        // now this function returns
    }

}
