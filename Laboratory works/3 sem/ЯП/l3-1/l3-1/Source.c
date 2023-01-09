/*

	3.1 - найти длину самого короткого слова


*/
#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <string.h>


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

int main() {
	
	char str[LEN] = {""};
	char buf[LEN][LEN] = {""};
	puts("Enter line of words:");
	gets(str);
	int num = parse(str, buf, LEN);
	puts("Words of this line:");

	int minL = LEN+1, minN = -1;
	int curLen = 0;
	for (int i = 0; i < num; i++)
	{	
		curLen = strlen(buf[i]);
		if (minL >= curLen)
		{
			minL = curLen;
			minN = i;
		}
		printf("%s\n", buf[i]);
	}

	printf("\nThe shortest word is: '%s' with length %d", buf[minN], minL);

	
	
	return 0;
}