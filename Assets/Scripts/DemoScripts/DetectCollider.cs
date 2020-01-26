﻿using UnityEngine;

public class DetectCollider : MonoBehaviour
{
    public struct udsPointData
    {
        public Vector3 point { get; set; }
        public int dataIndex { get;set; }
        public udsPointData(Vector3 v_point)
        {
            point = v_point;
            dataIndex = 0;
        }
    }

    udsPointData[] m_sttObjPointData;
    public udsPointData[] pointDatas { get { return m_sttObjPointData;} }

    Vector3[] m_v3FinalPos;
    public Vector3[] finalPoints{ get{ return m_v3FinalPos;} }

    Vector2[] m_v2FinalPos;
    public Vector2[] finalPoints2D { get{ return m_v2FinalPos;} }

    Vector2[] m_v2Normals;
    public Vector2[] normals { get{return m_v2Normals;} }

    public string objName { get{return gameObject.name;} }
    
    const float DRAW_SPHERE_RADIO = 0.05f;

    [SerializeField] Camera m_camera;
    int m_iPointCnt = 8;
    float m_fCenterHight;
    Vector3 m_v3CenterPos;
    Vector3[] m_v3VecA;
    Vector3[] m_v3VecB;
    Vector3[] m_v3ResultPos;
    float[] m_fDisAO;
    float[] m_fDisBO;
    float[] m_fDisCO;
    Vector3[] m_v3EndVec;

    Color m_oriColor;
    Color m_changeColor = Color.red;

    [SerializeField] Vector3[] m_v3AryPoint;
    bool m_bToogle;
    bool m_bEnable = false;

    void OnEnable()
    {
        m_bEnable = !m_bEnable;
    }

    void Awake()
    {
        m_iPointCnt = m_v3AryPoint.Length;
        m_bToogle = false;
        m_v3CenterPos = new Vector3(0.0f, 0.0f, 0.0f);
        
        m_oriColor = GetComponentInChildren<MeshRenderer>().material.GetColor("_Color");
    }
    
    void Start()
    {
        m_sttObjPointData = new udsPointData[m_iPointCnt];
        DrawManager.Regist(this);
        m_v3VecA = new Vector3[m_iPointCnt];
        m_v3VecB = new Vector3[m_iPointCnt];
        m_v3ResultPos = new Vector3[m_iPointCnt];
        m_fDisAO = new float[m_iPointCnt];
        m_fDisBO = new float[m_iPointCnt];
        m_fDisCO = new float[m_iPointCnt];
        m_v3EndVec = new Vector3[m_iPointCnt];
        m_v3FinalPos = new Vector3[m_iPointCnt];
        m_v2FinalPos = new Vector2[m_iPointCnt];
        m_v2Normals = new Vector2[m_iPointCnt];

        CalRot();
    }

    void Update()
    {
        CalVertexs();
        UpdateNormals();
        CheckTrigger();
    }

    void CheckTrigger()
    {
        if (m_bToogle)
            GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", m_changeColor);
        else
            GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", m_oriColor);
    }

    void CalVertexs()
    {
        m_fCenterHight = Vector3.Distance(m_camera.transform.position, m_v3CenterPos);

        CalRot();

        for (int i = 0; i < m_iPointCnt; i++)
        {
            m_v3VecA[i] = m_sttObjPointData[i].point - m_camera.transform.position;
            m_v3VecB[i] = m_v3CenterPos - m_camera.transform.position;

            float fUnit = (Vector3.Dot(m_v3VecA[i], m_v3VecB[i]) / Common.DisForVector3(m_v3VecB[i]));
            m_v3ResultPos[i] = new Vector3(m_v3VecB[i].x * fUnit, m_v3VecB[i].y * fUnit, m_v3VecB[i].z * fUnit) + m_camera.transform.position;
            m_fDisBO[i] = Vector3.Distance(m_camera.transform.position, m_v3ResultPos[i]);
            m_fDisAO[i] = Vector3.Distance(m_camera.transform.position, m_sttObjPointData[i].point);
            m_fDisCO[i] = (m_fCenterHight * m_fDisAO[i]) / m_fDisBO[i];

            Vector3 v3UnitVecA = Vector3.Normalize(m_v3VecA[i]);
            m_v3EndVec[i] = v3UnitVecA * m_fDisCO[i];
            m_v3FinalPos[i] = m_v3EndVec[i] + m_camera.transform.position; 
            m_v2FinalPos[i] = new Vector2(m_v3FinalPos[i].x, m_v3FinalPos[i].z);
        }
    }

    void CalRot()
    {
        float fAxisY = gameObject.transform.localEulerAngles.y * (-1);
        Vector3 v3ModifyPos = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
        
        for (int i = 0; i < m_iPointCnt; i++)
        {
            Vector3 v3Target = gameObject.transform.position + new Vector3(0.0f, m_v3AryPoint[i].y, 0.0f);
            Vector3 v3Ref = gameObject.transform.position + m_v3AryPoint[i];
            Vector3 v3Point = Common.RoataeToPosAxisY(v3Target, v3Ref, fAxisY) + v3ModifyPos;
            m_sttObjPointData[i] = new udsPointData(v3Point);
        }
}

    void UpdateNormals()
    {
        int iArrayIndex = 0;
        Vector2 v2PointA;
        Vector2 v2PointB;

        for (int i = 1; i < m_v2FinalPos.Length; i++)
        {
            v2PointA = m_v2FinalPos[i-1];
            v2PointB = m_v2FinalPos[i];

            m_v2Normals[iArrayIndex] = Common.GetNormalR(v2PointB - v2PointA);
            iArrayIndex++;
        }

        v2PointA = m_v2FinalPos[m_v2FinalPos.Length - 1];
        v2PointB = m_v2FinalPos[0];
        m_v2Normals[iArrayIndex] = Common.GetNormalR(v2PointB - v2PointA);
    }

    public void TriggerEvent(bool v_key)
    {
        m_bToogle = v_key;
    }

    void OnDrawGizmosSelected()
    {
        if (m_bEnable == false)
            return;
            
        Gizmos.color = Color.blue;
        for (int i = 0; i < m_iPointCnt; i++)
            Gizmos.DrawSphere(m_sttObjPointData[i].point, DRAW_SPHERE_RADIO);

        Gizmos.color = Color.yellow;

        for (int i = 0; i < m_iPointCnt; i++)
            Gizmos.DrawLine(m_camera.transform.position, m_v3EndVec[i] + m_camera.transform.position);

        for (int i = 0; i < m_iPointCnt; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_v3FinalPos[i], 0.05f);

            // if (i == m_iPointCnt - 1)
            //     Gizmos.DrawLine(m_v3FinalPos[i], m_v3FinalPos[0]);
            // else
            //     Gizmos.DrawLine(m_v3FinalPos[i], m_v3FinalPos[i+1]);
        }

        // Draw normal point
        // Gizmos.color = Color.red;
        // for (int i = 0; i < normals.Length; i++)
        //     Gizmos.DrawSphere(new Vector3(normals[i].x, 0.0f, normals[i].y), DRAW_SPHERE_RADIO);
    }
}