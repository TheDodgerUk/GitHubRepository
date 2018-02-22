#ifndef CONSOLELOG_H
#define CONSOLELOG_H
#include <string>
#include <iostream>
#include <winsock2.h>
#include <thread>
#include <string>
#include <deque>
#include <mutex>
#include "StackWalker.h"
#include "ConsoleLog.h"
#pragma comment(lib, "Ws2_32.lib")


class ConsoleLogImpelementation : public ConsoleLog
{
private:
	bool m_SendOutput = true;
    void Setup();
    static void Loop(void*);
    std::thread*    m_Thread;
	std::mutex*		m_Mutex;
    WSADATA         m_WSAData;
    SOCKET          m_Server;
    SOCKADDR_IN     m_Addr;
	int				m_CurrentStringDequeIndex = 0;
    std::deque<std::string> m_StringDeque;
    static bool m_Created;

	std::string ReplaceString(std::string subject, const std::string& search, const std::string& replace);
public:
	ConsoleLogImpelementation() {}
	void Log(std::string lHeader, std::string lString);

};

#endif // CONSOLELOG_H
