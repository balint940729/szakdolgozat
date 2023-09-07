using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;

    public static AudioManager instance;

    private GameObject enemy;
    public GameObject character;

    private bool approach;

    private void Awake() {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            if (s.enemy != null)
                enemy = s.enemy;
        }
    }

    private void Start() {
        Play("Fight");
        Play("Theme");
    }

    private void Update() {
        if (approach) {
            ApproachingBoss();
        }
    }

    public void ApproachingBoss() {
        if (Vector3.Distance(enemy.transform.position, character.transform.position) <= 40) {
            approach = true;
            Stop("Theme");
            Play("Approach");
        }
        if (Vector3.Distance(enemy.transform.position, character.transform.position) > 40) {
            approach = false;
            Stop("Approach");
            Play("Theme");
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void Stop(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }
}