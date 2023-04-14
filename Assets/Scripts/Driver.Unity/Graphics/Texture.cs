using Nofun.Driver.Graphics;
using UnityEngine;
using System;

namespace Nofun.Driver.Unity.Graphics
{
    public class Texture : ITexture
    {
        private Texture2D uTexture;
        private Driver.Graphics.TextureFormat format;
        private int mipCount;

        public Texture2D NativeTexture => uTexture;

        public int Width => uTexture.width;
        public int Height => uTexture.height;
        public int MipCount => mipCount;
        public Driver.Graphics.TextureFormat Format => format;

        private bool DoesFormatNeedTransform(Driver.Graphics.TextureFormat format)
        {
            return (format <= Driver.Graphics.TextureFormat.RGB332) || (format == Driver.Graphics.TextureFormat.RGB555)
                    || (format >= Driver.Graphics.TextureFormat.ARGB1555) || (format == Driver.Graphics.TextureFormat.RGB444);
        }

        private UnityEngine.TextureFormat MapMophunFormatToUnity(Driver.Graphics.TextureFormat format)
        {
            switch (format)
            {
                case Driver.Graphics.TextureFormat.RGB565:
                    return UnityEngine.TextureFormat.RGB565;

                case Driver.Graphics.TextureFormat.RGB888:
                    return UnityEngine.TextureFormat.RGB24;

                case Driver.Graphics.TextureFormat.ARGB4444:
                    return UnityEngine.TextureFormat.ARGB4444;

                case Driver.Graphics.TextureFormat.ARGB8888:
                    return UnityEngine.TextureFormat.ARGB32;

                default:
                    throw new ArgumentException("Unconvertable Mophun format to native Unity format: " + format);
            }
        }

        private void UploadData(byte[] data, int width, int height, int mipCount)
        {
            bool needTransform = DoesFormatNeedTransform(format);
            int bitsPerPixel = TextureUtil.GetPixelSizeInBits(format);

            // These format normally also need individual conversion
            bool shouldUseBitStream = (bitsPerPixel % 8 != 0);
            int dataPointerInBits = 0;

            for (int i = 0; (i < mipCount) && (width !=0) && (height != 0); i++)
            {
                // Gurantee that this is in byte-unit.
                if (!needTransform)
                {
                    uTexture.SetPixelData(data, i, dataPointerInBits >> 3);
                }
                else
                {
                    // Transform data into separate buffer, then set pixel data
                    throw new Exception("Format that need to be converted is unhandled!");
                }

                dataPointerInBits += width * height * bitsPerPixel;

                width >>= 2;
                height >>= 2;
            }

            uTexture.Apply();
        }

        public Texture(byte[] data, int width, int height, int mipCount, Driver.Graphics.TextureFormat format)
        {
            bool needTransform = DoesFormatNeedTransform(format);

            this.format = format;
            this.mipCount = mipCount;

            uTexture = new Texture2D(width, height, needTransform ? UnityEngine.TextureFormat.ARGB32 : MapMophunFormatToUnity(format), true);
            UploadData(data, width, height, mipCount);
        }

        public void SetData(byte[] data, int mipLevel)
        {
            uTexture.SetPixelData(data, mipLevel);
        }

        public void Apply()
        {
            uTexture.Apply();
        }
    }
}