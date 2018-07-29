// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		//std::cout << "HooksUnmanaged.dll DLL_PROCESS_ATTACH" << std::endl;
	case DLL_THREAD_ATTACH:
		//std::cout << "HooksUnmanaged.dll DLL_THREAD_ATTACH" << std::endl;
	case DLL_THREAD_DETACH:
		//std::cout << "HooksUnmanaged.dll DLL_THREAD_DETACH" << std::endl;
	case DLL_PROCESS_DETACH:
		//std::cout << "HooksUnmanaged.dll DLL_PROCESS_DETACH" << std::endl;
		break;
	}
	return TRUE;
}

