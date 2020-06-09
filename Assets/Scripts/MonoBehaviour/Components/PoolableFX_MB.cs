using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PoolableFX_MB : MonoBehaviour, IPoolable_MB
{
    private bool _available = true;
    public bool Available
    {
        get
        {
            return _available;
        }
        set
        {
            if(!value && _available)
            {
                PlayFX();
            }
            _available = value;
        }
    }

    private AudioSource _audio;
    private AudioSource Audio
    {
        get
        {
            if(!_audio)
            {
                _audio = GetComponentInChildren<AudioSource>();
            }
            return _audio;
        }
    }

    private VisualEffect _vFX;
    private VisualEffect VFX
    {
        get
        {
            if (!_vFX)
            {
                _vFX = GetComponentInChildren<VisualEffect>();
            }
            return _vFX;
        }
    }

    public GameObject GO
    {
        get
        {
            return gameObject;
        }
    }

    private void Update()
    {
        if(!Available)
        {
            if(!Audio.isPlaying && VFX.aliveParticleCount <= 0)
            {
                Available = true;
            }
        }
    }

    private void PlayFX()
    {
        VFX.SendEvent("Play");
        Audio.Play();
    }
}
