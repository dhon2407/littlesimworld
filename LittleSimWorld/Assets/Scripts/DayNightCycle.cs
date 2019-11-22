using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Experimental.Rendering.LWRP;
public class DayNightCycle : MonoBehaviour
{
    public UnityEngine.Experimental.Rendering.Universal.Light2D light2d;
    public float time;
    public TimeSpan currenttime;
    public TextMeshProUGUI HoursText;
    public TextMeshProUGUI MinutesText;
    public TextMeshProUGUI SeasonText;
    public TextMeshProUGUI AmPmText;
    public TextMeshProUGUI DayNumberText;
    public TextMeshProUGUI DayNameText;

    public GameObject Axis;
    public int days = 1;
    public int season;
    public int WeekDay;
    public float intensity;
    public Color fogday = Color.grey;
    public Color fognight = Color.blue;
    public int speed = 60;
    public int currentTimeSpeedMultiplier = 1;
    public string CurrentTime;
    public Color startColor;
    public Color endColor;
    public Color Midcolor;
    public static DayNightCycle Instance;
    public AnimationCurve curve;
    public Gradient LightGradient;
    

    public float every;   //The public variable "every" refers to "Lerp the color every X"
    float colorstep;
    Color[] colors = new Color[4]; //Insert how many colors you want to lerp between here, hard coded to 4
    int i;
    Color lerpedColor = Color.white;  //This should optimally be the color you are going to begin with


    //public bool repeat = false;
    //float startTime;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        colors[0] = Color.white;
        colors[1] = Midcolor;
        colors[2] = endColor;
        colors[3] = Color.white;
        light2d = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        //startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeTime();
        if (WeatherSystem.Instance.cloudyToday)
        {
            light2d.intensity = intensity - 0.30f;
        }
        if (WeatherSystem.Instance.sunnyToday)
        {
            light2d.intensity = intensity + 0.15f;
        }
        else
        {
            light2d.intensity = intensity;
        }


