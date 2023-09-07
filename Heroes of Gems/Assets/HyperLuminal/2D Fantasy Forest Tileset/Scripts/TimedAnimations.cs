using UnityEngine;

public class TimedAnimations : MonoBehaviour {

    #region Member Variables

    /// <summary>
    /// A local reference of this objects animator
    /// </summary>
    private Animator animator;

    /// <summary>
    /// The time between animation changes
    /// </summary>
    public float Timer = 0.0f;

    /// <summary>
    /// Pause time
    /// </summary>
    public float PauseTime;

    #endregion Member Variables

    // Use this for initialization
    private void Start() {
        // get a local reference
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update() {
        // update the timer and switch state when we reach the threshold
        Timer += Time.deltaTime;
        if (Timer > 1.0f + PauseTime) {
            Timer = 0.0f;
            animator.Play("PlayOnce");
        }
    }
}