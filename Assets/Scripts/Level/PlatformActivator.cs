using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivator : MonoBehaviour
{
    private void OnEnable()
    {
        for (int i = 3; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            if (transform.GetChild(i).IsChildOf(transform.GetChild(i)))
            {
                Transform bundle = transform.GetChild(i);
                for (int j = 0; j < bundle.childCount; j++)
                {
                    bundle.GetChild(j).gameObject.SetActive(true);
                }
            }
        }

    }

}
