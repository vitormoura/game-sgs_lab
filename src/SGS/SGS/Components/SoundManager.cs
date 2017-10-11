using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace SGS.Components
{
    /// <summary>
    /// Componente responsável pelo gerenciamento de todo o áudio do jogo
    /// </summary>
    public class SoundManager
    {
        private Dictionary<string, SoundEffect> sfxCollection;
        private Dictionary<string, Song> songsCollection;
        private List<SoundEffectInstance> sfxCurrentlyPlaying;
        private AudioListener listener;
        public bool IsSFXMuted { get; set; }

        private float sfxVolume;
        public float SFXVolume
        {
            get
            {
                return sfxVolume;
            }
            set
            {
                sfxVolume = value < 0 ? 0 : value;
                sfxVolume = value > 2 ? 2 : value;

                if (value == 0)
                    IsSFXMuted = true;
                else
                    IsSFXMuted = false;
            }
        }

        private float songVolume;
        public float SongVolume
        {
            get
            {
                return songVolume;
            }
            set
            {
                songVolume = value < 0 ? 0 : value;
                songVolume = value > 2 ? 2 : value;

                if (value == 0)
                    MediaPlayer.IsMuted = true;
                else
                    MediaPlayer.IsMuted = false;

                MediaPlayer.Volume = value;
            }
        }

        public SoundManager()
        {
            sfxCollection = new Dictionary<string, SoundEffect>();
            songsCollection = new Dictionary<string, Song>();
            sfxCurrentlyPlaying = new List<SoundEffectInstance>();

            IsSFXMuted = false;
            SFXVolume = 0.2f;
            SongVolume = 0.2f;

            MediaPlayer.Volume = SongVolume;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.IsMuted = false;
        }

        public void LoadSFX(String sound)
        {
            if (!sfxCollection.ContainsKey(sound))
            {
                var sfx = GameManager.Content.Load<SoundEffect>(sound);
                sfxCollection.Add(sound, sfx);
            }
        }

        public void PlaySFX(String sound)
        {
            if (!IsSFXMuted)
            {
                var sfx = sfxCollection[sound].CreateInstance();
                sfx.Volume = SFXVolume;
                sfx.Play();
                //sfxCurrentlyPlaying.Add(sfx);
            }
        }

        public void LoadSong(String sound)
        {
            if (!songsCollection.ContainsKey(sound))
            {
                var song = GameManager.Content.Load<Song>(sound);
                songsCollection.Add(sound, song);
            }
        }

        public void PlaySong(String sound)
        {
            var song = songsCollection[sound];
            MediaPlayer.Play(song);
        }

        public void ToggleMute()
        {
            IsSFXMuted = !IsSFXMuted;
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
        }

        public void ToggleMuteSFX()
        {
            IsSFXMuted = !IsSFXMuted;
        }

        public void ToggleMuteSong()
        {
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
        }

        public void RegisterListener(Vector3 forward, Vector3 position, Vector3 up, Vector3 velocity)
        {
            listener = new AudioListener();
            listener.Forward = forward;
            listener.Position = position;
            listener.Up = up;
            listener.Velocity = velocity;
        }

        public void StopCurrentSounds()
        {
            /*foreach (var sfx in sfxCurrentlyPlaying)
            {
                if (sfx.State != SoundState.Stopped)
                    sfx.Stop();

                sfxCurrentlyPlaying.Remove(sfx);
            }*/

            MediaPlayer.Stop();
        }

        public void Pause()
        {
            /*foreach (var sfx in sfxCurrentlyPlaying)
            {
                if (sfx.State == SoundState.Playing)
                    sfx.Pause();
            }*/

            MediaPlayer.Pause();
        }

        public void Resume()
        {
            /*foreach (var sfx in sfxCurrentlyPlaying)
            {
                if (sfx.State == SoundState.Paused)
                    sfx.Resume();
            }*/

            MediaPlayer.Resume();
        }
    }
}
