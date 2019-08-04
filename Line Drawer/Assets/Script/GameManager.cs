using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public string nextLevel = "";

    private bool isGameStart = false;

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!isGameStart && Input.GetMouseButtonUp(0) && !IsPointerOverUIObject())
        {
            isGameStart = true;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in players)
            {
                Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            string filePath = Application.dataPath + "/Resources/" + SceneManager.GetActiveScene().name + "_Default.png";
            ScreenShot.instance.TakeShopByOtherCameraScreen(filePath);
        }
    }

    public void Win()
    {
        Debug.Log("Win");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        //截圖
        string filePath = Application.persistentDataPath + "//" + SceneManager.GetActiveScene().name + "_FullScreenShot.png";
        ScreenShot.instance.TakeShopByOtherCameraScreen(filePath);

        //儲存   暫時不需要  因為只有鎖  無其他資料
        //SaveLevel();

        //解鎖  
        string loadJson = PlayerPrefs.GetString("Level " + nextLevel, "");
        LevelData loadData = JsonUtility.FromJson<LevelData>(loadJson);

        LevelData levelData = new LevelData(false);
        string saveString = JsonUtility.ToJson(levelData);
        PlayerPrefs.SetString("Level " + nextLevel, saveString);
    }

    public void SaveLevel()
    {
        string loadJson = PlayerPrefs.GetString(SceneManager.GetActiveScene().name, "");
        LevelData loadData = JsonUtility.FromJson<LevelData>(loadJson);

        LevelData levelData = new LevelData(false);
        string saveString = JsonUtility.ToJson(levelData);
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, saveString);
    }

    private bool IsPointerOverUIObject()  //true : UI
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}