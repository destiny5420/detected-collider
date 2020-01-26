using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
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

    const float DRAW_SPHERE_RADIO = 0.05f;
    [SerializeField] Camera m_camera;
    [SerializeField] DetectCollider m_clsDetectObj_A;
    float m_fCenterHight;
    Vector3 m_v3CenterPos;
    static List<DetectCollider> m_lisData;

    void Awake()
    {
        m_lisData = new List<DetectCollider>();
    }

    public static void Regist(DetectCollider r_obj)
    {
        m_lisData.Add(r_obj);
    }

    void Update()
    {
        if (m_lisData.Count >= 2)
        {
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

    void OnDrawGizmosSelected()
    { 
        
    }
}
