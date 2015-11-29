NET STOP "AI Battles Simulator Service"
sc delete "AI Battles Simulator Service"
timeout 10
CD %~dp0
C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil "..\OnlineGames.Workers.BattlesSimulator.exe"
NET START "AI Battles Simulator Service"
pause