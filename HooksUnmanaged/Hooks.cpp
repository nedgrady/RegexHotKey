#include "Hooks.h"

const std::string HOOK_DLL_LOCATION = "H:\\Code\\RegexHotKey\\Debug\\Hook.dll";
const std::string HOOK_PROC = "HookProc";

std::vector<CharCallback_t> _subscribers = std::vector<CharCallback_t>();

int main(int argc, char* argv[])
{
	try
	{
		CreateHook();
	}
	catch (const windows_exception& ex)
	{
		std::cout << ex.what() << std::endl;
	}

	MSG message;
	while (GetMessage(&message, NULL, 0, 0))
	{
		TranslateMessage(&message);
		DispatchMessage(&message);
	}
	//UnhookWindowsHookEx(ret);
}

void __stdcall KeyDown(char c)
{
	RaiseKeyFired(c);
}

extern "C" __declspec(dllexport) bool RegisterRawHandler(CharCallback_t fCharCallback)
{
	if (!fCharCallback)
		return false;

	return ::CreateHook() && ::AddExternalSubscriber(fCharCallback);

}

bool __stdcall CreateHook()
{
	static bool created = false;

	if (created)
		return true;

	HMODULE hDll = ::LoadLibrary(HOOK_DLL_LOCATION.c_str());

	if (!hDll || GetLastError())
	{
		throw module_not_loaded_exception(HOOK_DLL_LOCATION);
		return false;
	}

	//remote function that windows will call on mouse input
	HOOKPROC hookProc = (HOOKPROC)GetProcAddress(hDll, HOOK_PROC.c_str());

	if (!hookProc || GetLastError())
	{
		throw proc_address_not_found_exception(HOOK_PROC, HOOK_DLL_LOCATION);
		return false;
	}

	//register our local callback function, with our remote DLL that is receiving the windows messages.
	RegisterCallback_t fRegister = (RegisterCallback_t)GetProcAddress(hDll, "Register");

	if (!fRegister(&KeyDown))
	{
		throw std::exception("Local Callback failed to register");
		return false;
	}


	HHOOK hHook = SetWindowsHookEx(
		WH_KEYBOARD_LL,
		hookProc,
		hDll,
		0
	);

	if (!hHook || GetLastError())
	{
		throw windows_exception(GetLastError(), "Failed To Set Windows Hook");
		return false;
	}
	created = true;
	return true;
}

bool __stdcall AddExternalSubscriber(CharCallback_t fCharCallback)
{
	if (!fCharCallback)
		return false;

	_subscribers.push_back(fCharCallback);

}

void __stdcall RaiseKeyFired(char c)
{
	for (auto subscriber : _subscribers)
	{
		subscriber(c);
	}
}