#include "Hooks.h"

const std::string HOOK_PROC = "HookProc";

std::vector<CharCallback> _subscribers = std::vector<CharCallback>();

std::map<const HSubscriber, const DLLEXPORT CharCallback> _subscribersMap = std::map<const HSubscriber, const DLLEXPORT CharCallback>();

HHOOK _hook;
bool _hooked = false;

void INTEROP Test(const WCHAR c)
{
	std::wcout << c << std::endl;
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

void INTEROP KeyDown(const WCHAR c)
{
	RaiseKeyFired(c);
}

extern "C" HSubscriber DLLEXPORT RegisterExternalSubscriber(const CharCallback fCharCallback)
{
	if (!fCharCallback)
		return NULL_CALLBACK;

	if(::CreateHook())
	return ::AddExternalSubscriber(fCharCallback);

	return UNABLE_TO_CREATE_HOOK;
}

extern "C" bool DLLEXPORT UnregisterExternalHandler(const HSubscriber fCharCallback)
{
	if (!fCharCallback)
		return false;

	return ::RemoveExternalSubscriber(fCharCallback);
}


bool CreateHook()
{
	if (_hooked)
		return true;

	SetLastError(0);
	HMODULE hDll = ::LoadLibrary(HOOK_DLL_LOCATION.c_str());

#if _DEBUG
	std::cout << "hDll: " << hDll << " LastError: " << GetLastError() << " " << GetLastErrorMessage() << std::endl;
#endif

	if (!hDll || GetLastError())
	{
		throw module_not_loaded_exception(HOOK_DLL_LOCATION);
		return false;
	}

	//remote function that windows will call on mouse input
	HOOKPROC hookProc = (HOOKPROC)GetProcAddress(hDll, HOOK_PROC.c_str());

#if _DEBUG
	std::cout << "hookProc: " << hookProc << " LastError: " << GetLastError() << " " << GetLastErrorMessage() << std::endl;
#endif

	if (!hookProc || GetLastError())
	{
		throw proc_address_not_found_exception(HOOK_PROC, HOOK_DLL_LOCATION);
		return false;
	}

	//register our local callback function, with our remote DLL that is receiving the windows messages.
	const RegisterCallback fRegister = (RegisterCallback)GetProcAddress(hDll, "Register");

#if _DEBUG
	std::cout << "fRegister: " << fRegister << " LastError: " << GetLastError() << " " << GetLastErrorMessage() << std::endl;
#endif

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

#if _DEBUG
	std::cout << "_hook: " << _hook << " LastError: " << GetLastError() << " " << GetLastErrorMessage() << std::endl;
#endif

	if (!_hook || GetLastError())
	{
		throw windows_exception(GetLastError(), "Failed To Set Windows Hook");
		return false;
	}
	_hooked = true;
	return true;
}

HSubscriber AddExternalSubscriber(const CharCallback fCharCallback)
{
	if (!fCharCallback)
		return NULL_CALLBACK;

	HSubscriber h;
	do
	{
		h = ::NextHandle();

		if (h > MAX_HANDLES)
			return TOO_MANY_CALLBACKS;
	}
	while (_subscribersMap.count(h));

	_subscribersMap.insert(CALLBACK_PAIR(h, fCharCallback));

	return h;
}

bool RemoveExternalSubscriber(const HSubscriber hSubscriber)
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

void RemoveHook()
{
	if (UnhookWindowsHookEx(_hook))
		_hooked = false;
}

void RaiseKeyFired(const WCHAR c)
{
	for (auto subscriberKvp : _subscribersMap)
	{
		subscriberKvp.second(c);
	}
}

HSubscriber NextHandle()
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