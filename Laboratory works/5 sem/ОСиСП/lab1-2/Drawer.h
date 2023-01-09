#pragma once
#include <wtypes.h>
#include <windef.h>

#include <objidl.h>
#include <gdiplus.h>
#include <gdiplus/gdiplusheaders.h>

#include <memory>

#include <vector>

namespace Drawer
{

	class Drawer
	{
	public:
		Drawer(HWND hwnd);
        ~Drawer();
		void Draw(HWND hwnd, HDC hdc);
        void MoveTo(int dx, int dy);
        void Update(int v);
        bool IsInside(int x, int y);
        POINT GetCurPoint();
        POINT GetCenter();
        POINT GetSize();
        void ChangeMode();
        void ChangeScreenSize(POINT sz);
	private:
        int x, y;
        int vx, vy;

        static int GetRand(int l, int r);

        ULONG_PTR gdiplusToken;

        Gdiplus::Image *curImage, *mainImage;
        std::vector<Gdiplus::Image*> imgList;

        Gdiplus::Color curColor;
        void ComputeMoving(int x, int y);
        int frameCnt;
        POINT scrSz = {500, 500};
        HWND Hwnd;
        enum MODE
        {
            SINGLE = 0,
            EXT = 1
        };
        MODE mode;
	};
}
