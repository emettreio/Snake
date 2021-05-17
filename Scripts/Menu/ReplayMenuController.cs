using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayMenuController : MonoBehaviour {
    public string LevelString;


    private int playerScore;
    private bool transition;

    public float transitionTime;
    private float transitionTimer;

    private Vector3 initialPosition;
    public Vector3 finalPosition;

    public Text scoreText;
    public Text coinText;
	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (transition)
        {
            transitionTimer += Time.deltaTime;
            float t = transitionTime;
            double lerpTime = (((1 / (t * t)) - ((4.5 - (t * t)) / ((3 * t * t) - (t * t * t * t)))) * transitionTimer * transitionTimer) + (((4.5 - (t * t)) / ((3 * t) - (t * t * t))) * transitionTimer);
            Debug.Log(lerpTime.ToString());
            Debug.Log(float.Parse(lerpTime.ToString()).ToString());
            transform.position = Vector3.Lerp(initialPosition, finalPosition, float.Parse(lerpTime.ToString()));
            if(transitionTimer > transitionTime)
            {
                Debug.Log("Reset");
                transition = false;
                transitionTimer = 0;
            }
        }
	}

    public void Activate()
    {
        transition = true;
    }

    public void LoadMainMenu()
    { 
        SceneManager.LoadScene("Start_" + LevelString);
    }

    public void Reload()
    {
        SceneManager.LoadScene("Play_" + LevelString);
    }

    public void SetValues(int valueA, int valueB)
    {
        scoreText.text = valueA.ToString();
        coinText.text = valueB.ToString();
    }
}
