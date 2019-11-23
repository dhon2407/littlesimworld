using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMProDropdown = TMPro.TMP_Dropdown;
using DropdownData = TMPro.TMP_Dropdown.OptionData;

public class ResolutionChanger : MonoBehaviour
{
    [SerializeField]//Value set on editor
    private TMProDropdown dropdownList = null;
    private List<Resolution> availableResolutions;

    private void Awake()
    {
        availableResolutions = new List<Resolution>(Screen.resolutions);

        dropdownList.ClearOptions();

        for (int i = 0; i < availableResolutions.Count; i++)
            dropdownList.options.Add(new DropdownData(string.Format("{0} x {1}",
                                                      availableResolutions[i].width,
                                                      availableResolutions[i].height)));

        dropdownList.value = availableResolutions.FindIndex(res => res.Equals(Screen.currentResolution));
        dropdownList.onValueChanged.AddListener(ResolutionChanged);
    }

    public void SetToActualScreenResolution(bool enable)
    {
        Debug.Log("Setting to actual screen resoltion");

        if (enable)
        {
            ResolutionChanged(availableResolutions.Count - 1);
            dropdownList.value = availableResolutions.Count - 1;
        }
    }

    private void ResolutionChanged(int index)
    {
        Screen.SetResolution(availableResolutions[index].width,
                             availableResolutions[index].height,
                             Screen.fullScreen);
    }
}
