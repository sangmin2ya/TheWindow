using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad; // 로드할 씬의 이름

    // 버튼을 눌러 씬을 새로 로드하는 함수
    public void LoadScene()
    {
        // 씬을 새로 로드 (기존 씬을 닫고 새 씬을 로드)
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
