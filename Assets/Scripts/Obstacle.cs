using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float speed = 5.0f; // Speed of the obstacle
    public float verticalSpeed = 0.0f; // Vertical speed of the obstacle

    private float DestroyXCoordinate = -10.0f;

    private Rigidbody2D mRigidbody;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.mGameLost) {
            speed = 0.0f;
        }

        // Check if the obstacle is beyond the destroy X-coordinate
        if (transform.position.x < DestroyXCoordinate)
        {
            GameManager.Instance.mCurrentScore += 1;

            // Destroy the obstacle
            Destroy(gameObject);
        }
    }

    // FixedUpdate is called every fixed frame-rate frame
    void FixedUpdate()
    {
        // Set the velocity to move the obstacle to the left
        mRigidbody.velocity = new Vector2(-speed, verticalSpeed);
    }

    // Called when a collision with another collider is detected
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the "Top" or "Bottom" objects
        if (collision.gameObject.CompareTag("Top") || collision.gameObject.CompareTag("Bottom"))
        {
            // Reverse the vertical direction
            verticalSpeed = -verticalSpeed;
        }
    }
}