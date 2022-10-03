using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour
{
    /// <summary>
    /// 曲线最大点数
    /// </summary>
    public int MAX_FRAG_CNT = 100;

    private LineRenderer lineRenderer;

    private List<Vector3> posList = new List<Vector3>();

    public Transform startPoint;
    public Transform endPoint;

    private void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    // 三阶贝塞尔曲线
    private Vector3 cubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 aa = a + (b - a) * t;
        Vector3 bb = b + (c - b) * t;
        Vector3 cc = c + (d - c) * t;

        Vector3 aaa = aa + (bb - aa) * t;
        Vector3 bbb = bb + (cc - bb) * t;
        return aaa + (bbb - aaa) * t;
    }

    /// <summary>
    /// 绘制曲线
    /// </summary>
    /// <param name="fromPos">起始坐标</param>
    /// <param name="ctrlPoint1">控制点1</param>
    /// <param name="ctrlPoint2">控制点2</param>
    /// <param name="toPos">目标坐标</param>
    //public void DrawRay(Vector3 fromPos, Vector3 ctrlPoint1, Vector3 ctrlPoint2, Vector3 toPos)
    //{
    //    posList.Clear();
    //    for (int i = 0; i <= MAX_FRAG_CNT; ++i)
    //    {
    //        posList.Add(cubicBezier(fromPos, ctrlPoint1, ctrlPoint2, toPos, (float)i / MAX_FRAG_CNT));
    //        lineRenderer.positionCount = posList.Count;
    //    }
    //    lineRenderer.SetPositions(posList.ToArray());
    //}

    public IEnumerator DrawRay(Vector3 earthPos, Vector3 fromPos, Vector3 ctrlPoint1, Vector3 ctrlPoint2, Vector3 toPos)
    {

        startPoint.gameObject.SetActive(true);
        startPoint.forward=fromPos- earthPos;
        startPoint.localPosition = fromPos + startPoint.forward * 0.01f;

        for(int i = 0; i <= MAX_FRAG_CNT; ++i)
        {
            posList.Clear();
            for(int j = 0; j <= i; ++j)
            {
                posList.Add(cubicBezier(fromPos,ctrlPoint1,ctrlPoint2,toPos,(float)j/MAX_FRAG_CNT));
            }
            lineRenderer.positionCount=posList.Count;
            lineRenderer.SetPositions(posList.ToArray());
            yield return new WaitForSeconds(0.02f);
        }

        endPoint.gameObject.SetActive(true);
        endPoint.forward =  toPos- earthPos;
        endPoint.localPosition = toPos + endPoint.forward * 0.01f;
        yield return new WaitForSeconds(2);
        startPoint.gameObject.SetActive(false);

        for (int i = 0; i <= MAX_FRAG_CNT; ++i)
        {
            posList.Clear();
            for (int j = i; j <= MAX_FRAG_CNT; ++j)
            {
                posList.Add(cubicBezier(fromPos, ctrlPoint1, ctrlPoint2, toPos, (float)j / MAX_FRAG_CNT));
            }
            lineRenderer.positionCount = posList.Count;
            lineRenderer.SetPositions(posList.ToArray());
            yield return new WaitForSeconds(0.001f);
        }

        Destroy(gameObject);
    }
}


