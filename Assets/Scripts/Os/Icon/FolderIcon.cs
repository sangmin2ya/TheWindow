using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FolderIcon : MonoBehaviour
{
    public List<GameObject> nestedFilePrefabs; // 해당 파일이 포함하는 하위 파일 프리팹 리스트 (폴더처럼 작동)
    private Button button; // 파일 버튼
    private FolderSystem folderSystem; // 파일을 관리하는 시스템 참조

    private void Awake()
    {
        // FolderSystem 스크립트 자동 참조
        folderSystem = FindObjectOfType<FolderSystem>();

        if (folderSystem == null)
        {
            Debug.LogError("FolderSystem이 계층 구조에 없습니다!");
        }

        // Button 컴포넌트를 자동으로 찾기
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button 컴포넌트가 없습니다!");
        }

        // 버튼 클릭 시 해당 파일의 하위 파일을 표시
        button.onClick.AddListener(OnFileClick);
    }

    // 파일 버튼 클릭 시 호출되는 함수
    public void OnFileClick()
    {
        if (folderSystem != null && nestedFilePrefabs.Count > 0)
        {
            // 하위 파일 목록이 있는 경우 그리드에 표시
            folderSystem.UpdateFileList(nestedFilePrefabs);
        }
        else
        {
            Debug.Log("하위 파일 목록이 없습니다.");
        }
    }

    // 파일에 하위 파일 프리팹을 추가하는 함수
    public void AddNestedFile(GameObject nestedFilePrefab)
    {
        nestedFilePrefabs.Add(nestedFilePrefab);
    }
}