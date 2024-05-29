#ifndef BARRIER_H
#define BARRIER_H

#include <QMutex>
#include <QWaitCondition>
#include <QSharedPointer>

class Barrier {
public:
    Barrier(int count) : threadsToWait(count), barrierReached(0) {}

    void wait() {
        mutex.lock();
        int generation = barrierGeneration;
        barrierReached++;
        if (barrierReached == threadsToWait) {
            barrierGeneration++;
            barrierReached = 0;
            condition.wakeAll();
        } else {
            while (generation == barrierGeneration) {
                condition.wait(&mutex);
            }
        }
        mutex.unlock();
    }

private:
    QMutex mutex;
    QWaitCondition condition;
    int threadsToWait;
    int barrierReached;
    int barrierGeneration;
};

#endif // BARRIER_H
