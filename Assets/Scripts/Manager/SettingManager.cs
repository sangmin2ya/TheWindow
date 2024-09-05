using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingManager : MonoBehaviour
{
    private static SettingManager _instance;
    public static SettingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject settingManagerObject = new GameObject("SettingManager");
                _instance = settingManagerObject.AddComponent<SettingManager>();
            }
            return _instance;
        }
    }
    private bool visibility = false;
    public bool Visibility { get { return visibility; } set { visibility = value; } }
    private float brightness = 0.5f;
    public float Brightness { get { return brightness; } set { brightness = value; } }
    private float volume = 1.0f;
    public float Volume { get { return volume; } set { volume = value; } }
    private bool unrockLogin = false;
    public bool UnrockLogin { get { return unrockLogin; } set { unrockLogin = value; } }
    public void SetVisibility(bool value)
    {
        visibility = value;
        DesktopIcon[] desktopIcons = Resources.FindObjectsOfTypeAll<DesktopIcon>();

        foreach (DesktopIcon icon in desktopIcons)
        {
            icon.gameObject.SetActive(icon.IconType == IconVisibleType.Normal);
        }
    }
    public void SetBrightness(float value)
    {
        Volume volume = GameObject.Find("Brightness").GetComponent<Volume>();
        if (volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
        {
            // Post Exposure 수치 조정
            colorAdjustments.postExposure.overrideState = true;
            colorAdjustments.postExposure.value = value * 2 - 1.5f;
        }
    }
}