using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FolderSystem : MonoBehaviour
{
    public List<GameObject> initialFilePrefabs = new List<GameObject>();  // 외부에서 할당된 초기 파일 프리팹 리스트
    public TextMeshProUGUI pathText;  // 경로를 표시할 TextMeshPro 텍스트
    public Transform fileGrid;  // 파일 버튼들이 배치될 그리드의 Transform
    public GameObject fileButtonPrefab;  // 파일 버튼의 프리팹
    public Button backButton;  // 뒤로가기 버튼
    public TextMeshProUGUI fileNameText;  // 열려 있는 파일 이름을 표시할 TextMeshPro 텍스트

    private List<GameObject> currentFilePrefabs = new List<GameObject>(); // 현재 파일 프리팹 리스트
    private Stack<FolderState> backStack = new Stack<FolderState>(); // 뒤로가기 스택
    private List<string> currentPath = new List<string>(); // 현재 경로를 추적
    private FolderState initialState; // 초기 상태 저장 (최초 상태)

    private bool isNavigatingBack = false; // 뒤로가기를 통해 이동 중인지 확인하는 플래그
    private bool isFirstUpdate = true; // 첫 번째 업데이트인지 확인

    private void Start()
    {
        // 버튼 이벤트 설정
        backButton.onClick.AddListener(OnBackButtonClick);

        // 초기 경로 설정 (가장 처음 상태의 경로를 설정)
        currentPath = new List<string> { "My Computer" };  // 첫 경로 "My Computer"로 설정

        // 초기 상태를 외부에서 주어진 파일 프리팹 리스트로 설정
        initialState = new FolderState(new List<string>(currentPath), new List<GameObject>(initialFilePrefabs));
        backStack.Push(initialState);  // 초기 상태를 스택에 저장

        // 경로 텍스트 초기화
        pathText.text = string.Join(" > ", currentPath.ToArray());

        // 최초 상태 유지
        UpdateFileList(initialFilePrefabs);
    }

    // 파일 목록을 업데이트하는 함수
    public void UpdateFileList(List<GameObject> filePrefabs)
    {
        // 현재 파일 목록 업데이트
        currentFilePrefabs = filePrefabs;

        // 그리드 내의 기존 파일 버튼 제거
        foreach (Transform child in fileGrid)
        {
            Destroy(child.gameObject);
        }

        // 파일 버튼 새로 생성
        foreach (GameObject filePrefab in filePrefabs)
        {
            CreateFileButton(filePrefab);
        }

        // 경로 텍스트 갱신
        UpdatePathText();

        // 뒤로가기 버튼 상태 갱신
        UpdateBackButtonState();
    }

    // 경로 텍스트 갱신하는 함수
    private void UpdatePathText()
    {
        const int maxDisplayPathLength = 4; // 표시할 최대 경로 길이
        if (currentPath.Count > maxDisplayPathLength)
        {
            // 경로가 길 경우 첫 번째, 두 번째, 마지막 경로를 표시하고 중간은 "..."으로 생략
            string shortenedPath = $"{currentPath[0]} > {currentPath[1]} > ... > {currentPath[currentPath.Count - 1]}";
            pathText.text = shortenedPath;
        }
        else
        {
            // 경로가 짧을 경우 그냥 전체 경로 표시
            pathText.text = string.Join(" > ", currentPath.ToArray());
        }
    }


    private void CreateFileButton(GameObject filePrefab)
    {
        if (filePrefab.GetComponent<FolderIcon>() == null)
        {
            var go = Instantiate(filePrefab, fileGrid); // 기존의 fileButtonPrefab 대신 filePrefab 자체를 인스턴스화
            return;
        }
        GameObject button = Instantiate(fileButtonPrefab, fileGrid);

        // 파일 버튼의 텍스트에 프리팹 이름 설정
        button.GetComponentInChildren<TMP_Text>().text = filePrefab.name;

        // FileIcon 컴포넌트의 하위 파일 목록을 가져와 클릭 시 하위 파일 목록을 업데이트
        FolderIcon fileIcon = filePrefab.GetComponent<FolderIcon>();
        if (fileIcon != null && fileIcon.nestedFilePrefabs.Count > 0)
        {
            button.GetComponent<Button>().onClick.AddListener(() => OnFolderButtonClick(fileIcon));
        }
        else
        {
            // 하위 파일이 없는 경우 경로를 업데이트하고 빈 그리드 표시
            button.GetComponent<Button>().onClick.AddListener(() => OnFileClick(filePrefab));
        }
    }
    public void OnFolderButtonClick(FolderIcon fileIcon)
    {
        // 현재 경로와 파일 목록을 Back 스택에 저장
        backStack.Push(new FolderState(new List<string>(currentPath), new List<GameObject>(currentFilePrefabs)));

        // 현재 경로를 업데이트
        currentPath.Add(fileIcon.gameObject.name);

        // 파일 이름 텍스트 갱신
        fileNameText.text = fileIcon.gameObject.name;

        // 하위 파일로 폴더 열기
        UpdateFileList(fileIcon.nestedFilePrefabs);

        // 경로 텍스트도 업데이트
        pathText.text = string.Join(" > ", currentPath.ToArray());
    }


    // 하위 파일이 없는 파일을 클릭했을 때 호출되는 함수 (그리드 비우기)
    public void OnFileClick(GameObject filePrefab)
    {
        // 현재 경로와 파일 목록을 Back 스택에 저장 (빈 파일 목록도 스택에 추가)
        backStack.Push(new FolderState(new List<string>(currentPath), new List<GameObject>(currentFilePrefabs)));

        // 파일 이름 텍스트 갱신
        fileNameText.text = filePrefab.name;

        // 현재 경로에 파일 이름 추가
        currentPath.Add(filePrefab.name);

        // 빈 파일 목록 업데이트 (빈 그리드)
        UpdateFileList(new List<GameObject>());
    }
    public void OnBackButtonClick()
    {
        if (backStack.Count > 0)
        {
            // 스택에서 이전 상태를 가져옴
            FolderState previousState = backStack.Pop();

            // 경로와 파일 목록을 즉시 복원
            currentPath = new List<string>(previousState.path);  // 경로 복원
            currentFilePrefabs = new List<GameObject>(previousState.filePrefabs);  // 파일 목록 복원

            // 파일 목록과 경로 텍스트를 즉시 갱신
            UpdateFileList(currentFilePrefabs);

            // 경로 텍스트도 함께 업데이트
            pathText.text = string.Join(" > ", currentPath.ToArray());

            // 상단 파일 이름 텍스트 갱신
            if (currentPath.Count > 0)
            {
                fileNameText.text = currentPath[currentPath.Count - 1];  // 현재 경로의 마지막 파일 이름을 상단에 표시
            }
            else
            {
                fileNameText.text = "My Computer";  // 루트 폴더로 돌아간 경우 기본 값 설정
            }
        }

        // 뒤로가기 버튼 상태 갱신
        UpdateBackButtonState();
    }
    // 뒤로가기 버튼의 상태를 갱신하는 함수
    private void UpdateBackButtonState()
    {
        // 초기 화면일 때는 뒤로가기 버튼을 비활성화
        backButton.interactable = backStack.Count > 0;
    }

    // 폴더 상태를 저장하는 클래스 (경로와 파일 목록)
    private class FolderState
    {
        public List<string> path; // 경로
        public List<GameObject> filePrefabs; // 파일 목록

        public FolderState(List<string> path, List<GameObject> filePrefabs)
        {
            this.path = path;
            this.filePrefabs = filePrefabs;
        }
    }

}