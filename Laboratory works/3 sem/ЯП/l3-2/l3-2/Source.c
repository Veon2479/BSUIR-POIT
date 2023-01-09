/*

	3.2 - find words with 'A'

*/
#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <locale.h>
#include <Windows.h>
#include <string.h>

#define LEN 200

char parse(char str[LEN], char buf[LEN][LEN], char lng)
{
	char i = 0, j = 0, last = 0;
	char st[LEN] = { 0 };
	while (str[i] == ' ')
		i++;
	while (str[i] != 0)
	{
		j = i;
		while ((str[j] != ' ') && (str[j] != 0))
			j++;
		strncpy(st, &str[i], (char)(j - i));
		strncpy(&buf[last], st, (char)(j - i));
		last++;
		i = j;
		while (str[i] == ' ')
			i++;
	}
	return last;
}

int main()
{
	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);
	char str[LEN] = { 0 }, buf[LEN][LEN] = { 0 };
	puts("Enter line of capitalized letters : ");
	gets(str);
	
	char num = parse(str, buf, LEN);
	int fl = 0;
	puts("Words with letter 'A':");
	for (int i = 0; i < num; i++)
	{
		if (strchr(buf[i], 'A') != NULL || strchr(buf[i], 'À') != NULL)
		{
			fl++;
			printf("%s ", buf[i]);
		}
	}
	if (fl == 0) puts("There's no such words..");
	return 0;
}