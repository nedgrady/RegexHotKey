#include "stdafx.h"
#include "Hook.h"

CharCallback fChar = nullptr;

extern "C" bool __declspec(dllexport) Register(const CharCallback fChar)
{
	if (!fChar)
		return false;

	::fChar = fChar;
	return true;
}

extern "C" LRESULT __declspec(dllexport) HookProc(int nCode, WPARAM wParam, LPARAM lParam)
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
				fChar(pwszBuff[i]);
			}
		}
		
	}

	return CallNextHookEx(nullptr, nCode, wParam, lParam);
}