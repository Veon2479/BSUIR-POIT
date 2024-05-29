#include "parser.h"
#include <QTextStream>
#include <QDebug>

Parser::Parser()
{

}

void Parser::parse(QFile &src, QList<Model> &dest)
{
    if (!src.open(QIODevice::ReadOnly | QIODevice::Text))
        return;

    entry lastEntry = v, entry = v;
    Model model;
    float max = -1000000000;

    QTextStream in(&src);
    while (!in.atEnd())
    {
        QString line = in.readLine();
        entry = getType(line);
        if (entry != none)
        {
            if (entry == v && lastEntry != v)
            {
                dest.push_back(model);
                model = Model();
            }
            switch (entry)
            {
                case v:
                case vt:
                case vn:
                {
                    QStringList tokens = line.split(' ');
                    QList<float> nums;
                    foreach (const QString& token, tokens)
                    {
                        if (token != "v" || token != "vt" || token != "vn")
                        {
                            bool ok = true;
                            float number = token.toFloat(&ok);
                            if (ok)
                            {
                                nums.push_back(number);
                            }
                        }
                    }
                    switch (entry)
                    {
                        case v:
                        {
                            while (nums.size() != 4)
                                nums.push_back(1);
                            Model::vert t;
                            t.x = nums[0];
                            t.y = nums[1];
                            t.z = nums[2];
                            t.w = nums[3];
                            float mx = std::abs(t.x);
                            if (mx < std::abs(t.y))
                                mx = std::abs(t.y);
                            if (mx < std::abs(t.z))
                                mx = std::abs(t.z);
                            if (mx > max)
                                max = mx;
                            model.verts.push_back(t);
                        }
                            break;
                        case vt:
                        {
                            while (nums.size() != 3)
                                nums.push_back(0);
                            Model::vert_t t;
                            t.u = nums[0];
                            t.v = nums[1];
                            t.w = nums[2];
                            model.vert_ts.push_back(t);
                        }
                        case vn:
                        {
                            while (nums.size() != 3)
                                nums.push_back(std::rand());
                            Model::vert_n t;
                            t.i = nums[0];
                            t.j = nums[1];
                            t.k = nums[2];
                            model.vert_ns.push_back(t);
                        }
                        default:
                            break;
                    }
                }
                    break;
                case f:
                {
                    QStringList tokens = line.split(' ');
                    Model::pol polygon;
                    float index;
                    foreach (const QString& token, tokens)
                    {
                        if (token != "f")
                        {
                            Model::pol_item polygon_item = {};
                            bool ok;
                            if (token != 'f')
                            {
                                QStringList items = token.split('/');
                                polygon_item.v = items[0].toFloat(&ok) - 1;
                                int len = items[0].length();
                                if (token[len] != '/')
                                {
                                    polygon_item.vt = items[1].toFloat(&ok) - 1;
                                    polygon_item.vn = -1;
                                }
                                else
                                {
                                    if (items.length() != 3)
                                    {
                                        polygon_item.vt = -1;
                                        polygon_item.vn = items[1].toFloat(&ok) - 1;
                                    }
                                    else
                                    {
                                        polygon_item.vt = items[1].toFloat(&ok) - 1;
                                        polygon_item.vn = items[2].toFloat(&ok) -1;
                                    }
                                }

                            }
                            polygon.items.push_back(polygon_item);
                        }

                    }
                    model.pols.push_back(polygon);
                }
                    break;
                default:
                    break;
            }
            lastEntry = entry;
        }
    }
    dest.push_back(model);
    int num = 0;
    int verts = 0;
    for (int j = 0; j < dest.size(); j++)
    {
        if (max != 0)
            for (int i = 0; i < dest[j].verts.size(); i++)
            {
                dest[j].verts[i].x /= max;
                dest[j].verts[i].y /= max;
                dest[j].verts[i].z /= max;

            }
//        for (int i = 0; i < dest[j].pols.size(); i++)
//        {
//            for (int k = 0; k < dest[j].pols[i].items.size(); k++)
//            {
//                dest[j].pols[i].items[k].v -= verts;
//                dest[j].pols[i].items[k].vn -= verts;
//                dest[j].pols[i].items[k].vt -= verts;

//            }
//        }

        verts += dest[j].verts.size();
        qDebug() << "Loaded" << dest[j].pols.size() << "polygons";
        num += dest[j].pols.size();
    }

    // convert to the single-model store
    model = Model();
    for (int i = 0; i < dest.length(); i++)
    {
        model.verts.append(dest[i].verts);
        model.vert_ns.append(dest[i].vert_ns);
        model.vert_ts.append(dest[i].vert_ts);
        model.pols.append(dest[i].pols);
    }
    dest.clear();
    dest.push_back(model);

    qDebug() << "Total:" << num << "polygons";
}

Parser::entry Parser::getType(QString line)
{
    if (line.length() >= 3)
    {
        if (line[0] == 'v' && line[1] == ' ')
            return v;
        if (line[0] == 'v' && line[1] == 't' && line[2] == ' ')
            return vt;
        if (line[0] == 'v' && line[1] == 'n' && line[2] == ' ')
            return vn;
        if (line[0] == 'f' && line[1] == ' ')
            return f;
    }
    return none;
}
