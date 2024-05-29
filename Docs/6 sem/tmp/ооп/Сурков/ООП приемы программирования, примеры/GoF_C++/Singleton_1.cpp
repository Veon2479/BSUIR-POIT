#include <iostream>
#include <fstream>
#include <memory>

////////////////////////////////////////////////////////////////////////////////

class Config
{
	static std::unique_ptr<Config> _instance;

public:
	static Config* instance()
	{
		if (!_instance)
		{
			_instance.reset(new Config());
		}

		return _instance.get();
	}

	static void destroy()
	{
		_instance.reset(nullptr);
	}

	std::string logPath()
	{
		return "c:\\logs.txt";
	}
};

std::unique_ptr<Config> Config::_instance;

////////////////////////////////////////////////////////////////////////////////

class Logger
{
	static std::unique_ptr<Logger> _instance;
	std::string _logPath;

	Logger()
	{
		_logPath = Config::instance()->logPath();
	}

public:
	static Logger* instance()
	{
		if (!_instance)
		{
			_instance.reset(new Logger());
		}

		return _instance.get();
	}

	static void destroy()
	{
		_instance.reset(nullptr);
	}

	void log(const char* message)
	{
		std::fstream(_logPath.c_str(), std::ios_base::app | std::ios_base::out) << message << std::endl;
	}
};

std::unique_ptr<Logger> Logger::_instance;

////////////////////////////////////////////////////////////////////////////////

int main()
{
	Logger::instance()->log("hello!");
	
	return 0;
}