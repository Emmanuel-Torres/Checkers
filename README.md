## Project history

The idea and first iteration of this project goes all the way back to my junior year in college. Starting as a mobile application using Xamarin.Forms, the project quickly evolved into something more than just a "class final project." Rather than scrapping it after I was done with my class, I decided to keep improving it and make it my main personal project. Since its first iteration, the project has gone through 2 major redesigns that shifted it from a mobile app into a web application that is hosted online. Through this project I have been able to practice many of my skills as a Software Developer, and to learn new things along the way.

## Project's Description

This project allows you to play checkers against another person in real time. You can try it yourself using the following url: [https://www.checkersonline.link](https://www.checkersonline.link).

## Technologies

### Backend

The backend was written in C# using ASP.NET and signalR. This backend allows clients to connect via web sockets, that allows clients to have real time communication with each other. The following files contain all the server side signalR code:

- [`ICheckersHub.cs`](https://github.com/Emmanuel-Torres/Checkers/blob/main/src/checkers-api/Hubs/ICheckersHub.cs) describes all the interactions that can be perform by the clients and the server.
- [`CheckersHub.cs`](https://github.com/Emmanuel-Torres/Checkers/blob/main/src/checkers-api/Hubs/CheckersHub.cs) implements the interactions that the clients can have with the server.

### Frontend

The frontend was written in TypeScript using React. This allowed me to create a responsive frontend that updates as the game progresses. The following file contains all the client side signalR code:

- [`RoomView.tsx`](https://github.com/Emmanuel-Torres/Checkers/blob/main/src/checkers-client/src/views/Room/RoomView.tsx) creates a connection to the main server and describes how the client interacts with it.
