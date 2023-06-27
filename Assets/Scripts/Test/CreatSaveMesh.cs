using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class CreatSaveMesh : MonoBehaviour
{
    public Texture sp1;
    public Texture sp2;
    public MeshFilter mf;
    public MeshRenderer mr;
    public Material rendermat;
    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        CreatMesh();
        SaveMesh();
    }
    private void SaveMesh()
    {
        var bound = mf.mesh.bounds;
        Vector2Int size = new Vector2Int(Mathf.CeilToInt(bound.size.x * 100), Mathf.CeilToInt(bound.size.y * 100));
        RenderTexture rt = RenderTexture.GetTemporary(size.x, size.y);

        int pi = 0;
        var vis = mf.mesh.vertices;
        int matpropid = Shader.PropertyToID("_V4");
        //var mat = mr.materials[1];
        //pi += 4;
        foreach (var mat in mr.materials) 
        {
            var rt0 = mat.mainTexture;
            var rt0size = new Vector2(vis[pi + 2].x - vis[pi].x, vis[pi + 1].y - vis[pi + 2].y);
            var rt0pos = new Vector2((vis[pi + 2].x + vis[pi].x) / 2 - bound.min.x, (vis[pi + 1].y + vis[pi + 2].y) / 2 - bound.min.y) - rt0size / 2;
            var rt0scale = new Vector2(bound.size.x / rt0size.x, bound.size.y / rt0size.y);
            var rt0offset = -new Vector2(rt0pos.x / rt0size.x, rt0pos.y / rt0size.y);
            var v4 = new Vector4(rt0scale.x, rt0scale.y, rt0offset.x, rt0offset.y);
            var newmat = new Material(rendermat);
            newmat.SetVector(matpropid, v4);
            Graphics.Blit(rt0, rt, newmat);
            pi += 4;
            Debug.Log(rt0size + " " + rt0pos);
            Debug.Log(rt0.name + " " + v4);
        }
        SaveTexture(rt);
    }
    private void SaveTexture(RenderTexture rt)
    {
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] databytes = png.EncodeToPNG();
        string path = Application.dataPath + "/savepng.png";
        Debug.Log(path);
        FileStream fs = File.Open(path, FileMode.OpenOrCreate);
        fs.Write(databytes, 0, databytes.Length);
        fs.Flush();
        fs.Close();
        png = null;
        RenderTexture.active = null;
    }
    private void CreatMesh()
    {
        List<Vector3> points = new List<Vector3>()
        {
            new Vector3(-0.5f,0.5f),
            new Vector3(0.5f,0.5f),
            new Vector3(0.5f,-0.5f),
            new Vector3(-0.5f,-0.5f),
            new Vector3(-1,0),
            new Vector3(0,0),
            new Vector3(0,-1),
            new Vector3(-1,-1),
        };
        List<int> tris = new List<int>()
        {
            0,1,3,2,3,1,4,5,7,6,7,5,
        };
        List<Vector2> uvs = new List<Vector2>()
        {
            Vector2.up,
            Vector2.one,
            Vector2.right,
            Vector2.zero,
            Vector2.up,
            Vector2.one,
            Vector2.right,
            Vector2.zero,
        };
        Mesh cmesh = new Mesh()
        {
            vertices = points.ToArray(),
            uv = uvs.ToArray(),
            triangles = tris.ToArray(),
        };
        cmesh.subMeshCount = 2;
        cmesh.SetSubMesh(0, new UnityEngine.Rendering.SubMeshDescriptor(0, 6));
        cmesh.SetSubMesh(1, new UnityEngine.Rendering.SubMeshDescriptor(6, 6));
        var shader = Shader.Find("Sprites/Default");
        mr.materials = new Material[]
        {
            new Material(shader),
            new Material(shader),
        };
        mr.materials[0].mainTexture = sp1;
        mr.materials[1].mainTexture = sp2;
        mf.mesh = cmesh;
    }
}
