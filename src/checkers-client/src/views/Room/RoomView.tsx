import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { FC, useEffect, useState } from "react";
import BoardComponent from "../../components/game/board/BoardComponent";
import Location from "../../models/game/location";
import Move from "../../models/game/move";
import HubMethods from "../../models/helper/hub-methods";
import PlayerIndicatorComponent from "../../components/game/player-indicator/PlayerIndicatorComponent";
import RoomInfo from "../../models/room/roomInfo";
import JoinView from "../Join/JoinView";
import RoomInfoComponent from "../../components/room/room-info/RoomInfoComponent";
import GameInfo from "../../models/game/gameInfo";
import Player from "../../models/game/player";
import ValidMove from "../../models/game/validMove";
import styles from "./RoomView.module.css"

const RoomView: FC = (): JSX.Element => {
    const [connection, setConnection] = useState<HubConnection>();
    const [player, setPlayer] = useState<Player>();
    const [isRoomOwner, setIsRoomOwner] = useState<boolean>(false);
    const [roomInfo, setRoomInfo] = useState<RoomInfo>();
    const [gameInfo, setGameInfo] = useState<GameInfo>();
    const [validMoves, setValidMoves] = useState<ValidMove[]>([]);

    useEffect(() => {
        console.log(connection);
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
                console.log(player.playerId);
                setPlayer(player);
                setIsRoomOwner(isRoomOwner);
            });

            connection.on(HubMethods.sendRoomInfo, (roomInfo: RoomInfo) => {
                console.log(roomInfo);
                setRoomInfo(roomInfo);
            });

            connection.on(HubMethods.sendGameInfo, (gameInfo: GameInfo) => {
                setGameInfo(gameInfo);
                setValidMoves([]);
            });

            connection.on(HubMethods.sendValidMoves, (source: Location, validMoves: ValidMove[]) => {
                console.log("valid moves for:" + source);
                setValidMoves(validMoves);
            });
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

    const getValidMoves = async (source: Location) => {
        try {
            console.log("getting valid moves");
            await connection?.send(HubMethods.getValidMoves, source);
        }
        catch (e) {
            console.error(e);
        }
    }

    const makeMove = async (moves: Move[]) => {
        try {
            console.log("making move");
            console.log(moves);
            await connection?.send(HubMethods.makeMove, moves)
        }
        catch (e) {
            console.error(e);
        }
    }

    return (
        <div className={styles.container}>
            {!roomInfo && <JoinView onCreateRoom={createRoom} onJoinRoom={joinRoom} />}
            {roomInfo && <RoomInfoComponent roomInfo={roomInfo} gameInfo={gameInfo} />}
            {roomInfo?.roomGuest && !gameInfo && <button className={styles.button} onClick={startGame}>Start Game</button>}
            {roomInfo?.roomGuest && gameInfo && gameInfo.winner && <button className={styles.button} onClick={startGame}>Start New Game</button>}
            {gameInfo && <PlayerIndicatorComponent player={player!} gameInfo={gameInfo} />}
            {gameInfo && <BoardComponent currentTurnId={gameInfo.nextPlayerTurn?.playerId} yourId={player?.playerId!} board={gameInfo.board} isReversed={isRoomOwner} validMoves={validMoves} getValidMoves={getValidMoves} makeMove={makeMove} />}
        </div>
    )
}

export default RoomView;