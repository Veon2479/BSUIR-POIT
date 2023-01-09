#include <stdio.h>
#include <stdlib.h>
#define node struct node
//drank - 4
//i'm - 15
node
{
   int field;
   node *left, *right; 
};


node* create(node* NODE, int field)
{
    NODE = (node*) malloc(sizeof(node));
    NODE->field = field;
    NODE->left = NULL;
    NODE->right = NULL;
    return NODE;
 
}


void treePrint(node* Head)
{
    if (Head != NULL)
    {
        treePrint(Head->right);
        printf("%d ", Head->field);
        treePrint(Head->left);
    }
}

void showSymmTree(node* Head)
{
        puts("LOG: printing tree");
    treePrint(Head->right);
        puts("\nLOG: finished\n");
}

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

void showLine(char* c, int p, int s) {
    int t=s;
    for(int i=0; i<p; i++) 
    {printf(t&1 ? "|  " : "   "); t/=2;}
    printf(c);
}

void showTree(node* Head, int p, int s) {
    //Head = Head->right;
    if (Head == NULL) return;
    printf("%d\n", Head->field);
    
    if (Head->left != NULL)
    {
        showLine("|\n", p, s);
        showLine("L: ", p, s);
        showTree(Head->left, p+1, s + ((Head->right == NULL ? 0 : 1)<<p));
    }
    
    if (Head->right != NULL) {        
        showLine("|\n", p, s);
        showLine("R: ", p, s);
        showTree(Head->right, p+1, s);
    }
        
}

void insert(node** Head, int field)
{
    if (*Head == NULL)
    {
        (*Head) = (node*) malloc(sizeof(node));
        (*Head)->field = field;
        (*Head)->left = NULL;
        (*Head)->right = NULL;
    }
    else if (field <= (*Head)->field) 
            insert( &((*Head)->left), field );
        else
            insert( &((*Head)->right), field );
}

void del(node** Head, node** w, node** q)
{
  
    if ((*w)->right != NULL)
            del(Head, &((*w)->right), q);
        else
        {
            *q = *w;
            (*Head)->field = (*w)->field;
            *w = (*w)->left;
            
        }
    
}


void delNode(node** Head, int field)
{
    
    node* q = NULL;
    if ((*Head) != NULL)
    {
        if (field < (*Head)->field)
            {
                delNode(&(*Head)->left, field);
            }
            else if (field > (*Head)->field)
            {
                delNode(&(*Head)->right, field);
            }
            else
            {
                q = *Head;
                if ((*Head)->right == NULL)
                        (*Head) = (*Head)->left;
                    else
                    {
                        if ((*Head)->left == NULL)
                                (*Head) = (*Head)->right;
                            else 
                                del(Head, &((*Head)->left), &q);
                        
                    }
                free(q);
            }
            
    }
        
    
    
}

int main(int argc, char *argv[])
{
    
    node* Head = NULL;
    Head = create(Head, 0);
    int num = 0, code = 0;
    insert(&(Head->right), 0);
    for (int i=1; i<=7; i++)
    {
        insert(&(Head->right), i); //1-7
        insert(&(Head->right), 50-i); //43-49
      //  insert(&(Head->right), 100-i); //93-99
    }
  /*  for (int i=18; i<=24; i++)
    {
        insert(&(Head->right), i+10); //28-34
        insert(&(Head->right), i);  //18-24
        insert(&(Head->right), 200+i);  //218-224
    }*/
    
       
    showTree(Head->right, 0, 0);
    node* tmp = NULL;
    while (1)
    {
        code = 0;
        printf("\n%s\n", "Enter node's field to delete it or -1 to exit");
        printf("> ");
        scanf("%d", &num);
        if (num == -1)
            break;
        delNode(&(Head->right), num); 
        showTree(Head->right, 0, 0);
                   
    }
    char exitC = 0;
    printf("LOG: preparing for destroying tree..");
    scanf("%c", &exitC);
    scanf("%c", &exitC);
    while (Head->right != NULL)
    {
        delNode(&(Head->right), Head->right->field);
    }
    free(Head);
    return 0;
}
