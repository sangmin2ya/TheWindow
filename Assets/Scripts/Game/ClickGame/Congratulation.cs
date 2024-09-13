using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 추가

public class Congratulation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeTextColorCoroutine(GetComponent<TextMeshProUGUI>()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator ChangeTextColorCoroutine(TextMeshProUGUI textMeshPro)
    {
        float hue = 0f; // 색상값 (0은 빨강, 1은 다시 빨강)
        float saturation = 1f; // 채도 (1은 최대 채도)
        float value = 1f; // 명도 (1은 최대 밝기)

        while (true)
        {
            // 색상을 HSV로 변환 후 텍스트 색상에 적용
            textMeshPro.color = Color.HSVToRGB(hue, saturation, value);

            // 색상(hue)값을 시간에 따라 증가시킴 (무지개 효과)
            hue += Time.deltaTime / 3;
            if (hue > 1f)
            {
                hue -= 1f; // hue는 0~1 사이 값을 가지므로 1을 초과하면 다시 0으로 리셋
            }

            // 프레임마다 업데이트
            yield return null;
        }
    }
}
