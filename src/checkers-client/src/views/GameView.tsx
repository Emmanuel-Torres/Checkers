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
import RoomView from "./Room/RoomView";

const GameView: FC = (): JSX.Element => {
    const [connection, setConnection] = useState<HubConnection>();
    const [roomInfo, setRoomInfo] = useState<RoomInfo>();
    // const [isLoading, setIsLoading] = useState<boolean>(true);
    // const [isMatchMaking, setIsMatchMaking] = useState<boolean>(false);
    // const [inGame, setInGame] = useState<boolean>(false);
    // const [isGameOver, setIsGameOver] = useState<boolean>(false);
    // const [board, setBoard] = useState<Square[]>([]);
    // const [validLocations, setValidMoves] = useState<BoardLocation[]>([]);
    // const [yourTurn, setYourTurn] = useState<boolean>(false);
    // const [yourColor, setYourColor] = useState<string>("");
    // const [winner, setWinner] = useState<string>("");

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

            connection.on(HubMethods.sendRoomInfo, (roomInfo: RoomInfo) => {
                console.log(roomInfo);
                setRoomInfo(roomInfo);
            });

            // connection.on(HubMethods.moveSuccessful, (board: Square[]) => {
            //     setYourTurn(false);
            //     setBoard(board);
            //     setValidMoves([]);
            //     connection.send(HubMethods.moveCompleted);
            // });
            // connection.on(HubMethods.sendJoinConfirmation, (name: string, color: string, board: Square[]) => {
            //     setIsMatchMaking(false);
            //     setInGame(true);
            //     setYourColor(color);
            //     setBoard(board);
            // });
            // connection.on(HubMethods.sendValidMoveLocations, (locations: BoardLocation[]) => {
            //     setValidMoves(locations);
            // });
            // connection.on(HubMethods.sendMessage, (sender: string, message: string) => {
            //     console.log(message);
            // });
            // connection.on(HubMethods.yourTurnToMove, (board: Square[]) => {
            //     setYourTurn(true);
            //     setBoard(board);
            // });
            // connection.on(HubMethods.gameOver, (winner: string, board: Square[]) => {
            //     setIsGameOver(true);
            //     setWinner(winner);
            //     setBoard(board);
            // });

            // setIsLoading(false);
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

    // const matchMake = async () => {
    //     try {
    //         console.log("finding game");
    //         setIsMatchMaking(true);
    //         await connection?.send(HubMethods.matchMake);
    //     }
    //     catch (e) {
    //         console.error(e);
    //     }
    // }
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
    // const getValidMoves = async (source: BoardLocation) => {
    //     try {
    //         if (yourTurn && !isGameOver) {
    //             await connection?.send(HubMethods.getValidMoves, source);
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
            {/* {isLoading && <h2>Loading, please wait</h2>}
            {!isLoading && !isMatchMaking && !inGame && <button type="button" onClick={matchMake}>Match Make</button>}
            {isMatchMaking && <h2>You are MatchMaking, please wait</h2>}
            {inGame && <>
                <PlayerIndicatorComponent yourTurn={yourTurn} />
                <BoardComponent board={board} isReversed={yourColor === "White"} validLocations={validLocations} onGetValidMoves={getValidMoves} onMakeMove={makeMove} />
            </>}
            {isGameOver && <h2>Player {winner} won!</h2>} */}
        </>
    )
}

export default GameView;