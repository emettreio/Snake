using UnityEngine;
using System.Collections;

public class SnakeBehaviour : MonoBehaviour {
    public float speed = 5f;
    private Rigidbody2D m_rigidbody;
    public float rotationalSpeed = 90f;
    public float speedEnhancer = 1.5f;
    private bool alive = true;

    private bool leftHit = false;
    private bool rightHit = false;
    private bool enhancedRotation = false;
    public float enhancedTime = 1f;
    private float enhancedTimer = 0;

    private float cooldownTimer = 0;
    public float cooldown;

    private AudioSource snakeAudio;
    private PowerUpHandler powerUpHandler;
	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        powerUpHandler = GameObject.Find("Powerup").GetComponent<PowerUpHandler>();
        snakeAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (alive)
        {
            
            cooldownTimer += Time.deltaTime;
            if (enhancedRotation)
            {
                enhancedTimer += Time.deltaTime;
                if (enhancedTimer > enhancedTime)
                {
                    enhancedTimer = 0;
                    enhancedRotation = false;
                    rotationalSpeed /= speedEnhancer;
                }
            }
            
            m_rigidbody.velocity = transform.up * -speed;
            if(Input.GetButtonDown("Left"))
            {
                if (leftHit && cooldownTimer < cooldown)
                {
                    AdjustRotationalSpeed(-1f);
                    leftHit = false;
                }
                else
                {
                    if (rightHit)
                    {
                        rightHit = false;
                    }
                    cooldownTimer = 0;
                    leftHit = true;
                }
            }

            if (Input.GetButtonDown("Right"))
            {
                if (rightHit && cooldownTimer < cooldown)
                {
                    AdjustRotationalSpeed(1f);
                    rightHit = false;
                }
                else
                {
                    if (leftHit)
                    {
                        leftHit = false;
                    }
                    cooldownTimer = 0;
                    rightHit = true;
                }
            }
            if (Input.GetAxis("Horizontal") != 0)
            {
                m_rigidbody.angularVelocity = Input.GetAxis("Horizontal") * rotationalSpeed;
                //if(!snakeAudio.isPlaying)
                //    snakeAudio.Play();
            }
            else if (Input.touchCount > 0)
            {
                if (Input.touches[0].position.x <= Screen.width / 2)
                {
                    if (leftHit && cooldownTimer < cooldown)
                    {
                        AdjustRotationalSpeed(1f);
                        leftHit = false;
                    }
                    else
                    {
                        if (rightHit)
                        {
                            rightHit = false;
                        }
                        cooldownTimer = 0;
                        leftHit = true;
                    }
                    m_rigidbody.angularVelocity = -1 * rotationalSpeed;
                }
                else if (Input.touches[0].position.x >= Screen.width / 2)
                {
                    if (rightHit && cooldownTimer < cooldown)
                    {
                        AdjustRotationalSpeed(-1f);
                        rightHit = false;
                    }
                    else
                    {
                        if (leftHit)
                        {
                            leftHit = false;
                        }
                        cooldownTimer = 0;
                        rightHit = true;
                    }
                    m_rigidbody.angularVelocity = 1 * rotationalSpeed;
                }
            }
            else
            {
                m_rigidbody.angularVelocity = 0;
            }
        }
        else
        {
            m_rigidbody.angularVelocity = 0;
            m_rigidbody.velocity = new Vector2();
            enhancedTimer = 0;
        }
	}

    void AdjustRotationalSpeed(float direction)
    {
        transform.Rotate(0, 0, 45 * direction);
        transform.rotation = new Quaternion(0, 0, transform.rotation.z, transform.rotation.w);
        powerUpHandler.Detach();
        //rotationalSpeed *= speedEnhancer;
        //enhancedRotation = true;
    }

    public void isDead()
    {
        alive = false;
    }

    public void isAlive()
    {
        alive = true;
    }


}