        if (time >= 38100) {
            if (colorstep < every)
            { //As long as the step is less than "every"
                lerpedColor = Color.Lerp(colors[i], colors[i + 1], colorstep);
                light2d.color = lerpedColor;
                colorstep += 0.025f;  //The lower this is, the smoother the transition
            }
            else
            { //Once the step equals the time we want to wait for the color, increment to lerp to the next color

                colorstep = 0;

                if (i < (colors.Length - 2))
                { //Keep incrementing until i + 1 equals the Lengh
                    i++;
                }
                else
                { //and then reset to zero
                    i = 0;
                }
            }
        }
        else if (time <= 38000)
        {
            light2d.color = Color.white;
        }
    }

    public void ChangeTime()
    {
        String[] week = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        String[] val = { "summer", "autumn", "winter", "spring" };
        time += (Time.deltaTime * speed) * currentTimeSpeedMultiplier;

        if (time > 86400)
        {
            days += 1;
            WeekDay += 1;
            if(JobsPopUp.CurrentJob!= null)
            JobsPopUp.CurrentJob.WorkedToday = false;
            WeatherSystem.Instance.ChooseWeatherForTheDay();
            Bank.Instance.AddPercentageToBalance();
            if (WeekDay >= 7)
            {
                WeekDay = WeekDay - WeekDay;
            }
            if (days >= 31)
            {
                days = days - days + 1;
                season += 1;

                if (season >= 4)
                {
                    season = season - season;
                }
            }
            time = 0;
        }

        currenttime = TimeSpan.FromSeconds(time);









        string[] temptime = currenttime.ToString().Split(":"[0]);
        temptime[1] = (Mathf.RoundToInt((float.Parse(temptime[1]) / 10)) * 10).ToString();
        if (temptime[1] == "0")
        {
            temptime[1] = "00";
        }
        if (temptime[1] == "60")
        {
            temptime[1] = "50";
        }
        //TimeText.text = temptime[0] + ":" + temptime[1];


        string hours = string.Format("{0:00}", TimeSpan.FromSeconds(time).Hours);
        string minutes = string.Format("{0:00}", temptime[1]);
        CurrentTime = hours + ":" + minutes;
        string AMhours = TimeSpan.FromSeconds(time).Hours.ToString();
        MinutesText.text = minutes;
        HoursText.text = hours;

        /*if (TimeSpan.FromSeconds(time).Hours >= 0 && TimeSpan.FromSeconds(time).Hours < 1)
        {
            CurrentTime  = 12 + ":" + temptime[1];
            MinutesText.text = temptime[1];
            HoursText.text = "12";
            AmPmText.text = "AM";

        }
        else if (TimeSpan.FromSeconds(time).Hours >= 1 && TimeSpan.FromSeconds(time).Hours < 12)
        {
            CurrentTime  = TimeSpan.FromSeconds(time).Hours + ":" + temptime[1];
            MinutesText.text = temptime[1];
            HoursText.text = TimeSpan.FromSeconds(time).Hours.ToString();
            AmPmText.text = "AM";
        }
        else if (TimeSpan.FromSeconds(time).Hours >= 12 && TimeSpan.FromSeconds(time).Hours < 13)
        {
            CurrentTime  = TimeSpan.FromSeconds(time).Hours + ":" + temptime[1];
            MinutesText.text = temptime[1];
            HoursText.text = TimeSpan.FromSeconds(time).Hours.ToString();
            AmPmText.text = "PM";
        }
        else if (TimeSpan.FromSeconds(time).Hours >= 13 && TimeSpan.FromSeconds(time).Hours < 24)
        {
            CurrentTime  = TimeSpan.FromSeconds(time).Hours - 12 + ":" + temptime[1];
            MinutesText.text = temptime[1];
            HoursText.text = (TimeSpan.FromSeconds(time).Hours - 12).ToString();
            AmPmText.text = "PM";
        }*/





        DayNumberText.text =  days.ToString();
        DayNameText.text = week[WeekDay];
        SeasonText.text = val[season];

       // transform.RotateAround(Axis.transform.position, Vector3.right, 1 * (Time.deltaTime / 240) * speed);



       /*if (time >= 0)
           intensity = 0;
        if (time > 21600)
            intensity = Mathf.Lerp(0, 0.5f, (time - 21600) / (46800));
        if (time >= 46800 + 21600)
            intensity = Mathf.Lerp(0.5f, 0, (time - (46800 + 21600)) / (86400 ));*/
        intensity = curve.Evaluate(Mathf.Lerp(0, 1, time / 86400));
        light2d.color = LightGradient.Evaluate(Mathf.Lerp(0, 1, time / 86400));



        /* if (!repeat)
         {
             if (time >= 43200)
             {
                 float t = (Time.deltaTime - startTime) * speed;
                 GetComponent<Light>().color = Color.Lerp(startColor, endColor, t);
             }
         }
         else
         {
             float t = (Mathf.Sin(Time.time - startTime) * speed);
             GetComponent<Light>().color = Color.Lerp(startColor, endColor, t);
         } */

        RenderSettings.fogColor = Color.Lerp(fognight, fogday, intensity * intensity);
    }

    public void ChangeSpeedToNormal()
    {
       
            currentTimeSpeedMultiplier = 1;
       
        
        
    }
    public void ChangeSpeedToFaster()
    {
        if (GameLibOfMethods.canInteract && !GameLibOfMethods.cantMove && !GameLibOfMethods.isSleeping)
        {
            currentTimeSpeedMultiplier = 10;
           
       }
            
    }
    public void ChangeSpeedToSupaFast()
    {
        if (!GameLibOfMethods.animator.GetBool("Walking") && !GameLibOfMethods.animator.GetBool("Jumping"))
        {
            currentTimeSpeedMultiplier = 100;
          
        }

    }
    public void ChangeSpeedToSleepingSpeed()
    {
        if (!GameLibOfMethods.animator.GetBool("Walking") && !GameLibOfMethods.animator.GetBool("Jumping"))
        {
            currentTimeSpeedMultiplier = 25;

        }

    }
}
