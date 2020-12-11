using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public bool teleported = false;
    public Teleporter destination;
    public GameObject thePlayer;
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (!teleported)
            {
                animator.SetBool("StartTeleport", true);
                StartCoroutine(Teleport(thePlayer));              
            }
    }

    IEnumerator Teleport(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        destination.teleported = true;

        Camera.main.transform.position += destination.gameObject.transform.position - go.transform.position;
        go.transform.position = destination.gameObject.transform.position;
        animator.SetBool("StartTeleport", false);
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
            teleported = false;
    }
}
