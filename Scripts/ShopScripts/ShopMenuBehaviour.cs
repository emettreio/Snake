using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ShopMenuBehaviour : MonoBehaviour {
    public string MainMenuScene;

    private string selectedTheme;

    public Text coinText;
    public Text changingButtonText;

    private int coinValue;
	// Use this for initialization
	void Start () {
        LoadData();
        coinText.text = coinValue.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangingButtonBehaviour()
    {
        if(changingButtonText.text == "Select")
        {
            LoadCustomMenu("Start" + selectedTheme.Substring(selectedTheme.LastIndexOf('_')));
        }
        else
        {
            if(coinValue >= 100)
            {
                //The theme will be unlocked
                coinValue -= 100;

                BinaryFormatter ibf = new BinaryFormatter();
                FileStream ifile = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

                PlayerData previousData = (PlayerData)ibf.Deserialize(ifile);
                ifile.Close();

                Dictionary<string, bool> themes = previousData.themeStates;
                themes[selectedTheme.Substring((selectedTheme.LastIndexOf('_') + 1))] = true;

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.OpenOrCreate);
                
                PlayerData data = new PlayerData { currentScore = previousData.currentScore, coins = coinValue, highScore = previousData.highScore, themeStates = themes };

                bf.Serialize(file, data);
                file.Close();
                Debug.Log("No Problemo");
                LoadCustomMenu("Start" + selectedTheme.Substring(selectedTheme.LastIndexOf('_')));
    
}
        }
    }

    public void LoadNewTheme()
    {
        SceneManager.LoadScene(selectedTheme);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }

    void LoadCustomMenu(string menuName)
    {
        SceneManager.LoadScene(menuName);
    }

    public void ChangeSelectedTheme(string newTheme)
    {
        selectedTheme = newTheme;
        Debug.Log(selectedTheme.Substring((selectedTheme.LastIndexOf('_') + 1)));
    }

    void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            coinValue = data.coins;
        }
    }
}
