using UnityEngine;

public class BombController : MonoBehaviour
{
    public float lifetime = 1f; // 폭탄이 생성된 후 유지되는 시간
    public GameObject explosionPrefab; // 폭발 프리팹
    private void Start()
    {
        // 일정 시간이 지나면 폭탄 제거
        Invoke("Missed", lifetime);
    }

    public void OnMouseDown()
    {
        // 폭탄 클릭 시 제거
        BombGameManager.instance.BombClicked(); // GameManager에서 점수 업데이트
        Destroy(gameObject);
    }

    private void Missed()
    {
        // 폭탄을 클릭하지 못했을 경우
        if (gameObject != null)
        {
            BombGameManager.instance.BombMissed(); // GameManager에서 목숨 감소
            Instantiate(explosionPrefab, transform.parent).transform.localPosition = transform.localPosition;
            Destroy(gameObject);
        }
    }
}
