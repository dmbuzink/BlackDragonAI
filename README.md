# BlackDragonAI
A twitch chat bot created in .NET Core and C#. Uses a API and databases for storing commands.

## How to use the bot as a moderator:
### Basics
`!new !<COMMAND_TO_CREATE> "<COMMAND_TEXT>" p/<PERMISSION> t/<TIMER>`

`!edit !<COMMAND_TO_EDIT> "<NEW_COMMAND_TEXT>"`

`!delete !<COMMAND_TO_DELETE>`

### Permission and timer
Permission and timer are extra options for a command. When not given it will default to *everyone* and *60 seconds* respectively. Timer can be any positive integer (a whole number). Permission can be one of the following values:
|permission|Command available to|
|---|---|
|SUBS|Subscribers, moderators and admins|
|MODS|Moderators and admins|
|ADMINS|Admins only|

### Special Operators
Specials operators can be used to add dynamic parts to a command. The following operators are available:
|Operators|What gets added|
|---|---|
|$subs|Amount of subscribers|
|$followers|Amount of followers|
|$followage|Amount of time since the user, whom called this command, has followed the channel|
|$game|The game currently being played|
|$uptime|Amount of time since the stream has been started|
|$commands|All available commands for normal users and/or subs only|
|$recipient|Adds the name you give when you call the command*|

### $recipient
This special operator is a bit more powerful than the others and also a bit more difficult to get your head around. This is how the shoutout command actually is stored: 
>Shoutout naar $recipient, check allemaal even het kanaal op twitch.tv/$recipient

As you can see it uses the $recipient command twice. As a user you would call this command like: `!so @BlackDragonAI` (or `!so BlackDragonAI`). This can also be used to execute commands through the bot. This is how the !clear command works: 
>/timeout $recipient 1

It uses the normal twitch timeout command (`/timeout`) and a duration of 1 second. To dynamically make it usable on different users the `$recipient` gets used. A `!ban` could be created like this too. **WARNING: MAKE SURE THE PERMISSION OF THE COMMAND IS EQUAL OR HIGHER THAN THE PERMISSION OF THE BOT. YOU WOULD NOT WANT A NORMAL USER TO BE ABLE TO BAN OTHER USERS!**
