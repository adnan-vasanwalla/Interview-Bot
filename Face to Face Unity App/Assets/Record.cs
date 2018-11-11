using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Record : MonoBehaviour {
    private string m_URL = "http://35.154.136.198:90/speech/api/upload.php";
    private string fileName = "myfile.wav";
    // Use this for initialization
    public AudioClip audio;
    public static string ans="Loading";
    bool LoadOut = false;
    string LoadOutText = "";
    void Start()
    {

        /*audio = GetComponent<AudioSource>();
        audio.clip = Microphone.Start ("", true, 10, 44100);
        audio.loop = true;
        while (!(Microphone.GetPosition (null) > 0)) {
        }
        audio.Play();*/
        foreach (string device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

    }
    void OnGUI()
    {
        
        if (GUI.Button(new Rect(10, 10, 80, 70), "Start"))
        {
            audio = Microphone.Start("Mic in at front panel (black) (Realtek High Definition Audio)", false, 10, 44100);
        }
        if (GUI.Button(new Rect(10, 90, 80, 70), "Stop"))
        {
            SavWav.Save(fileName, audio);
            UploadFile(m_URL);
            //        audio.Play();
        }
        if (GUI.Button(new Rect(10,170, 80, 70), "Repeat"))
        {
            Speak.qs_no =Speak.qs_no-1;
            Speak.startSpeaking();
        }
        if (LoadOut)
        {
            //GUI.skin = guiSkin;
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 30, 300, 60), LoadOutText);
        }
        GUI.contentColor = Color.black;
        GUI.Label(new Rect(100, 50, 1200, 200), ans);
    }
    IEnumerator UploadFileCo(string uploadURL)
    {
        var filePath = Path.Combine(Application.persistentDataPath, fileName);
        WWW localFile = new WWW("file:///" + filePath);
        Debug.Log(filePath);
        yield return localFile;
        if (localFile.error == null)
        
            Debug.Log("Loaded file successfully");
        else
        {
            Debug.Log("Open file error: " + localFile.error);
            yield break; // stop the coroutine here
        }
        WWWForm postForm = new WWWForm();
        // version 1
        //postForm.AddBinaryData("theFile",localFile.bytes);
        // version 2
        LoadOut = true;
        LoadOutText = "Processing your answer...";
        postForm.AddBinaryData("theFile", localFile.bytes, fileName, "text/plain");
        WWW upload = new WWW(uploadURL, postForm);
        yield return upload;        
        ans = upload.text;
        if (upload.error == null)
        {
            Debug.Log("Result= " + upload.text);
            LoadOut = false;
            Speak.startSpeaking();
        }
        else
            Debug.Log("Error during upload: " + upload.error);
    }
    void UploadFile(string uploadURL)
    {
        uploadURL += "?Qs_name=" + Speak.qs[Speak.qs_no]+"&user_name="+PlayerPrefs.GetString("RegUser");
        Debug.Log("uploadUrl=" + uploadURL);
        StartCoroutine(UploadFileCo(uploadURL));
    }


    // Update is called once per frame
    void Update()
    {

    }

}
