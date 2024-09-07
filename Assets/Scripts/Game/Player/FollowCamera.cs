using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Vector2 cameraOffset;

    void LateUpdate()
    {
        if (PlayerController.Instance != null)
            transform.position = new Vector3(0, PlayerController.Instance.transform.position.y, transform.position.z) + (Vector3)cameraOffset;
    }
}
