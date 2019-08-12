# Simple Trading Card Game

## Idea

My idea was to implement simple game structure and make it easy to consume and improve. It is relatively easy to add network features, because only one interface (IPlayerCommunicator) is responsible from communicating right now. 

## Project Structure

### TradingCardGame.Domain

This project contains components of the game logic.

Note that Game class could also be in a service layer; since it is not exactly an Aggregate root.

Game receives IPlayerCommunicator instances to provide communication with the players. It drives all the game logic inside by manipulating Player and PlayerState which manipulates Deck and Hand objects itself.

Game uses a tick mechanism to try to solve synchronization issues that may occur. Each player movement (playing card, ending turn) increases tick and for each tick, Game sends current game state to both players.

I did not optimize some parts; since I know hand size is 5 at maximum.

### TradingCardGame.Impl.InProcess

Contains KnuthShuffler implementation for deck shuffling and simple InProcessPlayerCommunicator for preparing a very simple demo application.

### TradingCardGame.Impl.InProcessConsoleClient

Simple console client application. It uses InProcessPlayerCommunicator to run the Game instance. Both players share same console; so game is not really enjoyable :) It is still fully functional.

## Running the project

I have used dotnetcore to code the project.

Library projects are netstandard2.0

ConsoleClient is a netcore2.2 Console Application.

It is possible to run using docker and docker-compose tools with the following command:


```
docker-compose up
```