# BlackDragonAI
A twitch chat bot created in .NET Core and C#. Uses a API and databases for storing commands.

## How to use the bot as a moderator:
### Some commands you should be aware of
*When you see `USERNAME`, it can be `@USERNAME` as well*

*Also, when you see 'defaults to', it means the specified value is only optional*

|Command|Result|
|---|---|
|!clear `USERNAME`|Timeouts specified user for 1 second removing all previous messages|
|!permit `USERNAME`|Allows a normal user to send a single message with a URL (web link) in it|
|!timeout `USERNAME` `TIMEOUT_DURATION`|Timeouts specified user for the specified timeout duration (defaults to 1 minute)|
|!setgame `GAME`|Sets the game of the stream to the specified game (a.k.a. a category, so can also be "Special Events" for example). Spelling is very important here!|
|!startcommercial `DURATION`|Start a commercial for the specified duration. Defaults to 30 seconds but can be 30, 60, 90, 120, 150 and 180 seconds|

### Command management
> `!new !<COMMAND_TO_CREATE> "<COMMAND_TEXT>" p/<PERMISSION> t/<TIMER>` (`p/<PERMISSION>` and `t/TIMER` are optional)

> `!edit !<COMMAND_TO_EDIT> "<NEW_COMMAND_TEXT>"`

> `!delete !<COMMAND_TO_DELETE>`

> `!setalias !<COMMAND_TO_MAKE_ALIAS_OF> !<ALIAS>`

> `!deletealias !<ALIAS_TO_DELETE>` (this commands keeps existing if other alias' exists)

### Permission and timer
Permission and timer are extra options for a command. When not given it will default to *everyone* and *60 seconds* respectively. Timer can be any positive integer (a whole number). Permission can be one of the following values:
|permission|Command available to|
|---|---|
|subs|Subscribers, moderators and admins|
|mods|Moderators and admins|
|admins|Admins only|

### Examples of command management
> !new !discord "Je kunt mijn Discord server, genaamd The Dragon's Den, via deze link joinen: https://discord.gg/6VqTtf6"

> !new !ban "/ban $recipient" p/mods

> !new !followage "$user, je volgt BlackDragon al voor $followage" t/30

> !edit !test "Aangepast test commando"

> !delete !test

> !setalias !discord !dc

> !deletealias !dc


### Special Operators
Specials operators can be used to add dynamic parts to a command. Multiple special operators can be used in the same command. The following operators are available:
|Operators|What gets added|
|---|---|
|$subs|Amount of subscribers|
|$followers|Amount of followers|
|$followage|Amount of time since the user, whom called this command, has followed the channel|
|$game|The game currently being played|
|$uptime|Amount of time since the stream has been started|
|$commands|All available commands for normal users and/or subs only|
|$user|The name of the user who called the command|
|$recipient|Adds the name you give when you call the command|

### $recipient
This special operator is a bit more powerful than the others and also a bit more difficult to get your head around. For example, this is how the shoutout command is stored: 
>Shoutout naar $recipient, check allemaal even het kanaal op twitch.tv/$recipient

As you can see it uses the $recipient command twice. As a user you would call this command like: `!so @BlackDragonAI` (or `!so BlackDragonAI`). Be aware that this command will only work if you give a name when invoking the command, otherwise an error message will be returned. This can also be used to execute commands through the bot. This is how the !clear command is stored: 
>/timeout $recipient 1

It uses the normal twitch timeout command (`/timeout`) and a duration of 1 second. To dynamically make it usable on different users the `$recipient` gets used. A `!ban` command could be created like this too. **WARNING: MAKE SURE THE PERMISSION OF THE COMMAND IS EQUAL OR HIGHER THAN THE PERMISSION OF THE BOT. YOU WOULD NOT WANT A NORMAL USER TO BE ABLE TO BAN OTHER USERS!**
