# Logitech GamePanel LCD FFXIV Applet
An application that reads data from FFXIV and displays it on the Logitech GamePanel  
Works on both monochrome and color LCDs  
  
  
# Sharlayan
This applet uses Icehunter's sharlayan.dll to read from the game. If a patch is realeased for FFXIV and this applet stops working, try grabbing the latest sharlayan DLL from his repo.  
  
# GameEnginesWrapper
The wrapper DLL from Logitech's offical c++ SDK.  
Imported to c# through LogitechLCD.cs  
  
# Building
Build either x86 or x64 bassed upon your OS and Logitech Driver version, NOT bassed on DX9/DX11 FFXIV.  
Withing the app itself there is a selctor for DX9/DX11.  
