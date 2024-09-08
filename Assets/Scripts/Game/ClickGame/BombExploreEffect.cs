using UnityEngine;
class BombExploreEffect : MonoBehaviour
{
    private void Start()
    {
        Invoke("DestroyEffect", 1f);
    }
    private void DestroyEffect()
    {
        Destroy(gameObject);
    }
}