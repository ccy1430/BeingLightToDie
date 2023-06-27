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
