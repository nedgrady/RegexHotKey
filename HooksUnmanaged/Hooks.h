#pragma once
#include "stdafx.h"
#include "ModuleNotLoadedException.h"
#include "ProcAddressNotFoundException.h"

typedef void(__stdcall *CharCallback_t)(char c);

typedef bool(*RegisterCallback_t)(CharCallback_t);

void __stdcall KeyDown(char c);

bool __stdcall CreateHook();

void __stdcall RaiseKeyFired(char key);

bool __stdcall AddExternalSubscriber(CharCallback_t fCharCallback);