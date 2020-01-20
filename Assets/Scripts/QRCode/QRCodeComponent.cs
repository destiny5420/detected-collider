using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QRCodeComponent
{
    int m_iEncodedWidth;
    public int encodedWidth { get{ return m_iEncodedWidth; } }

    int m_iEncodedHeight;
    public int encodedHeight { get{ return m_iEncodedHeight; } }

    Texture2D m_tex2DEncoded;

    Color32[] m_color32;

    public QRCodeComponent(int v_width, int v_height)
    {
        m_iEncodedWidth = v_width;
        m_iEncodedHeight = v_height;
        m_tex2DEncoded = new Texture2D(m_iEncodedWidth, m_iEncodedHeight);
    }

    public Texture2D CreateQRcodeTex2D(string v_msg)
    {
        m_color32 = UseEncode(v_msg, m_iEncodedWidth, m_iEncodedHeight);
        m_tex2DEncoded.SetPixels32(m_color32);
        m_tex2DEncoded.Apply();
        return m_tex2DEncoded;
    }

    Color32[] UseEncode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE, // Setting format to QR_CODE
            Options = new QrCodeEncodingOptions // Setting width and height of qr code image.
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding); // return qrcode color32[]
    }
}
