using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarDropdownManager : MonoBehaviour
{
    public TMP_Dropdown yearDropdown;  // 년도 선택용 드롭다운
    public TMP_Dropdown monthDropdown; // 월 선택용 드롭다운
    public TMP_Dropdown dayDropdown;   // 날짜 선택용 드롭다운
    public List<GameObject> weatherPrefabs; // 6개의 랜덤 날씨 프리팹
    public GameObject cloudyPrefab; // 2002년 4월 12일 고정 날씨 프리팹 (흐림)
    public Transform weatherDisplay; // 날씨 프리팹을 표시할 위치

    private int selectedYear;
    private int selectedMonth;

    // 날짜별 날씨 데이터
    private Dictionary<string, GameObject> weatherData = new Dictionary<string, GameObject>();

    private void Start()
    {
        PopulateYearDropdown();  // 년도 드롭다운 초기화
        PopulateMonthDropdown(); // 월 드롭다운 초기화

        // 현재 연도와 월 기본 값 설정
        selectedYear = DateTime.Now.Year;
        selectedMonth = DateTime.Now.Month;

        // 기본 선택값 설정
        yearDropdown.value = yearDropdown.options.FindIndex(option => option.text == selectedYear.ToString());
        monthDropdown.value = selectedMonth - 1;

        PopulateDayDropdown(); // 날짜 드롭다운 초기화
        GenerateWeatherData(); // 게임 시작 시 날짜별 날씨 생성
    }

    // 년도 드롭다운 초기화
    private void PopulateYearDropdown()
    {
        yearDropdown.ClearOptions();
        List<string> years = new List<string>();
        for (int i = 1900; i <= 2100; i++)
        {
            years.Add(i.ToString());
        }
        yearDropdown.AddOptions(years);
        yearDropdown.onValueChanged.AddListener(delegate { OnYearChanged(); });
    }

    // 월 드롭다운 초기화
    private void PopulateMonthDropdown()
    {
        monthDropdown.ClearOptions();
        List<string> months = new List<string>
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };
        monthDropdown.AddOptions(months);
        monthDropdown.onValueChanged.AddListener(delegate { OnMonthChanged(); });
    }

    // 날짜 드롭다운 초기화
    private void PopulateDayDropdown()
    {
        dayDropdown.ClearOptions();
        List<string> days = new List<string>();

        // 현재 선택된 년도와 월의 날짜 수 계산
        int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
        for (int i = 1; i <= daysInMonth; i++)
        {
            days.Add(i.ToString());
        }
        dayDropdown.AddOptions(days);

        // 현재 날짜에 맞춰 기본 선택값 설정
        dayDropdown.value = DateTime.Now.Day - 1;
    }

    // 년도 변경 시 호출되는 함수
    private void OnYearChanged()
    {
        selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        PopulateDayDropdown(); // 년도가 변경되면 날짜 드롭다운 업데이트
    }

    // 월 변경 시 호출되는 함수
    private void OnMonthChanged()
    {
        selectedMonth = monthDropdown.value + 1; // 1월이 0이므로 +1
        PopulateDayDropdown(); // 월이 변경되면 날짜 드롭다운 업데이트
    }

    // 게임 시작 시 모든 날짜에 랜덤 날씨 할당
    private void GenerateWeatherData()
    {
        DateTime startDate = new DateTime(1900, 1, 1);
        DateTime endDate = new DateTime(2100, 12, 31);
        System.Random random = new System.Random();

        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            string dateKey = date.ToString("yyyy-MM-dd");

            if (date.Year == 2002 && date.Month == 4 && date.Day == 12)
            {
                // 2002년 4월 12일은 흐림 날씨 고정
                weatherData[dateKey] = cloudyPrefab;
            }
            else
            {
                // 그 외 날짜는 랜덤 날씨 할당
                int randomIndex = random.Next(weatherPrefabs.Count);
                weatherData[dateKey] = weatherPrefabs[randomIndex];
            }
        }
    }

    // 검색 버튼 클릭 시 호출되는 함수
    public void OnSearchButtonClick()
    {
        int selectedDay = dayDropdown.value + 1;
        string selectedDateKey = $"{selectedYear}-{selectedMonth:D2}-{selectedDay:D2}";

        if (weatherData.ContainsKey(selectedDateKey))
        {
            // 기존 날씨 프리팹 제거
            foreach (Transform child in weatherDisplay)
            {
                Destroy(child.gameObject);
            }

            // 선택한 날짜에 해당하는 날씨 프리팹 인스턴스화
            GameObject weatherPrefab = weatherData[selectedDateKey];
            Instantiate(weatherPrefab, weatherDisplay);
        }
        else
        {
            Debug.LogError("해당 날짜에 대한 날씨 데이터가 없습니다.");
        }
    }
}
