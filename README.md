# Santec_Full_band_IL
C# and Visual Basic project to control the Santec Full Band System

1. Overview of Project
======================
  This is an example software of a scan test system for full-band IL measurement.
     •	Development environment  Visual Studio 2015
     •	Windows Framework        4.0 or later
     •	Instrument DLL           2.5.1
     •	STSProcess.DLL           2.2.2
     •	NI DLL                   15.5 or later

2. Configuration
================
  (1)	Tunable laser TSL Series (TSL-550/TSL-710/TSL-570)
  (2)	Power meter MPM Series (MPM-210/210H/211/212/213/215)
  (3)	Optical switch OSU series (OSU-100/OSU-110)
  This sample software allows you to control up to two MPM main frames (MPM-210 or MPM-210H).

3. Connection setting
=====================
  Tunable laser (TSL)control
     TSL-550/710: GPIB
     TSL-570:GPIB, TCP/IP or USB
     *It can be changed on the source code, but the initial value is the delimiter CRLF specification.
  Power meter (MPM) control
     MPM-210/210H: GPIB or TCP/IP 
  Optical switch (OSU) control
     OSU-100: USB
     OSU-110: GPIB, TCP/IP or USB

  Connection system (2 TSL connections, OSU-110)
     Connect with BNC cable as shown in figure 1.
     TSL-*** Trigger Output	->	OSU-110 Trigger Input
	 OSU-110 Trigger Output	->	MPM-*** Trigger Input　
	 OSU-110 Power Monitor	->	MPM-*** Power Monitor　

