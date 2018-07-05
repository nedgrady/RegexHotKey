#include "ErrorUtils.h"

std::string GetErrorMessage(DWORD dwMessageId)
{
	LPVOID lpBuffer = NULL;

	::FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER |
		FORMAT_MESSAGE_FROM_SYSTEM |
		FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL,
		dwMessageId,
		LANG_USER_DEFAULT,
		(LPSTR)&lpBuffer,
		0,
		NULL
	);

	std::string ss;
	ss = std::string((LPSTR)lpBuffer);

	//LocalFree(lpBuffer);

	return ss;
}

std::string GetLastErrorMessage()
{
	return ::GetErrorMessage(GetLastError());
}
