using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public static SoundHandler instance;

    
    [SerializeField] private GameObject audioContainer;

    private Dictionary<string, AudioSource> loadedAudios = new Dictionary<string, AudioSource>();


    void loadAudios()
    {
        loadedAudios.Clear();
        foreach(Transform child in audioContainer.transform)
        {
            AudioSource source = child.GetComponent<AudioSource>();
            loadedAudios.Add(child.name, source);
        }
        
    }
    

    void Awake()
    {
        instance = this;
        
        loadAudios();
    }



    public void playAudio(string name,bool loop=false)
    {
        if (loadedAudios.ContainsKey(name))
        {
            if (loop)
                loadedAudios[name].loop = true;
            loadedAudios[name].Play();
            
        }
    }

    public void stopAudio(string name)
    {
        if (loadedAudios.ContainsKey(name))
        {
            loadedAudios[name].Stop();
            
        }
    }

    public void setVolume(string name, float volume)
    {
        if (loadedAudios.ContainsKey(name))
        {
            loadedAudios[name].volume = volume;
        }
    }

    public float getVolume(string name)
    {
        if (loadedAudios.ContainsKey(name))
        {
            return loadedAudios[name].volume;
        }
        return 0.0f;
    }

    public float getAudioPosition(string name)
    {
        if (loadedAudios.ContainsKey(name))
        {
            return loadedAudios[name].time;
        }
        return -1;
    }

    public void setAudioPosition(string name, float timeVal)
    {
        if (loadedAudios.ContainsKey(name))
        {
            loadedAudios[name].time = timeVal;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
