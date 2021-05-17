using UnityEngine;
using System.Collections;

public class MoveWithCursor : MonoBehaviour {
    public bool track = true;
    public float speed = 15f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            track = ToggleBool(track);
        }
        if (track)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            float distance = calculateDistanceBetweenVectors2D(transform.position, newPosition);
            transform.position = Vector3.Lerp(transform.position, newPosition, ((speed * Time.deltaTime) / distance));
            
        }
	}

    bool ToggleBool(bool value) {
        if (value == false)
        {
            return true;
        }
        else return false;
    }

    float calculateDistanceBetweenVectors2D(Vector2 a, Vector2 b)
    {
        float distance = Mathf.Sqrt((Mathf.Pow((a.x - b.x), 2) + Mathf.Pow((a.y - b.y), 2)));
        return distance;
    }
}
