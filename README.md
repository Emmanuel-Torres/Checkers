# Checkers online

## Contents

1. Project's Description
2. Game Rules
3. Technologies
4. Next Features

## Project's Description

This project allows you to play checkers against other players online. You can find a live version of this project [here](https://www.checkersonline.link)

## Game Rules

This project follows the standard checkers rules.

## Technologies

### Backend

The backend was written in C# using ASP.NET and signalR. This backend allows clients to connect via web sockets, so that players can play live against each other. This is done by the main hub ([CheckersHub.cs](https://github.com/Emmanuel-Torres/Checkers/blob/main/src/checkers-api/Hubs/CheckersHub.cs)). Then the clients can send requests to any of the Hub methods after the initial connection is established.

### Frontend

The frontend app was written in TypeScript using React. This allowed me to create a responsive frontend that updates as the game progresses. I was able to add this functionality using the npm package for SignalR. When I navigate to my game page ([GameView.tsx](https://github.com/Emmanuel-Torres/Checkers/blob/main/src/checkers-client/src/views/GameView.tsx)), the frontend will establish a web sockets connection with the server, allowing tha client to start match-making.

## Next Features

In the future, I would love to add more features to this project. Some of those include:

- Player vs Machine game-mode.
- Non-live turn based multiplayer.
- Play against your friends via invites.
