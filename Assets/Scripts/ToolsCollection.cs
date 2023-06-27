using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GenericTools
{
    private static DelayComponent delayComponent = null;
    private class DelayComponent : MonoBehaviour { }
    private static DelayComponent Delay
    {
        get
        {
            if (delayComponent == null) CreatDelayGO();
            return delayComponent;
        }
    }
    private static void CreatDelayGO()
    {
        var go = new GameObject("delaygo", typeof(DelayComponent));
        go.transform.parent = null;
        GameObject.DontDestroyOnLoad(go);
        delayComponent = go.GetComponent<DelayComponent>();
    }
    public static Coroutine DelayFun(float delayTime, Action<float> delayingFun, Action callBack)
    {
        return Delay.StartCoroutine(DelayFun_Cor(delayTime, delayingFun, callBack));
    }
    public static Coroutine DelayFun(int frame, Action callback)
    {
        return Delay.StartCoroutine(DelayFun_Cor(frame, callback));
    }
    public static IEnumerator DelayFun_Cor(float delayTime, Action<float> delayingFun, Action callBack)
    {
        float timer = 0;
        if (delayTime > 0)
        {
            if (delayingFun == null) yield return new WaitForSeconds(delayTime);
            else
            {
                while (timer < delayTime)
                {
                    delayingFun.Invoke(timer / delayTime);
                    yield return null;
                    timer += Time.deltaTime;
                }
            }
        }
        if (delayingFun != null)
        {
            delayingFun(1);
        }
        if (callBack != null)
        {
            callBack();
        }
    }
    public static IEnumerator DelayFun_Cor(int frame, Action callback)
    {
        while (frame-- > 0)
        {
            yield return null;
        }
        if (callback != null) callback();
    }
    /// <summary>
    /// 不加参数停止组件上的所有协程 加了停一个
    /// </summary>
    /// <param name="cor"></param>
    public static void StopCoroutine(Coroutine cor = null)
    {
        if (cor == null) Delay.StopAllCoroutines();
        else Delay.StopCoroutine(cor);
    }
    public static Coroutine StartCoroutine(IEnumerator routine)
    {
        return Delay.StartCoroutine(routine);
    }
    /// <summary> 梯形的面积应该是1 side<0.5f </summary>
    public static float TrapezoidF(float f, float side)
    {
        if (f < side) return f * f / 2 / side / (1 - side);
        else if (f > 1 - side) return 1 - (1 - f) * (1 - f) / 2 / side / (1 - side);
        else return (f - side / 2) / (1 - side);
    }
    public static float SShapeF(float f)
    {
        return f * f * (-2 * f + 3);
    }

    #region static cache  跨场景数据或者没地方放的数据
    private readonly static Dictionary<string, object> cacheSth = new Dictionary<string, object>();
    public static void CacheSth(string key, object sth)
    {
        if (cacheSth.ContainsKey(key)) cacheSth[key] = sth;
        else cacheSth.Add(key, sth);
    }
    public static object GetCache(string key)
    {
        if (cacheSth.ContainsKey(key)) return cacheSth[key];
        else return null;
    }
    /// <summary> 取出cache并删掉chache </summary>
    public static object PopCache(string key)
    {
        if (cacheSth.ContainsKey(key))
        {
            object res = cacheSth[key];
            cacheSth.Remove(key);
            return res;
        }
        else return null;
    }
    public static void ClearCache()
    {
        cacheSth.Clear();
    }
    #endregion

    /// <summary>
    /// 通过反射获取一个类下面的所有子类
    /// </summary>
    public static Type[] GetChildClass(Type parentClass)
    {
        return parentClass.Assembly.GetTypes().Where(t => { return t.IsClass && t.BaseType == parentClass; }).ToArray();
    }

    public static void ColorLog(Color col, params object[] sths)
    {
        StringBuilder sb = new StringBuilder();
        string col_html = ColorUtility.ToHtmlStringRGB(col);
        sb.AppendFormat("<color=#{0}>", col_html);
        foreach (var item in sths)
        {
            sb.Append(item.ToString());
        }
        sb.Append("</color>");
        Debug.Log(sb.ToString());
    }

    #region 扩展函数
    /// <summary>
    /// 在从小到大的有序数组中 寻找第一个大于等于的数字 函数本身不判断是否有序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static int GreaterOrEqual<T>(this IList<T> array, T item) where T : IComparable
    {
        int l = 0, r = array.Count;
        while (l < r)
        {
            int mid = l + (r - l) / 2;
            int compare = array[mid].CompareTo(item);
            if (compare == 0) return mid;
            else if (compare < 0) l = mid + 1;
            else r = mid;
        }
        return l;
    }
    public static void Swap<T>(this IList<T> array, int l, int r)
    {
        if (l < 0 || r < 0 || l >= array.Count || r >= array.Count) throw new Exception("swap toolscollection error");
        T t = array[l];
        array[l] = array[r];
        array[r] = t;
    }
    /// <summary>
    /// 存在则替换 不存在添加
    /// </summary>
    public static void ReAdd<Key, Val>(this Dictionary<Key, Val> dic, Key key, Val val)
    {
        //如果val是struct 那tryget就有可能出错 也可以用where限制一下
        if (dic.ContainsKey(key)) dic[key] = val;
        else dic.Add(key, val);
    }
    public static void TurnBack(this Transform trs)
    {
        Vector3 v3 = trs.localScale;
        v3.x *= -1;
        trs.localScale = v3;
    }
    public static int ScaleToLRDir(this Transform trs)
    {
        Vector3 v3 = trs.localScale;
        return v3.x > 0 ? 1 : -1;
    }

    /// <summary>
    /// cam.orthographicSize * cam.aspect;
    /// </summary>
    /// <param name="cam"></param>
    /// <returns></returns>
    public static float HalfWidth(this Camera cam)
    {
        return cam.orthographicSize * cam.aspect;
    }
    public static Vector3 RightUp(this Camera cam)
    {
        return new Vector3(cam.orthographicSize * cam.aspect, cam.orthographicSize, 0);
    }
    public static Vector3 FullScreen(this Camera cam)
    {
        float scrmul = cam.orthographicSize * 2;
        return new Vector3(scrmul * cam.aspect, scrmul, 1);
    }

    #endregion

    #region File
    /// <summary>
    /// utf-8 编码下储存文件
    /// </summary>
    public static void SaveFile(string path, string file)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        byte[] bytes = new UTF8Encoding().GetBytes(file);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
        Debug.LogFormat("write success {0}", path);
    }
    /// <summary>
    /// utf-8 编码下读取解码文件
    /// </summary>
    public static string ReadFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("read file no path:" + path);
            return null;
        }
        FileStream file = File.Open(path, FileMode.Open);
        StreamReader sr = new StreamReader(file, System.Text.Encoding.UTF8);
        var res = sr.ReadToEnd();
        file.Close();
        Debug.Log("read success " + path);
        return res;
    }
    public static Sprite ReadSprite(string path)
    {
        string pathwithformat = null;
        if (File.Exists(path + ".jpg"))
        {
            pathwithformat = path + ".jpg";
        }
        else if (File.Exists(path + ".png"))
        {
            pathwithformat = path + ".png";
        }
        if (pathwithformat == null)
        {
            throw new Exception("read file no path:" + path);
        }

        FileStream file = File.Open(pathwithformat, FileMode.Open);
        byte[] imgByte = new byte[file.Length];
        file.Read(imgByte, 0, imgByte.Length);
        file.Close();

        Texture2D t2d = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        t2d.LoadImage(imgByte);
        Debug.Log("read success " + path);
        Vector2 v2 = new Vector2(t2d.width, t2d.height);
        return Sprite.Create(t2d, new Rect(Vector2.zero, v2), Vector2.one / 2);
    }
    /// <summary>
    /// 拿到一个路径下的所有filetype文件
    /// </summary>
    public static string[] GetFilesName(string path, string filetype = "txt")
    {
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] files = di.GetFiles(string.Format("*.{0}", filetype));
        return files.Select(item => item.Name).ToArray();
    }
    public static void TraverseTxtFiles(string path, Func<string, string> deal)
    {
        if (deal == null) return;
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] files = di.GetFiles("*.txt");
        foreach (var item in files)
        {
            SaveFile(item.FullName, deal(ReadFile(item.FullName)));
        }
    }
    public static void RenameFile(string oldpath, string newpath)
    {
        if (!File.Exists(oldpath)) return;
        File.Move(oldpath, newpath);
    }
    public static void DeleteFile(string path)
    {
        File.Delete(path);
        Debug.Log("delete " + path);
    }
    #endregion
}
public enum GenericSign
{
    backMenu,
    level_swear,
    level_swear_end,
    nextLevel,
    playerDie,
    startLevel,
    loadLevel,
    loadFlower,

}
public class GenericPool<T>
{
    private int maxCount;
    private Stack<T> poolSt;
    private Func<T> creatT;
    private Action<T> enPool;
    private Action<T> dePool;
    public GenericPool(Func<T> creatT, Action<T> enPool, Action<T> dePool, int maxCount = 128)
    {
        this.creatT = creatT;
        this.enPool = enPool;
        this.dePool = dePool;
        this.maxCount = maxCount;
        poolSt = new Stack<T>(maxCount);
    }
    public T GetT()
    {
        if (poolSt.Count > 0)
        {
            var res = poolSt.Pop();
            if (dePool != null) dePool(res);
            return res;
        }
        else
        {
            return creatT != null ? creatT() : default(T);
        }
    }
    public void EnPool(T t)
    {
        if (enPool != null) enPool(t);
        if (poolSt.Count != maxCount) poolSt.Push(t);
    }
    public void Clear()
    {
        creatT = null;
        enPool = null;
        dePool = null;
        poolSt.Clear();
    }

