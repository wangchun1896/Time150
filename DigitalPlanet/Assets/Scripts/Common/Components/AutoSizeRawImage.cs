using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class AutoSizeRawImage : AutoSizeBase
{
    public RawImage rawImage; // ��Inspector������RawImage
    private RectTransform rectTransform;
    public float wide = 290f;

    public void SetImageTexture(Texture2D texture)
    {
        rawImage.texture = texture;
        UpdateImageSize();
    }

    private void UpdateImageSize()
    {
        // ��ȡ����Ŀ��
        float textureWidth = rawImage.texture.width;
        float textureHeight = rawImage.texture.height;

        // �����µĸ߶ȣ����ֿ������Ϊ 290 ����
        float targetWidth = wide;
        float targetHeight = (textureHeight / textureWidth) * targetWidth;

        // ���� RawImage �� RectTransform �ߴ�
        rectTransform = rawImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(targetWidth, targetHeight);
        onHeightChange.Invoke();
    }
    private void OnDestroy()
    {
        if(rawImage!=null)
        {
            rawImage.texture = null;
        }
       
    }
}
