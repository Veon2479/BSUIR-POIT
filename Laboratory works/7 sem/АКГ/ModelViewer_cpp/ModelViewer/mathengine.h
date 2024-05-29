#ifndef MATHENGINE_H
#define MATHENGINE_H

#include "model.h"
#include "Barier.h"
#include <QSemaphore>
#include <QVarLengthArray>
#include <condition_variable>
#include <thread>

class MathEngine
{
public:
    MathEngine(QList<Model::vert> *model, QList<Model::vert> *dest);

    struct matrix
    {
        QVarLengthArray<float, 16> items;
        matrix();
        matrix(QVarLengthArray<float, 16> items);
    };

    static void transform_w(const Model::vert& src, Model::vert& dest, matrix *rule);

    void applyTransform_w(matrix *rule);

    matrix getMoveMatrix(float x, float y, float z);
    matrix getScaleMatrix(float x, float y, float z);

    matrix getRotXMatrix(float a);
    matrix getRotYMatrix(float a);
    matrix getRotZMatrix(float a);

    matrix getProjectGlobalMatrix(Model::vert eye, Model::vert target, Model::vert up);
    matrix getProjectPlaneMatrix(float FOV, float aspect, float zfar, float znear);
    matrix getProjectViewMatrix(float width, float height, float xmin, float ymin);

    float scalarMult(Model::vert v1, Model::vert v2);
    Model::vert vectMult(Model::vert v1, Model::vert v2);
    Model::vert normalize3D(Model::vert v);

    MathEngine::matrix multMatrix(const matrix& a, const matrix& b);
    matrix getIdentityMatr();
private:
    int cores;
    QList<std::thread*> workers;
    QList<Model::vert> *src, *dest;
    matrix *rule;
    std::atomic<int> sharedInteger;
    QSemaphore *semaphore;

    Barrier *barrier;

    void worker_unit(int start, int end);


};

#endif // MATHENGINE_H
