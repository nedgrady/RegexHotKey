#pragma once
#include "stdafx.h"
#include "Hooks_Common.h"
#include "ModuleNotLoadedException.h"
#include "ProcAddressNotFoundException.h"
#include "Constants.h"

#define INTEROP __stdcall
#define DLLEXPORT __declspec(dllexport)

#define CALLBACK_PAIR std::pair<HSubscriber, CharCallback>
#define NULL_CALLBACK -1
#define CALLBACK_NOT_FOUND -2
#define TOO_MANY_CALLBACKS -3
#define UNABLE_TO_CREATE_HOOK -4
#define MAX_HANDLES 50


typedef void(INTEROP *CharCallback)(const WCHAR);

typedef bool(*RegisterCallback)(const CharCallback);


//Could just avoid using a handle system at all, but that would mean
//One subscriber couldn't subscribe multiple times...is that even a benefit?
typedef int HSubscriber;

// external entry point for registering
extern "C" HSubscriber DLLEXPORT RegisterExternalSubscriber(const CharCallback fCharCallback);

//external enrty point for removing
extern "C" bool DLLEXPORT UnregisterExternalHandler(const HSubscriber fCharCallback);

void INTEROP KeyDown(const WCHAR c);

bool CreateHook();

void RemoveHook();

void RaiseKeyFired(WCHAR key);

// add to map here
HSubscriber AddExternalSubscriber(const CharCallback fCharCallback);

//remove from map here...
bool RemoveExternalSubscriber(const HSubscriber hSubscriber);

HSubscriber NextHandle();