    private static readonly Dictionary<GenericSign, GenericPool<T>> Dic_SignPools = new Dictionary<GenericSign, GenericPool<T>>();
    private static readonly Dictionary<GenericSign, int> Dic_SignPoolCount = new Dictionary<GenericSign, int>();
    /// <summary>
    /// 请务必记得unlink
    /// </summary>
    /// <param name="sign"></param>
    /// <param name="creat"></param>
    /// <returns></returns>
    public static GenericPool<T> LinkSignPool(GenericSign sign, Func<GenericPool<T>> creat)
    {
        if (Dic_SignPools.ContainsKey(sign))
        {
            Dic_SignPoolCount[sign]++;
            return Dic_SignPools[sign];
        }
        else
        {
            var res = creat();
            Dic_SignPools.Add(sign, res);
            Dic_SignPoolCount.Add(sign, 1);
            return res;
        }
    }
    public static void UnLinkSignPool(GenericSign sign)
    {
        if (Dic_SignPoolCount.ContainsKey(sign))
        {
            Dic_SignPoolCount[sign]--;
            if (Dic_SignPoolCount[sign] == 0)
            {
                Dic_SignPools[sign].Clear();
                Dic_SignPools.Remove(sign);
                Dic_SignPoolCount.Remove(sign);
            }
        }
        else Debug.LogError("no find sign pool");
    }
}

