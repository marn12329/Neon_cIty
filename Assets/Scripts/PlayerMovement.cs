using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 2.0f;
    private Rigidbody2D rb;
    private Vector2 movement;
    
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(speed);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // ซ้าย-ขวา (A,D หรือ ลูกศร)
        float moveY = Input.GetAxis("Vertical");   // ขึ้น-ลง (W,S หรือ ลูกศร)
        movement = new Vector2(moveX, moveY).normalized;
    }
    
    void FixedUpdate()
    {
        // ใช้ Rigidbody2D ในการเคลื่อนที่
        rb.velocity = movement * speed;
    }

    
}
