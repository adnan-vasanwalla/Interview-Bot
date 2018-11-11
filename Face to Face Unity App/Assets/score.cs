using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class score : MonoBehaviour
{
    public double baseLev, finalLev, happy , sad , fear, surprise , neutral,attempts;
	// Use this for initialization
    private string m_URL = "http://35.154.136.198:90/speech/api/getdata.php?user_name=";
    //{"similarity":1,"anger":"0","contempt":"0","disgust":"0","fear":"0","happiness":"1","neutral":"0","sadness":"0","surprise":"0"}
    void Start()
    {
        m_URL+=PlayerPrefs.GetString("RegUser");
        WWW www = new WWW(m_URL);
        StartCoroutine(WaitForRequest(www));
        //{"similarity",""}
    }
    private void Processjson(string jsonString)
    {
        var json = JSON.Parse(jsonString);
        finalLev = Convert.ToDouble(json["baseLev"].AsDouble);
        /*happy = Convert.ToDouble(json["happy"].AsDouble);
        sad = json["sad"].AsDouble;
        fear = json["fear"].AsDouble;
        surprise = json["surprise"].AsDouble;
        neutral = json["neutral"].AsDouble;*/
    }
    void OnGUI()
    {
        GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
        centeredStyle.alignment = TextAnchor.UpperCenter;
        //centeredStyle.font.material.color = Color.black;
        attempts = Picture.attempts;
        //GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 120, 300, 30), "Base Level: "+Picture.baseLev*100.0/attempts+"%",centeredStyle);
        GUI.contentColor = Color.black;
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 80, 300, 30), "Final Level: " + finalLev, centeredStyle);
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 40, 300, 30), "Happiness: " + Picture.happy * 100.0 / attempts + "%", centeredStyle);
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 30), "Saddness: " + Picture.sad * 100.0 / attempts + "%", centeredStyle);
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 40, 300, 30), "Fear: " + Picture.fear * 100.0 / attempts + "%", centeredStyle);
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 80, 300, 30), "Neutral: " + Picture.neutral * 100.0 / attempts + "%", centeredStyle);
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 120, 300, 30), "Surprise: " + Picture.surprise * 100.0 / attempts + "%", centeredStyle);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW success: " + www.text);
            Processjson(www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

}
