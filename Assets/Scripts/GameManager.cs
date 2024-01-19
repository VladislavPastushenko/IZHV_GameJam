using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    /// GameObject used for the start text.
    public GameObject startText;
    
    /// GameObject used for the loss text.
    public GameObject lossText;
    
    /// GameObject used for the score text.
    public GameObject scoreText;

    /// GameObject representing the spawner.
    public GameObject spawner;

    /// GameObject representing the triangle.
    public GameObject triangle;

    /// Current accumulated score.
    public int mCurrentScore = 0;

    /// Is the game lost?
    public bool mGameLost = false;

    /// Did we start the game?
    private static bool sGameStarted = false;

    /// Singleton instance of the GameManager.
    private static GameManager sInstance;
    
    /// Getter for the singleton GameManager object.
    public static GameManager Instance
    { get { return sInstance; } }

    /// Called when the script instance is first loaded.
    private void Awake()
    {
        // Initialize the singleton instance, if no other exists.
        if (sInstance != null && sInstance != this)
        { Destroy(gameObject); }
        else
        { sInstance = this; }
        
        // Setup the game scene.
        SetupGame();
    }
    

     /// Called before the first frame update.
    void Start()
    { }

    // Update is called once per frame
    void Update()
    {
        // Start the game after the first "SwitchDirection".
        if (!sGameStarted && Input.GetButtonDown("SwitchDirection"))
        { StartGame(); }
        
        // Reset the game if requested.
        if (Input.GetButtonDown("Cancel"))
        { ResetGame(); }

        if (sGameStarted && !mGameLost)
        {
            // Update the score text.
            GetChildNamed(scoreText, "Value").GetComponent<Text>().text = $"{(int)(mCurrentScore)}";
        }
    }

    public void SetupGame()
    {
        
        if (sGameStarted)
        { // Setup already started game -> Retry.
            startText.SetActive(false);
            scoreText.SetActive(true);
            lossText.SetActive(false);
        }
        else
        { // Setup a new game -> Wait for start.
            spawner.GetComponent<Spawner>().spawnObstacles = false;
            triangle.GetComponent<Triangle>().gameHasStarted = false;
            
            // Setup the text.
            startText.SetActive(true);
            scoreText.SetActive(false);
            lossText.SetActive(false);
        }
        
        // Set the state.
        mGameLost = false;
    }

    /// Set the game to the "started" state.
    public void StartGame()
    {
        // Reload the scene as started.
        sGameStarted = true; 
        ResetGame();
    }
    
    /// Reset the game to the default state.
    public void ResetGame()
    {
        // Reload the active scene, triggering reset...
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// Set the game to the "lost" state.
    public void LooseGame()
    {
        // Get the spawner script.
        var sp = spawner.GetComponent<Spawner>();
        // Stop the obstacles.
        sp.obstacleSpeed = 0.0f;
        // Stop spawning.
        sp.spawnObstacles = false;

        triangle.GetComponent<Triangle>().gameHasStarted = false;

        // Show the loss text.
        lossText.SetActive(true);
        // Loose the game.
        mGameLost = true;
    }

    public void SetCurrentScore(int score)
    {
        mCurrentScore = score;
    }

    private static GameObject GetChildNamed(GameObject go, string name) 
    {
        var childTransform = go.transform.Find(name);
        return childTransform == null ? null : childTransform.gameObject;
    }
}
