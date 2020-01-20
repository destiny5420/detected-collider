using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public udsPointData[] pointDatas { get { return m_sttPointData;} }

    public struct udsBoundData
    {
        public DetectCollider m_object;
        public float minX { get;set; }
        public float maxX { get;set; }
        public float minY { get;set; }
        public float maxY { get;set; }

        public udsBoundData(DetectCollider r_obj)
        {
            m_object = r_obj;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
        }
    }
    public udsBoundData boundData { get { return m_sttBoundData;} }
    
    public Transform target;
    //[SerializeField] BoxCollider m_collider;
    udsPointData[] m_sttPointData;
    udsBoundData m_sttBoundData;
    const int POINT_DATA_COUNT = 4;
    const float DRAW_SPHERE_RADIO = 0.05f;

    float m_fCenterHight;
    Vector3 m_v3CenterPos;
    public Camera m_camera;
    Vector3[] m_v3VecA;
    Vector3[] m_v3VecB;
    Vector3[] m_v3ResultPos;
    float[] m_fDisAO;
    float[] m_fDisBO;
    float[] m_fDisCO;
    Vector3[] m_v3EndVec;
    Vector3[] m_v3FinalPos;

    Color m_oriColor;
    Color m_changeColor = Color.red;

    bool m_bToogle;

    [SerializeField] Vector3[] m_v3AryPoint;

    void Awake()
    {
        m_bToogle = false;
        m_v3CenterPos = new Vector3(0.0f, 0.0f, 0.0f);
        m_fCenterHight = Vector3.Distance(m_camera.transform.position, m_v3CenterPos);
        m_oriColor = GetComponentInChildren<MeshRenderer>().material.GetColor("_Color");
    }
    
    void Start()
    {
        m_sttPointData = new udsPointData[POINT_DATA_COUNT];
        m_sttBoundData = new udsBoundData(this);
        DrawManager.Regist(this);
        m_v3VecA = new Vector3[POINT_DATA_COUNT];
        m_v3VecB = new Vector3[POINT_DATA_COUNT];
        m_v3ResultPos = new Vector3[POINT_DATA_COUNT];
        m_fDisAO = new float[POINT_DATA_COUNT];
        m_fDisBO = new float[POINT_DATA_COUNT];
        m_fDisCO = new float[POINT_DATA_COUNT];
        m_v3EndVec = new Vector3[POINT_DATA_COUNT];
        m_v3FinalPos = new Vector3[POINT_DATA_COUNT];
        // m_sttPointData[0] = new udsPointData(new Vector3(m_collider.bounds.max.x, m_collider.bounds.max.y, m_collider.bounds.max.z));
        // m_sttPointData[1] = new udsPointData(new Vector3(m_collider.bounds.max.x, m_collider.bounds.max.y, m_collider.bounds.min.z));
        // m_sttPointData[2] = new udsPointData(new Vector3(m_collider.bounds.min.x, m_collider.bounds.max.y, m_collider.bounds.max.z));
        // m_sttPointData[3] = new udsPointData(new Vector3(m_collider.bounds.min.x, m_collider.bounds.max.y, m_collider.bounds.min.z));

        m_sttPointData[0] = new udsPointData(Common.RoataeToPos2(gameObject.transform.position, gameObject.transform.position + m_v3AryPoint[0], gameObject.transform.localEulerAngles.y * (-1)) + new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z));
        m_sttPointData[1] = new udsPointData(Common.RoataeToPos2(gameObject.transform.position, gameObject.transform.position + m_v3AryPoint[1], gameObject.transform.localEulerAngles.y * (-1)) + new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z));
        m_sttPointData[2] = new udsPointData(Common.RoataeToPos2(gameObject.transform.position, gameObject.transform.position + m_v3AryPoint[2], gameObject.transform.localEulerAngles.y * (-1)) + new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z));
        m_sttPointData[3] = new udsPointData(Common.RoataeToPos2(gameObject.transform.position, gameObject.transform.position + m_v3AryPoint[3], gameObject.transform.localEulerAngles.y * (-1)) + new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z));
    }

    void Update()
    {
        // m_sttPointData[0].point = new Vector3(m_collider.bounds.max.x, m_collider.bounds.max.y, m_collider.bounds.max.z);
        // m_sttPointData[1].point = new Vector3(m_collider.bounds.max.x, m_collider.bounds.max.y, m_collider.bounds.min.z);
        // m_sttPointData[2].point = new Vector3(m_collider.bounds.min.x, m_collider.bounds.max.y, m_collider.bounds.max.z);
        // m_sttPointData[3].point = new Vector3(m_collider.bounds.min.x, m_collider.bounds.max.y, m_collider.bounds.min.z);
        m_sttPointData[0] = new udsPointData(Common.RoataeToPos2(gameObject.transform.position, gameObject.transform.position + m_v3AryPoint[0], gameObject.transform.localEulerAngles.y * (-1)) + new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z));
        m_sttPointData[1] = new udsPointData(Common.RoataeToPos2(gameObject.transform.position, gameObject.transform.position + m_v3AryPoint[1], gameObject.transform.localEulerAngles.y * (-1)) + new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z));
        m_sttPointData[2] = new udsPointData(Common.RoataeToPos2(gameObject.transform.position, gameObject.transform.position + m_v3AryPoint[2], gameObject.transform.localEulerAngles.y * (-1)) + new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z));
        m_sttPointData[3] = new udsPointData(Common.RoataeToPos2(gameObject.transform.position, gameObject.transform.position + m_v3AryPoint[3], gameObject.transform.localEulerAngles.y * (-1)) + new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z));

        DrawRefLine();
        UpdateBoundData();
        
        if (transform.name == "Bullet")
        {
            Debug.LogWarning("0 / " + m_v3FinalPos[0]);
            Debug.LogWarning("1 / " + m_v3FinalPos[1]);
            Debug.LogWarning("2 / " + m_v3FinalPos[2]);
            Debug.LogWarning("3 / " + m_v3FinalPos[3]);

            Debug.LogWarning("fMinX / " + m_sttBoundData.minX);
            Debug.LogWarning("fMaxX / " + m_sttBoundData.maxX);
            Debug.LogWarning("fMinY / " + m_sttBoundData.minY);
            Debug.LogWarning("fMaxY / " + m_sttBoundData.maxY);
        }

        if (m_bToogle)
            GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", m_changeColor);
        else
            GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", m_oriColor);
    }

    void UpdateBoundData()
    {
        float fMinX = 0.0f;
        float fMaxX = 0.0f;
        float fMinY = 0.0f;
        float fMaxY = 0.0f;

        // Min X
        for (int i = 0; i < m_v3FinalPos.Length; i++)
        {
            if (i == 0)
                fMinX = m_v3FinalPos[0].x;
            else
            {
                if (m_v3FinalPos[i].x < fMinX)
                    fMinX = m_v3FinalPos[i].x;
            }    
        }

        // Max X
        for (int i = 0; i < m_v3FinalPos.Length; i++)
        {
            if (i == 0)
                fMaxX = m_v3FinalPos[0].x;
            else
            {
                if (m_v3FinalPos[i].x > fMaxX)
                    fMaxX = m_v3FinalPos[i].x;
            }    
        }

        // Min Y
        for (int i = 0; i < m_v3FinalPos.Length; i++)
        {
            if (i == 0)
                fMinY = m_v3FinalPos[0].z;
            else
            {
                if (m_v3FinalPos[i].z < fMinY)
                    fMinY = m_v3FinalPos[i].z;
            }    
        }

        // Max Y
        for (int i = 0; i < m_v3FinalPos.Length; i++)
        {
            if (i == 0)
                fMaxY = m_v3FinalPos[0].z;
            else
            {
                if (m_v3FinalPos[i].z > fMaxY)
                    fMaxY = m_v3FinalPos[i].z;
            }    
        }

        m_sttBoundData.minX = fMinX;
        m_sttBoundData.maxX = fMaxX;
        m_sttBoundData.minY = fMinY;
        m_sttBoundData.maxY = fMaxY;
    }

    void DrawRefLine()
    {
        for (int i = 0; i < POINT_DATA_COUNT; i++)
        {
            m_v3VecA[i] = m_sttPointData[i].point - m_camera.transform.position;
            m_v3VecB[i] = m_v3CenterPos - m_camera.transform.position;

            float fUnit = (Vector3.Dot(m_v3VecA[i], m_v3VecB[i]) / DisForVector3(m_v3VecB[i]));
            m_v3ResultPos[i] = new Vector3(m_v3VecB[i].x * fUnit, m_v3VecB[i].y * fUnit, m_v3VecB[i].z * fUnit) + m_camera.transform.position;
            m_fDisBO[i] = Vector3.Distance(m_camera.transform.position, m_v3ResultPos[i]);
            m_fDisAO[i] = Vector3.Distance(m_camera.transform.position, m_sttPointData[i].point);
            m_fDisCO[i] = (m_fCenterHight * m_fDisAO[i]) / m_fDisBO[i];

            Vector3 v3UnitVecA = Vector3.Normalize(m_v3VecA[i]);
            m_v3EndVec[i] = v3UnitVecA * m_fDisCO[i];
            m_v3FinalPos[i] = m_v3EndVec[i] + m_camera.transform.position; 
        }
    }

    float DisForVector3(Vector3 v_vec)
    {
        return Mathf.Pow(Mathf.Sqrt(Mathf.Pow(v_vec.x, 2) + Mathf.Pow(v_vec.y, 2) + Mathf.Pow(v_vec.z, 2)), 2);
    }

    public void TriggerEvent(bool v_key)
    {
        m_bToogle = v_key;
    }

    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;

            for (int i = 0; i < POINT_DATA_COUNT; i++)
                Gizmos.DrawSphere(m_sttPointData[i].point, DRAW_SPHERE_RADIO);
        }

        Gizmos.color = Color.yellow;

        for (int i = 0; i < POINT_DATA_COUNT; i++)
            Gizmos.DrawLine(m_camera.transform.position, m_v3EndVec[i] + m_camera.transform.position);

        for (int i = 0; i < POINT_DATA_COUNT; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_v3FinalPos[i], 0.05f);

            if (i == POINT_DATA_COUNT - 1)
                Gizmos.DrawLine(m_v3FinalPos[i], m_v3FinalPos[0]);
            else
                Gizmos.DrawLine(m_v3FinalPos[i], m_v3FinalPos[i+1]);
        }
    }
}
