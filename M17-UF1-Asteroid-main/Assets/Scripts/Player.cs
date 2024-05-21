using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    public GameObject prefabBullet;
    public float speedThrusting = 1.0f;
    public float speedTurn = 1.0f;
    public float turnDirection = 0.0f;
    private bool thrusting = false;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.InGame)
        {
            thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                turnDirection = .5f;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                turnDirection = -.5f;
            }
            else
            {
                turnDirection = 0.0f;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Shot();
            }
        }
        else
        {
            turnDirection = 0.0f;
            thrusting = false;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rb.AddForce(transform.up*speedThrusting);
        }

        if (turnDirection != 0)
        {
            rb.AddTorque(turnDirection*speedTurn);    
        }
    }

    private void Shot()
    {
        GameObject o = Instantiate(prefabBullet, transform.position, transform.rotation, transform);
        Bullet b = o.GetComponent<Bullet>();
        b.Shot(transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
        {
            gm.TakeDamage();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TopBoundary"))
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y + 0.1f, transform.position.z);
        }

        if (collision.CompareTag("LeftBoundary"))
        {
            transform.position = new Vector3(-transform.position.x -0.1f, transform.position.y, transform.position.z);
        }

        if (collision.CompareTag("RightBoundary"))
        {
            transform.position = new Vector3(-transform.position.x + 0.1f, transform.position.y, transform.position.z);
        }

        if (collision.CompareTag("BottomBoundary"))
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y -0.1f, transform.position.z);
        }
    }
}
