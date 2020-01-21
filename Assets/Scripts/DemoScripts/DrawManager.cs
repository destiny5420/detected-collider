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

        TextVec();
    }

    public static void Regist(DetectCollider r_obj)
    {
        m_lisData.Add(r_obj);
    }

    void Start()
    {

    }

    // bool ComparerCollider(DetectCollider r_r1, DetectCollider r_r2)
    // {
    //     if (
    //     r_r1.boundData.maxX > r_r2.boundData.minX && 
    //     r_r2.boundData.maxX > r_r1.boundData.minX && 
    //     r_r1.boundData.maxY > r_r2.boundData.minY && 
    //     r_r2.boundData.maxY > r_r1.boundData.minY 
    //     )
    //     {
    //          return true;
    //     }
    //     else
    //         return false;
    // }

    bool ComparerCollider(DetectCollider r_r1, DetectCollider r_r2)
    {
        Vector2[] v2AryNormals_R1 = r_r1.GetNormals();
        Vector2[] v2AryNormals_R2 = r_r2.GetNormals();
        
        udsMinMaxData MinMax_P_A = GetMinMax(r_r1.m_v3FinalPos, v2AryNormals_R1[0]);
        udsMinMaxData MinMax_P_B = GetMinMax(r_r2.m_v3FinalPos, v2AryNormals_R1[0]);
        udsMinMaxData MinMax_Q_A = GetMinMax(r_r1.m_v3FinalPos, v2AryNormals_R1[1]);
        udsMinMaxData MinMax_Q_B = GetMinMax(r_r2.m_v3FinalPos, v2AryNormals_R1[1]);

        udsMinMaxData MinMax_R_A = GetMinMax(r_r1.m_v3FinalPos, v2AryNormals_R2[0]);
        udsMinMaxData MinMax_R_B = GetMinMax(r_r2.m_v3FinalPos, v2AryNormals_R2[0]);
        udsMinMaxData MinMax_S_A = GetMinMax(r_r1.m_v3FinalPos, v2AryNormals_R2[1]);
        udsMinMaxData MinMax_S_B = GetMinMax(r_r2.m_v3FinalPos, v2AryNormals_R2[1]);

        bool bSeparate_P = false;
        bool bSeparate_Q = false;
        bool bSeparate_R = false;
        bool bSeparate_S = false;
        
        if (MinMax_P_B.min > MinMax_P_A.max || MinMax_P_A.min > MinMax_P_B.max)
            bSeparate_P = true;

        if (MinMax_Q_B.min > MinMax_Q_A.max || MinMax_Q_A.min > MinMax_Q_B.max)
            bSeparate_Q = true;

        if (MinMax_R_B.min > MinMax_R_A.max || MinMax_R_A.min > MinMax_R_B.max)
            bSeparate_R = true;

        if (MinMax_S_B.min > MinMax_S_A.max || MinMax_S_A.min > MinMax_S_B.max)
            bSeparate_S = true;

        if (bSeparate_P == true || bSeparate_Q == true || bSeparate_R == true || bSeparate_S == true)
        {
            Debug.LogWarning("分離");
            return false;
        }
        else
        {
            Debug.LogError("碰撞");
            return true;
        }
    }

    void Update()
    {
        if (m_lisData.Count >= 2)
        {
            for (int i = 0; i < m_lisData.Count - 1; i++)
            {
                if (ComparerCollider(m_lisData[i], m_lisData[i + 1]))
                {
                    m_lisData[i].TriggerEvent(true);
                    m_lisData[i+1].TriggerEvent(true);
                }
                else
                {
                    m_lisData[i].TriggerEvent(false);
                    m_lisData[i+1].TriggerEvent(false);
                }
            }
        }
    }

    void TextVec()
    {
        Vector2 v2VecA = new Vector2(3, 4);
        Debug.LogWarning("v2VecA length: " + GetVecLength(v2VecA));
    }

    float GetVecLength(Vector2 v_vec)
    {
        return Mathf.Sqrt((v_vec.x*v_vec.x) + (v_vec.y*v_vec.y));
    }

    float GetVecDot(Vector2 v_vecA, Vector2 v_vecB)
    {
        return (v_vecA.x * v_vecB.x) + (v_vecA.y * v_vecB.y);
    }

    float GetProjectLenghtOnto(Vector2 v_projectVec, Vector2 v_projectBaseVec)
    {
        float fDotProduct = GetVecDot(v_projectVec, v_projectBaseVec);
        float fProjectBaseVecLength = GetVecLength(v_projectBaseVec);
        return fDotProduct / fProjectBaseVecLength;
    }

    struct udsMinMaxData
    {
        public float min{ get; set; }
        public float max{ get; set; }
        
        public udsMinMaxData(float v_min, float v_max)
        {
            min = v_min;
            max = v_max;
        }
    }

    Vector2 m_v2Axis = new Vector2(1.0f, -1.0f);
    int m_iMin_Index = 0;
    int m_iMax_Index = 0;

    udsMinMaxData GetMinMax(Vector3[] r_verticex, Vector2 v_axis)
    {
        float fMin_DotPorduct = GetProjectLenghtOnto(new Vector2(r_verticex[0].x, r_verticex[0].z), m_v2Axis);
        float fMax_DotPorduct = GetProjectLenghtOnto(new Vector2(r_verticex[0].x, r_verticex[0].z), m_v2Axis);

        for (int i = 0; i < r_verticex.Length; i++)
        {
            float fTmp = GetProjectLenghtOnto(new Vector2(r_verticex[i].x, r_verticex[i].z), m_v2Axis);

            if (fTmp < fMin_DotPorduct)
            {
                fMin_DotPorduct = fTmp;
                m_iMin_Index = i;
            }

            if (fTmp > fMax_DotPorduct)
            {
                fMax_DotPorduct = fTmp;
                m_iMin_Index = i;
            }
        }

        return new udsMinMaxData(fMin_DotPorduct, fMax_DotPorduct);
    }

    void OnDrawGizmosSelected()
    {
        
    }
}
