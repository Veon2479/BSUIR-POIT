#include "Module2.h"

void MyIntQueue::Enqueue(int value)
{
	m_array.push_back(value);
}

int MyIntQueue::Dequeue()
{
	int result = *(m_array.begin());
	m_array.erase(m_array.begin());
	return result;
}
