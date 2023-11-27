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
        //����һ�������Ƹ�ʽ������
        BinaryFormatter bf = new BinaryFormatter();
        //����һ���ļ���
        FileStream fileStream = File.Create(path);
        //�ö����Ƹ�ʽ����������л����������л�Save����,�������������ļ�������Ҫ���л��Ķ���
        bf.Serialize(fileStream, data);
        //�ر���
        fileStream.Close();
    }
    public static void Load()
    {
        if (File.Exists(path))
        {
            //�����л�����
            //����һ�������Ƹ�ʽ������
            BinaryFormatter bf = new BinaryFormatter();
            //��һ���ļ���
            FileStream fileStream = File.Open(path, FileMode.Open);
            //���ø�ʽ������ķ����л����������ļ���ת��Ϊһ��Save����
            data = (SaveDataPart)bf.Deserialize(fileStream);
            //�ر��ļ���
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
        //����һ�������Ƹ�ʽ������
        BinaryFormatter bf = new BinaryFormatter();
        //����һ���ļ���
        FileStream fileStream = File.Create(path);
        //�ö����Ƹ�ʽ����������л����������л�Save����,�������������ļ�������Ҫ���л��Ķ���
        bf.Serialize(fileStream, data);
        //�ر���
        fileStream.Close();
    }
    public static void Load()
    {
        if (File.Exists(path))
        {
            //�����л�����
            //����һ�������Ƹ�ʽ������
            BinaryFormatter bf = new BinaryFormatter();
            //��һ���ļ���
            FileStream fileStream = File.Open(path, FileMode.Open);
            //���ø�ʽ������ķ����л����������ļ���ת��Ϊһ��Save����
            data = (T)bf.Deserialize(fileStream);
            //�ر��ļ���
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