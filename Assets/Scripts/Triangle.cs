using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.VisualScripting;

public class Triangle : MonoBehaviour
{
    /// Current vertical speed of the triangle.
    public float verticalSpeed = 1.0f;

    /// Our RigidBody used for physics simulation.
    private Rigidbody2D mRB;
    
    /// Our BoxCollider used for collision detection.
    private BoxCollider2D mBC;
    
    /// Sprite renderer of the child sprite GameObject.
    private SpriteRenderer mSpriteRenderer;

    /// Transform of the child sprite GameObject.
    private Transform mSpriteTransform;

    /// Target angle of rotation in degrees.
    private Quaternion mTargetRotation;

    /// Direction of flying of the arrow
    private bool mFlyDirectionUp = true;

    /// Has game started
    public bool gameHasStarted = true;

    // Start is called before the first frame update
    void Start() {
        mRB = GetComponent<Rigidbody2D>();
        mBC = GetComponent<BoxCollider2D>();
        mSpriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        mSpriteTransform = gameObject.transform.GetChild(0).GetComponent<Transform>();
        mTargetRotation = mSpriteTransform.rotation;
    }

    // Update is called once per frame
    void Update() {
        var switchDirectionButtonClicked = Input.GetButtonDown("SwitchDirection");
        
        if (switchDirectionButtonClicked) {
            mFlyDirectionUp = !mFlyDirectionUp;
        }

        UpdatePlayerDirection();
    }

    // Update current vertical speed and direction of triangle according to current speed and direction.
    void UpdatePlayerDirection() {
        if (gameHasStarted) {
            var movementDirection = mFlyDirectionUp ? new float2(0.0f, 1.0f) : new float2(0.0f, -1.0f);
            mRB.velocity = movementDirection * verticalSpeed;

            var rotation = mFlyDirectionUp ? -50f : -130f;
            mRB.rotation = rotation;
        } else {
            mRB.velocity = new float2(0.0f, 0.0f);
            mRB.rotation = -90.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        mRB.velocity = new float2(0.0f, 0.0f);
        // Loose the game.
        GameManager.Instance.LooseGame();
    }
}
