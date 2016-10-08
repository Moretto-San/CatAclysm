using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {

    public GameObject obstacle;     //Não é setado no inspector
    public GameObject player;       //Não é setado no inspector

    private string playerLayer, ceilingLayer;

	void Start ()
    {
        playerLayer = "Player";
        ceilingLayer = "Ceiling";
	}
	
	void Update ()
    {
        collide();
        rotate();
	}

    void collide()
    {
        if (obstacle.GetComponent<Rigidbody2D>().IsTouchingLayers(LayerMask.GetMask(ceilingLayer)))
        {
            Debug.Log("Entrou");
            Destroy(obstacle);
        }

        if (!player.GetComponent<PlayerController>().getIsDead())
        {
            if (obstacle.GetComponent<Rigidbody2D>().IsTouchingLayers(LayerMask.GetMask(playerLayer)))
            {
                player.GetComponent<PlayerController>().bump();
                Destroy(obstacle);
            }
        }

        
    }

    void rotate()
    {
        obstacle.transform.Rotate(new Vector3(0, 0, 1), 5);
    }
}
