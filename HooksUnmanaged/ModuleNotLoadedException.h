#pragma once
#include "stdafx.h"
#include "WindowsException.h"
#include "ErrorUtils.h"

class module_not_loaded_exception
	: public ::windows_exception
{
public:
	module_not_loaded_exception(std::string strModule, DWORD dwMessageId = GetLastError())
		:windows_exception(dwMessageId),
		_strError(CreateErrorString(strModule, dwMessageId))
	{ }

	~module_not_loaded_exception();

	virtual const char * what() const throw()
	{
		return _strError.c_str();
	}

private: 
	const std::string _strError;

	std::string CreateErrorString(std::string strModule, DWORD dwMessageId)
	{
		std::stringstream ss;
		ss << "Error Loading Module "
			<< strModule
			<< std::endl
			<< this->windows_exception::what();

		return ss.str();
	}
};

