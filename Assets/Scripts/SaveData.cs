using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveData
{
    private static SaveDataPart data = null;
    private static readonly string path = Application.persistentDataPath + "/sd.save";
    public static SaveDataPart Data
    {
        get
        {
            if (data == null)
            {
                Load();
            }
            return data;
        }
    }
    public static void Save()
    {
        //创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //创建一个文件流
        FileStream fileStream = File.Create(path);
        //用二进制格式化程序的序列化方法来序列化Save对象,参数：创建的文件流和需要序列化的对象
        bf.Serialize(fileStream, data);
        //关闭流
        fileStream.Close();
    }
    public static void Load()
    {
        if (File.Exists(path))
        {
            //反序列化过程
            //创建一个二进制格式化程序
            BinaryFormatter bf = new BinaryFormatter();
            //打开一个文件流
            FileStream fileStream = File.Open(path, FileMode.Open);
            //调用格式化程序的反序列化方法，将文件流转换为一个Save对象
            data = (SaveDataPart)bf.Deserialize(fileStream);
            //关闭文件流
            fileStream.Close();
            if (data.version < SaveDataPart.VERSION)
            {
                if (data.version < 2)
                {
                    data.remererLightSize = 1;
                    data.remererLightRandColor = false;
                }
                data.version = SaveDataPart.VERSION;
                Save();
            }
        }
        else
        {
            data = new SaveDataPart();
            Save();
        }
    }
    public static void Delete()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
[System.Serializable]
public class SaveDataPart
{
    public const int VERSION = 2;

    public int version = VERSION;
    //version 0
    //version 1
    public int levelIndex = 0;
    public float bgmVolume = 1;
    public float audVolume = 0.6f;
    public float playerLightSize = 1;
    public int rememerCount = 16;
    public bool hadHurt = true;
    public bool hadChooseLevel = false;
    public float pledgeSpeed = 1;
    public bool jumpPledge = false;
    //version 2
    public float remererLightSize = 1;
    public bool remererLightRandColor = false;
}
public static class SaveData<T> where T : new()
{
    private static T data;
    private static readonly string path = Application.persistentDataPath + $"/{typeof(T)}.save";
    public static T Data
    {
        get
        {
            if(data == null)
            {
                Load();
            }
            return data;
        }
    }
    public static void Save()
    {
        //创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //创建一个文件流
        FileStream fileStream = File.Create(path);
        //用二进制格式化程序的序列化方法来序列化Save对象,参数：创建的文件流和需要序列化的对象
        bf.Serialize(fileStream, data);
        //关闭流
        fileStream.Close();
    }
    public static void Load()
    {
        if (File.Exists(path))
        {
            //反序列化过程
            //创建一个二进制格式化程序
            BinaryFormatter bf = new BinaryFormatter();
            //打开一个文件流
            FileStream fileStream = File.Open(path, FileMode.Open);
            //调用格式化程序的反序列化方法，将文件流转换为一个Save对象
            data = (T)bf.Deserialize(fileStream);
            //关闭文件流
            fileStream.Close();
        }
        else
        {
            data = new();
            Save();
        }
    }
    public static void Reset()
    {
        data = new T();
        Save();
    }
    public static void Delete()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}