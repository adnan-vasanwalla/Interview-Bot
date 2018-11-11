using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RogoDigital.Lipsync;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
public class Speak : MonoBehaviour {
    public static LipSyncData clip;
    public static LipSync component;

    public static float secs = 10f;
    public static float startVal = 0f;
    public static float progress = 0f;
	// Use this for initialization
    public static string[] qs = new string[6];
    string[] assets = new string[]
    {
      "Garbage-Collection", "iterator", "Scheduling", "Synchronization","abstract-class","data-structure","dml",
      "ds-recursion","heterogeneous-link-list","view-db"
    };
    public static int qs_no = 0;
	void Start () {
        createQs();
        startSpeaking();
	}

	// Update is called once per frame
	void Update () {
        /*secs = 10;
        startVal = (float)EditorApplication.timeSinceStartup;
        if (secs>=1)
            EditorUtility.DisplayProgressBar("Simple Progress Bar", "Shows a progress bar for the given seconds", 1-secs/10);
        else
            EditorUtility.ClearProgressBar();
        secs--;*/
	}
    public static void startSpeaking()
    {
        //EditorUtility.DisplayProgressBar("Simple Progress Bar", "Shows a progress bar for the given seconds",0.5f);
        qs_no++;
        if (qs_no <= 5)
        {
            string path = "Lipsync/Custom/" + qs[qs_no];
            clip = Resources.Load<LipSyncData>(path);
            component = GameObject.Find("lincoln").GetComponent<LipSync>();
            component.Play(clip);
        }
        else
            SceneManager.LoadScene("result");
    }
    /*void OnGUI()
    {
        if (GUI.Button(new Rect(10, 130, 130, 50), "Go to Webcam"))
        {
            #if UNITY_5_3 || UNITY_5_3_OR_NEWER
            Picture.wct.Stop();
            SceneManager.LoadScene("WebCamTextureFaceTrackerExample");
            #else
                Application.LoadLevel("WebCamTextureFaceTrackerExample");
            #endif
        }

    }*/
    public void createQs()
    {
        int i = 1;
        IEnumerable<int> questions = Enumerable.Range(1, 10).OrderBy(x => Guid.NewGuid()).Take(5);
        foreach (int num in questions)
        {
            Debug.Log("Qs_no=" + num);
            qs[i] = assets[num-1];
            i++;
        }
        //if (!(String.IsNullOrEmpty(qs)))
        //   qs=qs.Substring(0, qs.Length - 1);
        //return qs;
    }
}
