# Santec_Full_band_IL
C# and Visual Basic project to control the Santec Full Band System

## 1. Overview of Project
This is an example software of a scan test system for full-band IL measurement.
  + Development environment  Visual Studio 2015
  +	Windows Framework        4.0 or later
  +	Instrument DLL           2.5.1
  +	STSProcess.DLL           2.2.2
  +	NI DLL                   15.5 or later

## 2. Configuration
1.	Tunable laser TSL Series (TSL-550/TSL-710/TSL-570)
2.	Power meter MPM Series (MPM-210/210H/211/212/213/215)
3.	Optical switch OSU series (OSU-100/OSU-110)
This sample software allows you to control up to two MPM main frames (MPM-210 or MPM-210H).

## 3. Connection setting
### Tunable laser (TSL)control
  - TSL-550/710: GPIB
  - TSL-570: GPIB, TCP/IP or USB
    
    Note: It can be changed on the source code, but the initial value is the delimiter CRLF specification.
### Power meter (MPM) control
  - MPM-210/210H: GPIB or TCP/IP 

### Optical switch (OSU) control
  - OSU-100: USB
  - OSU-110: GPIB, TCP/IP or USB

  Here below is an example tp connect 2 TSLs , 1 OSU-110 and 1 MPM:
  
  - TSL-*** Trigger Output	->	OSU-110 Trigger Input;
  
  - OSU-110 Trigger Output	->	MPM-*** Trigger Input;
  
  - OSU-110 Power Monitor	->	MPM-*** Power Monitor;
  
![Picture1](https://user-images.githubusercontent.com/103238519/220315376-7c432444-8d6f-4c68-a627-8d5e0d921457.png)
![Picture2](https://user-images.githubusercontent.com/103238519/220315398-fc2ccc09-1372-4b3f-b35e-5ae853895230.png)

Here below is an example tp connect 2 TSLs , 1 OSU-100 and 1 MPM:
  - TSL-***Trigger Output->  OSU-***Trigger Input
  - OSU -***Power Monitor->  MPM-***Trigger Input
  
![Picture3](https://user-images.githubusercontent.com/103238519/220320150-54f0f501-0a28-439c-9894-b0a2feee5346.png)
![Picture4](https://user-images.githubusercontent.com/103238519/220320191-89377eae-5bd9-48ff-927d-fb99e987eebc.png)

## 4. Operational steps

Refere to the Manual 

