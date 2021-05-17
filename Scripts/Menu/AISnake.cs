using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISnake : MonoBehaviour {
    public GameObject pickupDrop;
    public GameObject transitionCircle;
    public float speed = 8f;
    public int spawnLength = 4;
    public float spawnCooldown = .2f;
    public float changeDirectionTime = 2f;

    private bool change = false;
    private Transform initialScale;
    private float changeDirectionTimer = 0f;
    private Color currentColor;
    private Color nextColor;
    private SpriteRenderer m_spriteRenderer;
    private float colorChangeTimer = 0f;
    private List<GameObject> spawnedPickups = new List<GameObject>();
    private float spawnTimer = 0;
    private Rigidbody2D m_rigidbody2D;
	// Use this for initialization
	void Start () {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        currentColor = GetRandomColor();
        m_spriteRenderer.color = currentColor;
        initialScale = transitionCircle.transform;
        nextColor = GetRandomColor();
        ChooseRandomDirection();
    }
	
	// Update is called once per frame
	void Update () {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnCooldown && spawnLength > spawnedPickups.Count) {
            GameObject newSpawn = Instantiate(pickupDrop, transform.position, Quaternion.identity) as GameObject;
            newSpawn.GetComponent<SpriteRenderer>().color = m_spriteRenderer.color;
            spawnedPickups.Add(newSpawn);
            spawnTimer = 0;
        } 
        else if(spawnTimer > spawnCooldown)
        {
            spawnTimer = 0;
            int nullCount = 0;
            for (int i = spawnedPickups.Count - 1; i >= 0; i--) {
                if(spawnedPickups[i] == null)
                {
                    nullCount += 1;
                }
            }
            if (nullCount == spawnedPickups.Count && spawnedPickups.Count != 0)
            {
                change = true;
                transitionCircle.GetComponent<SpriteRenderer>().enabled = true;
                m_rigidbody2D.velocity = new Vector2();
                changeDirectionTimer += Time.deltaTime + spawnCooldown;
                
                if (changeDirectionTimer > changeDirectionTime) {
                    change = false;
                    changeDirectionTimer = 0;
                    ChooseRandomDirection();
                    currentColor = m_spriteRenderer.color;
                    nextColor = GetRandomColor();
                    colorChangeTimer = 0;
                    spawnedPickups = new List<GameObject>();
                    transitionCircle.GetComponent<SpriteRenderer>().enabled = false;
                    transitionCircle.transform.localScale = initialScale.localScale;
                    spawnLength += 1;
                }
            }
        }
        //Lerping
        else
        {
            if (change) {
                transitionCircle.transform.localScale = Vector3.Lerp(initialScale.localScale, new Vector3(), Time.deltaTime);
            }
            colorChangeTimer += Time.deltaTime;
            m_spriteRenderer.color = Color.Lerp(currentColor, nextColor, colorChangeTimer / (spawnCooldown * spawnLength));
        }

    }

    void ChooseRandomDirection() {
        transform.rotation = Random.rotation;
        Vector3 angles = transform.rotation.eulerAngles;
        angles.x = angles.y = 0;
        transform.rotation = Quaternion.Euler(angles);
        m_rigidbody2D.velocity = transform.up * -speed;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "BounceVertical") {
            Vector2 tempVelocity = m_rigidbody2D.velocity;
            tempVelocity.x *= -1;
            m_rigidbody2D.velocity = tempVelocity; 
        }
        if (collider.tag == "BounceHorizontal")
        {
            Vector2 tempVelocity = m_rigidbody2D.velocity;
            tempVelocity.y *= -1;
            m_rigidbody2D.velocity = tempVelocity;
        }
    }

    Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);

    }
}
