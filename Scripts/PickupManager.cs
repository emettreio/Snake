using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {
    public Transform topLeft;
    public Transform bottomRight;


    private SpriteRenderer thisRenderer;

    private ColorManager colorController;

    private bool colorChange = false;
    private float colorTimer = 0f;
    private float colorChangeTime = .5f;

    private Color initialColor;
    private Color newColor;
    // Use this for initialization
	void Start () {
        
        thisRenderer = GetComponent<SpriteRenderer>();
        colorController = GetComponent<ColorManager>();
        float newX = Random.Range(topLeft.position.x, bottomRight.position.x);
        float newY = Random.Range(topLeft.position.y, bottomRight.position.y);
        transform.position = new Vector3(newX, newY, transform.position.z);
        initialColor = GetRandomColor();
        colorChange = true;
        newColor = GetRandomColor();
    }
	
	// Update is called once per frame
	void Update () {
        if (colorChange)
        {
            colorTimer += Time.deltaTime;
            thisRenderer.color = Color.Lerp(new Color(1, 1, 1), newColor, colorTimer / colorChangeTime);
            if(colorTimer > colorChangeTime)
            {
                colorTimer = 0;
                colorChange = false;
                initialColor = thisRenderer.color;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        float newX = Random.Range(topLeft.position.x, bottomRight.position.x);
        float newY = Random.Range(topLeft.position.y, bottomRight.position.y);
        transform.position = new Vector3(newX, newY, transform.position.z);
        colorChange = true;
        newColor = GetRandomColor();
    }

    Color GetRandomColor() {
        return colorController.GetRandomColor();

    }


}
