using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MediaPlayer : MonoBehaviour
{
    private AudioSource m_AudioSource;
    public AudioClip[] m_tracks;
    public float m_volume;
    public bool m_songPaused = false;
    [SerializeField] private float m_trackTimer;
    [SerializeField] private int m_PlayedTracks;
    [SerializeField] private bool[] m_beenPlayed;
    [SerializeField] private int[] m_played;
    private int m_lastTrack;
    private int m_randomNumber;
    private int m_randomselection;
    private int m_TrackCount;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();

        //Set tracks played and the order they've been played in to be the same length as the playlist
        m_beenPlayed = new bool[m_tracks.Length];
        //The order songs have been played in and whether they have been played are stored seperately to avoid having to have search loops for every track change
        m_played = new int[m_tracks.Length];

        //Initial Play
        if (!m_AudioSource.isPlaying && m_songPaused == false)
        {
            NextTrack(RandomTrack());
        }
        //Zero tracks play on load
        m_PlayedTracks = 1;
        m_TrackCount =m_tracks.Length;

    }

    // Update is called once per frame
    void Update()
    {
        //set volume
        m_AudioSource.volume = m_volume;

        //If no clip is playing and the user hasn't paused, play another track
        if (!m_AudioSource.isPlaying && m_songPaused==false)
        {
            NextTrack(RandomTrack());
        }
        //If the song has ended and the user hasn't paused, play another track
        else if (m_songPaused == false && m_trackTimer>=m_AudioSource.clip.length)
        {
            NextTrack(RandomTrack());
        }
        else if (m_AudioSource.isPlaying)
        {
            //Increment Timer
            m_trackTimer += Time.deltaTime;
        }


    }

    public void NextTrack(int m_trackPicked)
    {         
        //reset timer
        m_trackTimer = 0;
            //Change song to next track based on shuffle
            m_AudioSource.clip = m_tracks[m_trackPicked];
            //play the next song
            m_AudioSource.Play();
            //Store song played in the playlist
            m_played[m_PlayedTracks] = m_trackPicked;
 
        m_PlayedTracks++;
        if (m_PlayedTracks >= m_tracks.Length)
        {
            for (int i = 0; i < m_tracks.Length; i++)
            {
                m_beenPlayed[i] = false;
            }
            m_PlayedTracks = 1;
        }
        m_beenPlayed[m_trackPicked] = true;
    }

    public void PreviousTrack()
    {
        //If this is the first track in the playlist
        if (m_PlayedTracks <= 1)
        {
            //Make sure that Played tracks doesn't go negative
            //Play any random song as none have been played before in this list
            m_AudioSource.clip = m_tracks[UnityEngine.Random.Range(0, m_tracks.Length)];
        }
        else
        {
            m_PlayedTracks--;
            m_lastTrack = m_played[m_PlayedTracks];
            //reset timer
            m_trackTimer = 0;
            //Change song to next track based on shuffle
            m_AudioSource.clip = m_tracks[m_lastTrack];
        }
            m_AudioSource.Play();
    }

    public void PauseTrack()
    {
        if (m_AudioSource.isPlaying)
        {
            //Set user pause flag to true and pause song
            m_songPaused = true;
            m_AudioSource.Pause();
        }
        else if (!m_AudioSource.isPlaying)
        {
            m_songPaused = false;
            m_AudioSource.UnPause();
        }

    }

    public void ResumeTrack()
    {
        if (m_AudioSource.isPlaying)
        {
            //Set user pause flag to true and pause song
            m_songPaused = true;
            m_AudioSource.Pause();
        }
        else if (!m_AudioSource.isPlaying)
        {
            m_songPaused = false;
            m_AudioSource.UnPause();
        }
    }

    public void ClearArrays()
    {
        //Clears arrays when 
        Array.Clear(m_beenPlayed,0,m_beenPlayed.Length);
        Array.Clear(m_played, 0, m_played.Length);
        m_PlayedTracks = 0;
    }

    public int RandomTrack()
    {
       
        //Generate a random number
        m_randomNumber = UnityEngine.Random.Range(0, m_TrackCount);
        //check if that number has been used before in 
        if (m_beenPlayed[m_randomNumber]==true)
        {
            RandomTrack();
        }
        else
        {
            m_randomselection = m_randomNumber;    
        }

            return m_randomselection;
    }

    public void ClickNextTrack()
    {
        NextTrack(RandomTrack());
        if (!m_AudioSource.isPlaying)
        {
            m_songPaused = false;
            m_AudioSource.UnPause();
        }
    }
}
