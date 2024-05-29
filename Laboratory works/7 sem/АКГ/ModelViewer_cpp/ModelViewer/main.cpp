#include "mainwindow.h"

#include <QApplication>

#include <QDebug>



int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    MainWindow w;
    w.setWindowState(Qt::WindowFullScreen);
    const QRect screenGeometry = QApplication::primaryScreen()->geometry();
    w.resize(screenGeometry.size());
    w.show();
    return a.exec();
}
