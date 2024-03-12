import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { FC, useEffect, useState } from "react";
import BoardComponent from "../components/game/board/BoardComponent";
import BoardLocation from "../models/game/location";
import MoveRequest from "../models/game/moveRequest";
import Square from "../models/game/square";
import HubMethods from "../models/helper/hub-methods";
import PlayerIndicatorComponent from "../components/game/player-indicator/PlayerIndicatorComponent";
import RoomInfo from "../models/room/roomInfo";
import JoinView from "./JoinView";
import RoomView from "../components/room/room-info/RoomInfoComponent";
import GameInfo from "../models/game/gameInfo";
import Player from "../models/game/player";

const GameView: FC = (): JSX.Element => {
    const [connection, setConnection] = useState<HubConnection>();
    const [player, setPlayer] = useState<Player>();
    const [isRoomOwner, setIsRoomOwner] = useState<boolean>(false);
    const [roomInfo, setRoomInfo] = useState<RoomInfo>();
    const [gameInfo, setGameInfo] = useState<GameInfo>();

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl("/hubs/checkers")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Debug)
            .build();
        setConnection(newConnection);
    }, [])

    useEffect(() => {
        if (connection) {
            console.log("connecting...");

            connection
                .start()
                .then(() => {
                    console.log("Connection Succeeded");
                }).catch(err => {
                    console.error("Connection failed: ", err);
                });

            connection.on(HubMethods.sendPlayerInfo, (player: Player, isRoomOwner: boolean) => {
                console.log(player);
                setPlayer(player);
                setIsRoomOwner(isRoomOwner);
            });

            connection.on(HubMethods.sendRoomInfo, (roomInfo: RoomInfo) => {
                console.log(roomInfo);
                setRoomInfo(roomInfo);
            });

            connection.on(HubMethods.sendGameInfo, (gameInfo: GameInfo) => {
                console.log(JSON.stringify(gameInfo));
                setGameInfo(gameInfo);
            });

            // connection.on(HubMethods.sendMessage, (sender: string, message: string) => {
            //     console.log(message);
            // });
        }
    }, [connection])

    const createRoom = async (name: string) => {
        try {
            console.log("creating room");
            await connection?.send(HubMethods.createRoom, name, null);
        }
        catch (e) {
            console.error(e);
        }
    }

    const joinRoom = async (roomId: string, name: string) => {
        try {
            console.log("creating room");
            await connection?.send(HubMethods.joinRoom, roomId, name);
        }
        catch (e) {
            console.error(e);
        }
    }

    const startGame = async () => {
        try {
            console.log("starting game");
            await connection?.send(HubMethods.startGame);
        }
        catch (e) {
            console.error(e);
        }
    }
    // const makeMove = async (moveRequest: MoveRequest) => {
    //     try {
    //         if (yourTurn && !isGameOver) {
    //             await connection?.send(HubMethods.makeMove, moveRequest);
    //         }
    //     }
    //     catch (e) {
    //         console.error(e);
    //     }
    // }
    return (
        <>
            {!roomInfo && <JoinView onCreateRoom={createRoom} onJoinRoom={joinRoom} />}
            {roomInfo && <RoomView roomInfo={roomInfo} />}
            {roomInfo?.roomGuest && !gameInfo && <button type="button" onClick={startGame}>Start Game</button>}
            {gameInfo && <>
                <PlayerIndicatorComponent yourTurn={gameInfo.nextPlayerTurn.playerId === player?.playerId} />
                <BoardComponent currentTurnId={gameInfo.nextPlayerTurn.playerId} yourId={player?.playerId!} board={gameInfo.board} isReversed={isRoomOwner} />
            </>}
        </>
    )
}

export default GameView;