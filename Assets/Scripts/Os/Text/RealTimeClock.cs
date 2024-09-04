using TMPro;
using UnityEngine;
using System;

public class RealTimeClock : MonoBehaviour
{
    public TMP_Text timeText; // TextMeshPro 텍스트 컴포넌트

    void Update()
    {
        // 현재 시간 가져오기
        DateTime now = DateTime.Now;

        // 시간과 날짜 포맷팅 (시간:분:초 형식으로 표시)
        string currentTime = now.ToString("HH:mm:ss\nyyyy-MM-dd");

        // 텍스트 컴포넌트에 현재 시간 출력
        timeText.text = currentTime;
    }
}
