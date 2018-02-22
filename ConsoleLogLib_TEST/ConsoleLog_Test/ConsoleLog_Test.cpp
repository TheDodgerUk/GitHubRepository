// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "ConsoleLog.h"
#include <iostream>
#include <Windows.h>

void Test2()
{
	ConsoleLog::GetInstance()->Log("99999", "output 11");
	ConsoleLog::GetInstance()->Log("99999", "output 12");
	ConsoleLog::GetInstance()->Log("99999", "output 13");
	ConsoleLog::GetInstance()->Log("99999", "output 14");
	ConsoleLog::GetInstance()->Log("99999", "output 15");

}

void Test1()
{
	Test2();
	ConsoleLog::GetInstance()->Log("99999", "output 16");
	
}
int main()
{
	Test1();
	/*ConsoleLog::GetInstance()->Log("11118811", "output 1");
	ConsoleLog::GetInstance()->Log("222222", "output 2");
	ConsoleLog::GetInstance()->Log("333333", "output 3");
	ConsoleLog::GetInstance()->Log("444444", "output 4");
	Sleep(500);
	ConsoleLog::GetInstance()->Log("111111", "output 11");
	ConsoleLog::GetInstance()->Log("222222", "output 22");
	ConsoleLog::GetInstance()->Log("333333", "output 33");
	ConsoleLog::GetInstance()->Log("444444", "output 44");
	Sleep(500);
	ConsoleLog::GetInstance()->Log("111111", "output 111");
	ConsoleLog::GetInstance()->Log("222222", "output 222");
	ConsoleLog::GetInstance()->Log("333333", "output 333");
	ConsoleLog::GetInstance()->Log("444444", "output 444");
	Test1();
	std::cout << "12 messages have been sent \n";*/
	Sleep(5000);
    return 0;
}

