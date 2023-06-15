# Discord-Bot
An RPG Discord bot created in DSharp library in C# language
[Add screen here]

## About Bot
This bot has been created as a side project for fun. It provides simple commands as well as an RPG system with customisable database written using EF Core.  
Every user can add new items, mobs and lore to create his own unique RPG world.

## Bugs
This bot is still under development so it may contain bugs, which will be fixed in the future if found. Combat and leveling system may be still unbalanced but is balanced whenever
possible

## Future Development
There are few things planned to be added in the future
- Talking with bot via Chat GPT
- Writing Web UI to improve admin experience and make database managment easier (now commands are used to maintain database)
- Adding music player 
- Adding new features to the RPG system
- Overall user experience improvements

Of course development and maintanance of existing features will always take place

# How to use bot?
This section will show tutorial how to start using bot and open in localhost
- Download this repository (on the right side *Code* -> *Download ZIP*)
- Login to [Discord Developer Portal](https://discord.com/developers/applications)
  + Create new application
  + Setup your bot
  + Click *Bot* and generate token **DO NOT SHARE TOKEN WITH ANYONE**
  + Open config.json and change token to your token
- *Discord Developer Portal* -> *OAuth2* -> *URL Generator*
  + *Scopes* -> *Bot*
  + *Bot permissions* -> *Administrator*
  + Generate your new URL, add to any server
- Open .sln file in Visual Studio
   + *Tools* -> *NuGet Packet Manager* -> *Console* [Add screen here]
   + Change project to DB [Add screen here]
   + Type Update-Database
- Run Bot using Ctrl + F5
   + New window in website will open as well as console - **DO NOT CLOSE THEM**
   + You can restart bot just by clicking Ctrl + F5, new website window will open
- On the server, where your bot is type !help, it should work now and throw few *Does not exist in database exceptions* - Voila  😎👍

This was the most basic setup, if this project will get noticed by anyone, who would like to use it I will write more about maintaining bot and its commands

## Bug reports
I will be very glad if you spot any bug, you can add issues on github, I try to fix every bug i find. And the most important  - **Thank for your contribution**
