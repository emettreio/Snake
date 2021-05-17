using UnityEngine;
using System.Collections;


public class ArcadeModePickupManager : MonoBehaviour {
    public Transform topLeft;
    public Transform bottomRight;
    public GameObject leadingCircle1;
    public GameObject leadingCircle2;
    public float distanceFactor = .6f;
	// Use this for initialization
	void Start () {
        	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void moveCircle(GameObject circle, Vector2 direction) {
        Vector3 position = circle.transform.position;
        position.x += direction.x * distanceFactor;
        position.y += direction.y * distanceFactor;
        if (position.x < topLeft.position.x) {
            position.x = topLeft.position.x + (topLeft.position.x - position.x);
        }
        if (position.x > bottomRight.position.x) {
            position.x = bottomRight.position.x - (position.x - bottomRight.position.x);
        }
        if(position.y < bottomRight.position.y)
        {
            position.y = bottomRight.position.y + (bottomRight.position.y - position.y);
        }
        if(position.y > topLeft.position.y)
        {
            position.y = topLeft.position.y - (position.y - topLeft.position.y);
        }
        circle.transform.position = position;

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Rigidbody2D otherCollider = collider.gameObject.GetComponent<Rigidbody2D>();
        Vector2 playerVelocity = otherCollider.velocity;
        Debug.Log(playerVelocity);
        Debug.Log(Mathf.Sqrt((playerVelocity.x * playerVelocity.x) + (playerVelocity.y * playerVelocity.y)));
        transform.position = leadingCircle1.transform.position;
        leadingCircle1.transform.position = leadingCircle2.transform.position;
        moveCircle(leadingCircle2, playerVelocity);
    }
}
