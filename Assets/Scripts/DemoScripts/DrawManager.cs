using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    float m_fCenterHight;
    Vector3 m_v3CenterPos;
    [SerializeField] Camera m_camera;
    [SerializeField] DetectCollider m_clsDetectObj_A;
    static List<DetectCollider> m_lisData;

    void Awake()
    {
        m_lisData = new List<DetectCollider>();
        m_sttAryMinMaxData = new udsMinMaxData[2];
    }

    public static void Regist(DetectCollider r_obj)
    {
        m_lisData.Add(r_obj);
    }

    bool ComparerCollider(DetectCollider r_r1, DetectCollider r_r2)
    {
        // Vector2[] v2AryNormals_R1 = r_r1.GetNormals();
        // Vector2[] v2AryNormals_R2 = r_r2.GetNormals();
        
        // udsMinMaxData MinMax_P_A = GetMinMax(r_r1.finalPoints, v2AryNormals_R1[0]);
        // udsMinMaxData MinMax_P_B = GetMinMax(r_r2.finalPoints, v2AryNormals_R1[0]);
        // udsMinMaxData MinMax_Q_A = GetMinMax(r_r1.finalPoints, v2AryNormals_R1[1]);
        // udsMinMaxData MinMax_Q_B = GetMinMax(r_r2.finalPoints, v2AryNormals_R1[1]);

        // udsMinMaxData MinMax_R_A = GetMinMax(r_r1.finalPoints, v2AryNormals_R2[0]);
        // udsMinMaxData MinMax_R_B = GetMinMax(r_r2.finalPoints, v2AryNormals_R2[0]);
        // udsMinMaxData MinMax_S_A = GetMinMax(r_r1.finalPoints, v2AryNormals_R2[1]);
        // udsMinMaxData MinMax_S_B = GetMinMax(r_r2.finalPoints, v2AryNormals_R2[1]);

        // bool bSeparate_P = false;
        // bool bSeparate_Q = false;
        // bool bSeparate_R = false;
        // bool bSeparate_S = false;
        
        // if (MinMax_P_B.min > MinMax_P_A.max || MinMax_P_A.min > MinMax_P_B.max)
        //     bSeparate_P = true;

        // if (MinMax_Q_B.min > MinMax_Q_A.max || MinMax_Q_A.min > MinMax_Q_B.max)
        //     bSeparate_Q = true;

        // if (MinMax_R_B.min > MinMax_R_A.max || MinMax_R_A.min > MinMax_R_B.max)
        //     bSeparate_R = true;

        // if (MinMax_S_B.min > MinMax_S_A.max || MinMax_S_A.min > MinMax_S_B.max)
        //     bSeparate_S = true;

        // if (bSeparate_P == true || bSeparate_Q == true || bSeparate_R == true || bSeparate_S == true)
        // {
        //     Debug.LogWarning("分離");
        //     return false;
        // }
        // else
        // {
        //     Debug.LogError("碰撞");
        //     return true;
        // }

        return false;
    }

    void Update()
    {
        TestFunction();

        if (CalTrigger() == true)
        {
            for (int i = 0; i < m_lisData.Count; i++)
                m_lisData[i].TriggerEvent(false);
        }
        else
        {
            for (int i = 0; i < m_lisData.Count; i++)
                m_lisData[i].TriggerEvent(true);
        }
    }

    Vector3 m_v3EndVec = new Vector3(10.0f, 0.0f, 3.0f);
    Vector3 m_v3StartVec = new Vector3(-5.0f, 0.0f, 3.0f);
    Vector3 m_v3Axis;
    udsMinMaxData[] m_sttAryMinMaxData;
    int m_iMinMaxDataIndex = 0;

    void TestFunction()
    {
        m_iMinMaxDataIndex = 0;
        m_v3Axis = m_v3EndVec - m_v3StartVec;
        Vector2 v2VecBaseAxis = new Vector2(m_v3Axis.x, m_v3Axis.z);

        for (int j = 0; j < m_lisData.Count; j++)
        {
            // for (int i = 0; i < m_lisData[j].finalPoints.Length; i++)
            //     Debug.Log(m_lisData[j].objName +  " / Detedced Vertex["+i+"]: " + m_lisData[j].finalPoints2D[i]);

            m_sttAryMinMaxData[m_iMinMaxDataIndex] = GetMinMax(m_lisData[j].objName, m_lisData[j].finalPoints2D, v2VecBaseAxis);
            m_iMinMaxDataIndex++;
        }
    }

    bool CalTrigger()
    {
        for (int i = 0; i < m_lisData[0].normals.Length; i++)
        {
            udsMinMaxData sttMinMaxDataA = GetMinMax(m_lisData[0].objName, m_lisData[0].finalPoints2D, m_lisData[0].normals[i]);
            udsMinMaxData sttMinMaxDataB = GetMinMax(m_lisData[1].objName, m_lisData[1].finalPoints2D, m_lisData[0].normals[i]);
            if (sttMinMaxDataA.min > sttMinMaxDataB.max || sttMinMaxDataB.min > sttMinMaxDataA.max)
                return true;
        }

        for (int i = 0; i < m_lisData[1].normals.Length; i++)
        {
            udsMinMaxData sttMinMaxDataA = GetMinMax(m_lisData[0].objName, m_lisData[0].finalPoints2D, m_lisData[1].normals[i]);
            udsMinMaxData sttMinMaxDataB = GetMinMax(m_lisData[1].objName, m_lisData[1].finalPoints2D, m_lisData[1].normals[i]);
            if (sttMinMaxDataA.min > sttMinMaxDataB.max || sttMinMaxDataB.min > sttMinMaxDataA.max)
                return true;
        }

        return false;
    }

    struct udsMinMaxData
    {
        public float min{ get; set; }
        public float max{ get; set; }
        public string name { get;set; }

        public udsMinMaxData(string v_name, float v_min, float v_max)
        {
            name = v_name;
            min = v_min;
            max = v_max;
        }
    }

    udsMinMaxData GetMinMax(string v_name, Vector2[] r_verticex, Vector2 v_axis)
    {
        float fMin_DotPorduct = Common.GetProjectLenghtOnto(r_verticex[0], v_axis);
        float fMax_DotPorduct = Common.GetProjectLenghtOnto(r_verticex[0], v_axis);

        for (int i = 0; i < r_verticex.Length; i++)
        {
            float fTmp = Common.GetProjectLenghtOnto(r_verticex[i], v_axis);

            if (fTmp < fMin_DotPorduct)
                fMin_DotPorduct = fTmp;

            if (fTmp > fMax_DotPorduct)
                fMax_DotPorduct = fTmp;
        }

        return new udsMinMaxData(v_name, fMin_DotPorduct, fMax_DotPorduct);
    }

    const float DRAW_SPHERE_RADIO = 0.05f;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_v3EndVec, m_v3StartVec);
        Gizmos.DrawSphere(m_v3EndVec, DRAW_SPHERE_RADIO);
        Gizmos.DrawSphere(m_v3StartVec, DRAW_SPHERE_RADIO);
    }
}
