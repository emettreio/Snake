using UnityEngine;
using System.Collections;

public class SnakeWrapperMovement : MonoBehaviour {
    private SpriteRenderer _renderer;
 
    private bool isWrapping = false;
    private float timer = 0;
    private float wrappingThreshold = .5f;
    // Use this for initialization
    void Start () {
	    _renderer = this.GetComponent<SpriteRenderer>();
        
    }
	
	// Update is called once per frame
	void Update () {
        handleWrapping();
        checkIsStillWrapping();
        timer += Time.deltaTime;
	}

    void handleWrapping () {
        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        Debug.Log(viewportPosition);
        if ((viewportPosition.x < 0 || viewportPosition.x > 1) && (!isWrapping))
        {
            wrapToOtherSide();
        }

    }

    void wrapToOtherSide () {
        timer = 0;
        isWrapping = true;
        var newPosition = transform.position;
        newPosition.x = newPosition.x * -1;
        transform.position = newPosition;
    }

    void checkIsStillWrapping() {
        if (isWrapping) {
            if (timer > wrappingThreshold)
            {
                var cam = Camera.main;
                var viewportPosition = cam.WorldToViewportPoint(transform.position);
                isWrapping = (viewportPosition.x > 0 && viewportPosition.x < 1);
            }
        }
    }
}
