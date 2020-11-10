using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    Animator anim;
    SpriteRenderer renderer;
    Rigidbody2D rigidbody2D;
    public bool controls = true;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
        }
        // Right
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            anim.SetBool("MoveRight", true);
            anim.SetBool("MoveLeft", false);
            anim.SetBool("MoveUp", false);
            anim.SetBool("MoveDown", false);
            anim.speed = 0.5f;
        }
        // Left
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            anim.SetBool("MoveRight", false);
            anim.SetBool("MoveLeft", true);
            anim.SetBool("MoveUp", false);
            anim.SetBool("MoveDown", false);
            anim.speed = 0.5f;
        }
        // Up
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            anim.SetBool("MoveRight", false);
            anim.SetBool("MoveLeft", false);
            anim.SetBool("MoveUp", true);
            anim.SetBool("MoveDown", false);
            anim.speed = 0.5f;
        }
        // Down
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            anim.SetBool("MoveRight", false);
            anim.SetBool("MoveLeft", false);
            anim.SetBool("MoveUp", false);
            anim.SetBool("MoveDown", true);
            anim.speed = 0.5f;
        }
        else anim.speed = 0;
    }

}
