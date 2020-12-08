using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FollowerGFX : MonoBehaviour
{
    public AIPath aiPath;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            anim.SetBool("isMoving", true);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            anim.SetBool("isMoving", true);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (aiPath.desiredVelocity.x == 0f)
        {
            anim.SetBool("isMoving", false);
        }

        
    }
}
