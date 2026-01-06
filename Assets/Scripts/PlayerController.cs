using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float thrustForce = 1f;
    public GameObject boosterFlame;
    public UIDocument uiDocument;
    public GameObject explosionEffect;

    private float elapsedTime = 0f;
    private float score = 0f;
    private float scoreMultiplier = 10f;
    private Label scoreText;
    private Button restartButton;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
        Debug.Log(score);
        // Calculate mouse direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        Vector2 direction = (mousePos - transform.position).normalized;

        // Aim rocket toward mouse position
        transform.up = direction;

        // Move rocket in direction of mouse
        if (Mouse.current.leftButton.isPressed)
        {
            boosterFlame.SetActive(true);
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(direction * thrustForce);
        }
        else
        {
            boosterFlame.SetActive(false);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        Destroy(gameObject);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
