using UnityEngine;

public class BombController : MonoBehaviour
{
    public float lifetime = 1f; // 폭탄이 생성된 후 유지되는 시간

    private void Start()
    {
        // 일정 시간이 지나면 폭탄 제거
        Invoke("Missed", lifetime);
    }

    private void OnMouseDown()
    {
        // 폭탄 클릭 시 제거
        Destroy(gameObject);
        BombGameManager.instance.BombClicked(); // GameManager에서 점수 업데이트
    }

    private void Missed()
    {
        // 폭탄을 클릭하지 못했을 경우
        if (gameObject != null)
        {
            BombGameManager.instance.BombMissed(); // GameManager에서 목숨 감소
            Destroy(gameObject);
        }
    }
}
