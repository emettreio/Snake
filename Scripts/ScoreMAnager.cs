using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ScoreMAnager : MonoBehaviour {
    private int score = 0;
    public float maxSize = 60f;
    private float transitionTimer = 0f;
    public float transitionTime = 2f;
    private bool transition = false;
    private float initialSize;
    public Text scoreText;

    private int highScore = 0;
    public int CoinScore = 0;

    public int changeNumber;
    private ChangeBackgroundColor backgroundColor;

    private AudioSource scoreAudio;

    public GameObject ReplayMenu;
    private ReplayMenuController replayMenuController;
	// Use this for initialization
	void Start () {
        initialSize = scoreText.fontSize;
        backgroundColor = Camera.main.GetComponent<ChangeBackgroundColor>();
        replayMenuController = ReplayMenu.GetComponent<ReplayMenuController>();
        scoreAudio = GetComponent<AudioSource>();
        GameObject testSubject = Resources.Load("TestPrefab", typeof(GameObject)) as GameObject;
        Debug.Log(testSubject.name);
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = score.ToString();
        if (transition)
        {
            transitionTimer += Time.deltaTime;
            scoreText.fontSize = Mathf.RoundToInt(Mathf.Lerp(maxSize, initialSize, transitionTimer / transitionTime));
            scoreText.color = Color.Lerp(Color.grey, Color.white, transitionTimer / transitionTime);
            if(transitionTimer >= transitionTime)
            {
                transitionTimer = 0;
                transition = false;
                scoreText.color = Color.white;
                scoreText.fontSize = Mathf.RoundToInt(initialSize);
            }

        }
	}

    public void AddToScore(int amount)
    {
        score += amount;
        transition = true;
        if (score % changeNumber == 0)
        {
            backgroundColor.setChangeColor();
        }
        scoreAudio.Play();
    }

    public void Reset()
    {
        score = 0;
    }

    public void EndGame()
    {
        PlayerData currentScore = Load();
        CoinScore += currentScore.coins;
        if(score > currentScore.highScore)
        {
            highScore = score;
        }
        else
        {
            highScore = currentScore.highScore;
        }
        replayMenuController.SetValues(score, CoinScore);
        Save();
        scoreText.enabled = false;
        
        replayMenuController.Activate();
    }

    public void Save()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter ibf = new BinaryFormatter();
            FileStream ifile = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

            PlayerData previousData = (PlayerData)ibf.Deserialize(ifile);
            ifile.Close();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.OpenOrCreate);

            PlayerData data = new PlayerData { currentScore = score, coins = CoinScore, highScore = highScore, themeStates = previousData.themeStates };

            bf.Serialize(file, data);
            file.Close();
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.OpenOrCreate);
            Dictionary<string, bool> themes = new Dictionary<string, bool>();
            themes.Add("Default", true);
            themes.Add("Winter", false);
            themes.Add("Forest", false);
            PlayerData data = new PlayerData { currentScore = score, coins = CoinScore, highScore = highScore, themeStates = themes};

            bf.Serialize(file, data);
            file.Close();
        }
    }

    public PlayerData Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            return data;
        }
        return new PlayerData();
    }


}

[Serializable]
public class PlayerData
{
    public int currentScore { get; set; }

    public int coins { get; set; }

    public int highScore { get; set; }

    
    // Booleans for unlock state of themes
    public Dictionary<string, bool> themeStates { get; set; }
}
