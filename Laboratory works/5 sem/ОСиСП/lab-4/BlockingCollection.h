//
// Created by Andmin on 23.11.2022.
//

#ifndef OSASP_LAB_4_BLOCKINGCOLLECTION_H
#define OSASP_LAB_4_BLOCKINGCOLLECTION_H

#include <vector>
#include <mutex>

template <typename T>
class BlockingCollection {

public:

    void Add(T item)
    {
        mutex.lock();
        vector.push_back(item);
        mutex.unlock();
    }

    bool GetItem(int num, T* item)
    {
        if (item != nullptr) {
            bool flag = false;
            mutex.lock();
            if (num >= 0 && num < vector.size()) {
                *item = vector[num];
                flag = true;
            }
            mutex.unlock();
            return flag;
        }
        else return false;
    }

    bool DeleteItem(int num)
    {
        bool flag = false;
        mutex.lock();
        if (num >= 0 && num < vector.size())
        {
            vector.erase(num);
            flag = true;
        }
        mutex.unlock();
        return flag;
    }

    int GetSize()
    {
        mutex.lock();
        int res = vector.size();
        mutex.unlock();
        return res;
    }

private:
    std::vector<T> vector;
    std::mutex mutex;
};


#endif //OSASP_LAB_4_BLOCKINGCOLLECTION_H
