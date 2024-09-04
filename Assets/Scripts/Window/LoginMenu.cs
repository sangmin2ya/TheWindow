using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UserLogin1()
    {
        SceneManager.LoadScene("User1");
    }
    public void UserLogin2()
    {
        SceneManager.LoadScene("User2");
    }
}
