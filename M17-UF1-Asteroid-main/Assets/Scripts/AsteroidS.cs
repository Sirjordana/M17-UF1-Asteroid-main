using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidS : MonoBehaviour
{
    public GameManager gm;
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
            gm.addPoints(1);
            Destroy(gameObject);
        }
    }
}
