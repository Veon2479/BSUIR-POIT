//
// Created by Andmin on 23.11.2022.
//

#ifndef OSASP_LAB_4_CONCURRENTCOLLECTION_H
#define OSASP_LAB_4_CONCURRENTCOLLECTION_H


#include <vector>
#include <atomic>

template <typename T>
class ConcurrentCollection {

public:

    void Add(T item)
    {
        while (!std::atomic_flag_test_and_set(&isUsing));
        vector.push_back(item);
        isUsing.clear();
    }

    bool GetItem(int num, T* item)
    {
        bool res = false;
        if (item != nullptr)
        {
            while (!std::atomic_flag_test_and_set(&isUsing));
            if (num >= 0 && num < vector.size())
            {
                *item = vector[num];
                res = true;
            }
            isUsing.clear();
        }
        return res;
    }

    bool DeleteItem(int num)
    {
        bool res = false;
        while (!std::atomic_flag_test_and_set(&isUsing));
        if (num >= 0 && num < vector.size())
        {
            vector.erase(num);
            res = true;
        }
        isUsing.clear();
        return res;
    }

    int GetSize()
    {
        while (!std::atomic_flag_test_and_set(&isUsing));
        int res = vector.size();
        isUsing.clear();
        return res;
    }

private:
    std::vector<T> vector;
    std::atomic_flag isUsing = false;

};


#endif //OSASP_LAB_4_CONCURRENTCOLLECTION_H
