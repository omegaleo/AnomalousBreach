using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : InstancedBehavior<AudioManager>
{

    public Sound[] sounds;

    private Sound _prevSound = null;
    
    // Update is called once per frame
    public void Play(string name)
    {
        var s = sounds.FirstOrDefault(sound => sound.name.Equals(name, StringComparison.OrdinalIgnoreCase));
        
        if (s == null) return;

        var newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = s.clip;

        newSource.volume = s.volume;
        newSource.pitch = s.pitch;
        
        if (_prevSound == s)
        {
            newSource.pitch = UnityEngine.Random.Range(0.75f, 1.25f);
        }

        _prevSound = s;
        
        newSource.Play();

        StartCoroutine(WaitToDestroySource(newSource));
    }

    private IEnumerator WaitToDestroySource(AudioSource source)
    {
        yield return new WaitForSeconds(0.1f);
        
        while (source.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        Destroy(source);
    }
}
