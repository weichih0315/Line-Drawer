using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour {

    //初始化路徑，實際應該用Application.persistentDataPath，
    //因為使用dataPath就是Asset文件不能讀寫操作
    //定義圖片路徑
    //Ex: Application.dataPath + "/Resources/FullScreenShot.png";

    public Camera camera;

    public static ScreenShot instance;

    private void Awake()
    {
        instance = this;
    }

    public void TakeShopByFullScreen(string fullShotPath)
    {
        StartCoroutine(CaptureByRect(new Rect(0,0,Screen.width - 1, Screen.height - 1), fullShotPath));
    }

    public void TakeShopByPartScreen(Rect rect,string partShotPath)
    {
        StartCoroutine(CaptureByRect(rect, partShotPath));
    }

    public void TakeShopByOtherCameraScreen(string filePath)
    {
        StartCoroutine(CaptureByCamera(camera, new Rect(0, 0, Screen.width - 1, Screen.height - 1), filePath));
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

    private IEnumerator CaptureByRect(Rect rect, string filePath)
    {
        //等待渲染程式结束
        yield return new WaitForEndOfFrame();
        //初始化Texture2D, 大小可以根據需求更改
        Texture2D texture2D = new Texture2D((int)rect.width, (int)rect.height,
                                 TextureFormat.RGB24, false);
        //讀取屏幕像素信息並存儲為纹理數據
        texture2D.ReadPixels(rect, 0, 0, false);
        //更新貼圖的mipmap  //Mipmap是一種貼圖渲染的常用技術
        texture2D.Apply();
        //將讀到的貼圖轉換成byte格式
        byte[] bytes = texture2D.EncodeToPNG();
        //保存
        System.IO.File.WriteAllBytes(filePath, bytes);
    }

    private IEnumerator CaptureByCamera(Camera camera, Rect rect, string filePath)
    {
        //等待渲染程式结束
        yield return new WaitForEndOfFrame();
        //初始化RenderTexture   深度只能是【0、16、24】截不全圖請修改
        RenderTexture render = new RenderTexture((int)rect.width, (int)rect.height, 16);
        //設置相機的渲染目標
        camera.targetTexture = render;
        //开始渲染
        camera.Render();
        //激活渲染贴圖讀取信息
        RenderTexture.active = render;
        Texture2D mTexture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        //讀取屏幕像素信息並存儲為纹理數據
        mTexture.ReadPixels(rect, 0, 0, false);
        //更新貼圖的mipmap  //Mipmap是一種貼圖渲染的常用技術
        mTexture.Apply();
        //釋放相機，銷毀渲染貼圖
        camera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(render);
        //將讀到的貼圖轉換成byte格式
        byte[] bytes = mTexture.EncodeToPNG();
        //保存
        System.IO.File.WriteAllBytes(filePath, bytes);

        GameUI.instance.Win();
    }
}
