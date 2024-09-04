using UnityEngine;
using System.Collections.Generic;

public class FolderManager : MonoBehaviour
{
    public Transform fileGrid; // 파일 프리팹들이 배치될 그리드
    public GameObject filePrefab; // 기본 파일 프리팹 (필요 시)

    // 파일 목록 업데이트
    public void UpdateFileList(List<GameObject> filePrefabs)
    {
        // 기존 파일들을 제거
        foreach (Transform child in fileGrid)
        {
            Destroy(child.gameObject);
        }

        // 새로운 파일 프리팹 추가
        foreach (GameObject filePrefab in filePrefabs)
        {
            // 그리드에 파일 프리팹을 인스턴스화
            Instantiate(filePrefab, fileGrid);
        }
    }
}
