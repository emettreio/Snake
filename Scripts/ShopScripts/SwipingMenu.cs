using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class SwipingMenu : MonoBehaviour {
    public List<GameObject> menuOptions;
    public float maxSize;
    public float minSize;

    public float xSeparation = 4f;
    public bool moveLeft = false;
    public bool moveRight = false;
    public float transitionTime = .5f;

    private float transitionTimer = 0f;
    private int currentOption; //May become public later
    private List<Vector3> initialPositions;


    //Unlock states of themes
    Dictionary<string, bool> themeStates;

    public GameObject themeLock;
    public Text buttonText;
    public Text priceText;
    public GameObject coinDetail;
    // Use this for initialization
    void Start () {
        ScaleMenuOptions();
        PositionMenuOptions();
        RecordInitialPositions();
        //Use function below to reset main save data
        //CreateData();
        LoadData();
        LockOptions();
	}
	
	// Update is called once per frame
	void Update () {
        if(moveLeft && currentOption == (menuOptions.Count - 1))
        {
            moveLeft = false;
        }
        if (moveRight && currentOption == 0)
        {
            moveRight = false;
        }


        if (moveLeft || moveRight)
        {
            if (priceText.color.a == 1)
            {
                HideDetails();
            }
            transitionTimer += Time.deltaTime;
            float distance = xSeparation;
            if (moveLeft)
            {
                distance *= -1;
            }
            MoveMenu(distance, (transitionTimer / transitionTime));
            ScaleCurrentOptions((transitionTimer / transitionTime));
            if(transitionTimer > transitionTime)
            {
                if (moveLeft)
                    currentOption += 1;
                else if (moveRight)
                    currentOption -= 1;
                moveLeft = false;
                moveRight = false;
                transitionTimer = 0;
                RecordInitialPositions();

                HandleUnlockDetails();
            }
        }
        


	}

    void ScaleCurrentOptions(float timing)
    {
        menuOptions[currentOption].transform.localScale = Vector3.Lerp(new Vector3(maxSize, maxSize, maxSize), new Vector3(minSize, minSize, minSize), timing);
        if (moveLeft)
        {
            menuOptions[(currentOption + 1)].transform.localScale = Vector3.Lerp(new Vector3(minSize, minSize, minSize), new Vector3(maxSize, maxSize, maxSize), timing);
        }
        else if (moveRight)
        {
            menuOptions[(currentOption - 1)].transform.localScale = Vector3.Lerp(new Vector3(minSize, minSize, minSize), new Vector3(maxSize, maxSize, maxSize), timing);
        }
    }

    void MoveMenu(float distance, float timing)
    {
        for(int i = 0; i < menuOptions.Count; i++)
        {
            Vector3 targetPosition = initialPositions[i];
            targetPosition.x += distance;
            menuOptions[i].transform.position = Vector3.Lerp(initialPositions[i], targetPosition, timing);
        }
    }

    void RecordInitialPositions()
    {
        initialPositions = new List<Vector3>();
        for(int i = 0; i < menuOptions.Count; i++)
        {
            initialPositions.Add(menuOptions[i].transform.position);
        }
    }

    void ScaleMenuOptions()
    {
        menuOptions[0].transform.localScale = new Vector3(maxSize, maxSize, maxSize);
        for (int i = 1; i < menuOptions.Count; i++)
        {
            menuOptions[i].transform.localScale = new Vector3(minSize, minSize, minSize);
        }
    }

    void PositionMenuOptions()
    {
        Vector3 firstPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));
        firstPosition.z = -1;
        for(int i = 0; i < menuOptions.Count; i++)
        {
            Vector3 newPosition = firstPosition;
            newPosition.x += xSeparation * i; //Relies on X Separation Value
            menuOptions[i].transform.position = newPosition;
        }
        currentOption = 0;
    }

    void CreateData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.OpenOrCreate);
        Dictionary<string, bool> values = new Dictionary<string, bool>();
        values.Add("Default", true);
        values.Add("Winter", false);
        values.Add("Forest", false);
        PlayerData data = new PlayerData { themeStates = values, highScore = 46, coins = 150};

        bf.Serialize(file, data);
        file.Close();
    }

    void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            themeStates = data.themeStates;
        }
    }

    void LockOptions()
    {
        foreach(GameObject theme in menuOptions)
        {
            if(themeStates[theme.name] == false)
            {
                SpriteRenderer themeColour = theme.GetComponent<SpriteRenderer>();
                Color visible = themeColour.color;
                visible.a = .75f;
                themeColour.color = visible;
                GameObject newLock = Instantiate(themeLock, theme.transform.position, Quaternion.identity) as GameObject;
                newLock.transform.SetParent(theme.transform);
                newLock.transform.localPosition = new Vector3(0, 0, -1);
            }
        }
    }

    void HandleUnlockDetails()
    {
        if(themeStates[menuOptions[currentOption].name] == false)
        {
            buttonText.text = "Unlock";

            Color visible = priceText.color;
            visible.a = 1;
            priceText.color = visible;

            SpriteRenderer coinRenderer = coinDetail.GetComponent<SpriteRenderer>();
            visible = coinRenderer.color;
            visible.a = 1;
            coinRenderer.color = visible;
        }
        else
        {
            buttonText.text = "Select";    
        }
    }

    void HideDetails()
    {
        Color invisible = priceText.color;
        invisible.a = 0;
        priceText.color = invisible;

        SpriteRenderer coinRenderer = coinDetail.GetComponent<SpriteRenderer>();
        invisible = coinRenderer.color;
        invisible.a = 0;
        coinRenderer.color = invisible;
    }

}
