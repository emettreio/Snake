using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartScreenSnakeBehaviour : MonoBehaviour {
    private Rigidbody2D m_rigidbody;

    public float rotationalSpeed = 230f;
    public float speed = 8f;
    public float turnCount = (90 / 230);

    private float turnCounter = 0;
    public bool turn = false;

    int targetRotation = 3;
    int currentRotation = 2;


    Dictionary<int, Vector3> rotations;

    // Use this for initialization
    void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        rotations = new Dictionary<int, Vector3>();
        rotations.Add(2, new Vector3(0, 0, 270)); //Left
        rotations.Add(1, new Vector3(0, 0, 180)); //Up
        rotations.Add(0, new Vector3(0, 0, 90));  //Right
        rotations.Add(3, new Vector3(0, 0, 0));   //Down
        rotations.Add(5, new Vector3(0, 0, 360));
    }
	
	// Update is called once per frame
	void Update () {
        MoveForward();
        if (turn)
        {
            turnCounter += Time.deltaTime;
            Quaternion rotation = transform.rotation;
            if (targetRotation != 3)
            {
                rotation.eulerAngles = Vector3.Lerp(rotations[currentRotation], rotations[targetRotation], turnCounter / turnCount);
            }
            else
            {
                rotation.eulerAngles = Vector3.Lerp(rotations[currentRotation], rotations[5], turnCounter / turnCount);
            }
            transform.rotation = rotation;
            if(turnCounter > turnCount)
            {
                turn = false;
            }

            /*
            if(turnCounter > turnCount)
            {
                Debug.Log("Finished Turning");
                m_rigidbody.angularVelocity = 0;
                turn = false;
                turnCounter = 0;
            }
            */
        }
	}

    void MoveForward()
    {
        m_rigidbody.velocity = transform.up * -speed;
    }

    void Turn(float direction)
    {
        m_rigidbody.angularVelocity = direction * rotationalSpeed;
    }

    void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "TurnMarker" && turn == false)
        {
            Debug.Log("Holla");
            turn = true;
            currentRotation += 1;
            if (currentRotation > 3)
                currentRotation = 0;
            targetRotation += 1;
            if (targetRotation > 3)
                targetRotation = 0;

            turnCounter = 0;
        }
    }
}
