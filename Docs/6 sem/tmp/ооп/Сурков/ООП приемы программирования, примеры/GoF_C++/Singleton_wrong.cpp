#include <iostream>
#include <fstream>

////////////////////////////////////////////////////////////////////////////////

class Config
{
public:
	std::string logPath()
	{
		return "c:\\logs.txt";
	}
};

Config gConfig;

////////////////////////////////////////////////////////////////////////////////

class Logger
{
	std::string _logPath;

public:
	Logger()
	{
		_logPath = gConfig.logPath();
	}

	void log(const char* message)
	{
		std::fstream(_logPath.c_str(), std::ios_base::app | std::ios_base::out) << message << std::endl;
	}
};

Logger gLogger;

////////////////////////////////////////////////////////////////////////////////

int main()
{
	gLogger.log("hello!");
	
	return 0;
}