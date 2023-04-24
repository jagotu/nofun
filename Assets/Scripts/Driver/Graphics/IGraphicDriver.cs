using System.Collections.Generic;
using System;

namespace Nofun.Driver.Graphics
{
    public interface IGraphicDriver
    {
        ITexture CreateTexture(byte[] data, int width, int height, int mipCount, TextureFormat format, Span<SColor> palettes = new Span<SColor>());

        void DrawText(int posX, int posY, int sizeX, int sizeY, List<int> positions, ITexture atlas,
            TextDirection direction, SColor textColor);

        void ClearScreen(SColor color);

        void EndFrame();

        void FlipScreen();

        void SetClipRect(int x0, int y0, int x1, int y1);
        void GetClipRect(out int x0, out int y0, out int x1, out int y1);

        void DrawTexture(int posX, int posY, int centerX, int centerY, int rotation, ITexture texture,
            int sourceX = -1, int sourceY = -1, int width = -1, int height = -1, bool blackAsTransparent = false);
        
        void FillRect(int x0, int y0, int x1, int y1, SColor color);

        void DrawSystemText(short x0, short y0, string text, SColor backColor, SColor foreColor);

        void SelectSystemFont(uint fontSize, uint fontFlags, int charCodeShouldBeInFont);

        int GetStringExtentRelativeToSystemFont(string str);

        void SetViewport(int left, int top, int width, int height);

        int ScreenWidth { get; }

        int ScreenHeight { get; }
    };
}