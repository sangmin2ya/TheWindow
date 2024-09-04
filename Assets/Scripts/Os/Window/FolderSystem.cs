using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FolderSystem : MonoBehaviour
{
    // 잠긴 파일 이름과 그에 대응하는 비밀번호 리스트 선언
    public List<string> lockedFileNames; // 잠긴 폴더의 이름 리스트
    public List<string> filePasswords;   // 잠긴 폴더의 비밀번호 리스트

    public GameObject passwordPromptPrefab; // 비밀번호 입력 창 프리팹
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
        currentPath = new List<string> { transform.parent.GetComponent<Window>().windowType == WindowType.Folder ? "My Computer" : "Trash Can" };  // 첫 경로 "My Computer"로 설정

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
        string folderName = fileIcon.gameObject.name;

        // 잠긴 폴더인지 확인
        if (lockedFileNames.Contains(folderName))
        {
            int folderIndex = lockedFileNames.IndexOf(folderName);
            string folderPassword = filePasswords[folderIndex];

            // 비밀번호 입력 창 띄우기
            ShowPasswordPrompt(folderPassword, () => OpenFolder(fileIcon));
        }
        else
        {
            // 잠기지 않은 폴더는 바로 열기
            OpenFolder(fileIcon);
        }
    }

    // 잠금 해제된 폴더를 여는 함수
    private void OpenFolder(FolderIcon fileIcon)
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
        UpdatePathText();
    }
    public void OnFileClick(GameObject filePrefab)
    {
        string fileName = filePrefab.name;

        // 파일 이름이 잠긴 파일 리스트에 있는지 확인
        if (lockedFileNames.Contains(fileName))
        {
            int fileIndex = lockedFileNames.IndexOf(fileName);  // 파일의 인덱스를 가져옴
            string filePassword = filePasswords[fileIndex];     // 해당 파일의 비밀번호를 가져옴

            // 비밀번호 입력 창을 띄움
            ShowPasswordPrompt(filePassword, () => OpenFile(filePrefab));
        }
        else
        {
            // 잠기지 않은 파일은 바로 염
            OpenFile(filePrefab);
        }
    }
    // 비밀번호 창을 띄우는 함수
    private void ShowPasswordPrompt(string correctPassword, System.Action onPasswordCorrect)
    {
        // Canvas를 찾아서 그 안에 비밀번호 입력 창을 추가
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas != null)
        {
            // 비밀번호 입력 창을 Canvas의 자식으로 설정
            GameObject promptInstance = Instantiate(passwordPromptPrefab, canvas.transform);
            PasswordPrompt prompt = promptInstance.GetComponent<PasswordPrompt>();

            if (prompt != null)
            {
                prompt.Initialize(correctPassword, onPasswordCorrect,
                                  () => Debug.Log("비밀번호가 틀렸습니다!"));
            }
        }
        else
        {
            Debug.LogError("Canvas가 계층 구조에 없습니다!");
        }
    }
    // 잠금 해제된 파일을 여는 함수
    private void OpenFile(GameObject filePrefab)
    {
        // 현재 경로와 파일 목록을 Back 스택에 저장
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