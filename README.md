<p align="right"> <a href="https://www.santec.com/en/" target="_blank" rel="noreferrer"> <img src="https://www.santec.com/dcms_media/image/common_logo01.png" alt="santec" 
  width="250" height="45"/> </a> </p>

# Santec_Full_band_IL
Full Band System developed using C# and VB.NET on Visual Studio Platform.

## List of content

  - ### Overview of Project
    This is an example software of a scan test system for full-band IL measurement.

  - ### System Requirements
    + Development environment  Visual Studio 2015
    +	Windows Framework        4.0 or later
    +	Instrument DLL           2.5.1
    +	STSProcess.DLL           2.2.2
    +	NI DLL                   15.5 or later

  - ### Tech Stack
    <p align="left"> <a href="https://dotnet.microsoft.com/en-us/languages/csharp" target="_blank" rel="noreferrer"> <img src="https://th.bing.com/th/id/OIP.1C3f4vlPHd2AU3xuVL3OEQAAAA?w=228&h=256&rs=1&pid=ImgDetMain" alt="cs" 
    width="50" height="50"/> </a> </p> 
    <p align="left"> <a href="https://visualstudio.microsoft.com/vs/features/net-development/" target="_blank" rel="noreferrer"> <img src="https://th.bing.com/th/id/OIP.0-pGgiUq08VxtxFYUQZElgHaEb?w=400&h=239&rs=1&pid=ImgDetMain" 
    alt="vb.net" 
    width="70" height="40"/> </a> </p> 
    <p align="left"> <a href="https://visualstudio.microsoft.com/" target="_blank" rel="noreferrer"> <img src="https://th.bing.com/th/id/OIP.I9TwwZg3mQbfGOk7sGJTiwHaHa?w=550&h=550&rs=1&pid=ImgDetMain" alt="visualstudio" 
    width="70" height="40"/> </a> </p> 

  - ### Configuration
    - Tunable laser TSL Series (TSL-550/TSL-710/TSL-570)
    - Power meter MPM Series (MPM-210/210H/211/212/213/215)
    - Optical switch OSU series (OSU-100/OSU-110)

    ***Note: This sample software allows you to control up to two MPM main frames (MPM-210 or MPM-210H).***

  - ### Connection setting
    - ### Tunable laser (TSL)control
      - TSL-550/710: GPIB
      - TSL-570: GPIB, TCP/IP or USB
            
      ***Note: It can be changed on the source code, but the initial value is the delimiter CRLF specification.***

 - ### Power meter (MPM) control
   - MPM-210/210H: GPIB or TCP/IP 

 - ### Optical switch (OSU) control
   - OSU-100: USB
   - OSU-110: GPIB, TCP/IP or USB

### Here below is an example tp connect 2 TSLs , 1 OSU-110 and 1 MPM:  

  - TSL - *** Trigger Output	->	OSU-110 Trigger Input
    
  - OSU-110 Trigger Output	->	MPM - *** Trigger Input
    
  - OSU-110 Power Monitor	->	MPM - *** Power Monitor <br/>
    
  ![Picture1](https://user-images.githubusercontent.com/103238519/220315376-7c432444-8d6f-4c68-a627-8d5e0d921457.png)
  ![Picture2](https://user-images.githubusercontent.com/103238519/220315398-fc2ccc09-1372-4b3f-b35e-5ae853895230.png)

### Here below is an example tp connect 2 TSLs , 1 OSU-100 and 1 MPM: 
  - TSL - ***Trigger Output->  OSU - ***Trigger Input
    
  - OSU - ***Power Monitor->  MPM - ***Trigger Input <br/>
  
  ![Picture3](https://user-images.githubusercontent.com/103238519/220320150-54f0f501-0a28-439c-9894-b0a2feee5346.png)
  ![Picture4](https://user-images.githubusercontent.com/103238519/220320191-89377eae-5bd9-48ff-927d-fb99e987eebc.png)


### Operational steps
 Refer to the below operational manual to run the script,
  [ English Manual ](https://github.com/santec-corporation/Santec_Full_band_IL/blob/main/Santec%20Full-Band%20IL%20Swept%20Test%20System%20Manual%20V1.pdf)

## For more information on Swept Test System [CLICK HERE](https://inst.santec.com/products/componenttesting/sts)
