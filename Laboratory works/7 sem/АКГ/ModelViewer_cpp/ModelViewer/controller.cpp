#include "controller.h"

Controller::Controller()
{
    engine = new MathEngine(new QList<Model::vert>(), new QList<Model::vert>());
}

Controller::Controller(QImage* image, QList<Model>* models)
{
    this->models = models;
    for (Model model : *models)
    {
        warped.push_back(QList<Model::vert>(model.verts.size()));
    }

    engine = new MathEngine(&(*models)[0].verts, &(warped[0]));


    this->image = image;
    size = image->size();
    this->bkg = Qt::black;
    this->px = Qt::white;
    QSize size = image->size();
    this->viewport = engine->getProjectViewMatrix(size.width(), size.height(), 0, 0);
    float fov = 3.1415 / 2;
    float zfar = 100, znear = 1;
    float aspect = float(size.width()) / size.height();
    this->projection = engine->getProjectPlaneMatrix(fov, aspect, zfar, znear);
    eye = {0};
    eye.x = 0;
    eye.y = 0;
    eye.z = 1;
    target = {0};
    target.x = 0.0;
    target.y = 0.0;
    target.z = 0.0;
    up = {0};
    up.y = 1;
    projectGlobal = engine->getProjectGlobalMatrix(eye, target, up); //view

    cached = engine->multMatrix(viewport, projection);
    cached = engine->multMatrix(cached, projectGlobal);

    scale = 1;
    rx = 0, ry = 0, rz = 0;
    mx = 0, my = 0, mz = 0;


}

//for rasterization - do it with int variables, no floats!!!!!!
// pick the highest and lowest y value of polygon verts
// compute bounding points on the polygon borders
// write horizontal lines!!!!!!

void Controller::redraw()
{
    image->fill(bkg);

    MathEngine::matrix rule = engine->getMoveMatrix(mx, my, mz);
    rule = engine->multMatrix(engine->getRotZMatrix(rz), rule);
    rule = engine->multMatrix(engine->getRotYMatrix(ry), rule);
    rule = engine->multMatrix(engine->getRotXMatrix(rx), rule);
    rule = engine->multMatrix(engine->getScaleMatrix(scale, scale, scale), rule);

    rule = engine->multMatrix(cached, rule);

    for (int i = 0; i < warped.size(); i++)
    {
        engine->applyTransform_w(&rule);
    }

    // Plain polygon rendering!
    int modelcnt = 0;
    foreach(Model model, *models)
    {
        int polcnt = 0;
        foreach (Model::pol pol, model.pols)
        {
            for (int i = 0; i < pol.items.size() - 1; i++)
            {
                Model::vert& v1 = warped[modelcnt][pol.items[i].v];
                Model::vert& v2 = warped[modelcnt][pol.items[i + 1].v];
                drawLine(image, px, v1.x, v1.y, v2.x, v2.y);
            }
            Model::vert& v1 = warped[modelcnt][pol.items[0].v];
            Model::vert& v2 = warped[modelcnt][pol.items[pol.items.size() - 1].v];
            drawLine(image, px, v1.x, v1.y, v2.x, v2.y);
            polcnt++;
        }
        modelcnt++;
    }

    //        // Multi-thread polygon rendering!!! - much slower
    //    // not thread-safe access to the QImage* image is doing brrrrrrr

    //    QList<QFuture<void>> waitList;

    //    int modelcnt = 0;
    //    foreach(Model model, *models)
    //    {
    //        int polcnt = 0;
    //        foreach (Model::pol pol, model.pols)
    //        {
    //            for (int i = 0; i < pol.items.size() - 1; i++)
    //            {
    //                Model::vert& v1 = viewported[modelcnt][pol.items[i].v];
    //                Model::vert& v2 = viewported[modelcnt][pol.items[i + 1].v];
    //                QFuture<void> future = QtConcurrent::run(drawLine, image, px, v1.x, v1.y, v2.x, v2.y);
    //                waitList.push_back(future);
    //            }
    //            polcnt++;
    //        }
    //        modelcnt++;
    //    }

    //    foreach (QFuture<void> future, waitList)
    //    {
    //        future.waitForFinished();
    //    }

}

void Controller::drawLine(QImage* image, Qt::GlobalColor px, float x1, float y1, float x2, float y2)
{
    int cx1 = (int)(x1), cx2 = (int)(x2), cy1 = (int)(y1), cy2 = (int)(y2);
    const int deltaX = abs(cx2 - cx1);
    const int deltaY = abs(cy2 - cy1);
    const int signX = cx1 < cx2 ? 1 : -1;
    const int signY = cy1 < cy2 ? 1 : -1;
    int error = deltaX - deltaY;
    if (cx2 >= 0 && cx2 < image->size().width() && cy2 >= 0 && cy2 < image->size().height())
        image->setPixelColor(cx2, cy2, px);
    while(cx1 != cx2 || cy1 != cy2)
    {
        if (cx1 >= 0 && cx1 < image->size().width() && cy1 >= 0 && cy1 < image->size().height())
            image->setPixelColor(cx1, cy1, px);
        int error2 = error * 2;
        if(error2 > -deltaY)
        {
            error -= deltaY;
            cx1 += signX;
        }
        if(error2 < deltaX)
        {
            error += deltaX;
            cy1 += signY;
        }
    }


}

void Controller::updateScale(float ds)
{
    scale += ds;
}

void Controller::updateRot(float dx, float dy, float dz)
{
    rx += dx;
    ry += dy;
    rz += dz;
}

void Controller::updateMov(float dx, float dy, float dz)
{
    mx += dx;
    my += dy;
    mz += dz;
}

