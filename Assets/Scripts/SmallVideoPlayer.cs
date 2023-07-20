using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class SmallVideoPlayer : MonoBehaviour {
	public string videoFileName;
	public bool playOnAwake = true;
	public string loadingText = "Loading...";
	public string loadedText = "Ready...";
	public GameObject[] relatedObjects;

	private VideoPlayer videoPlayer;
	private RawImage rawImage;
	private Text text;

	private void OnEnable() {

		for (var i = 0; i < relatedObjects.Length; i++) {
			relatedObjects[i].SetActive(true);
		}

		if (videoPlayer != null) {
			videoPlayer.time = 0;
			if (playOnAwake) videoPlayer.Play();
		}
	}

	private void Start() {
		videoPlayer = gameObject.GetComponent<VideoPlayer>();
		videoPlayer.source = VideoSource.Url;
		videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

		text = gameObject.GetComponentInChildren<Text>();
		if (!videoPlayer.isPrepared) {
			text.text = loadingText;
		}

		rawImage = gameObject.GetComponent<RawImage>();

		videoPlayer.Prepare();
		if (playOnAwake) {
			videoPlayer.Play();
		}
	}

	private void Update() {
		if (videoPlayer.isPrepared) {
			text.text = loadedText;
		}

		if (videoPlayer.isPlaying) text.gameObject.SetActive(false);
	}

	public void PlayVideo() {
		rawImage.texture = videoPlayer.targetTexture;
		videoPlayer.Play();
	}

	public void PauseVideo() {
		videoPlayer.Pause();
		text.gameObject.SetActive(true);
	}

	public void StopVideo() {
		rawImage.texture = null;
		videoPlayer.Pause();
		videoPlayer.time = 0;
		text.gameObject.SetActive(true);
	}

	public void SkipVideo() {
		StopVideo();
		for (var i = 0; i <  relatedObjects.Length; i++) {
			relatedObjects[i].SetActive(false);
		}
		gameObject.SetActive(false);
	}
}
