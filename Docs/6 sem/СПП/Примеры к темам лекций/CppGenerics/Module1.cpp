#include "Module1.h"

void MyIntArray::Add(int value)
{
	m_array.push_back(value);
}

void MyIntArray::Remove(int index)
{
	m_array.erase(m_array.begin() + index);
}

int MyIntArray::GetItem(int index)
{
	return m_array.at(index);
}
