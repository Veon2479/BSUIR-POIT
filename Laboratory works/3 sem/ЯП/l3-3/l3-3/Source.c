/*

	3.3 - sort words

*/
#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <Windows.h>

#define LEN 200

char parse(char str[LEN], char buf[LEN][LEN], char lng) {
	char i = 0, j = 0, last = 0;
	char st[LEN] = { 0 };

	while (str[i] == ' ')		//finding the pos of the next letter
		i++;

	while (str[i] != 0)
	{


		j = i;
		while ((str[j] != ' ') && (str[j] != 0))	//----asdfghj---
			j++;									//----i******j--

		strncpy(st, &str[i], (char)(j - i));

		strncpy(&buf[last], st, (char)(j - i));

		last++;
		i = j;

		while (str[i] == ' ')		//finding the pos of the next letter
			i++;

	}

	return last;

}

int main()
{
	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);

	char str[LEN] = { "" };
	char buf[LEN][LEN] = { "" };
	puts("Enter line of words:");
	gets(str);
	int num = parse(str, buf, LEN);
	
	int i = 0, j = 0, curMin = 0;
	char tmp[LEN] = { "" };
	strcpy(str, "");

	for (i = 0; i < num; i++)
	{
		curMin = i;
		for (j=i+1; j<num; j++)
			if (strcmp(buf[j], buf[curMin]) < 0)
			{
				curMin = j;
			}
		if (curMin != i)
		{
			strcpy(tmp, buf[i]);
			strcpy(buf[i], buf[curMin]);
			strcpy(buf[curMin], tmp);
		}
		strncat(str, buf[i], LEN);
		strncat(str, " ", LEN);
	}

	puts("New line is:");
	puts(str);


	return 0;
}