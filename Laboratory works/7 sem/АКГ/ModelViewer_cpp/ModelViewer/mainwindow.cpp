#include "mainwindow.h"
#include "./ui_mainwindow.h"

#include <QPainter>
#include <QPixmap>



MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    QFile input("./model.obj");
    parser.parse(input, models);

    scene = new QGraphicsScene(this);
    ui->graphicsView->setScene(scene);

    QSize size = QApplication::primaryScreen()->geometry().size();
    float scale = 1;
    size.setHeight((int)(size.height() * scale));
    size.setWidth((int)(size.width() * scale));
    image = new QImage(size, QImage::Format_RGB32);
    controller = new Controller(image, &models);

    this->setFocus();

}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::paintEvent(QPaintEvent *event)
{
    if (controller != nullptr)
        controller->redraw();
    scene->addPixmap(QPixmap::fromImage(*image));
    scene->setSceneRect(image->rect());
//    ui->graphicsView->fitInView(scene->sceneRect(), Qt::KeepAspectRatio);

    ui->graphicsView->fitInView(scene->sceneRect(), Qt::IgnoreAspectRatio);

}

void MainWindow::keyPressEvent(QKeyEvent *event)
{
    float rotStep = 0.025, movStep = 0.025, scaleStep = 0.025;
    switch (event->key())
    {
        case Qt::Key_Plus:
        {
            controller->updateScale(scaleStep);
            break;
        }
        case Qt::Key_Minus:
        {
            controller->updateScale(-scaleStep);
            break;
        }

        case Qt::Key_Q:
        {
            controller->updateMov(movStep, 0, 0);
            break;
        }
        case Qt::Key_A:
        {
            controller->updateMov(-movStep, 0, 0);
            break;
        }
        case Qt::Key_W:
        {
            controller->updateMov(0, movStep, 0);
            break;
        }
        case Qt::Key_S:
        {
            controller->updateMov(0, -movStep, 0);
            break;
        }
        case Qt::Key_E:
        {
            controller->updateMov(0, 0, movStep);
            break;
        }
        case Qt::Key_D:
        {
            controller->updateMov(0, 0, -movStep);
            break;
        }

        case Qt::Key_Y:
        {
            controller->updateRot(rotStep, 0, 0);
            break;
        }
        case Qt::Key_H:
        {
            controller->updateRot(-rotStep, 0, 0);
            break;
        }
        case Qt::Key_U:
        {
            controller->updateRot(0, rotStep, 0);
            break;
        }
        case Qt::Key_J:
        {
            controller->updateRot(0, -rotStep, 0);
            break;
        }
        case Qt::Key_I:
        {
            controller->updateRot(0, 0, rotStep);
            break;
        }
        case Qt::Key_K:
        {
            controller->updateRot(0, 0, -rotStep);
            break;
        }



        default:
            break;
    }
    controller->redraw();
    this->update();
}


