﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType { title, menu, underground, inGame }

public class SoundManager : MonoBehaviour {

    public AudioSource sfxSource1;
    public AudioSource sfxSource2;
    public AudioSource sfxSource3;

    public static AudioSource sfxSourceStatic1;
    public static AudioSource sfxSourceStatic2;
    public static AudioSource sfxSourceStatic3;

    public AudioSource musicSource;
    public static AudioSource musicSourceStatic;

    public AudioSource laserAudioSource;
    public static AudioSource laserAudioSourceStatic;

    public float maxLaserVol = 1f;
    public float targetVol;
    public float laserVol;

    public AudioClip chargeClip;
    public AudioClip fireClip;
    public AudioClip loopClip;

    public AudioClip titleMusic;
    public AudioClip menuMusic;
    public AudioClip inGameMusic;

    AudioClip nextMusicClip;
    float musicVol;
    public float maxMusicVol;
    float targMusicVol;

    public static SoundManager instance;

    // Use this for initialization
    void Start () {
        sfxSourceStatic1 = sfxSource1;
        sfxSourceStatic2 = sfxSource2;
        sfxSourceStatic3 = sfxSource3;
        musicSourceStatic = musicSource;
        laserAudioSourceStatic = laserAudioSource;


        instance = this;

        musicVol = maxMusicVol;
        targMusicVol = maxMusicVol;
    }

    private void Update() {
        laserVol += (targetVol - laserVol) * Time.deltaTime * 3f;
        laserAudioSource.volume = laserVol;

        musicVol += (targMusicVol - musicVol) * Time.deltaTime * 5f;
        musicSource.volume = musicVol;
    }

    public static void PlaySfx(params AudioClip[] clips) {
        PlaySfx(1f, clips);
    }

    public static void PlaySfx(float vol, params AudioClip[] clips) {
        int index = Random.Range(0, clips.Length);
        PlaySingleSfx(clips[index], vol);
    }

    public static void PlaySingleSfx(AudioClip clip, float vol = 1f, int pitchIndex = -1) {
        if (GameManager.settings == null) return;
        if (!GameManager.settings.soundOn) return;
        pitchIndex = pitchIndex >= 0 ? pitchIndex : Random.Range(0, 3);
        switch (pitchIndex) {
            case 0: if (sfxSourceStatic1 != null) sfxSourceStatic1.PlayOneShot(clip, vol); break;
            case 1: if (sfxSourceStatic2 != null) sfxSourceStatic2.PlayOneShot(clip, vol); break;
            case 2: if (sfxSourceStatic3 != null) sfxSourceStatic3.PlayOneShot(clip, vol); break;
        }
    }

    public static void StartLaser() {
        instance.targetVol = GameManager.settings.soundOn ? instance.maxLaserVol : 0;
        instance.laserVol = instance.targetVol;
    }

    public static void EndLaser() {
        instance.targetVol = 0;
    }

    public static void InstantSetLaserVol(float v) {
        instance.laserVol = v;
        instance.laserAudioSource.volume = instance.laserVol;
    }

    public void ChangeMusic(MusicType type) {
        switch (type) {
            case MusicType.title: nextMusicClip = titleMusic; break;
            case MusicType.menu: nextMusicClip = menuMusic; break;
            case MusicType.inGame: nextMusicClip = inGameMusic; break;
                
        }
        targMusicVol = 0;
        CancelInvoke("SwapMusic");
        Invoke("SwapMusic", 0.6f);

    }

    void SwapMusic() {
        targMusicVol = GameManager.settings.musicOn ? maxMusicVol : 0;
        musicSource.clip = nextMusicClip;
        musicSource.Play();
    }

    public void UpdateMusicOnOff() {
        targMusicVol = GameManager.settings.musicOn ? maxMusicVol : 0;
        musicVol = targMusicVol;
    }


}
