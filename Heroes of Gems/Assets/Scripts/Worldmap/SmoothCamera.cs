using UnityEngine;

public class SmoothCamera : MonoBehaviour {
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Transform target = default;
    [SerializeField] private Vector3 offset = default;

    private void Start() {
        transform.position = target.position;
    }

    private void Update() {
        if (target) {
            Vector3 anchorPos = transform.position + offset;
            Vector3 movement = target.position - anchorPos;
            Vector3 newCamPos = transform.position + movement * speed * Time.deltaTime;
            transform.position = newCamPos;
        }
    }
}