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
    public int levelIndex = 1;
    public float bgmVolume = 1;
    public float audVolume = 1;
}
