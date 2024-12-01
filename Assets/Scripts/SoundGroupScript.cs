using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroupScript : MonoBehaviour
{
    public List<GameObject> soundObjects = new List<GameObject>();
        
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] sources = GetComponentsInChildren<AudioSource>(); // Only children should have an audiosource
        foreach(AudioSource source in sources) {
            soundObjects.Add(source.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playRandomSound() {
        int i = Random.Range(0, soundObjects.Count);
        float delay = soundObjects[i].GetComponent<Sound>().delay;
        AudioSource source = soundObjects[i].GetComponent<AudioSource>();
        source.Stop();
        source.time = delay;
        source.Play();
    }
}
