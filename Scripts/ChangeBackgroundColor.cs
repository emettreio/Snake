using UnityEngine;
using System.Collections;

public class ChangeBackgroundColor : MonoBehaviour {
    public GameObject colorObject;

    //public float changeIntervals; //Replaced with change every score interval
    private bool changeColor;
    public float lerpTime;

    private float changeTimer;
    private ColorManager colorManager;
    private Camera m_camera;

    private Color oldColor;
    private Color newColor;
	// Use this for initialization
	void Start () {
        colorManager = colorObject.GetComponent<ColorManager>();
        m_camera = GetComponent<Camera>();
        m_camera.backgroundColor = SelectRandomColor();
        oldColor = m_camera.backgroundColor;
        newColor = m_camera.backgroundColor;
    }
	
	// Update is called once per frame
	void Update () {
        
        if(changeColor)
        {
            changeTimer += Time.deltaTime;
            if (newColor == oldColor)
            {
                newColor = SelectRandomColor();
            }
            m_camera.backgroundColor = Color.Lerp(oldColor, newColor, (changeTimer / lerpTime));
            if(changeTimer >= lerpTime)
            {
                changeTimer = 0;
                oldColor = newColor;
                changeColor = false;
            }
        }
	}

    Color SelectRandomColor()
    {
        return colorManager.colorOptions[Random.Range(0, (colorManager.colorOptions.Count - 1))];
    }

    public void setChangeColor()
    {
        changeColor = true;
    }
}
