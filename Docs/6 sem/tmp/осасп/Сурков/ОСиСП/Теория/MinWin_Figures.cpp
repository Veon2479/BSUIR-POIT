#include <windows.h>
#include <vector>

LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

std::vector<Figure*> figures;

int APIENTRY WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, 
    LPTSTR lpCmdLine, int nCmdShow)
{
    figures.push_back(new TRectangle(10, 20, 100, 200));
    figures.push_back(new TCircle(50, 50, 100));
    figures.push_back(new TRectangle(30, 50, 300, 200));
    figures.push_back(new TCircle(10, 10, 200));
    figures.push_back(new TCircle(50, 100, 250));
	
	WNDCLASSEX wcex;
    HWND hWnd;
    MSG msg;

    wcex.cbSize = sizeof(WNDCLASSEX); 
    wcex.style = CS_DBLCLKS;
    wcex.lpfnWndProc = WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInstance;
    wcex.hIcon = LoadIcon(NULL, IDI_APPLICATION);
    wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName = NULL;
    wcex.lpszClassName = "HelloWorldClass";
    wcex.hIconSm = wcex.hIcon;

    RegisterClassEx(&wcex);

    hWnd = CreateWindow("HelloWorldClass", "Hello, World!", WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);

    while (GetMessage(&msg, NULL, 0, 0)) 
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return (int) msg.wParam;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message) 
    {
	case WM_PAINT:
		PAINTSTRUCT* p;
		HDC dc = BeginPaint(hWnd, p);
		for (int i = 0; i < figures.size(); i++)
			figures[i].Draw(dc);
		EndPaint(hWnd, p);
		break;
    case WM_LBUTTONDBLCLK:
        MessageBox(hWnd, "Hello, World!", "Message", MB_OK);
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}


class TFigure
{
protected:
	virtual void Draw(HDC dc) = 0;
public:
    void Paint(HDC dc)
    {
        Draw(dc);
        // Notify();
    }
};

class TRectangle : public TFigure
{
private:
	int x;
	int y;
	int width;
	int height;

protected:
	virtual void Draw(HDC dc)
	{
	    Rectangle(dc, x, y, x + width, y + height);
	}

public:
	TRectangle(int aX, int aY, int aWidth, int aHeight)
	{
		x = aX; y = aY; width = aWidth; height = aHeight;
	}
};

class TCircle : public TFigure
{
private:
	int x;
	int y;
	int radius;

protected:
	virtual void Draw(HDC dc)
	{
		Ellipse(dc, x - radius, y - radius, x + radius, y + radius);
	}

public:
	TCircle(int aX, int aY, int aRadius)
	{
		x = aX; y = aY; radius = aRadius;
	}
};