public class GenericMsg<T>
{
    public delegate Recursion Recursion(GenericSign sign, Action<T> reveiver);
    private static readonly Dictionary<GenericSign, Action<T>> dic_Receivers = new Dictionary<GenericSign, Action<T>>();
    private static readonly Dictionary<GenericSign, int> dic_ReceiverCount = new Dictionary<GenericSign, int>();

    public static void Trigger(GenericSign sign, T data)
    {
        if (dic_Receivers.ContainsKey(sign))
        {
            dic_Receivers[sign].Invoke(data);
        }
    }
    public static Recursion AddReceiver(GenericSign sign, System.Action<T> receiver)
    {
        if (dic_Receivers.ContainsKey(sign))
        {
            dic_Receivers[sign] += receiver;
            dic_ReceiverCount[sign] += 1;
        }
        else
        {
            dic_Receivers.Add(sign, receiver);
            dic_ReceiverCount.Add(sign, 1);
        }
        return AddReceiver;
    }
    public static Recursion DelReceiver(GenericSign sign, System.Action<T> receiver)
    {
        if (dic_Receivers.ContainsKey(sign))
        {
            dic_Receivers[sign] -= receiver;
            dic_ReceiverCount[sign] -= 1;
            if (dic_Receivers[sign] == null)
            {
                dic_Receivers.Remove(sign);
                dic_ReceiverCount.Remove(sign);
            }
        }
        return DelReceiver;
    }

