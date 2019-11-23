using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance;

    [Header("Sound options")]
   /* public Slider MasterVolumeSlider;
    public Toggle MasterVolumeToggle;

    public Slider SoundEffectsSlider;
    public Toggle SoundEffectsToggle;*/
    public List<AudioSource> SoundSources = new List<AudioSource>();

   /* public Slider MusicVolumeSlider;
    public Toggle MusicVolumeToggle;*/
    public List<AudioSource> MusicSources;
    public List<AudioSource> FootstepSources = new List<AudioSource>();


    // public bool IsFullscreen = true;

    private List<FullScreenMode> DisplayModes = new List<FullScreenMode>();
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        Initialize();
        SceneManager.sceneLoaded += delegate { Initialize(); };

        /* if (PlayerPrefs.HasKey("SoundEffectsVolume"))
         {
             SoundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");
         }

         else
         {
             Debug.Log("Don't have the key, creating new one.");
             PlayerPrefs.SetFloat("SoundEffectsVolume", 0.5f);

         }
         ChangeSoundEffectsVolume();
         SoundEffectsToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("SoundEffectsToggle"));
         SwitchSoundEffectsVolumeToggle();

         if (PlayerPrefs.HasKey("MusicEffectsVolume"))
         {
             MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicEffectsVolume");
         }

         else
         {
             Debug.Log("Don't have the key, creating new one.");
             PlayerPrefs.SetFloat("MusicEffectsVolume", 0.5f);

         }
         ChangeMusicVolume();
         MusicVolumeToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("MusicEffectsToggle"));
         SwitchMusicVolumeToggle();

         if (PlayerPrefs.HasKey("MasterEffectsVolume"))
         {
             MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterEffectsVolume");
         }

         else
         {
             Debug.Log("Don't have the key, creating new one.");
             PlayerPrefs.SetFloat("MasterEffectsVolume", 1);

         }
         ChangeMasterEffectsVolume();
         MasterVolumeToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("MasterEffectsToggle"));
         SwitchMasterVolumeToggle();



         ResolutionOptions.value = PlayerPrefs.GetInt("Resolution");
         ChangeResolution(ResolutionOptions);

         DisplayModeOptions.value = PlayerPrefs.GetInt("DisplayMode");
         ChangeDisplayMode(DisplayModeOptions);
         */

    }
    

   public void Initialize()
    {
        DisplayModes.Clear();
        DisplayModes.Add(FullScreenMode.Windowed);
        DisplayModes.Add(FullScreenMode.ExclusiveFullScreen);
        DisplayModes.Add(FullScreenMode.FullScreenWindow);
        DisplayModes.Add(FullScreenMode.MaximizedWindow);


        if (GameLibOfMethods.player)
        {
            FootstepSources.Add(GameLibOfMethods.player.GetComponent<AudioSource>());
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            foreach(AudioSource source in MusicSources)
            {
                source.enabled = (false);
            }
        }
        else
        {
            foreach (AudioSource source in MusicSources)
            {
                source.enabled = (true);
            }

        }
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (source)
            {


                if (!SoundSources.Contains(source) && !MusicSources.Contains(source) && !FootstepSources.Contains(source))
                {
                    SoundSources.Add(source);
                }
            }
        }
      

        foreach (Slider slider in FindObjectsOfType<Slider>())
        {
            slider.onValueChanged.AddListener(delegate { OnSliderChange(slider); });
            LoadSlider(slider);
            if (slider.gameObject.name == "Master Volume")
            {
                slider.onValueChanged.AddListener(delegate { ChangeMasterVolume(slider); });
                slider.onValueChanged.AddListener(delegate { UpdateAllSoundsAndToggles(); });
            }
            if (slider.gameObject.name == "Sound Volume")
            {
                slider.onValueChanged.AddListener(delegate { ChangeSoundVolumeOfAllSoundSources(slider); });
                slider.onValueChanged.AddListener(delegate { UpdateAllSoundsAndToggles(); });
            }
            if (slider.gameObject.name == "Music Volume")
            {
                slider.onValueChanged.AddListener(delegate { ChangeMusicVolumeOfAllMusicSources(slider); });
                slider.onValueChanged.AddListener(delegate { UpdateAllSoundsAndToggles(); });
            }
            if (slider.gameObject.name == "Footstep Volume")
            {
                slider.onValueChanged.AddListener(delegate { ChangeFootstepVolumeOfAllFootstepSources(slider); });
                slider.onValueChanged.AddListener(delegate { UpdateAllSoundsAndToggles(); });
            }
        }
        foreach (Button button in FindObjectsOfType<Button>())
        {
           
            if (button.gameObject.name == "Reset")
            {
                button.onClick.AddListener(delegate { ResetOptions(); });

            }
            if (button.gameObject.name == "Save")
            {
                button.onClick.AddListener(delegate { SavePlayerPrefs(); });

            }
           
        }


        foreach (TMP_Dropdown dropdown in FindObjectsOfType<TMP_Dropdown>())
        {
            LoadDropdown(dropdown);
            dropdown.onValueChanged.AddListener(
                delegate { OnDropdownChange(dropdown);});
            
           
            if(dropdown.gameObject.name == "Display Mode")
            {
                dropdown.ClearOptions();
                foreach(FullScreenMode mode in DisplayModes)
                {
                    dropdown.options.Add(new TMP_Dropdown.OptionData(mode.ToString()));
                }
            }
            
            LoadDropdown(dropdown);

        }
        
        foreach (Toggle toggle in FindObjectsOfType<Toggle>())
        {
            
            toggle.onValueChanged.AddListener(delegate { OnToggleChange(toggle); });
            LoadToggle(toggle);
            if (toggle.gameObject.name == "Master Toggle")
            {
                toggle.onValueChanged.AddListener(delegate { ApplyMasterToggle(toggle); });
                toggle.onValueChanged.AddListener(delegate { UpdateAllSoundsAndToggles(); });
            }
            if (toggle.gameObject.name == "Sound Toggle")
            {
                toggle.onValueChanged.AddListener(delegate { ApplySoundToggle(); });
                toggle.onValueChanged.AddListener(delegate { UpdateAllSoundsAndToggles(); });
            }
            if (toggle.gameObject.name == "Music Toggle")
            {
                toggle.onValueChanged.AddListener(delegate { ApplyMusicToggle(); });
                toggle.onValueChanged.AddListener(delegate { UpdateAllSoundsAndToggles(); });
            }
            if (toggle.gameObject.name == "Footstep Toggle")
            {
                toggle.onValueChanged.AddListener(delegate { ApplyFootstepToggle(); });
                toggle.onValueChanged.AddListener(delegate { UpdateAllSoundsAndToggles(); });
            }
          

            if (toggle.gameObject.name == "VSync Toggle")
            {
                toggle.onValueChanged.AddListener(delegate { SwitchVSync(toggle); });
                SwitchVSync(toggle);
            }
            LoadToggle(toggle);
            foreach (TMP_InputField Text in FindObjectsOfType<TMP_InputField>())
            {



                if (Text.gameObject.name == "MaxFPS")
                {
                    LoadInputField(Text);
                    Text.onEndEdit.AddListener(
                        delegate { OnInputChange(Text); });
                }

                LoadInputField(Text);

            }


        }
        UpdateAllSoundsAndToggles();

        Debug.Log("OnLevelWasLoaded");
    }
  
    public void SwitchVSync(Toggle toggle)
    {


        PlayerPrefs.SetInt("VSync Toggle", System.Convert.ToInt32(toggle.isOn));
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync Toggle", 0);


    }
    public void LoadSlider(Slider slider)
    {
       
            slider.value = PlayerPrefs.GetFloat(slider.gameObject.name, 0.5f);
        

        OnSliderChange(slider);
    }
   
    public void OnSliderChange(Slider slider)
    {
        PlayerPrefs.SetFloat(slider.gameObject.name, slider.value);
        

    }
    public void ChangeMusicVolumeOfAllMusicSources(Slider MusicVolumeSlider)
    {
        foreach (AudioSource source in MusicSources)
        {
            if(source)
            source.volume = MusicVolumeSlider.value * PlayerPrefs.GetFloat("Master Volume");
        }
    }
    public void ChangeFootstepVolumeOfAllFootstepSources(Slider FootstepVolumeSlider)
    {
        foreach (AudioSource source in FootstepSources)
        {
            if (source)
                source.volume = FootstepVolumeSlider.value * PlayerPrefs.GetFloat("Footstep Volume");
        }
    }
    public void ChangeSoundVolumeOfAllSoundSources(Slider MusicVolumeSlider)
    {
        foreach (AudioSource source in SoundSources)
        {
            if (source)
                source.volume = MusicVolumeSlider.value * PlayerPrefs.GetFloat("Master Volume");
        }
    }
    public void ChangeMasterVolume(Slider MasterSlider)
    {
        foreach (AudioSource source in MusicSources)
        {
            if(source)
            source.volume = MasterSlider.value * PlayerPrefs.GetFloat("Music Volume");
        }
        foreach (AudioSource source in SoundSources)
        {
            if (source)
                source.volume = MasterSlider.value * PlayerPrefs.GetFloat("Sound Volume"); 
        }
    }
    public void LoadToggle(Toggle toggle)
    {
      
       toggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt(toggle.gameObject.name));
        OnToggleChange(toggle);
    }
    public void OnToggleChange(Toggle toggle)
    {
        //SwitchSoundEffectsVolumeToggle();
        //SwitchMusicVolumeToggle();
        PlayerPrefs.SetInt(toggle.gameObject.name, System.Convert.ToInt32(toggle.isOn));


        if (toggle.gameObject.name == "VSync Toggle")
        {
            SwitchVSync(toggle);
        }
    }
    public void ApplySoundToggle()
    {
        foreach (AudioSource source in SoundSources)
        {
            if (source)
            {
                if (!System.Convert.ToBoolean(PlayerPrefs.GetInt("Sound Toggle")) && !System.Convert.ToBoolean(PlayerPrefs.GetInt("Master Toggle")))
                {
                    source.mute = false;
                }
                else
                {
                    source.mute = true;
                }
                
            }
               

        }
    }
    public void ApplyMusicToggle()
    {
        foreach (AudioSource source in MusicSources)
        {
            if (source)
            {
                if (!System.Convert.ToBoolean(PlayerPrefs.GetInt("Music Toggle")) && !System.Convert.ToBoolean(PlayerPrefs.GetInt("Master Toggle")))
                {
                    source.mute = false;
                }
                else
                {
                    source.mute = true;
                }

            }


        }
    }
    public void ApplyFootstepToggle()
    {
        foreach (AudioSource source in FootstepSources)
        {
            if (source)
            {
                if (!System.Convert.ToBoolean(PlayerPrefs.GetInt("Footstep Toggle")) && !System.Convert.ToBoolean(PlayerPrefs.GetInt("Master Toggle")))
                {
                    source.mute = false;
                }
                else
                {
                    source.mute = true;
                }

            }


        }
    }

    public void ApplyMasterToggle(Toggle toggle)
    {
        ApplyMusicToggle();
        ApplySoundToggle();
        ApplyFootstepToggle();
    }
    public void UpdateAllSoundsAndToggles()
    {
        foreach (AudioSource source in SoundSources)
        {
            if (source)
            {
                source.volume = PlayerPrefs.GetFloat("Master Volume") * PlayerPrefs.GetFloat("Sound Volume");
                if (!System.Convert.ToBoolean(PlayerPrefs.GetInt("Sound Toggle")) && !System.Convert.ToBoolean(PlayerPrefs.GetInt("Master Toggle")))
                {
                    source.mute = false;
                }
                else
                {
                    source.mute = true;
                }


            }



        }
        foreach (AudioSource source in MusicSources)
        {
            if (source)
            {
                source.volume = PlayerPrefs.GetFloat("Master Volume") * PlayerPrefs.GetFloat("Music Volume");
                if (!System.Convert.ToBoolean(PlayerPrefs.GetInt("Music Toggle")) && !System.Convert.ToBoolean(PlayerPrefs.GetInt("Master Toggle")))
                {
                    source.mute = false;
                }
                else
                {
                    source.mute = true;
                }


            }


        }
        foreach (AudioSource source in FootstepSources)
        {
            if (source)
            {
                source.volume = PlayerPrefs.GetFloat("Master Volume") * PlayerPrefs.GetFloat("Footstep Volume");
                if (!System.Convert.ToBoolean(PlayerPrefs.GetInt("Footstep Toggle")) && !System.Convert.ToBoolean(PlayerPrefs.GetInt("Master Toggle")))
                {
                    source.mute = false;
                }
                else
                {
                    source.mute = true;
                }


            }


        }
    }

    public void LoadDropdown(TMP_Dropdown dropdown)
    {

        

        
        dropdown.value = PlayerPrefs.GetInt(dropdown.gameObject.name, 0);
        OnDropdownChange(dropdown);

    }
    public void LoadInputField(TMP_InputField input)
    {




        input.text = PlayerPrefs.GetInt(input.gameObject.name, 60).ToString();
        OnInputChange(input);
        Debug.Log("OnInputChange" + PlayerPrefs.GetInt("MaxFPS", 60));
    }
    public void OnDropdownChange(TMP_Dropdown dropdown)
    {
        PlayerPrefs.SetInt(dropdown.gameObject.name, dropdown.value);
        dropdown.RefreshShownValue();
        if (dropdown.gameObject.name == "Display Mode")
        {


            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, DisplayModes[PlayerPrefs.GetInt("Display Mode", 0)]);
            Debug.Log("Changed display mode");
        }



    }
    public void OnInputChange(TMP_InputField input)
    {
        PlayerPrefs.SetInt(input.gameObject.name, int.Parse(input.text));
        if (input.gameObject.name == "MaxFPS")
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("MaxFPS", 60);
            Debug.Log("OnInputChange" + PlayerPrefs.GetInt("MaxFPS", 60));
            //Debug.Log("Changed resolution");
        }
       


    }

    public void ResetOptions()
    {
        PlayerPrefs.DeleteAll();
        OptionsManager.Instance.Initialize();
    }
    /*
    public void ChangeMasterEffectsVolume()
    {
        ChangeSoundEffectsVolume();
        ChangeMusicVolume();
        PlayerPrefs.SetFloat("MasterEffectsVolume", MasterVolumeSlider.value);
        PlayerPrefs.Save();
    }
    public void SwitchMasterVolumeToggle()
    {
        SwitchSoundEffectsVolumeToggle();
        SwitchMusicVolumeToggle();
        PlayerPrefs.SetInt("MasterEffectsToggle", System.Convert.ToInt32(MasterVolumeToggle.isOn));
        PlayerPrefs.Save();

    }







    public void SwitchSoundEffectsVolumeToggle()
    {
        PlayerPrefs.SetInt("SoundEffectsToggle", System.Convert.ToInt32(SoundEffectsToggle.isOn));
        PlayerPrefs.Save();
        foreach (AudioSource source in SoundSources)
        {
            if (!MasterVolumeToggle.isOn)
                source.mute = SoundEffectsToggle.isOn;
            else
                source.mute = true;
        }
       
    }
    public void ChangeSoundEffectsVolume()
    {
        PlayerPrefs.SetFloat("SoundEffectsVolume", SoundEffectsSlider.value);
        PlayerPrefs.Save();
        foreach (AudioSource source in SoundSources)
        {
            if (source)
                source.volume = SoundEffectsSlider.value * MasterVolumeSlider.value;
        }
       
    }






    public void SwitchMusicVolumeToggle()
    {
        PlayerPrefs.SetInt("MusicEffectsToggle", System.Convert.ToInt32(MusicVolumeToggle.isOn));
        PlayerPrefs.Save();
        foreach (AudioSource source in MusicSources)
        {
            if (!MasterVolumeToggle.isOn)
                source.mute = MusicVolumeToggle.isOn;
            else
                source.mute = true;
        }
       
    }
    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicEffectsVolume", MusicVolumeSlider.value);
        PlayerPrefs.Save();
        foreach (AudioSource source in MusicSources)
        {
            source.volume = MusicVolumeSlider.value * MasterVolumeSlider.value;
        }
       
    }*/
    public void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
  
}
