#include "mathengine.h"

#include <QFuture>
#include <QRunnable>
#include <QThread>
#include <QtConcurrent>
#include <QtGlobal>
#include <qvector2d.h>
#include <thread>

void MathEngine::worker_unit(int start, int end)
{
    while (true)
    {
        semaphore->acquire();
        qDebug() << "sem acquired";
        for (int i = start; i < end; i++)
        {
            MathEngine::transform_w((*src)[i], (*dest)[i], rule);
        }
        sharedInteger--;
        qDebug() << "barrier reached!";
        barrier->wait();
    }

}

MathEngine::MathEngine(QList<Model::vert> *model, QList<Model::vert> *dest)
{
    cores = QThread::idealThreadCount() - 1;
    this->src = model;
    this->dest = dest;

    int segSize = model->size() / cores;
    int start = 0, end = 0;
    barrier = new Barrier(cores);
    semaphore = new QSemaphore(cores);

    sharedInteger = 0;
    for (int i = 0; i < cores; i++)
    {
        start = i * segSize;
        end = (i == cores - 1) ? model->size() : (start + segSize);
//        workers.push_back(std::thread(worker_unit, start, end));
        workers.push_back(new std::thread([this, start, end]() {
            this->worker_unit(start, end);
        }));
    }
}

// assume the model is only one in the list!
void MathEngine::applyTransform_w(matrix *rule)
{

    this->rule = rule;
    this->sharedInteger = cores + 1;
    semaphore->release();
    barrier->wait();


}


void MathEngine::transform_w(const Model::vert &src, Model::vert &dest, matrix *rule)
{
    dest.w = rule->items[12] * src.x +
             rule->items[13] * src.y +
             rule->items[14] * src.z +
             rule->items[15] * src.w;
    dest.x = (rule->items[0] * src.x +
              rule->items[1] * src.y +
              rule->items[2] * src.z +
              rule->items[3] * src.w) / dest.w;
    dest.y = (rule->items[4] * src.x +
              rule->items[5] * src.y +
              rule->items[6] * src.z +
              rule->items[7] * src.w) / dest.w;
    dest.z = (rule->items[8] * src.x +
              rule->items[9] * src.y +
              rule->items[10] * src.z +
              rule->items[11] * src.w) / dest.w;
        dest.w = 1;
}

//void modifyLists(const QList<Model::vert> &model, QList<Model::vert> &dest, MathEngine::matrix &rule, int start, int end)
//{
//    for (int i = start; i < end; i++)
//    {
//        MathEngine::transform_w(model[i], dest[i], rule);
//    }
//}



 MathEngine::matrix MathEngine::getMoveMatrix(float x, float y, float z)
{
    QVarLengthArray<float, 16> items = {1, 0, 0, x,
                                        0, 1, 0, y,
                                        0, 0, 1, z,
                                        0, 0, 0, 1};
    return matrix(items);
}

 MathEngine::matrix MathEngine::getScaleMatrix(float x, float y, float z)
{
    QVarLengthArray<float, 16> items = {x, 0, 0, 0,
                                        0, y, 0, 0,
                                        0, 0, z, 0,
                                        0, 0, 0, 1};
    return matrix(items);
}

 MathEngine::matrix MathEngine::getRotXMatrix(float a)
{
    QVarLengthArray<float, 16> items = {1, 0, 0, 0,
                                        0, std::cos(a), -std::sin(a), 0,
                                        0, std::sin(a), std::cos(a), 0,
                                        0, 0, 0, 1};
    return matrix(items);
}

 MathEngine::matrix MathEngine::getRotYMatrix(float a)
{
    QVarLengthArray<float, 16> items = {std::cos(a), 0, std::sin(a), 0,
                                        0, 1, 0, 0,
                                        -std::sin(a), 0, std::cos(a), 0,
                                        0, 0, 0, 1};
    return matrix(items);
}

 MathEngine::matrix MathEngine::getRotZMatrix(float a)
{
    QVarLengthArray<float, 16> items = {std::cos(a), -std::sin(a), 0, 0,
                                        std::sin(a), std::cos(a), 0, 0,
                                        0, 0, 1, 0,
                                        0, 0, 0, 1};
    return matrix(items);
}

 MathEngine::matrix MathEngine::getProjectGlobalMatrix(Model::vert eye, Model::vert target, Model::vert up)
{
    Model::vert axisX, axisY, axisZ;

    axisX.w = 0;
    axisY.w = 0;
    axisZ.w = 0;

    axisZ.x = eye.x - target.x;
    axisZ.y = eye.y - target.y;
    axisZ.z = eye.z - target.z;
    axisZ = normalize3D(axisZ);

    axisX = normalize3D(vectMult(up, axisZ));

    axisY = normalize3D(vectMult(axisZ, axisX));
    QVarLengthArray<float, 16> items = {axisX.x, axisX.y, axisX.z, -scalarMult(axisX, eye),
                                        axisY.x, axisY.y, axisY.z, -scalarMult(axisY, eye),
                                        axisZ.x, axisZ.y, axisZ.z, -scalarMult(axisZ, eye),
                                        0, 0, 0, 1};
    matrix rule = matrix(items);
    return rule;
}

