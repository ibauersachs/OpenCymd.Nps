// --------------------------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (C) 2012-2013 University of Applied Sciences Northwestern Switzerland
//   
//   This library is free software; you can redistribute it and/or
//   modify it under the terms of the GNU Lesser General Public
//   License as published by the Free Software Foundation; either
//   version 2.1 of the License, or (at your option) any later version.
//   
//   This library is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//   Lesser General Public License for more details.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// This is the main DLL file.

#include "stdafx.h"

#include <AuthIf.h>

using namespace System;
using namespace System::Runtime::InteropServices;

#using "OpenCymd.Nps.Plugin.dll" as_friend

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
