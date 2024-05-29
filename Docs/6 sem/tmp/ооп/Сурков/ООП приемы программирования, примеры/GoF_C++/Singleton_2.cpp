#include <iostream>
#include <fstream>

////////////////////////////////////////////////////////////////////////////////

class Config
{
	Config() {}

public:
	static Config* instance()
	{
		static Config config;
		return &config;
	}

	std::string logPath()
	{
		return "c:\\logs.txt";
	}
};

////////////////////////////////////////////////////////////////////////////////

class Logger
{
	std::string _logPath;

	Logger()
	{
		_logPath = Config::instance()->logPath();
	}

public:
	static Logger* instance()
	{
		static Logger logger;
		return &logger;
	}

	void log(const char* message)
	{
		std::fstream(_logPath.c_str(), std::ios_base::app | std::ios_base::out) << message << std::endl;
	}
};

////////////////////////////////////////////////////////////////////////////////

int main()
{
	Logger::instance()->log("hello!");
	
	return 0;
}