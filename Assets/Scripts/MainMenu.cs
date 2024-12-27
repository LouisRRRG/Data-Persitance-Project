using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public static MainMenu Instance;
    public string currentName;
    public string bestName;
    public int bestScore;
    public bool listenersActive = false;
    private bool fileExtracted = false;
    public Button startButton, exitButton;

    // Start is called before the first frame update
    private void Awake()
    {
        if(fileExtracted == false)
        {
            LoadScore();
        }

        startButton.onClick.AddListener(PlayGame);
        exitButton.onClick.AddListener(ExitGame);

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
       
        SetupListeners();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.buildIndex == 0) 
        {
            startButton = GameObject.Find("Start").GetComponent<Button>();
            exitButton = GameObject.Find("Exit").GetComponent<Button>();
            nameText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();

            
            if (!listenersActive)
            {
                SetupListeners();
                listenersActive = true;
            }
        }
       
    }

    // This method will set up the button listeners
    private void SetupListeners()
    {
        if (startButton != null)
            startButton.onClick.AddListener(PlayGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
    }


    public void PlayGame()
    {
        currentName = nameText.text;
        listenersActive = false;
        SceneManager.LoadScene(1);
        
    }

    

    public void ExitGame()
    {
        SaveScore();

        if (EditorApplication.isPlaying)
        {

            EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }

    }

    [System.Serializable]
    
    class Savedata
    {
        public string bestName;
        public int bestScore;
    }

    public void SaveScore()
    {
        Savedata data = new Savedata();
        data.bestName = bestName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Savedata data = JsonUtility.FromJson<Savedata>(json);

            bestName = data.bestName;
            bestScore = data.bestScore;
            fileExtracted = true;
        }
    }
}
