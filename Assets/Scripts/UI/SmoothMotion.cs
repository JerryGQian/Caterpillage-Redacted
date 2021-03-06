﻿using UnityEngine;
using System.Collections;

public class SmoothMotion : MonoBehaviour {
    public Vector3 startPos;
    public Vector3 endPos;
    public float duration = 1f;
    public bool began = false;
    public bool autoSetStart = true;
    private float startTime;

    bool isRectTransform = false;

    public bool deactivateOnFinish;
    public bool deleteOnFinish;
    RectTransform rect;
    void Awake() {
        if (GetComponent<RectTransform>() != null) {
            isRectTransform = true;
            rect = GetComponent<RectTransform>();
        }
    }
    void Start() {
        if (autoSetStart) {
            if (!isRectTransform) {
                startPos = transform.position;
            }
            else {
                startPos = rect.localPosition;
            }
        }
        if (began) {
            begin();
        }
    }
    public void begin() {
        startTime = Time.realtimeSinceStartup;
        began = true;

        Invoke("end", duration);
    }

    public void end() {
        began = false;
        if (!isRectTransform) {
            transform.position = endPos;
        }
        else {
            rect.localPosition = endPos;
        }
        if (deactivateOnFinish) {
            gameObject.SetActive(false);
        }
        if (deleteOnFinish) {
            Destroy(gameObject);
        }

    }

    public void reset() {
        if (!isRectTransform) {
            transform.position = startPos;
        }
        else {
            rect.localPosition = startPos;
        }
        began = false;
        CancelInvoke("end");
    }

    void Update() {
        if (began && Time.realtimeSinceStartup < startTime + duration) {
            float t = (Time.realtimeSinceStartup - startTime) / duration;
            if (!isRectTransform) {
                transform.position = new Vector3(Mathf.SmoothStep(startPos.x, endPos.x, t), Mathf.SmoothStep(startPos.y, endPos.y, t), Mathf.SmoothStep(startPos.z, endPos.z, t));
            }
            else {
                rect.localPosition = new Vector3(Mathf.SmoothStep(startPos.x, endPos.x, t), Mathf.SmoothStep(startPos.y, endPos.y, t), Mathf.SmoothStep(startPos.z, endPos.z, t));
            }
        }
    }
}