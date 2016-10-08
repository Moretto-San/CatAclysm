using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject player;    // Setado manualmente no inspector
    public GameObject obstacle;
    public float obstacleSpeed;
    public float obstacleChance;    //Percentual de chance de aparecer um obstáculo naquele frame
    public Text score;
    public GameObject borderTop, borderLeft, borderRight;

    private string obstacleLayer;

	void Start ()
    {
        player.SetActive(true);
        obstacleLayer = "Obstacle";
        Vector3 v = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0));
        float x = v.x, y = v.y, z = v.z;
        borderTop.transform.position.Set(x,y,z);
        v = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0));
        x = v.x;
        y = v.y;
        z = v.z;
        borderRight.transform.position.Set(x, y, z);
        v = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0));
        x = v.x;
        y = v.y;
        z = v.z;
        borderLeft.transform.position.Set(x, y, z);
    }
	
	void Update ()
    {
        if (!player.GetComponent<PlayerController>().getIsDead())
        {
            spawnObstacle();
            score.GetComponent <Text>().text = player.GetComponent<PlayerController>().getScore();
        }

        if(player.GetComponent<PlayerController>().getIsDead())
        {
            score.color = Color.red;
        }
    }

    void spawnObstacle()
    {
        if (Random.Range(0f, 100f) < obstacleChance)
        {
            Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 0f, 0));
            spawnPoint.z = 0;
            GameObject obstacleClone = (GameObject)Instantiate(obstacle, spawnPoint, obstacle.transform.rotation);
            obstacleClone.AddComponent<BoxCollider2D>();
            obstacleClone.AddComponent<ObstacleController>();
            obstacleClone.GetComponent<ObstacleController>().obstacle = obstacleClone;
            obstacleClone.GetComponent<ObstacleController>().player = player;
            obstacleClone.GetComponent<BoxCollider2D>().size = obstacleClone.GetComponent<SpriteRenderer>().sprite.bounds.size;
            obstacleClone.SetActive(true);
            obstacleClone.GetComponent<Rigidbody2D>().velocity = transform.up * obstacleSpeed;

            if (obstacle.GetComponent<Rigidbody2D>().IsTouchingLayers(LayerMask.GetMask(obstacleLayer)))
            {
                Destroy(obstacle);
            }
        }
    }

    public void endGame()
    {
        Debug.Log("Game Over!");
        Application.LoadLevel("Prototype");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
