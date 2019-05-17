using System;
using UnityEngine;
using GoogleARCore;
public class Converter  {
    /// <summary>
    /// input an string and return an Enum value same as the string
    /// C#7.0 feature will caseu error in console, but actually it works
    /// </summary>
    /// <typeparam name="e">Enum</typeparam>
    /// <typeparam name="i">input type</typeparam>
    /// <param name="input">input string</param>
    /// <returns>return an Enum value of e</returns>
    public static e StringEnumConverter<e, i>(i input) where e : struct,IConvertible
    {
        foreach (var num in Enum.GetValues(typeof(e)))
        {
            if (input.ToString() == ((e)num).ToString())
            {
                return (e)num;
            }
        }
        return default(e);
    }

    /// <summary>
    /// Get texture from CPU buffer and return it to scanner for scanning
    /// * Buffer size requirement is different on Editor and phone
    /// * Phone is grayscale so buffer size is small
    /// * Editro will need 4 time the size of buffer.
    /// * May need adjustment of TextureFormat on phone
    /// </summary>
    /// <param name="image">Current frame image. It uses YUV-420-888 Format</param>
    /// <see cref="https://developers.google.com/ar/reference/unity/struct/GoogleARCore/CameraImageBytes"/>
    /// <returns>The Texture from Camera</returns>
    public static Texture2D Print(CameraImageBytes image)
    {
        Texture2D texture = new Texture2D(image.Width, image.Height, TextureFormat.RGBA32, false, false);
        UnityEngine.Color bufferColor = new UnityEngine.Color();
        //byte[] m_image = new byte[image.Width * image.Height];
#if UNITY_EDITOR
        byte[] buffer_Y = new byte[image.Width * image.Height * 4];
        System.Runtime.InteropServices.Marshal.Copy(image.Y, buffer_Y, 0, image.Width * image.Height * 4);
#else
            byte[] buffer_Y = new byte[image.Width * image.Height];
            System.Runtime.InteropServices.Marshal.Copy(image.Y, buffer_Y, 0, image.Width * image.Height );
#endif
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                float Y = buffer_Y[y * image.Width + x];
                bufferColor.r = Y / 255f;
                //one color channel is enough
                //c.g = Y / 255f;
                //c.b = Y / 255f;
                //c.a = 1.0f;
                texture.SetPixel(x, y, bufferColor);
            }
        }
        //for (int i = 0; i < m_image.Length; i++)
        //{
        //    m_image[i] = buffer_Y[i * 4];
        //}
        //texture.LoadRawTextureData(m_image);
        //var path = Application.persistentDataPath + "/test.jpg";
        //var output = texture.EncodeToJPG();
        //System.IO.File.WriteAllBytes(path,output);
        return texture;

    }
}

