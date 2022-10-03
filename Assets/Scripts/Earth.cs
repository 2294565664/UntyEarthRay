using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    private Transform selfTrans;
    public Line line;

    private void Start()
    {
        selfTrans = this.transform;
        StartCoroutine(FireLine());
    }

    private void Update()
    {
        selfTrans.Rotate(Vector3.up * Time.deltaTime, Space.Self);//地球自转
    }

    IEnumerator FireLine()
    {
        this.line.gameObject.SetActive(false);
        while(true)
        {
            //循环生成曲线
            var line = Instantiate(this.line);
            line.gameObject.SetActive(true);
            line.transform.SetParent(selfTrans);

            //半径
            var radius = selfTrans.localScale.x / 2f;

            //随机一个起始点
            var from = selfTrans.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * radius;

            //随机一个终点
            var to = selfTrans.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * radius;
            var center = (from + to) / 2f;

            var ctrlPoint1 = from + (center - selfTrans.position).normalized * (from - to).magnitude * 0.6f;
            var ctrtlPoint2 = to + (center - selfTrans.position).normalized * (from - to).magnitude * 0.6f;

            StartCoroutine( line.DrawRay(this.transform.position,from, ctrlPoint1, ctrtlPoint2, to));

            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }
}
