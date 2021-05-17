using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SnakeCollisionHandling : MonoBehaviour {
    public string StartSceneName;

    public GameObject clockwiseBody;
    public GameObject aClockwiseBody;
    public GameObject standardBody;
    private TrailRenderer trail;
    private ParticleSystem m_particles;

    private List<GameObject> collisionVertexes = new List<GameObject>();
    private float intervalTimer = 0;
    public float collisionIntervals = 0.1f;
    public float deathIntervals = 0.05f;

    public float tailLength = 3f;
    public int tailNumber = 20;
    public int tailException = 10;
    public int pickupBonus = 2;

    private float totalTimer = 0;
    private bool isColliding = false;
    private float zOffset = 0.001f;
    private float red = 1;
    private float green = 1;
    private float blue = 1;
    private bool changeColor = false;
    private SpriteRenderer thisSprite;
    private float colorTimer = 0f;
    private float masterTimer = 0f;

    private bool isDead = false;

    private Vector3 initialPosition;
    private int initialLength;

    private Color colorToChange;
    private Color lastColor;

    public GameObject gameManager;
    private SnakeBehaviour snakeController;

    private ScoreMAnager scoreManager;
    private Rigidbody2D m_rigidbody;

    private bool fadeBody = false;
    private Color initialTailColor;

    private AudioSource deathAudio;


    private bool doNothing = false;
	// Use this for initialization
	void Start () {
        scoreManager = gameManager.GetComponent<ScoreMAnager>();
        snakeController = GetComponent<SnakeBehaviour>();
        trail = GetComponent<TrailRenderer>();
        thisSprite = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_particles = GetComponent<ParticleSystem>();
        initialLength = tailNumber;
        initialPosition = transform.position;
        initialTailColor = thisSprite.color;
        lastColor = thisSprite.color;
        deathAudio = GetComponent<AudioSource>();
        
    }
	
	// Update is called once per frame
	void Update () {
        intervalTimer += Time.deltaTime;
        masterTimer += Time.deltaTime;
        if (isDead)
        {
            if (intervalTimer > deathIntervals)
            {
                intervalTimer = 0;
                if (collisionVertexes.Count > 0)
                {
                    SpriteRenderer temporary = collisionVertexes[collisionVertexes.Count - 1].GetComponent<SpriteRenderer>();
                    thisSprite.color = temporary.color;
                    transform.position = collisionVertexes[collisionVertexes.Count - 1].transform.position;
                    transform.rotation = collisionVertexes[collisionVertexes.Count - 1].transform.rotation;
                    Destroy(collisionVertexes[collisionVertexes.Count - 1]);
                    collisionVertexes.RemoveAt(collisionVertexes.Count - 1);
                }
                else
                {
                    if (!doNothing)
                    {
                        scoreManager.EndGame();
                        doNothing = true;
                    }
                    //SceneManager.LoadScene(StartSceneName); // Scene Now Loads From Replay Menu
                    //Reset();
                    //snakeController.isAlive();
                    //isDead = false;
                    

                }
            }
        }
        else {
            if (intervalTimer > collisionIntervals)
            {
                intervalTimer = 0;
                GameObject collisionVertex;
                float randomBody = Random.Range(0, 1);
                if(m_rigidbody.angularVelocity > 0)
                {
                    collisionVertex = clockwiseBody;
                }
                else if(m_rigidbody.angularVelocity < 0)
                {
                    collisionVertex = aClockwiseBody;
                }
                else if(randomBody < .3333f)
                {
                    collisionVertex = standardBody;
                }
                else if(randomBody < .6666f)
                {
                    collisionVertex = clockwiseBody;
                }
                else
                {
                    collisionVertex = aClockwiseBody;
                }
                GameObject newVertex = Instantiate(collisionVertex, new Vector3(transform.position.x, transform.position.y, -zOffset), transform.rotation) as GameObject;
                SpriteRenderer newSprite = newVertex.GetComponent<SpriteRenderer>();
                if (changeColor)
                {
                    float colorTrack = masterTimer - colorTimer;
                    newSprite.color = Color.Lerp(lastColor, colorToChange, colorTrack);
                    thisSprite.color = Color.Lerp(lastColor, colorToChange, colorTrack);
                    m_particles.startColor = Color.Lerp(lastColor, colorToChange, colorTrack);
                    if (colorTrack >= 1)
                    {
                        colorTimer = 0;
                        changeColor = false;
                        lastColor = thisSprite.color;
                    }
                }
                else
                {
                    newSprite.color = lastColor;
                }

                collisionVertexes.Add(newVertex);
                zOffset += 0.001f;

            }
        }
        if (collisionVertexes.Count > tailNumber)
        {
            fadeBody = true;
            Destroy(collisionVertexes[0]);
            collisionVertexes.RemoveAt(0);
            initialTailColor = collisionVertexes[0].GetComponent<SpriteRenderer>().color;

        }
        else if(collisionVertexes.Count < tailNumber)
        {
            fadeBody = false;
        }
        if (fadeBody)
        {
            GameObject tail = collisionVertexes[0];
            SpriteRenderer tailColour = tail.GetComponent<SpriteRenderer>();
            tailColour.color = Color.Lerp(initialTailColor, new Color(initialTailColor.r, initialTailColor.g, initialTailColor.b, 0), intervalTimer / collisionIntervals);
        }

        
    }

    void Dead() {
        deathAudio.Play();
        isDead = true;
        snakeController.isDead();
    }

    void Reset() {
        collisionVertexes = new List<GameObject>();
        scoreManager.Reset();
        transform.position = initialPosition;
        tailNumber = initialLength;
        masterTimer = 0;
        intervalTimer = 0;

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "BounceVertical")
        {
            float mirroredAngle = (360 - transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler(0, 0, mirroredAngle);
        }
        else if (collider.tag == "BounceHorizontal")
        {
            float mirroredAngle = (transform.rotation.eulerAngles.z);
            if(mirroredAngle < 90)
            {
                mirroredAngle = (transform.rotation.eulerAngles.z + 90);
            }
            else if(mirroredAngle > 180 && mirroredAngle < 270)
            { 
                mirroredAngle = (transform.rotation.eulerAngles.z + 90);
            }
            else
            {
                mirroredAngle -= 90;
            }
            transform.rotation = Quaternion.Euler(0, 0, mirroredAngle); 
        }
        else if (collider.tag == "Pickup" || collider.tag == "SpawnedPickup")
        {
            changeColor = true;
            colorTimer = masterTimer;
            tailNumber += pickupBonus;
            SpriteRenderer pickupCurrent;
            pickupCurrent = collider.gameObject.GetComponent<SpriteRenderer>();
            colorToChange = pickupCurrent.color;
            lastColor = thisSprite.color;
            scoreManager.AddToScore(1);
            if(collider.tag == "SpawnedPickup")
            {
                Destroy(collider.gameObject);
            }
        }
        else if (collider.tag == "Boundary") {
            Dead();
        }
        else if(collider.tag == "Coin")
        {
            Debug.Log("Do Nothing");
        }
        else if(collider.tag == "TurnMarker")
        {
            Debug.Log("Do Nothing");
        }
        else
        {
            bool shouldDie = true;
            for(int i = (collisionVertexes.Count - 1); i >= 0; i--)
            {
                if (i > collisionVertexes.Count - 1 - tailException)
                {
                    if (collider.gameObject == collisionVertexes[i]) {
                        shouldDie = false;
                    }
                }
            }
            if (shouldDie)
            {
                Dead();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        isColliding = false;
    }

}
