# YellowDuckyBot
Should help user by asking questions about the code until user reaches "Aaahhh" moment.

It is based on simpleEchoBot from https://github.com/ttrzcinski/Microsoft.Bot.Sample.SimpleEchoBot.

WIPs:
- Add modes - like game mode, debug mode, joking mode, help mode
- Add generics and interfaces on common objects
- Add some sort of memory to bot in order not to repeat questions, if answer was already given.

TODOs:
- Add questions usable from point of being the Yellow Ducky
- Read questions from some externalized form, like JSON tree or some small DataBase.
- Add question dialog with two buttons
- Add a way to extend known list of questions from user's point, like special commands "ADD_QUESTION: Did You run static Analysis?/Yes/No"
- Taking care of git commands for user
- Add moods (faces) persented with responses
- Add Text-To-speech on responses
- Add Speech recognition

DONE:
- FactsBase - needs a memory to keep flags outside of processing
- Game of Lycopersicon
- Add JSON source file with questions and answers