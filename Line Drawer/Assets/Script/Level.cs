using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    public string titleText;

    public Image image;

    public GameObject lockLevel;

    public Text levelText;

    public bool isLock;

    private void OnEnable()
    {
        levelText.text = titleText;
        LoadLevelData();

        string filePath = Application.persistentDataPath + "//Level " + titleText + "_FullScreenShot.png";
        image.sprite = LoadSprite(filePath);
        
        if (image.sprite == null)
        {
            Texture2D texture2D = (Texture2D)Resources.Load("Level " + titleText + "_Default");
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            image.sprite = sprite;
        }

        lockLevel.SetActive(isLock);
    }

    public Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

    public void OnClick()
    {
        AudioManager.instance.PlaySound2D("Click");
        if (!isLock)
        {
            string sceneName = "Level " + levelText.text;
            LoadLevel(sceneName);
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void LoadLevelData()
    {
        string loadJson = PlayerPrefs.GetString("Level " + levelText.text, "");

        if (loadJson == "")
        {
            LevelData levelData;
            levelData = (levelText.text == "1 - 1") ? new LevelData(false) : new LevelData(true);

            string saveString = JsonUtility.ToJson(levelData);
            PlayerPrefs.SetString("Level " + levelText.text, saveString);
        }
        else
        {
            LevelData loadData;
            loadData = JsonUtility.FromJson<LevelData>(loadJson);
            isLock = loadData.isLock;
        }
    }
}
