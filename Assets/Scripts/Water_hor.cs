using System.Collections;
using UnityEngine;

public class Water_hor : MonoBehaviour
{
    private Mesh mesh = null;
    public float leftLimit;
    public float rightLimit;
    public float height = 0.3f;
    public Color upColor;
    public Color downColor;
    private Vector3[] curVertices = null;
    private const float inter = 0.05f;
    private const int sidePoint = 3;
    private int length = 0;
    private int lrRadian = 0;
    private GenericPool<ParticleSystem> particlePool;

    /// <summary>
    /// </summary>
    /// <param name="lrRadian">3 两边弧度 1 左边弧度 2 右边弧度 0 没有弧度 </param>
    public void Init(float left, float right, int lrRadian, GenericPool<ParticleSystem> particlePool = null)
    {
        leftLimit = left;
        rightLimit = right;
        this.lrRadian = lrRadian;
        CreatMesh();
        GetComponent<BoxCollider2D>().size = new Vector2(right - left + 0.2f, 0.3f);
        this.particlePool = particlePool;
    }
    [ContextMenu("CreatMesh")]
    private void CreatMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        leftLimit = Mathf.FloorToInt(leftLimit * 100) / 100f;
        rightLimit = Mathf.FloorToInt(rightLimit * 100) / 100f;
        length = Mathf.FloorToInt((rightLimit - leftLimit) / inter) + 1;
        if (length <= 0) return;
        length += 4;

        curVertices = new Vector3[length * 2];
        Color[] colors = new Color[length * 2];
        Vector2[] uvs = new Vector2[length * 2];
        for (int i = 0; i < length; i++)
        {
            float vertice_xx = leftLimit + (i - 2) * inter;
            curVertices[i * 2] = new Vector3(vertice_xx, 0);
            curVertices[i * 2 + 1] = new Vector3(vertice_xx, -height);
            colors[i * 2] = upColor;
            colors[i * 2 + 1] = downColor;
            uvs[i * 2] = new Vector2(i / length, 1);
            uvs[i * 2 + 1] = new Vector2(i / length, 0);
        }
        int[] triangles = new int[length * 6 - 6];
        for (int i = 0; i < length - 1; i++)
        {
            triangles[i * 6] = i * 2;
            triangles[i * 6 + 1] = i * 2 + 1;
            triangles[i * 6 + 2] = i * 2 + 2;
            triangles[i * 6 + 3] = i * 2 + 2;
            triangles[i * 6 + 4] = i * 2 + 1;
            triangles[i * 6 + 5] = i * 2 + 3;
        }

        mesh.vertices = curVertices;
        mesh.colors = colors;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        preHeights = new float[length];
        nextHeights = new float[length];
        if ((lrRadian & 1) != 0)
        {
            curVertices[0].y = -height / 3;
            curVertices[2].y = -height / 12;
        }
        if ((lrRadian & 2) != 0)
        {
            curVertices[2 * length - 2].y = -height / 3;
            curVertices[2 * length - 4].y = -height / 12;
        }

        InitMk();

        GetComponent<MeshRenderer>().sortingOrder = 2;
    }

    private void FixedUpdate()
    {
        UpdateHeight();
    }

    [Header("force")]
    public float spreadSp;
    public float damping;
    private float mkx, mky, mkz;
    private float[] preHeights;
    private float[] nextHeights;
    private void InitMk()
    {
        float d = damping * Time.fixedDeltaTime + 2;
        float ed = spreadSp * Time.fixedDeltaTime / inter;
        ed *= ed;
        mkx = (damping * Time.fixedDeltaTime - 2) / d;
        mky = (4 - 4 * ed) / d;
        mkz = 2 * ed / d;
    }
    public void AddForce(Vector2 pos, float f = -0.04f)
    {
        if (curVertices == null) return;
        int site = Mathf.RoundToInt((pos.x - transform.position.x - leftLimit) / inter) + 2;
        if (site < 0 || site >= length) return;
        if (site <= 2 || site >= length - 3)
        {
            AddForce(site, f);
            return;
        }
        if (site < 5 || site > length - 6)
        {
            AddForce(site, f);
            AddForce(site + 1, -f / 2);
            AddForce(site - 1, -f / 2);
            return;
        }
        AddForce(site, f);
        AddForce(site - 1, f / 2);
        AddForce(site + 1, f / 2);
        AddForce(site + 3, -f);
        AddForce(site - 3, -f);
        if (particlePool != null)
        {
            var particle = particlePool.GetT();
            particle.transform.position = new Vector3(pos.x, transform.position.y);
            particle.Play();
            StartCoroutine(RecyleParticle(particle));
        }
    }
    private WaitForSeconds wait1sce = new WaitForSeconds(1);
    private IEnumerator RecyleParticle(ParticleSystem particle)
    {
        yield return wait1sce;
        particlePool.EnPool(particle);
    }
    private void AddForce(int x, float f)
    {
        curVertices[x * 2].y += f;
    }
    private void UpdateHeight()
    {
        nextHeights[2] = mkx * preHeights[2] + mky * curVertices[4].y + mkz * curVertices[6].y;
        int lasti = length - 3;
        nextHeights[lasti] = mkx * preHeights[lasti] + mky * curVertices[lasti * 2].y + mkz * curVertices[lasti * 2 - 2].y;
        for (int i = 3; i < lasti; i++)
        {
            nextHeights[i] = mkx * preHeights[i] + mky * curVertices[i * 2].y + mkz * (curVertices[i * 2 - 2].y + curVertices[i * 2 + 2].y);
        }
        for (int i = 2; i < length - 2; i++)
        {
            preHeights[i] = curVertices[i * 2].y;
            curVertices[i * 2].y = nextHeights[i];
        }
        if ((lrRadian & 1) != 0)
        {
            curVertices[2].y = curVertices[2].y * 0.5f + 0.5f * (-height / 12 + curVertices[4].y * 0.5f);
            curVertices[0].y = curVertices[2].y * 0.5f + 0.5f * (-height / 3 + curVertices[4].y * 0.25f);
        }
        else
        {
            curVertices[2].y = curVertices[2].y * 0.5f + curVertices[4].y * 0.25f;
            curVertices[0].y = curVertices[0].y * 0.5f + curVertices[4].y * 0.125f;
        }
        int sidesite = length * 2 - 6;
        if ((lrRadian & 2) != 0)
        {
            curVertices[length * 2 - 4].y = curVertices[length * 2 - 4].y * 0.5f + 0.5f * (-height / 12 + curVertices[sidesite].y * 0.5f);
            curVertices[length * 2 - 2].y = curVertices[length * 2 - 2].y * 0.5f + 0.5f * (-height / 3 + curVertices[sidesite].y * 0.25f);
        }
        else
        {
            curVertices[length * 2 - 4].y = curVertices[length * 2 - 4].y * 0.5f + curVertices[sidesite].y * 0.25f;
            curVertices[length * 2 - 2].y = curVertices[length * 2 - 2].y * 0.5f + curVertices[sidesite].y * 0.125f;
        }
        mesh.vertices = curVertices;
    }
}