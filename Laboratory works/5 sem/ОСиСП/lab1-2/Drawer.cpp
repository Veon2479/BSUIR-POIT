#include "Drawer.h"

namespace Drawer
{
	

	Drawer::Drawer(HWND hwnd)
	{
        mode = SINGLE;
        x = 10;
        y = 10;

        Gdiplus::GdiplusStartupInput gdiplusStartupInput;
        GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, nullptr);

        mainImage = Gdiplus::Image::FromFile(L"bird.bmp");
        imgList.push_back(Gdiplus::Image::FromFile(L"1.png"));
        imgList.push_back(Gdiplus::Image::FromFile(L"2.png"));
        imgList.push_back(Gdiplus::Image::FromFile(L"3.png"));

        curImage = mainImage;

        curColor = Gdiplus::Color(255, 255, 255);

	}

    Drawer::~Drawer()
    {
        curImage = nullptr;
        delete mainImage;
        for (int i = 0; i < imgList.size(); i++)
            delete imgList[i];
        Gdiplus::GdiplusShutdown(gdiplusToken);
        frameCnt = 0;

    }


    void Drawer::MoveTo(int dx, int dy)
    {

        x += dx;
        y += dy;
        POINT sz = this->GetSize();
        RECT rc;
        GetClientRect(Hwnd, &rc);
        long rightBorder = scrSz.x, bottomBorder = scrSz.y;
        if (x < 0)
        {
            x = 0;
            ComputeMoving(1, 0);
        }
        else
            if (x + sz.x > rightBorder)
            {
                x = rightBorder - sz.x;
                ComputeMoving(1, 0);
            }
        if (y < 0)
        {
            y = 0;
            ComputeMoving(0, 1);
        }
        else
            if (y + sz.y > bottomBorder)
            {
                y = bottomBorder - sz.y;
                ComputeMoving(0, 1);
            }
    }

    void Drawer::ComputeMoving(int x, int y)
    {
        if (x != 0)
        {
            vx = - vx;
        }
        if (y != 0)
        {
            vy = - vy;
        }
        if (mode == EXT)
        {
            frameCnt = (frameCnt + 1) % imgList.size();
            curImage = imgList[frameCnt];
            curColor = Gdiplus::Color(GetRand(0, 255), GetRand(0, 255), GetRand(0, 255));
        }
    }

    bool Drawer::IsInside(int px, int py)
    {
        bool res = true;
        if (px < x || px > x + curImage->GetWidth() || py < y || py > y + curImage->GetHeight())
            res = false;
        return res;
    }

    POINT Drawer::GetCurPoint()
    {
        return {x, y};
    }

    POINT Drawer::GetSize()
    {
        return {(long)curImage->GetWidth(), (long)curImage->GetHeight()};
    }

    POINT Drawer::GetCenter()
    {
        return {(x + (long)curImage->GetWidth()) / 2, (y + (long)curImage->GetHeight()) / 2};
    }

    void Drawer::Update(int v)
    {
        if (mode == EXT)
        {
            this->MoveTo(v * vx, v * vy);
        }
    }

	void Drawer::Draw(HWND hwnd, HDC hdc)
	{

        RECT rc;
        GetClientRect(hwnd, &rc);

        HDC memDC = CreateCompatibleDC(hdc);
        //const int nMemDc = SaveDC(memDC);

        HBITMAP hBitMap = CreateCompatibleBitmap(hdc, rc.right - rc.left, rc.bottom - rc.top);
        SelectObject(memDC, hBitMap);

        Gdiplus::Graphics graphics(memDC);
        Gdiplus::SolidBrush back(curColor);
        graphics.FillRectangle(&back, (int)rc.left, (int)rc.top, (int)(rc.right-rc.left), (int)(rc.bottom-rc.top));

        graphics.DrawImage(curImage, x, y);

        BitBlt(hdc, (int)rc.left, (int)rc.top, (int)(rc.right-rc.left), (int)(rc.bottom-rc.top), memDC, (int)rc.left, (int)rc.top, SRCCOPY);
        //RestoreDC(memDC, nMemDc);
        DeleteObject(hBitMap);

//  // without double buffering
//        Gdiplus::Graphics graphics(hdc);
//        graphics.DrawImage(curImage, x, y);

	}

    void Drawer::ChangeMode()
    {
        mode = (MODE)(1 - (int)mode);

        if (mode == SINGLE)
        {
            curImage = mainImage;
            vx = 0;
            vy = 0;
        }
        else if (mode == EXT)
        {
            frameCnt = 0;
            curImage = imgList[0];
            vx = GetRand(-5, 5);
            vy = GetRand(-5, 5);
        }
        this->MoveTo(0, 0);
    }

    int Drawer::GetRand(int l, int r)
    {
        return (std::rand() % abs(r - l)) + l;
    }

    void Drawer::ChangeScreenSize(POINT sz)
    {
        scrSz = sz;
    }

}