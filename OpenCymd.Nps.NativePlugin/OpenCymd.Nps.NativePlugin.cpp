// This is the main DLL file.

#include "stdafx.h"

#include <AuthIf.h>

using namespace System;
using namespace System::Runtime::InteropServices;

DWORD WINAPI RadiusExtensionInit(void)
{
    OutputDebugString(L"RadiusExtensionInit c++/clr\n");
    return OpenCymd::Nps::Plugin::Native::NpsPlugin::RadiusExtensionInit();
}

VOID WINAPI RadiusExtensionTerm(void)
{
    OutputDebugString(L"RadiusExtensionTerm c++/clr\n");
    OpenCymd::Nps::Plugin::Native::NpsPlugin::RadiusExtensionTerm();
}

DWORD WINAPI RadiusExtensionProcess2(__inout PRADIUS_EXTENSION_CONTROL_BLOCK pECB)
{
    OutputDebugString(L"RadiusExtensionProcess2 begin c++/clr: calling managed code\n");
    DWORD result = OpenCymd::Nps::Plugin::Native::NpsPlugin::RadiusExtensionProcess2(IntPtr(pECB));
    OutputDebugString(L"RadiusExtensionProcess2 end c++/clr\n");
    return result;
}
