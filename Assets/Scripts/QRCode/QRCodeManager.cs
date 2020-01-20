using UnityEngine;

public class QRCodeManager : MonoBehaviour
{
    const string QRCODE_STRING = "http://www.google.com";

    QRCodeComponent m_clsQRCodeComponent;
    Texture2D m_tex2DQRCode;

    void Start()
    {
        m_clsQRCodeComponent = new QRCodeComponent(256, 256);
        m_tex2DQRCode = m_clsQRCodeComponent.CreateQRcodeTex2D(QRCODE_STRING);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(100, 100, 256, 256), m_tex2DQRCode);
    }
}
