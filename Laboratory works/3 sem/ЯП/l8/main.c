#include <stdio.h>
#include <stdlib.h>

#define el struct el

el
{
    int id;
    int field;
    el* next;
};

el* init(el* ONE, int num, int id)
{
    ONE = (el*) malloc(sizeof(el));
    ONE->id = id;
    ONE->field = num;
    ONE->next = NULL;
    return ONE;
}

void showQueue(el* Head)
{

   
    while (Head->next != NULL)
    {
         Head = Head->next;
        printf("%d ", Head->field);
       
    }
}

void push(el* Head, el* ONE)
{
   // printf("\nPUSH: %d", ONE->field);
    el* last = Head;
    while (last->next != NULL)
        last = last->next;
    last->next = ONE;
}

el* pop(el* Head)
{
    el* tmp = Head->next;
    Head->next = tmp->next;
    tmp->next = NULL;    
    return tmp;
}

int getLen(el* Head)
{
    int res = 0;
    while (Head->next != NULL)
    {
        res++;
        Head = Head->next;
    }
    return res;
}

int isEmpty(el* Head)
{
    if (Head->next == NULL)
        return 1;
        else return 0;
}

int main(int argc, char *argv[])
{
    el *Head = NULL;
    Head = init(Head, 0, 0);
    int count = 1;
    el *tmp = NULL;
    while (count<23)
    {
       tmp = init(tmp, count, count);
       count++;
       push(Head, tmp);
       tmp = NULL;
    }
    puts("Old Queue:");
    showQueue(Head);
    ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
    int len = getLen(Head);
    el* tmp2 = NULL;
    while (len>0)
    {
        len -= 2;
        if (!isEmpty(Head))
            tmp = pop(Head);
     
        if (!isEmpty(Head) && (len != -1))
            tmp2 = pop(Head);
        
        
     //   printf("\nAAAA: %d, %d, %d", tmp->field, tmp2->field, len);
        if (tmp != NULL)
        {
            push(Head, tmp);
        }
            
        if (tmp2 != NULL)
            free(tmp2);
        tmp = NULL;
        tmp2 = NULL;
    }
    
    
    ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
    puts("\nNew Queue:");
    showQueue(Head);
    while (Head->next != NULL)
    {
        tmp = Head->next;
        Head->next = tmp->next;
        free(tmp);
    }
    free(Head);
    char exitStr = 0;
    scanf("%c", &exitStr);
    return 0;
}
