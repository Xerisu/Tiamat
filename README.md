# Tiamat - simple initiative ttrpg bot
Done for a group of friends by [bored CS student](https://github.com/Lank891) and [high school programmer-wannabe](https://github.com/Xerisu) in their free time.

### [Add bot to your server!](https://discord.com/api/oauth2/authorize?client_id=929072821060137050&permissions=11280&scope=bot%20applications.commands)

## Main features:
* Creates a channel where initiative will be tracked
* Players can be added and removed
* Turns can be tracked



## How to use:

* **/create-channel** - Adds the channel  `#tiamat-initiative-list`  in case it was removed or its name got changed

* **/join `player-name` `modifiers`** - Adds player to the initiative queue, with an `inactive` state (inactive players are skipped in the current round, and are hidden with ||spoilers||. Player-name can have spaces and special characters. To add modifiers, confirm typing name by pressing `Tab` , choose `modifiers` from the list (or press `Tab` twice) and then type modifiers of your initiative roll. Modifiers can be skipped or empty.

    **Possible modifiers:**
    * `adv` - adds advantage to a roll (2d20, drop lowest)
    * `dis` - adds disadvantage to a roll (2d20, drop highest)
    * `+<number>` - adds a constant number to a roll, ex. +8
    * `-<number>` - subtracts a constant number from a roll, ex. -1
    * `+d<number>` -  adds a dice to a roll, ex. +d8
    * `-d<number>` - subtracts a dice number from a roll, ex. -d10
    
        You can add or subtract multiple dices, ex. +2d4, -2d10

    When you add a player that already exists, it will remove the existing one and add him once again
    
    Example: /join `player-name: Nastija` `modifiers: adv +2d4-3` 

* **/remove `player-name`** - removes player from the queue.

* **/clear** - clears the queue and resets the round counter (use it only when the battle finishes)"


## How to host it yourself:
* Compile everything and run project `Discord`
* Alternatively, use [Docker Image](https://www.docker.com/) - you can build it from `Dockerfile` in the `InitiativeBot` directory
* You have to provide your discord bot token, there are two ways:
  * You can define environment variable `TIAMAT_TOKEN` with the token (probably you want to do it when using docker by using `-e` flag when running the container)
  * If environment variable is not found, file `./token` will be read and its content will be treated as the token
