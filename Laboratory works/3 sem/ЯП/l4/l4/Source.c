#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <Windows.h>

#define LEN 200

int main()
{
	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);

	char str[LEN] = { "" };
	char sub1[LEN] = { "" };
	char sub2[LEN] = { "" };
	char tmp[LEN] = { "" };
	char* t = NULL;
	puts("Введите исходную строку:");
	gets(str);
	int f = 0;
	while (f==0)
	{
		puts("Введите искомую подстроку:");
		gets(sub1);
		if (sub1[0] != 0) f++;
	}
	
	puts("Введите новую подстроку:");
	gets(sub2);
	char len = strlen(sub1);

	int count = 0;
	t = &str;
	while (count < 9)
	{
		t = strstr(t, sub1);
		if (t != NULL)
		{
			count++;
			*t = 0;
			t = t + len;
			strcpy(tmp, t);
			strcat(str, sub2);
			sprintf(str+strlen(str), "%d", count);
			strcat(str, tmp);
			t += strlen(sub2);
		}
		else count = 9;
	}

	puts("Новая строка:");
	puts(str);

	return 0;
}