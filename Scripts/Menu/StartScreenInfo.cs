using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class StartScreenInfo : MonoBehaviour {
    public int highScore;
    private int coins;

    public Text highScoreText;
    public Text coinsText;
	// Use this for initialization
	void Start () {
        Load();
        highScoreText.text = "High Score: " + highScore.ToString();
        coinsText.text = coins.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    /// <summary>
    /// Create intial availability status of themes. Code should only ever be executed once. 
    /// </summary>
    void InitisaliseThemeValues() {

        //Include all Themes here
        Dictionary<string, bool> values = new Dictionary<string, bool>();
        values.Add("Default", true);
        values.Add("Winter", false);
        values.Add("Forest", false);

        BinaryFormatter ibf = new BinaryFormatter();
        FileStream ifile = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

        PlayerData previousData = (PlayerData)ibf.Deserialize(ifile);
        ifile.Close();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.OpenOrCreate);

        PlayerData data = new PlayerData { currentScore = previousData.currentScore, coins = 325, highScore = previousData.highScore, themeStates = values };

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            highScore = data.highScore;
            coins = data.coins;
            /*Repurposed for live test purposes
            if(data.themeStates == null)
            {
                InitisaliseThemeValues();
            }
            */
            //For Testing Purposes Only
            if (data.themeStates != null)
            {

                if (data.themeStates["Winter"] == false && data.themeStates["Forest"] == false)
                {
                    InitisaliseThemeValues();
                }
            }
        }
    }
}
