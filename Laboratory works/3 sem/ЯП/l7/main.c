#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <string.h>

struct student {
    int group;
    char sName[40];
    int mark;
};
struct list {
    struct student field;
    struct list * next;
};

struct list* addEl(struct list* const Head){
    struct list* el;
    el = (struct list*) malloc(sizeof(struct list));
    el->field.group = 0;
    el->field.mark = 0;
    strcpy(el->field.sName, "");
    el->next = NULL;
    struct list* tmp = Head;
    while (tmp->next != NULL)
        tmp = tmp->next;
    tmp->next = el;
    return el;
}
void setField(struct list* const el, struct student field){
    el->field.group = field.group;
    el->field.mark = field.mark;
    memset(el->field.sName, '\0', 40);
    strncpy(el->field.sName, field.sName, 40);
}
int readNum(){
    printf("%s", "> ");
    int res = 0;
    int code = scanf("%d", &res);
    return res;
}
struct student readField(){
    struct student tmp = {0};
    puts("Enter group:");
    tmp.group=readNum();
    puts("Enter surname:");
    char str[200] = {0};
    printf("%s", "> ");
    scanf("%s", str);
    strncpy(tmp.sName,str,  40);
    puts("Enter last session mark:");
    tmp.mark=readNum();
    return tmp;

}
void showList(struct list* Head) {
    puts("");
    struct list* tmp = Head;
    if (tmp->next==NULL) puts("The list of students is empty!");
        else puts("(Group, Surname, Mark):");
    while (tmp->next != NULL)
    {
        tmp = tmp->next;
        printf("(%d, %s, %d)\n", tmp->field.group, tmp->field.sName, tmp->field.mark);
    }
}
void deleteEl(struct list* prevEl) {
 
    struct list* tmp = prevEl->next;
    prevEl->next = tmp->next;
    free(tmp);
    
}
void saveList(struct list* Head) {
 
    FILE * file;
    file = fopen("file.txt", "wb");
    
    struct list* tmp = Head;
    char * buffer = (char*) malloc( sizeof(int)+40*sizeof(char)+sizeof(int) );
    char * j = buffer;
    char * num = NULL;
    while (tmp->next != NULL)
    {
        tmp = tmp->next;
        j = buffer;
        num = (char*) &(tmp->field.group);
        memset(buffer, '\0', sizeof(int)+40*sizeof(char)+sizeof(int) );
        for (char i = 0; i<sizeof(int); i++)
        {
            *buffer = *num;
            //printf("%c", *buffer);
            buffer++;
            num++;
        }
        num = tmp->field.sName;
        for (char i = 0; i<40; i++)
        {   
            *buffer = *num;
            //printf("%c", *buffer);
            buffer++;
            num++;
        }
        num = (char*) &(tmp->field.mark);
        for (char i = 0; i<sizeof(int); i++)
        {
            *buffer = *num;
            //printf("%c", *buffer);
            buffer++;
            num++;
        }
        buffer -= sizeof(int)+40*sizeof(char)+sizeof(int);
        fwrite(buffer, sizeof(int)+40*sizeof(char)+sizeof(int), 1, file);
        
    }
    free(buffer);
    fclose(file);
    
}
void readFile(struct list* Head) {
    FILE * file;
    file = fopen("file.txt", "rb");
    char * buffer = (char*) malloc( sizeof(int)+40*sizeof(char)+sizeof(int) );
    char * saved = buffer;
    struct list* tmpEl = NULL;
    char * num = NULL;
     
    while (feof(file) == 0)
    {
       // puts("Reading item");
        memset(buffer, '\0', sizeof(int)+40*sizeof(char)+sizeof(int) );
        if (fread(buffer, 2*sizeof(int)+40*sizeof(char), 1, file) == 1)
        { 
        //fread(buffer, 2*sizeof(int)+40*sizeof(char), 1, file);
            
            tmpEl = addEl(Head);
         
            num = (char *) &(tmpEl->field.group);
            for (char i = 0; i<sizeof(int); i++)
            {
            *num = *buffer;
                //printf("%c", *buffer);
                buffer++;
                num++;
            }
            
            num = (char *) &(tmpEl->field.sName);
            for (char i = 0; i<40; i++)
            {
                *num = *buffer;
                buffer++;
                num++;
            }
            
            num = (char *) &(tmpEl->field.mark);
            for (char i = 0; i<sizeof(int); i++)
            {
                *num = *buffer;
                if (i==sizeof(int)-1)
                {
                    buffer++;
                    num++;
                }
            }
            
           
        }
        buffer = saved;
    }
   
    
    free(buffer);
    int code = 0;
    if (file) code = fclose(file);
    
   
    
}
int isEq(struct student field1, struct student field2) {
    if (field1.group == field2.group && field1.mark == field2.mark && strncmp(field1.sName, field2.sName, 40) == 0)
            return 1;
        else
            return 0;
}
struct list* findEl(struct list* Head, struct student field) {
        struct list* tmp = Head;
        while ((tmp->next != NULL) && !isEq(field, tmp->next->field))
        {
            tmp = tmp->next;
        }
        if (tmp->next == NULL) return NULL;
        return tmp;
}
int main()
{
    struct list * Head = NULL;
    Head = (struct list*) malloc(sizeof(struct list));
    Head->field.group = 0;
    (*Head).field.mark = 0;
    strcpy(Head->field.sName, "");
    Head->next = NULL;
    struct list* tmpEl = NULL;
    int code = 0, tmp = 0;
    struct student tmpField = {0};
    FILE* d;
    while (code != 5)
    {
        puts("");
        puts("////////////////////////////////////////");
        puts("");
        puts("Press key:");
        puts("  0 for viewing students");
        puts("  1 for adding new student");
        puts("  2 for selecting and changing student");
        puts("  3 for saving information to file");
        puts("  4 for reading file with information");
        puts("  5 for exit");
        code = readNum();
        switch (code)
        {
            case 0: //viewing list
                showList(Head);
                break;
            case 1:     //adding new students
                tmpEl = addEl(Head);
                tmpField = readField();
                setField(tmpEl, tmpField);
                break;
            case 2:     //selecting
                puts("Enter Student's requisites");
                tmpField = readField();
                tmpEl = findEl(Head, tmpField);
                if (tmpEl != NULL)
                    {
                        puts("Choose what to do with this student, press:");
                        puts("  1 - for re-entering information");
                        puts("  2 - for deleting this student");
                        tmp = readNum();
                        if (tmp == 1) 
                                {
                                    puts("Enter new information:");
                                    tmpField = readField();
                                    
                                    setField(tmpEl->next, tmpField);
                                   
                                    
                                }
                            else
                                {
                                    if (tmp == 2)
                                            deleteEl(tmpEl);
                                        else puts("Incorrect input");
                                        
                                }
                    }
                else
                    {
                        puts("There's no such students!");
                    }
                break;
            case 3:     //saving
                saveList(Head);
                break;
            case 4:     //reading
                while (Head->next != NULL)
                    deleteEl(Head);
                readFile(Head);
                break;
            case 5:
                puts("Goodbye, my good human, wish you success and happy HOLIDAYS!!!");
                char strokadlyascanf; 
                scanf("%c", &strokadlyascanf);
                scanf("%c", &strokadlyascanf);
                exit(0);
                break;
            default:
                puts("Incorrect input!");
                break;
        }


    }


    while (Head->next != NULL)
        deleteEl(Head);
    free(Head);
    return 0;
}
