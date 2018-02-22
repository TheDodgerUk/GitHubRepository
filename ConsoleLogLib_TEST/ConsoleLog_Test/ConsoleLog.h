#pragma once
#include <string>

class ConsoleLog
{
public:
	static ConsoleLog* GetInstance();
	virtual void Log(std::string lHeader, std::string lString) = 0;

};