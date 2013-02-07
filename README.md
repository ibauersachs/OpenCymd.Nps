OpenCymd.Nps
============
The OpenCymd.Nps project is a set of DLLs to provide a .NET wrapper around
the Network Policy Server Extensions API (abbreviated NPS or formerly known
as Internet Authentication Server, IAS) to use it easily from managed code.

Example
-------
Take a look at the project `OpenCymd.Nps.SamplePlugin`

Installation
------------
The two base libraries `OpenCymd.Nps.NativePlugin.dll` and
`OpenCymd.Nps.Plugin.dll` have to be installed into the Windows system
directory, which is usually `C:\Windows\system32`. To be recognized by
the Netword Policy Service, two Registry values of type `REG_MULTI_SZ`
need to be created or complemented:

 * `HKLM\System\CurrentControlSet\Services\AuthSrv\Parameters\ExtensionDLLs`
 * `HKLM\System\CurrentControlSet\Services\AuthSrv\Parameters\AuthorizationDLLs`

Both values need to contain the value `OpenCymd.Nps.NativePlugin.dll`.
They cannot contain a path to avoid the installation in the system directory.

The actual plug-ins need to be installed into the directory
`C:\Program Files\OpenCymd\Nps` (currently hardcoded). This directory MAY NOT
contain the two base libraries, as this would cause conflicts with libraries
from the system directory!

The log4net configuration is loaded from the file `NpsPlugin.config` and needs
also to be located in `C:\Program Files\OpenCymd\Nps`. There is an example in
the source directory `OpenCymd.Nps.Plugin`.

The service account of the Network Policy Server, usually Local System, needs
to have the permissions to read all files from `C:\Program Files\OpenCymd\Nps`
and to manage directories.

Compatibility
-------------
Only tested with Windows Server 2008 R2. It may or may not work with Windows
Server 2008 and/or Windows Server 2012.

License & Copyright
-------------------
[GNU Lesser General Public License (LGPL), Version 2.1](http://www.gnu.org/licenses/lgpl-2.1.html)

Please contact the copyright holder if you need a different license.

(c) 2012/2013  
    [University of Applied Sciences Northwestern Switzerland](http://www.fhnw.ch)  
    School of Engineering  
    Institute of Mobile and Distributed Systems  
    [info-imvs@fhnw.ch](mailto:info-imvs@fhnw.ch)
