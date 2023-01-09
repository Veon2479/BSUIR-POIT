// osasp-lab-1.cpp : Определяет точку входа для приложения.
//

#include <windows.h>
#include <windowsx.h>
#include <thread>
#include "Drawer.h"

// Глобальные переменные:
HINSTANCE hInst;                                // текущий экземпляр
Drawer::Drawer *drawer;
POINT lastPt;
bool isThreadRunning;
HWND hwnd;
std::thread thr;



// Отправить объявления функций, включенных в этот модуль кода:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
VOID CALLBACK TimerAPCPROC(LPVOID, DWORD, DWORD);


int APIENTRY WinMain(HINSTANCE hInstance,
                      HINSTANCE hPrevInstance,
                      LPSTR    lpCmdLine,
                      int       nCmdShow)
{
//    UNREFERENCED_PARAMETER(hPrevInstance);
//    UNREFERENCED_PARAMETER(lpCmdLine);

    MyRegisterClass(hInstance);

    if (!InitInstance (hInstance, nCmdShow))
    {
    return FALSE;
    }

    MSG msg;

    while (GetMessage(&msg, nullptr, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return (int) msg.wParam;
}

ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEXW wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style          = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc    = WndProc;
    wcex.cbClsExtra     = 0;
    wcex.cbWndExtra     = 0;
    wcex.hInstance      = hInstance;
    wcex.hIcon          = LoadIcon(hInstance, IDI_APPLICATION);
    wcex.hCursor        = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName = nullptr;
    wcex.lpszClassName  = L"OsaspLab1Class";
    wcex.hIconSm        = wcex.hIcon;

    return RegisterClassExW(&wcex);
}

VOID CALLBACK TimerAPCPROC(LPVOID lpArg, DWORD dwTimerLowValue, DWORD dwTimerHighValue)
{
    PostMessage(hwnd, WM_USER + 100, 1, 0);
}

void timerLoop()
{
    HANDLE hTimer;
    hTimer = CreateWaitableTimer(nullptr, FALSE, "myTimer");

    __int64 qwDueTime = -10;
    LARGE_INTEGER liDueTime;
    liDueTime.LowPart  = (DWORD) ( qwDueTime & 0xFFFFFFFF );
    liDueTime.HighPart = (LONG)  ( qwDueTime >> 32 );

    SetWaitableTimer(hTimer, &liDueTime, 10, TimerAPCPROC, nullptr, 0);

    isThreadRunning = true;

    while (isThreadRunning)
    {
        SleepEx(INFINITE, TRUE);
    }

    CancelWaitableTimer(hTimer);
    CloseHandle(hTimer);
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
    hInst = hInstance;

    HWND hWnd = CreateWindowW(L"OsaspLab1Class", L"Osasp lab 1", WS_OVERLAPPEDWINDOW,
                              CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, nullptr, nullptr, hInstance, nullptr);

    if (!hWnd)
    {
        return FALSE;
    }
    hwnd = hWnd;
    drawer = new Drawer::Drawer(hWnd);
    thr = std::thread(timerLoop);
    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);
    RECT rc;
    GetClientRect(hWnd, &rc);
    drawer->ChangeScreenSize({rc.right, rc.bottom});
    return TRUE;
}




LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
        case WM_PAINT:
        {
            PAINTSTRUCT ps;
            HDC hdc = BeginPaint(hWnd, &ps);
            // TODO: Добавьте сюда любой код прорисовки, использующий HDC...
            drawer->Draw(hWnd, hdc);
            EndPaint(hWnd, &ps);
        }
            break;
        case WM_KEYDOWN:
        {
            int step = 5;
            switch (wParam)
            {
                case 40: //down
                    drawer->MoveTo(0, step);
                    break;
                case 39:    //right
                    drawer->MoveTo(step, 0);
                    break;
                case 38:    //up
                    drawer->MoveTo(0, -step);
                    break;
                case 37:    //left
                    drawer->MoveTo(-step, 0);
                    break;
                case 32:    //space
                    drawer->ChangeMode();

            }
            InvalidateRect(hWnd, nullptr, TRUE);
        }

            break;
        case WM_MOUSEMOVE:
        {
            int xPos = GET_X_LPARAM(lParam);
            int yPos = GET_Y_LPARAM(lParam);
            if (wParam & MK_LBUTTON)
            {

                if (drawer->IsInside(xPos, yPos))
                {
                    int k = 1;
                    drawer->MoveTo((xPos - lastPt.x)/k, (yPos - lastPt.y)/k);
                    lastPt = {xPos, yPos};
                    InvalidateRect(hWnd, nullptr, TRUE);

                }

            }
            else
            {
                lastPt = {xPos, yPos};

            }
        }
            break;
        case WM_MOUSEWHEEL:
        {
            int wheel = GET_WHEEL_DELTA_WPARAM(wParam) / 5;
            if (wParam & MK_SHIFT)
            {
                drawer->MoveTo(wheel, 0);
            }
            else
            {
                drawer->MoveTo(0, - wheel);
            }
            InvalidateRect(hWnd, nullptr, TRUE);
        }
            break;
        case WM_ERASEBKGND:
            return FALSE;
            break;
        case WM_USER + 100:
            {
                drawer->Update(1);
                InvalidateRect(hWnd, nullptr, TRUE);
            }
            break;
        case WM_SIZE:
        {
            RECT rc;
            GetClientRect(hWnd, &rc);
            drawer->ChangeScreenSize({rc.right, rc.bottom});
        }
            break;
        case WM_DESTROY:
            PostQuitMessage(0);
            isThreadRunning = false;
            thr.join();
            delete drawer;
            break;
        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

