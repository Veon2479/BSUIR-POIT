#ifndef CONTROLLER_H
#define CONTROLLER_H

#include "mathengine.h"
#include <QImage>
#include <QKeyEvent>
#include <QFuture>
#include <QtConcurrent>

class Controller
{
public:
    Controller();
    Controller(QImage* image, QList<Model>* models);
    void extracted();
    void redraw();

    void updateScale(float ds);
    void updateRot(float dx, float dy, float dz);
    void updateMov(float dx, float dy, float dz);

private:
    QImage* image;
    QSize size;
    QList<Model>* models;
    QList<QList<Model::vert>> warped;

    MathEngine *engine;

    Qt::GlobalColor bkg, px;

    static void drawLine(QImage *image, Qt::GlobalColor px, float x1, float y1, float x2, float y2);

    MathEngine::matrix viewport, projection, projectGlobal, cached;

    Model::vert eye, target, up;

    float scale, rx, ry, rz, mx, my, mz;

};

#endif // CONTROLLER_H
