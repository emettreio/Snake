using UnityEngine;
using System.Collections;

public class CoinBehaviour : MonoBehaviour {
    public Sprite coin_1;
    public Sprite coin_5;
    public Sprite coin_10;


    public Transform topLeft;
    public Transform bottomRight;

    public GameObject pickupAnim;


    public float coinIntervalMax;
    public float coinIntervalMin;

    private int coinValue = 1;

    private float coinInterval;
    private float coinTimer = 0;

    private bool PickedUp = false;
    private bool fadeIn = false;

    private Color initialColour;
    private Color fadeColor;

    public float fadeTime = .25f;

    private SpriteRenderer m_spriteRenderer;

    private ScoreMAnager coinTally;
    private AudioSource coinAudio;
    // Use this for initialization
    void Start () {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        coinTally = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreMAnager>();
        NewCoinInterval();
        ChangeToNewPosition();
        ChooseCoin();
        initialColour = m_spriteRenderer.color;
        fadeColor = initialColour;
        fadeColor.a = 0;
        coinAudio = GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if(PickedUp)
            coinTimer += Time.deltaTime;
        if(coinTimer > coinInterval)
        {
            NewCoinInterval();
            ChangeToNewPosition();
            ChooseCoin();
            coinTimer = 0;
            PickedUp = false;
            fadeIn = true;
        }
        if (fadeIn)
        {
            coinTimer += Time.deltaTime;
            m_spriteRenderer.color = Color.Lerp(fadeColor, initialColour, (coinTimer / fadeTime));
            if(coinTimer > fadeTime)
            {
                m_spriteRenderer.color = initialColour;
                fadeIn = false;
                coinTimer = 0;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            Instantiate(pickupAnim, transform.position, Quaternion.identity);
            transform.position = new Vector3(-50, -50, transform.position.z);
            PickedUp = true;
            coinTally.CoinScore += 1;
            coinAudio.Play();
        }
    }

    void NewCoinInterval() {
        coinInterval = Random.Range(coinIntervalMin, coinIntervalMax);
    }

    void ChangeToNewPosition()
    {
        float newX = Random.Range(topLeft.position.x, bottomRight.position.x);
        float newY = Random.Range(topLeft.position.y, bottomRight.position.y);
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void ChooseCoin()
    {
        float randomNumber = Random.Range(0, 1);
        if(randomNumber > .98f)
        {
            m_spriteRenderer.sprite = coin_10;
            coinValue = 10;
        }
        else if(randomNumber > .80f){
            m_spriteRenderer.sprite = coin_5;
            coinValue = 5;
        }
        else{
            m_spriteRenderer.sprite = coin_1;
            coinValue = 1;
        }
    }
}
