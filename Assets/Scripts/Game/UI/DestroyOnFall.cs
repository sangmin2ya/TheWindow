using UnityEngine;

public class DestroyOnFall : MonoBehaviour
{
    public float destroyYCoordinate = -10f; // 특정 Y좌표 설정

    void Update()
    {
        if (transform.position.y <= destroyYCoordinate)
        {
            Destroy(gameObject); // 오브젝트 파괴
        }
    }
}