    public static int GetReceiverCount(GenericSign sign)
    {
        if (dic_ReceiverCount.ContainsKey(sign))
        {
            return dic_ReceiverCount[sign];
        }
        return 0;
    }
}
public class GenericMsg
{
    public delegate Recursion Recursion(GenericSign sign, Action reveiver);
    private static readonly Dictionary<GenericSign, Action> dic_Receivers = new Dictionary<GenericSign, Action>();
    private static readonly Dictionary<GenericSign, int> dic_ReceiverCount = new Dictionary<GenericSign, int>();

    public static void Trigger(GenericSign sign)
    {
        if (dic_Receivers.ContainsKey(sign))
        {
            dic_Receivers[sign].Invoke();
        }
    }
    public static Recursion AddReceiver(GenericSign sign, Action receiver)
    {
        if (dic_Receivers.ContainsKey(sign))
        {
            dic_Receivers[sign] += receiver;
            dic_ReceiverCount[sign] += 1;
        }
        else
        {
            dic_Receivers.Add(sign, receiver);
            dic_ReceiverCount.Add(sign, 1);
        }
        return AddReceiver;
    }
    public static Recursion DelReceiver(GenericSign sign, Action receiver)
    {
        if (dic_Receivers.ContainsKey(sign))
        {
            dic_Receivers[sign] -= receiver;
            dic_ReceiverCount[sign] -= 1;
            if (dic_Receivers[sign] == null)
            {
                dic_Receivers.Remove(sign);
                dic_ReceiverCount.Remove(sign);
            }
        }
        return DelReceiver;
    }

    public static int GetReceiverCount(GenericSign sign)
    {
        if (dic_ReceiverCount.ContainsKey(sign))
        {
            return dic_ReceiverCount[sign];
        }
        return 0;
    }

}
//https://blog.csdn.net/mkr67n/article/details/126277253 unity ondestroy 有不触发的坑


public class GenericChannel<T> where T : class
{
    private GenericChannel(T data) { this.data = data; }

    private T data;
    private System.Action<T> Channel_start;
    private System.Action<T> Channel_flow;
    private System.Action<T> Channel_end;

    private static GenericChannel<T> instance = null;
    public static void CreatChannel(T data)
    {
        instance = new GenericChannel<T>(data);
    }
    public static void Clear()
    {
        instance = null;
    }
    public static T Data
    {
        get
        {
            return instance.data;
        }
    }

    public static void TriggerS()
    {
        if (instance != null && instance.Channel_start != null)
        {
            instance.Channel_start(instance.data);
        }
    }
    public static void Trigger()
    {
        if (instance != null && instance.Channel_flow != null)
        {
            instance.Channel_flow.Invoke(instance.data);
        }
    }
    public static void TriggerE()
    {
        if (instance != null && instance.Channel_flow != null)
        {
            instance.Channel_end.Invoke(instance.data);
        }
    }
    public static void AddReceiver(System.Action<T> cs, System.Action<T> cf, System.Action<T> ce)
    {
        if (instance != null)
        {
            instance.Channel_start += cs;
            instance.Channel_flow += cf;
            instance.Channel_end += ce;
        }
    }
    public static void DelReceiver(System.Action<T> cs, System.Action<T> cf, System.Action<T> ce)
    {
        if (instance != null)
        {
            instance.Channel_start -= cs;
            instance.Channel_flow -= cf;
            instance.Channel_end -= ce;
        }
    }
}