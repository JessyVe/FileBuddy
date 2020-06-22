:: This script starts the needed components for the FileBuddy-System
:: NOTE: Be aware that this script only works if the .NET-Solution was build 
:: and the .exe-files are available

start /d "FileBuddy\API\bin\Debug\netcoreapp3.1" API.exe
start /d "FileBuddy\WebSocketServerUI\bin\Debug\netcoreapp3.1" WebSocketServerUI.exe
start /d "FileBuddy\FileBuddyUI\bin\Debug\netcoreapp3.1" FileBuddyUI.exe