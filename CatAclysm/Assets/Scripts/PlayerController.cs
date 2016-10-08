using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject player;   // Setado no inspector
    [SerializeField]
    float moveSpeed;     // Setado no inspector
    public float bumpForce;     // Setado no inspector
    public float defaultYVelocity;
    public float slowDownX, slowDownY;
    public float minHeightPercentage;     //Altura mínima que o jogador pode alcançar. Representado em percentagem da altura da tela
    public GameController gameController;

    private int score;
    private bool hasBumped;
    private string ceilingLayer;
    private float verticalSpeed;
    private bool isDead;
    private Rigidbody2D rb;
    private float screenHeightInWorldUnit;
    private float minHeight;

    void Start ()
    {
        isDead = false;
        hasBumped = false;
        ceilingLayer = "Ceiling";
        verticalSpeed = 0f;
        score = 0;
        rb = player.GetComponent<Rigidbody2D>();
        screenHeightInWorldUnit = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        minHeight = (screenHeightInWorldUnit) * (-1) * (1 - minHeightPercentage);
    }
	
	void Update ()
    {
        if (!isDead)
        {
            move();
            collideWithCeiling();

            if (hasBumped)
            {
                verticalSpeed -= Time.deltaTime * slowDownY;
                if (verticalSpeed <= 0)
                {
                    hasBumped = false;
                    verticalSpeed = 0f;
                }

                updateYVelocity(verticalSpeed);
            }
            else
            {
                //updateYVelocity(defaultYVelocity);
            }

            score += (int) (Time.deltaTime * 100);
        }
    }

    void updateYVelocity(float yVelocity)
    {
        Vector3 theVelocity = rb.velocity;
        theVelocity.y = yVelocity;
        rb.velocity = theVelocity;
    }

    void updateXVelocity(float xVelocity)
    {
        Vector3 theVelocity = rb.velocity;
        theVelocity.x = xVelocity;
        rb.velocity = theVelocity;
    }

    void move()
    {
        float HMovement = 0;
        Input.GetMouseButton(0);
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > Camera.main.WorldToScreenPoint(player.transform.position).x)
            {
                HMovement = moveSpeed;
            }
            else if (Input.mousePosition.x < Camera.main.WorldToScreenPoint(player.transform.position).x)
            {
                HMovement = -moveSpeed;
            }
            updateXVelocity(HMovement);
        }
        else
        {
            float velX = rb.velocity.x;

            if (rb.velocity.x > 0)
            {
                velX -= Time.deltaTime * slowDownX;
                if (velX < 0)
                    velX = 0;
            }
            if (rb.velocity.x < 0)
            {
                velX += Time.deltaTime * slowDownX;
                if (velX > 0)
                    velX = 0;
            }

            updateXVelocity(velX);
        }

        if (rb.position.y < minHeight)
        {
            updateYVelocity(0);
        }

        else
        {
            updateYVelocity(defaultYVelocity);
        }
    }

    void collideWithCeiling()
    {
        if (rb.IsTouchingLayers(LayerMask.GetMask(ceilingLayer)))
        {
            isDead = true;
            gameController.endGame();
            player.SetActive(false);
        }
    }

    public void bump()
    {
        if (!hasBumped)
        {
            hasBumped = true;
            verticalSpeed = bumpForce;
            updateYVelocity(verticalSpeed);
        }
    }

    public string getScore()
    {
        return score.ToString();
    }

    public bool getIsDead()
    {
        return isDead;
    }
}
