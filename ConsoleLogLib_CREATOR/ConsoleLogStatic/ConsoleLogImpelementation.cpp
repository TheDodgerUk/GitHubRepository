#include "ConsoleLogImpelementation.h"
#include <sstream>
#include "StackWalker.h"
#include <vector>
#pragma warning(disable : 4996) 
#define _WINSOCK_DEPRECATED_NO_WARNINGS
bool ConsoleLogImpelementation::m_Created = false;


ConsoleLog* ConsoleLog::GetInstance()
{
	static ConsoleLogImpelementation* INSTANCE = NULL;
	if (INSTANCE == NULL)
	{
		//HACK DEBUG std::cout << "ConsoleLog* ConsoleLog::GetInstance() : " << "\n";
		INSTANCE = new ConsoleLogImpelementation;
	}
	return INSTANCE;
}


void ConsoleLogImpelementation::Log(std::string lHeader, std::string lString)
{
	//HACK DEBUG std::cout << "ConsoleLogImpelementation : " << "\n";
	if (m_SendOutput == false)
	{
		return;
	}
	if (m_Created == false)
	{
		Setup();
		m_Created = true;
	}

	//HACK DEBUG std::cout << "ConsoleLogImpelementation::Log Time : " << "\n";
	// get current time 
	time_t     now = time(0);
	struct tm  tstruct;
	char       buf[80];
	tstruct = *localtime(&now);
	// Visit http://en.cppreference.com/w/cpp/chrono/c/strftime
	// for more information about date/time format
	strftime(buf, sizeof(buf), "%X", &tstruct);
	std::string lTime =std::string(buf);


	// get call stack 
	MyStackWalker lMyStackWalker;
	std::vector<std::string> lStringVector = lMyStackWalker.ShowCallstack();
	std::string lCallStackString;
	for (int i = 2; i < (int)lStringVector.size(); i++) // first 2 are callstack and helpers , not needed
	{
		lCallStackString += lStringVector[i];
	}

	char lDemin = '^';
	std::string lTemp = lHeader + lDemin + lTime + lDemin + lString + lDemin + lCallStackString + lDemin;
	lTemp = ReplaceString(lTemp, "\n", "~");
	m_Mutex->lock();
	m_StringDeque.push_back(lTemp);
	m_Mutex->unlock();
}


std::string ConsoleLogImpelementation::ReplaceString(std::string subject, const std::string& search,
	const std::string& replace) {
	size_t pos = 0;
	while ((pos = subject.find(search, pos)) != std::string::npos) {
		subject.replace(pos, search.length(), replace);
		pos += replace.length();
	}
	return subject;
}

void ConsoleLogImpelementation::Setup()
{
	m_Mutex = new std::mutex;
	m_Addr.sin_addr.s_addr	= inet_addr("127.0.0.1"); // replace the ip with your futur server ip address. If server AND client are running on the same computer, you can use the local ip 127.0.0.1
	m_Addr.sin_family		= AF_INET;
	m_Addr.sin_port			= htons(5555);

	WSAStartup(MAKEWORD(2, 0), &m_WSAData);
	m_Server = socket(AF_INET, SOCK_STREAM, 0);

	m_Thread = new std::thread(ConsoleLogImpelementation::Loop, this);
}


void ConsoleLogImpelementation::Loop(void* p)
{
	const int MAX_MESSAGE_SIZE = 2000;
	ConsoleLogImpelementation* pThis = static_cast<ConsoleLogImpelementation*>(p);
	int lStatus		  = -1;
	while (true)
	{
		Sleep(1000);
		lStatus = connect(pThis->m_Server, (SOCKADDR *)&pThis->m_Addr, sizeof(pThis->m_Addr));
		


		while (lStatus != SOCKET_ERROR)
		{
			Sleep(10);
			pThis->m_Mutex->lock();
			int lDequeSize = (int)pThis->m_StringDeque.size();
			
			// this is incase there are more than 1 to send 
			if (pThis->m_CurrentStringDequeIndex < lDequeSize)
			{
				std::string lString = pThis->m_StringDeque[pThis->m_CurrentStringDequeIndex];
				lStatus = send(pThis->m_Server, lString.c_str(), lString.length(), 0);
				if (lStatus != SOCKET_ERROR)
				{
					pThis->m_CurrentStringDequeIndex++;
				}
			}
			pThis->m_Mutex->unlock();
		}
		
	}


	closesocket(pThis->m_Server);
	WSACleanup();
}

