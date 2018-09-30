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
#if _DEBUG && VERBOSE_
		std::cout << "HooksUnmanaged.dll DLL_PROCESS_ATTACH" << std::endl;
#endif
	case DLL_THREAD_ATTACH:
#if _DEBUG && VERBOSE_
		std::cout << "HooksUnmanaged.dll DLL_THREAD_ATTACH" << std::endl;
#endif
	case DLL_THREAD_DETACH:
#if _DEBUG && VERBOSE_
		std::cout << "HooksUnmanaged.dll DLL_THREAD_DETACH" << std::endl;
#endif
	case DLL_PROCESS_DETACH:
#if _DEBUG && VERBOSE_
		std::cout << "HooksUnmanaged.dll DLL_PROCESS_DETACH" << std::endl;
#endif
		break;
	}
	return TRUE;
}

