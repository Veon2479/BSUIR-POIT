#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define node struct node

node 
{
    char field;
    node *right, *left;
};

node* init()
{
    node* res = (node*) malloc(sizeof(node));
    res->field = 0;
    res->left = NULL;
    res->right = NULL;
    return res;
}


void showLine(char* c, int p, int s) {
    int t=s;
    for(int i=0; i<p; i++) 
    {printf(t&1 ? "|  " : "   "); t/=2;}
    printf(c);
}

void showTree(node* Head, int p, int s) {
    //Head = Head->right;
    if (Head == NULL) return;
    printf("%c\n", Head->field);
    
    
    
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

void treePrint(node* Head)
{
    if (Head != NULL)
    {
        treePrint(Head->left);
        treePrint(Head->right);
        printf("%c", Head->field);
    }
}


int getPr(char ch)
{
    if (ch == '+' || ch == '-') return 1;
    if (ch == '*' || ch == '/') return 2;
    if (ch == '^') return 3;
    return 4;
}

void findRoot(node ** root, int l, int r, char str[40])
{
    char t = 0;
    int minPr = 5, minPos = 0, brCounter = 0, i = l;
    if (r-l <= 1)
    {
        if (l==r)
                (*root)->field = str[l];
            else
                if (str[l] == '(' || str[l] == ')')
                        (*root)->field = str[r];
                    else
                        (*root)->field = str[l];
    }
        else
        {
            minPr = 5, minPos = 0, brCounter = 0, i = l;
            while (i<=r)
            {
                t = str[i];
                if (str[i] == '(')
                {
                    brCounter++;
                    i++;
                }
                while (brCounter != 0)
                {
                    t = str[i];
                    
                    if (str[i] == '(') brCounter++;
                        else if (str[i] == ')') brCounter--;
                    i++;
                }
                if (i<=r)
                {
                    t = str[i];
                        if (t != ')' && getPr(t) <= minPr)
                    {
                        minPr = getPr(t);
                        minPos = i;
                    }
                    
                    
                   
                }
                if (i >= r && minPr == 5)
                    {
                        i = l;
                        l++;
                        r--;
                    }
                    
                if (i>=r && minPr == 3)
                {
                    minPos = l;
                    while (getPr(str[minPos]) != minPr)
                        minPos++;
                }
                
                i++;
                
            }
          //  if (minPr == 3 && str[minPos-2] == '^')
          //      minPos -= 2;
            (*root)->field = str[minPos];
            (*root)->left = init();
            (*root)->right = init();
            findRoot(&((*root)->left), l, minPos-1, str);
            findRoot(&((*root)->right), minPos+1, r, str);
        }
}



int main(int argc, char *argv[])
{
    char str[40] = {"(a+b*c*(y-g*d)^n^k+s*l)"};
    //char str[40] = {"((a+d)^b^c)"};
    //char str[40] = {"a+b"};
    printf("Input line is: %s\n", str);
    int len = strlen(str)-1;
    node* root = init();
    findRoot(&root, 0, len, str);
    printf("Tree:\n");
    showTree(root, 0, 0);
    printf("\nPostfix Notation:\n");
    treePrint(root);
    
    
    char exitC = 0;
    scanf("%c", &exitC);
//    scanf("%c", &exitC);
    return 0;
}
