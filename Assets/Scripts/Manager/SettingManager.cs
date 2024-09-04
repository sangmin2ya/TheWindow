using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float brightness = 1.0f;
    public float Brightness { get { return brightness; } set { brightness = value; } }
    public void SetVisibility(bool value)
    {
        visibility = value;
        DesktopIcon[] desktopIcons = Resources.FindObjectsOfTypeAll<DesktopIcon>();

        foreach (DesktopIcon icon in desktopIcons)
        {
            icon.gameObject.SetActive(icon.IconType == IconVisibleType.Normal);
        }
    }
}