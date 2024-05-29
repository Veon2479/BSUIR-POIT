#ifndef MODEL_H
#define MODEL_H

#include <QList>
#include <vector>

class Model
{
public:
    Model();

    struct vert
    {
        float x, y, z, w;
    };

    struct vert_t
    {
        float u, v, w;
    };

    struct vert_n
    {
        float i, j, k;
    };

    struct pol_item
    {
        int v, vt, vn;
    };

    struct pol
    {
        QList<pol_item> items;
    };

    QList<vert> verts;
    QList<vert_t> vert_ts;
    QList<vert_n> vert_ns;
    QList<pol> pols;

};

#endif // MODEL_H
