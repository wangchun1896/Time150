using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class AutoSizeRawImage : AutoSizeBase
{
    public RawImage rawImage; // 在Inspector中设置RawImage
    private RectTransform rectTransform;
    public float wide = 290f;

    public void SetImageTexture(Texture2D texture)
    {
        rawImage.texture = texture;
        UpdateImageSize();
    }

    private void UpdateImageSize()
    {
        // 获取纹理的宽高
        float textureWidth = rawImage.texture.width;
        float textureHeight = rawImage.texture.height;

        // 计算新的高度，保持宽度限制为 290 像素
        float targetWidth = wide;
        float targetHeight = (textureHeight / textureWidth) * targetWidth;

        // 更新 RawImage 的 RectTransform 尺寸
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
