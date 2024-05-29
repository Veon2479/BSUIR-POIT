#include <vector>

class MyIntArray
{
public:
	void Add(int value);
	void Remove(int index);
	int GetItem(int index);

private:
	std::vector<int> m_array;
};