MathEngine::matrix MathEngine::getProjectPlaneMatrix(float FOV, float aspect, float zfar, float znear)
{
    QVarLengthArray<float, 16> items = {1/(aspect * std::tan(FOV/2)), 0, 0, 0,
                                        0, 1/(std::tan(FOV/2)), 0, 0,
                                        0, 0, zfar/(znear - zfar), znear/(znear-zfar)*zfar,
                                        0, 0, -1, 0};
    return matrix(items);
}

MathEngine::matrix MathEngine::getProjectViewMatrix(float width, float height, float xmin, float ymin)
{
    QVarLengthArray<float, 16> items = {width/2, 0, 0, xmin + (width/2),
                                        0, -height/2, 0, ymin + (height/2),
                                        0, 0, 1, 0,
                                        0, 0, 0, 1};
    return matrix(items);
}

float MathEngine::scalarMult(Model::vert v1, Model::vert v2)
{
    return (v1.x*v2.x + v1.y*v2.y + v1.z*v2.z);
}

Model::vert MathEngine::vectMult(Model::vert v1, Model::vert v2)
{
    QVector3D p1, p2, res;
    p1 = {v1.x, v1.y, v1.z};
    p2 = {v2.x, v2.y, v2.z};
    res = QVector3D::crossProduct(p1, p2);
    Model::vert dest;
    dest.x = res.x();
    dest.y = res.y();
    dest.z = res.z();
    return dest;
}

Model::vert MathEngine::normalize3D(Model::vert v)
{
    Model::vert dest;
    float w = std::sqrt(v.x*v.x + v.y*v.y + v.z*v.z);
    dest.x = v.x / w;
    dest.y = v.y / w;
    dest.z = v.z / w;
    dest.w = 0;
    return dest;
}

MathEngine::matrix MathEngine::multMatrix(const matrix& a, const matrix& b)
{
    auto items = QVarLengthArray<float, 16>(16);
    int i = 0, j = 0;
    for (int k = 0; k < 16; k++)
    {
        items[k] = 0;
        i = k / 4;
        j = k % 4;
        for (int d = 0; d < 4; d++)
        {
            items[k] += a.items[i*4 + d] * b.items[d*4 + j];
        }
    }
    return matrix(items);
}

MathEngine::matrix MathEngine::getIdentityMatr()
{
    QVarLengthArray<float, 16> items = {1, 0, 0, 0,
                                        0, 1, 0, 0,
                                        0, 0, 1, 0,
                                        0, 0, 0, 1};
    return matrix(items);

}


MathEngine::matrix::matrix()
{

}

MathEngine::matrix::matrix(QVarLengthArray<float, 16> items)
{
    this->items = items;
}
