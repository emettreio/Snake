using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartScreenSnakeCollision : MonoBehaviour {

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
    private StartScreenSnakeBehaviour snakeController;

    private StartScreenInfo scoreInfo;
    private Rigidbody2D m_rigidbody;

    private bool fadeBody = false;
    private Color initialTailColor;

    float changeColorTimer = 0f;
    float changeColorTime;
    private ColorManager colorOptions;

    // Use this for initialization
    void Start()
    {
        scoreInfo = gameManager.GetComponent<StartScreenInfo>();
        snakeController = GetComponent<StartScreenSnakeBehaviour>();
        trail = GetComponent<TrailRenderer>();
        thisSprite = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_particles = GetComponent<ParticleSystem>();
        initialLength = tailNumber;
        initialPosition = transform.position;
        lastColor = new Color(1, 1, 1);

        tailNumber = tailNumber + scoreInfo.highScore;

        colorOptions = GameObject.Find("ColorComponents").GetComponent<ColorManager>();
        changeColorTime = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tailNumber < 6)
        {
            tailNumber = 5 + scoreInfo.highScore;
            //Set Maximum Boundary
            if(tailNumber > 104)
            {
                tailNumber = 104;
            }
        }
        intervalTimer += Time.deltaTime;
        masterTimer += Time.deltaTime;

        changeColorTimer += Time.deltaTime;
        if(changeColorTimer > changeColorTime)
        {
            changeColorTime = Random.Range(3, 6);
            colorTimer = masterTimer;
            changeColor = true;
            colorToChange = colorOptions.GetRandomColor();
            changeColorTimer = 0;
        }

        if (intervalTimer > collisionIntervals)
        {
            intervalTimer = 0;
            GameObject collisionVertex;
            if (snakeController.turn)
            {
                collisionVertex = clockwiseBody;
            }
            else
            {
                collisionVertex = standardBody;
            }

            GameObject newVertex = Instantiate(collisionVertex, new Vector3(transform.position.x, transform.position.y, -zOffset), transform.rotation) as GameObject;
            SpriteRenderer newSprite = newVertex.GetComponent<SpriteRenderer>();
            if (changeColor)
            {
                float colorTrack = masterTimer - colorTimer;
                newSprite.color = Color.Lerp(lastColor, colorToChange, colorTrack);
                thisSprite.color = Color.Lerp(lastColor, colorToChange, colorTrack);
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
            zOffset += 0.0000000001f;

        }
        if (collisionVertexes.Count > tailNumber)
        {
            fadeBody = true;
            Destroy(collisionVertexes[0]);
            collisionVertexes.RemoveAt(0);
            initialTailColor = collisionVertexes[0].GetComponent<SpriteRenderer>().color;

        }
        else if (collisionVertexes.Count < tailNumber)
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








}
