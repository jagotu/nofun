using System;

namespace Nofun.Driver.Graphics
{
    public static class TextureUtil
    {
        public static int GetPixelSizeInBits(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.Monochrome:
                case TextureFormat.Palette2:
                    return 1;

                case TextureFormat.Palette4:
                case TextureFormat.Gray4:
                    return 2;

                case TextureFormat.Gray16:
                case TextureFormat.Palette16:
                case TextureFormat.Palette16_Alt:
                case TextureFormat.Palette16_ARGB:
                    return 4;

                case TextureFormat.Palette256:
                case TextureFormat.Palette256_ARGB:
                case TextureFormat.Palette256_Alt:
                case TextureFormat.RGB332:
                    return 8;

                case TextureFormat.RGB444:
                    return 12;

                case TextureFormat.RGB555:
                    return 15;

                case TextureFormat.RGB565:
                case TextureFormat.ARGB4444:
                case TextureFormat.ARGB1555:
                    return 16;

                case TextureFormat.RGB888:
                    return 24;

                case TextureFormat.ARGB8888:
                    return 32;

                default:
                    throw new ArgumentException("Unknown texture format type: " +  format);
            }
        }

        public static long GetTextureSizeInBits(int width, int height, TextureFormat format)
        {
            return GetPixelSizeInBits(format) * width * height;
        }
    }
}