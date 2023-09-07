using UnityEngine;

public class SmoothCamera : MonoBehaviour {
    public float speed = 1.0f;
    public Transform target;

    public Vector3 offset;

    // Start is called before the first frame update
    private void Start() {
        offset = target.position - transform.position;
    }

    // Update is called once per frame
    private void Update() {
        if (target) {
            Vector3 anchorPos = transform.position + offset;
            Vector3 movement = target.position - anchorPos;

            Vector3 newCamPos = transform.position + movement * speed * Time.deltaTime;
            transform.position = newCamPos;
        }
    }
}