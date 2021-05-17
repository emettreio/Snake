using UnityEngine;
using System.Collections;

public class DetectSwipe : MonoBehaviour {
    public float yHeightAllowance = .75f;
    public float xTapAllowance = .25f;
    public float swipeSensitivity = 1f;

    public bool testRight;
    public bool testLeft; 


    private SwipingMenu menuController;
	// Use this for initialization
	void Start () {
        menuController = GetComponent<SwipingMenu>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0 && menuController.moveLeft == false && menuController.moveRight == false)
        {
            if(Input.GetTouch(0).position.y < (Screen.height * yHeightAllowance))
            {
                Touch playerTouch = Input.GetTouch(0);
                //Has player swiped left to right
                if (playerTouch.deltaPosition.x > swipeSensitivity)
                {
                    menuController.moveRight = true;
                }
                //Has player swiped right to left
                else if (playerTouch.deltaPosition.x < -swipeSensitivity)
                {
                    menuController.moveLeft = true;
                }
                //Has player tapped left side of screen
                else if (playerTouch.position.x < (Screen.width * xTapAllowance))
                {
                    menuController.moveRight = true;
                }
                //Has player tapped right side of screen
                else if (playerTouch.position.x > (Screen.width * (1 - xTapAllowance)))
                {
                    menuController.moveLeft = true;
                }
                
        }


    }
        if (testRight)
        {
            menuController.moveRight = true;
        }
        else if (testLeft)
        {
            menuController.moveLeft = true;
        }
    }
}
