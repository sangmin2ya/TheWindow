using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
    public Sprite onSprite;
    public Sprite offSprite;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // void OnEnable()
    // {
    //     audioSource.Pause();
    //     transform.Find("Singer/Play").GetComponent<UnityEngine.UI.Image>().sprite = onSprite;
    // }
    // Update is called once per frame
    void Update()
    {
        audioSource.volume = SettingManager.Instance.Volume;
    }
    public void OnOffMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            transform.Find("Image/Singer/Play").GetComponent<UnityEngine.UI.Image>().sprite = onSprite;
        }
        else
        {
            audioSource.Play();
            transform.Find("Image/Singer/Play").GetComponent<UnityEngine.UI.Image>().sprite = offSprite;
        }
    }

}
