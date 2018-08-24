#pragma once
#include "stdafx.h"
#include "ModuleNotLoadedException.h"
#include "ProcAddressNotFoundException.h"
#include "Constants.h"

typedef void(__stdcall *CharCallback_t)(char c);

typedef bool(*RegisterCallback_t)(CharCallback_t);

#define CALLBACK_PAIR std::pair<HSubscriber_t, CharCallback_t>
#define NULL_CALLBACK -1
#define CALLBACK_NOT_FOUND -2
#define TOO_MANY_CALLBACKS -3
#define UNABLE_TO_CREATE_HOOK -4
#define MAX_HANDLES 50

//Could just avoid using a handle system at all, but that would mean
//One subscriber couldn't subscribe multiple times...is that even a benefit?
typedef int HSubscriber_t;

void __stdcall KeyDown(char c);

bool __stdcall CreateHook();

void __stdcall RemoveHook();

void __stdcall RaiseKeyFired(char key);

// external entry point for registering
extern "C" __declspec(dllexport) HSubscriber_t RegisterExternalSubscriber(CharCallback_t fCharCallback);

// add to map here
HSubscriber_t __stdcall AddExternalSubscriber(CharCallback_t fCharCallback);

//external enrty point for removing
extern "C" __declspec(dllexport) bool UnregisterExternalHandler(HSubscriber_t fCharCallback);

//remove from map here...
bool __stdcall RemoveExternalSubscriber(HSubscriber_t hSubscriber);

HSubscriber_t __stdcall NextHandle();

