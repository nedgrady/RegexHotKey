#pragma once
#include "stdafx.h"
#include"WindowsException.h"


class proc_address_not_found_exception
	:public windows_exception
{
public:
	~proc_address_not_found_exception();

	proc_address_not_found_exception(std::string strProcName, std::string strDllName, DWORD dwMessageId = GetLastError())
		:windows_exception(dwMessageId),
		_strError(CreateErrorString(strProcName, strDllName, dwMessageId))
	{

	}

	virtual const char * what() const throw()
	{
		return _strError.c_str();
	}

private:
	const std::string _strError;

	std::string CreateErrorString(std::string strProcName, std::string strDllName, DWORD dwMessageId)
	{
		std::stringstream ss;
		ss << "Error Locating Function \""
			<< strProcName
			<< "\" in "
			<< strDllName
			<< std::endl
			<< this->windows_exception::what();

		return ss.str();
	}
};

