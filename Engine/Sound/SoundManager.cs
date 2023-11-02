using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Super_Duper_Shooter.Engine.States;

namespace Super_Duper_Shooter.Engine.Sound;

public class SoundManager
{
    private int _soundtrackIndex;
    private List<SoundEffectInstance> _soundTracks;
    private Dictionary<Type, SoundEffect> _soundBank = new();
    public void SetSoundtrack(List<SoundEffectInstance> tracks)
    {
        _soundTracks = tracks;
        _soundtrackIndex = tracks.Count - 1;
    }

    public void StopSoundTrack()
    {
        foreach (var track in _soundTracks)
        {
            track.Stop();
        }
    }

    public void RegisterSound(BaseGameStateEvent gameEvent, SoundEffect soundEffect)
    {
        _soundBank.Add(gameEvent.GetType(), soundEffect);
    }

    public void OnNotify(BaseGameStateEvent gameEvent)
    {
        if (_soundBank.ContainsKey(gameEvent.GetType()))
        {
            var sound = _soundBank[gameEvent.GetType()];
            sound.Play();
        }
    }

    public void PlaySoundtrack()
    {
        var numbOfTracks = _soundTracks.Count;
        
        var currentTrack = _soundTracks[_soundtrackIndex];
        var nextTrack = _soundTracks[(_soundtrackIndex + 1) % numbOfTracks];
        
        
        if (currentTrack.State == SoundState.Stopped)
        {
            nextTrack.Play();
            
            _soundtrackIndex++;
        
            if (_soundtrackIndex >= _soundTracks.Count)
            {
                _soundtrackIndex = 0;
            }
        }
    }

}