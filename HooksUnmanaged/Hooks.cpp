#include "Hooks.h"

const std::string HOOK_PROC = "HookProc";

std::vector<CharCallback_t> _subscribers = std::vector<CharCallback_t>();

std::map<HSubscriber_t, CharCallback_t> _subscribersMap = std::map<HSubscriber_t, CharCallback_t>();

HHOOK _hook;
bool _hooked = false;

void __stdcall Test(char c)
{
	std::cout << c << std::endl;
}

int main(int argc, char* argv[])
{
	try
	{
		::CreateHook();
		AddExternalSubscriber(Test);
	}
	catch (const windows_exception& ex)
	{
		std::cout << ex.what() << std::endl;
	}

	MSG message;
	while (GetMessage(&message, NULL, 0, 0))
	{
		::TranslateMessage(&message);
		::DispatchMessage(&message);
	}
	UnhookWindowsHookEx(::_hook);
}

void __stdcall KeyDown(char c)
{
	RaiseKeyFired(c);
}

extern "C" __declspec(dllexport) HSubscriber_t RegisterExternalSubscriber(CharCallback_t fCharCallback)
{
	if (!fCharCallback)
		return NULL_CALLBACK;

	if(::CreateHook())
	return ::AddExternalSubscriber(fCharCallback);

	return UNABLE_TO_CREATE_HOOK;
}

bool __stdcall CreateHook()
{
	if (_hooked)
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

	_hook = SetWindowsHookEx(
		WH_KEYBOARD_LL,
		hookProc,
		hDll,
		0
	);

	if (!_hook || GetLastError())
	{
		throw windows_exception(GetLastError(), "Failed To Set Windows Hook");
		return false;
	}
	_hooked = true;
	return true;
}

HSubscriber_t __stdcall AddExternalSubscriber(CharCallback_t fCharCallback)
{
	if (!fCharCallback)
		return NULL_CALLBACK;

	HSubscriber_t h;
	do
	{
		h = ::NextHandle();

		if (h > MAX_HANDLES)
			return TOO_MANY_CALLBACKS;
	}
	while (_subscribersMap.count(h));

	_subscribersMap.insert(CALLBACK_PAIR(h, fCharCallback));
	_subscribers.push_back(fCharCallback);
	return h;
}

extern "C" __declspec(dllexport) bool UnregisterExternalHandler(HSubscriber_t fCharCallback)
{
	if (!fCharCallback)
		return false;

	return ::RemoveExternalSubscriber(fCharCallback);
}

bool __stdcall RemoveExternalSubscriber(HSubscriber_t hSubscriber)
{
	//handle not found >:(
	if (_subscribersMap.count(hSubscriber) < 1)
		return false;

	_subscribersMap.erase(hSubscriber);

	if (_subscribersMap.empty())
		RemoveHook();
	else
		return false;

	return true;
}

void __stdcall RemoveHook()
{
	if (UnhookWindowsHookEx(_hook))
		_hooked = false;
}

void __stdcall RaiseKeyFired(char c)
{
	for (auto subscriber : _subscribers)
	{
		subscriber(c);
	}
}

HSubscriber_t __stdcall NextHandle()
{
	static int nCount = 0;
	return nCount++;
}

LRESULT HookProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	if (nCode < 0)
	{
		return CallNextHookEx(nullptr, nCode, wParam, lParam);
	}
	else if (nCode == HC_ACTION)
	{
		if (wParam == WM_KEYDOWN)
		{
			LPWORD lpChar = 0;

			BYTE lpKeyState[256];
			GetKeyboardState(lpKeyState);

			KBDLLHOOKSTRUCT* pKeys = (KBDLLHOOKSTRUCT*)(lParam);
			//DHOOKSTRUCT* pKeys = (KBDLLHOOKSTRUCT*)(lParam);

			WCHAR pwszBuff[2];


			int nChars = ToUnicode(
				pKeys->vkCode,
				pKeys->scanCode,
				lpKeyState,
				pwszBuff,
				2,
				pKeys->flags
			);

			for (int i = 0; i < nChars; i++)
			{
				std :: cout << (pwszBuff[i]);
			}
		}

	}

	return CallNextHookEx(nullptr, nCode, wParam, lParam);
}