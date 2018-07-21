using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

  public int playID;
  public int numSources;

    public static AudioPlayer Instance
    {
        get
        {
            return _instance;
        }
    }

    private static AudioPlayer _instance;

    private AudioSource[] sources;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start () {

        sources = new AudioSource[numSources];

        for( int i = 0; i < numSources; i++){
          sources[i] = gameObject.AddComponent<AudioSource>() as AudioSource;
            sources[i].playOnAwake = false;
        }
    }

    public void Play( AudioClip clip ){
        
        sources[playID].clip = clip;
        sources[playID].Play();

        playID ++;
        playID %= numSources;
    }


    public void Play( AudioClip clip , float pitch){

        sources[playID].volume = 1;
        sources[playID].pitch = pitch;
        Play(clip);
    }

    public void Play( AudioClip clip , float pitch , float volume){

        sources[playID].volume = volume;
        sources[playID].pitch = pitch;
        Play(clip);
    }

    public void Play( AudioClip clip , int step , float volume){

        float p = Mathf.Pow( 1.05946f , (float)step );
        sources[playID].volume = volume;
        sources[playID].pitch = p;
        Play(clip);
    }
}
