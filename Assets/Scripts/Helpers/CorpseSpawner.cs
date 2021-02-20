using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseSpawner : MonoBehaviour {
    [SerializeField] private GameObject spritePrefab;
    [SerializeField] private Vector3 offset;
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] private GameObject particlePrefab;

    public void SpawnCorpse() {
        if(spritePrefab != null) {
            GameObject spriteObject = Instantiate(spritePrefab);
            spriteObject.transform.position = transform.position + offset;
            spriteObject.GetComponent<Animator>()?.SetBool("isDead", true);
            spriteObject.GetComponent<AudioSource>()?.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
        }
        GameObject particleObject = Instantiate(particlePrefab);
        particleObject.transform.position = transform.position + offset;
    }
}
