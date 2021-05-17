using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    public string PlaySceneName;

    public string ThemeSceneName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadLevel()
    {
        SceneManager.LoadScene(PlaySceneName);
    }

    public void LoadThemesMenu()
    {
        SceneManager.LoadScene(ThemeSceneName);
    }
}
