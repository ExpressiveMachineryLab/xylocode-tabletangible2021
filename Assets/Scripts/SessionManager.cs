using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class SessionManager : MonoBehaviour {
	[DllImport("__Internal")]
	static extern void JSGetSIDQuery(); // in the javascript
	[DllImport("__Internal")]
	static extern string JSGetDate(); // in the javascript

	private string sessionID = "_";
	private GameLog _gameLog = new GameLog();

	private CountLogger logger;
	private SOInterpreter soInterpreter;
	public float logIntervalInSeconds = 120f;
	private float nextLogTime = 0f;
	private float currentTime = 0f;

	public string localFolder = "SessionLogs";
	public string currentLogFileName = "sessionlog.txt";
	public string dateTimeFormat = "yyyy_MM_dd_HHss";
	string fileTimestamp;
	void Start() {
		sessionID = PlayerPrefs.GetString("SessionID", "_");

		if (sessionID.Equals("_")) {
			sessionID = new RandomString().CreateRandomString();
		}

		PlayerPrefs.SetString("SessionID", sessionID);
		_gameLog.sessionID = sessionID;

		float halfInterval = logIntervalInSeconds/2;
		nextLogTime = Time.time + logIntervalInSeconds + Random.Range(-halfInterval, halfInterval);

		soInterpreter = FindObjectOfType<SOInterpreter>();
		if (soInterpreter == null) {
			soInterpreter = gameObject.AddComponent<SOInterpreter>();
		}

		fileTimestamp = System.DateTime.Now.ToString(dateTimeFormat);
#if !UNITY_EDITOR && UNITY_WEBGL
        JSGetSIDQuery();
#endif
	}

	void Update() {
		if (Time.time >= nextLogTime) {
			soInterpreter.GenerateSaveDataString();
			CallLogSession(soInterpreter.GetTextInput());

			nextLogTime = Time.time + logIntervalInSeconds;
		}

		currentTime = Time.time;
	}

	public string GetSessionID() {
		return sessionID;
	}

	public void SetSessionID(string newID) {
		sessionID = newID;
		_gameLog.sessionID = sessionID;
		PlayerPrefs.SetString("SessionID", sessionID);
	}

	public void SetTimeStamp(string newTime) {
		_gameLog.timeStamp = newTime;
	}

	public void CallLogSession(string gameState) {
		StartCoroutine(LogSession(gameState));
	}

	IEnumerator LogSession(string gameState) {
		_gameLog.timeStamp = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString();
		_gameLog.gameState = gameState;

		if (logger == null) logger = FindObjectOfType<CountLogger>();
		if (logger != null) _gameLog.trackedStates = logger.GetLog();

		// log local first

		string filename = fileTimestamp+"-"+currentLogFileName;
		string localOutput = _gameLog.timeStamp;
		localOutput += ",sessionID:" + _gameLog.sessionID;
		localOutput += ",trackedStates:" + _gameLog.trackedStates;
		localOutput += ",gameState:" + _gameLog.gameState;
		System.Threading.Tasks.Task task = null;
		try
		{
			System.IO.Directory.CreateDirectory(Application.persistentDataPath+"/"+localFolder);
			using (System.IO.StreamWriter sw = System.IO.File.AppendText(Application.persistentDataPath + "/" + localFolder + "/" + filename))
			{
				task = sw.WriteLineAsync(localOutput);
			}
		}
		catch (System.Exception ex)
        {
			Debug.LogError("Failed To Write Local Log File");
			Debug.LogError(ex);
        }

		if (task != null)
		{
			yield return new WaitUntil(() => { return task.IsCompleted; });
			Debug.Log("Wrote to local log: "  + Application.persistentDataPath + "/" + localFolder + "/" + filename);
		}

		// log online
		WWWForm form = new WWWForm();
		form.AddField("timeStamp", _gameLog.timeStamp); //this timestamp is not currently being logged in DB, it is logging the SQL generated timestamp instead
		form.AddField("sessionID", _gameLog.sessionID);
		form.AddField("trackedStates", _gameLog.trackedStates);
		form.AddField("gameState", _gameLog.gameState);

		//Debug.Log(string.Join(",", form.data));

		UnityWebRequest www = UnityWebRequest.Post("https://xylocode.lmc.gatech.edu/sqlconnect/register.php", form);
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError) {
			//Debug.Log("Data log failed. Error # " + www.error);
		}
		else if (www.downloadHandler.text == "0") {
			//Debug.Log("Data logged successfully.");
		} else {
			//Debug.Log("Server reached successfully but data not logged. Error: " + www.downloadHandler.text);
		}
	}

}

[System.Serializable]
public struct GameLog {
	public string sessionID;
	public string timeStamp;
	public string trackedStates;
	public string gameState;
}