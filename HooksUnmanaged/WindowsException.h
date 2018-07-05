#pragma once
#include "stdafx.h"
#include "ErrorUtils.h"

class windows_exception
	:public std::exception
{
public:
	windows_exception(DWORD dwMessageId = GetLastError(), std::string strCustomError = "")
		:_dwMessageId(dwMessageId),
		_strError(CreateErrorString(dwMessageId, strCustomError))
	{	};

	virtual inline const char * what() const throw() { return _strError.c_str(); }

	~windows_exception(){ }

private:
	const DWORD _dwMessageId;
	const std::string _strError;

	std::string CreateErrorString(DWORD dwMessageId, std::string strCustomError)
	{
		std::stringstream ss;
		ss << (strCustomError == "" ? strCustomError : std::string("No Custom Error"))
			<< std::endl
			<< "Windows Function Returned Error Code: "
			<< _dwMessageId
			<< std::endl
			<< ::GetErrorMessage(_dwMessageId)
			<< std::endl;

		return ss.str();
	}
};

