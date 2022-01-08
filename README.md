# Tiamat - simple initiative ttrpg bot
Done for a group of friends by [bored CS student](https://github.com/Lank891) and [high school programmer-wannabe](https://github.com/Xerisu) in their free time.

### [Add bot to your server!](https://discord.com/api/oauth2/authorize?client_id=929072821060137050&permissions=11280&scope=bot%20applications.commands)

## Main features:
* Creates a channel where initiative will be tracked
* Players can be added and removed
* Turns can be tracked



## How to use:
* There should go some kind of help message


## How to host it yourself:
* Compile everything and run project `Discord`
* Alternatively, use [Docker Image](https://www.docker.com/) - you can build it from `Dockerfile` in the main directory
* You have to provide your discord bot token, there are two ways:
  * You can define environment variable `TIAMAT_TOKEN` with the token (probably you want to do it when using docker by using `-e` flag when running the container)
  * If environment variable is not found, file `./token` will be read and its content will be treated as the token