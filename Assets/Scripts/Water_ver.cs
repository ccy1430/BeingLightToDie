using UnityEngine;

public class Water_ver : MonoBehaviour
{
    public int sortOrder = 20;
    private Vector3 fallPos;
    
    void Start()
    {
        GetComponent<MeshRenderer>().material.SetFloat("_Seed", Random.Range(0f, 10000f));
        GetComponent<MeshRenderer>().sortingOrder = sortOrder;
    }
    public void Init(float height)
    {
        Vector3 v3 = transform.localScale;
        v3.y = height;
        transform.localScale = v3;
        fallPos = transform.position;
        fallPos.y -= height / 2;
        GenericTools.DelayFun(1, GetLandHorWater);
    }
    private void GetLandHorWater()
    {
        waterhor = WaterMgr.Instance.GetPosWater(fallPos);
    }

    private Water_hor waterhor = null;
    private int count = 2;
    void FixedUpdate()
    {
        if (waterhor == null) return;
        count--;
        if (count == 0)
        {
            waterhor.AddForce(fallPos);
            count = Random.Range(2, 6);
        }
    }
}
