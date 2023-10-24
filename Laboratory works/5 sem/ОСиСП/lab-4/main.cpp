#include <iostream>
#include <sys/stat.h>
#include <filesystem>
#include <threadpoolapiset.h>
#include <synchapi.h>
#include <windows.h>
#include <chrono>
#include <fstream>

#include "BlockingCollection.h"
#include "ConcurrentCollection.h"

typedef std::pair<std::wstring, int> item;

BlockingCollection<item> blockingCollection;
ConcurrentCollection<item> concurrentCollection;

std::string TargetString = "";
long TaskCount = 0;

CRITICAL_SECTION criticalSection;
CONDITION_VARIABLE conditionVariable;

VOID CALLBACK CheckFileBlockingCallback(PTP_CALLBACK_INSTANCE Instance,
                                PVOID Parameter,
                                PTP_WORK Work);

VOID CALLBACK CheckFileConcurrentCallback(PTP_CALLBACK_INSTANCE Instance,
                                        PVOID Parameter,
                                        PTP_WORK Work);

void DoWork(VOID CALLBACK Callback(PTP_CALLBACK_INSTANCE Instance,
                                   PVOID Parameter,
                                   PTP_WORK Work), char** argv);

int main(int argc, char *argv[]) {

    if (argc != 3)
    {
        perror("Incorrect number of parameters!");
        return 1;
    }

    if (argv[2][0] == 0)
    {
        perror("Cannot search for an empty string!");
        return 1;
    }

    TargetString = argv[2];

    struct stat info = {0};
    if ( stat(argv[1], &info) != 0 )
    {
        perror("Incorrect path!");
        return 1;
    }

    if (!(info.st_mode & _S_IFDIR))
    {
        perror("Specified file is not directory!");
        return 1;
    }

    InitializeCriticalSection(&criticalSection);
    InitializeConditionVariable(&conditionVariable);

    std::cout << std::endl << "Path: " << argv[1] << std::endl;
    std::cout << "Target string: \"" << TargetString << "\"" << std::endl;

    std::cout << std::endl << "BLocking collection: " << std::endl;
    DoWork(CheckFileBlockingCallback, argv);

    std::cout << std::endl << "Concurrent collection: " << std::endl;
    DoWork(CheckFileConcurrentCallback, argv);


    std::cout << std::endl << "Result of blocking collection: " << std::endl;
    for (int i = 0; i < blockingCollection.GetSize(); i++)
    {
        item t;
        blockingCollection.GetItem(i, &t);
        std::wcout << "in file " << t.first << " was found " << t.second << " target string(s)" << std::endl;
    }

    std::cout << std::endl << "Result of concurrent collection: " << std::endl;
    for (int i = 0; i < concurrentCollection.GetSize(); i++)
    {
        item t;
        concurrentCollection.GetItem(i, &t);
        std::wcout << "in file " << t.first << " was found " << t.second << " target string(s)" << std::endl;
    }

    return 0;
}

int GetStringCount(std::wstring path)
{
    std::ifstream f;
    f.open(path.c_str());

    std::string line;
    int offset;

    int result = 0;

    if (f.is_open())
    {
        while (!f.eof())
        {
            std::getline(f, line);
            if ((offset = line.find(TargetString, 0)) != std::string::npos)
                result++;
        }
        f.close();
    }
    return result;
}

VOID CALLBACK CheckFileBlockingCallback(PTP_CALLBACK_INSTANCE Instance,
                                PVOID Parameter,
                                PTP_WORK Work)
{

    PWCHAR charstr = (PWCHAR) (Parameter);
    std::wstring str = L"";
    int i = 0;
    while (charstr[i] != 0)
    {
        str += (char)charstr[i];
        i++;
    }
//    std::wcout << str << std::endl;
    blockingCollection.Add(item(str, GetStringCount(str)));

    EnterCriticalSection(&criticalSection);
        TaskCount--;
        WakeAllConditionVariable(&conditionVariable);
    LeaveCriticalSection(&criticalSection);
}


VOID CALLBACK CheckFileConcurrentCallback(PTP_CALLBACK_INSTANCE Instance,
                                          PVOID Parameter,
                                          PTP_WORK Work)
{
    PWCHAR charstr = (PWCHAR) (Parameter);
    std::wstring str = L"";
    int i = 0;
    while (charstr[i] != 0)
    {
        str += (char)charstr[i];
        i++;
    }
//    std::wcout << str << std::endl;
    concurrentCollection.Add(item(str, GetStringCount(str)));

    EnterCriticalSection(&criticalSection);
        TaskCount--;
        WakeAllConditionVariable(&conditionVariable);
    LeaveCriticalSection(&criticalSection);
}

void DoWork(VOID CALLBACK Callback(PTP_CALLBACK_INSTANCE Instance,
                                   PVOID Parameter,
                                   PTP_WORK Work), char** argv)
{
    std::chrono::microseconds ms1 = duration_cast<std::chrono::microseconds>(std::chrono::system_clock::now().time_since_epoch());

    for (const auto &entry: std::filesystem::directory_iterator(argv[1]))
    {

        PWCHAR charStr = (PWCHAR) malloc(sizeof(WCHAR) * entry.path().string().length() + 2);
        for (int i = 0; i < entry.path().string().length(); i++)
        {
            charStr[i] = entry.path().string()[i];
        }
        charStr[entry.path().string().length()] = 0;
        EnterCriticalSection(&criticalSection);
        TaskCount++;
        LeaveCriticalSection(&criticalSection);
        SubmitThreadpoolWork(CreateThreadpoolWork(Callback, (PVOID) charStr, nullptr));
    }

    EnterCriticalSection(&criticalSection);
    while(TaskCount != 0)
        SleepConditionVariableCS(&conditionVariable, &criticalSection, INFINITE);
    LeaveCriticalSection(&criticalSection);

    std::chrono::microseconds ms2 = duration_cast<std::chrono::microseconds>(std::chrono::system_clock::now().time_since_epoch());

    std::cout << "Time elapsed: " << (ms2 - ms1).count() << " ms" << std::endl;
}