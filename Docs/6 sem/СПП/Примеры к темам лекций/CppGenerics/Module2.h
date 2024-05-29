#include <vector>

class MyIntQueue
{
public:
	void Enqueue(int value);
	int Dequeue();

private:
	std::vector<int> m_array;
};
