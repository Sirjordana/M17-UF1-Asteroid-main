using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidM : MonoBehaviour
{
    public GameManager gm;
    public GameObject AsteroidS;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Sprite[] asteroids;
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = asteroids[Random.Range(0, asteroids.Length)];
        rb = GetComponent<Rigidbody2D>();

        // Calculate random direction
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 15 || transform.position.y < -15 || transform.position.x > 20 || transform.position.x < -20)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            for (int i = 2; i != 0; i--)
            {
                // Calculate random direction for smaller asteroids
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                // Instantiate smaller asteroid with random direction
                GameObject asteroid = Instantiate(AsteroidS, transform.position, Quaternion.identity);
                Rigidbody2D asteroidRb = asteroid.GetComponent<Rigidbody2D>();
                asteroidRb.AddForce(randomDirection * Random.Range(minSpeed, maxSpeed), ForceMode2D.Impulse);
            }
            gm.addPoints(2);
            Destroy(gameObject);
        }
    }
}
