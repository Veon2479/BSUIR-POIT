#ifndef PARSER_H
#define PARSER_H

#include "model.h"
#include <QFile>

class Parser
{
public:
    Parser();
    void parse(QFile& src, QList<Model>& dest);
private:
    enum entry
    {
        v, vt, vn, f, none
    };
    entry getType(QString line);
};

#endif // PARSER_H
