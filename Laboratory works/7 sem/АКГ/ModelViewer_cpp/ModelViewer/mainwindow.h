#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include "parser.h"
#include "model.h"
#include "controller.h"
#include <QMainWindow>
#include <QtWidgets>


QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

protected:
    void paintEvent(QPaintEvent *event) override;
    void keyPressEvent(QKeyEvent *event) override;

private:
    Ui::MainWindow *ui;

    QGraphicsScene *scene;
    QImage* image;

    Controller *controller = nullptr;
    Parser parser;
    QList<Model> models;

};
#endif // MAINWINDOW_H
