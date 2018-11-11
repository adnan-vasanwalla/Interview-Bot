using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using SimpleJSON;
using System;

public class Picture : MonoBehaviour {
    public static double baseLev, finalLev, happy, sad, fear, surprise, neutral,attempts;
	public string deviceName;
	public static WebCamTexture wct;
	private string _SavePath ; //Change the path here!
	int _CaptureCounter = 0;
	private string m_URL = "http://35.154.136.198:90/speech/api/image_save.php";
	private string fileName;
    string confidence = "Loading..";
	// Use this for initialization
	void Start () {
		_SavePath = Application.persistentDataPath;
        WebCamDevice[] devices = WebCamTexture.devices;
        deviceName = devices[0].name;
        wct = new WebCamTexture(deviceName, 400, 300, 12);
        //renderer.material.mainTexture = wct;
        wct.Play();
        InvokeRepeating("takeSnaps", 0.0f, 30.0f);
	}
	public Texture2D heightmap;
	public Vector3 size = new Vector3(100, 10, 100);

	void OnGUI() {
        /*if (GUI.Button(new Rect(10, 200, 50, 30), "Click"))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            deviceName = devices[0].name;
            wct = new WebCamTexture(deviceName, 400, 300, 12);
            //renderer.material.mainTexture = wct;
            wct.Play();
            TakeSnapshot();
            UploadFile(m_URL);
        }*/
        GUI.contentColor = Color.black;
        GUI.Label(new Rect(100, 10, 1200, 200), confidence);
	}
    void takeSnaps()
    {
        //if (wct.isPlaying)
        //    wct.Stop();
        TakeSnapshot();
        UploadFile(m_URL);
    }
	void TakeSnapshot()
	{
		Debug.Log (_SavePath);
		Texture2D snap = new Texture2D(wct.width, wct.height);
		snap.SetPixels(wct.GetPixels());
		snap.Apply();
		fileName = _CaptureCounter.ToString() + ".png";
		System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
		++_CaptureCounter;
        //wct.Stop();
	}
	IEnumerator UploadFileCo(string uploadURL)
	{
		var filePath = Path.Combine(Application.persistentDataPath, fileName);
		WWW localFile = new WWW("file:///"+filePath);
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

		postForm.AddBinaryData("theFile", localFile.bytes, fileName, "text/plain");
		WWW upload = new WWW(uploadURL, postForm);
		yield return upload;
        if (upload.error == null)
        {
            Debug.Log("Result= " + upload.text);
            //confidence = upload.text;
            Processjson(upload.text);
        }
        else
            Debug.Log("Error during upload: " + upload.error);
	}
	void UploadFile(string uploadURL)
	{
        uploadURL += "?&user_name=" + PlayerPrefs.GetString("RegUser");
        Debug.Log("uploadUrl=" + uploadURL);
		StartCoroutine(UploadFileCo(uploadURL));
	}
	// Update is called once per frame
	void Update () {
		
	}
    private void Processjson(string jsonString)
    {
        attempts++;
        var json = JSON.Parse(jsonString);
        happy += json[0]["scores"]["happiness"];
        fear += json[0]["scores"]["fear"];
        sad += json[0]["scores"]["sadness"];
        neutral += json[0]["scores"]["neutral"];
        surprise += json[0]["scores"]["surprise"];
        confidence = "Happiness: " + json[0]["scores"]["happiness"] + "\t Fear: " + json[0]["scores"]["fear"]+
            "\t Sadness: " + json[0]["scores"]["sadness"] + "\t Neutral: " + json[0]["scores"]["neutral"] + "\t Surprise: " + json[0]["scores"]["surprise"];
    }

}
