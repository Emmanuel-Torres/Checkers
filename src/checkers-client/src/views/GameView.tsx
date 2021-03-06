import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { FC, useEffect, useState } from "react";
import BoardComponent from "../components/game/board/BoardComponent";
import BoardLocation from "../game-models/location";
import MoveRequest from "../game-models/moveRequest";
import Square from "../game-models/square";
import HubMethods from "../models/hub-methods";

const GameView: FC = (): JSX.Element => {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [isMatchMaking, setIsMatchMaking] = useState<boolean>(false);
    const [inGame, setInGame] = useState<boolean>(false);
    const [isGameOver, setIsGameOver] = useState<boolean>(false);
    const [connection, setConnection] = useState<HubConnection>();
    const [board, setBoard] = useState<Square[]>([]);
    const [validLocations, setValidMoves] = useState<BoardLocation[]>([]);
    const [yourTurn, setYourTurn] = useState<boolean>(false);
    const [yourColor, setYourColor] = useState<string>("");
    const [winner, setWinner] = useState<string>("");

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl("/hubs/checkers")
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
    }, [])

    useEffect(() => {
        if (connection) {
            connection.start().then(_ => {
                console.log("Connection Succeeded");
            }).catch(err => {
                console.error("Connection failed: ", err);
            });

            connection.on(HubMethods.moveSuccessful, (board: Square[]) => {
                setYourTurn(false);
                setBoard(board);
                setValidMoves([]);
                connection.send(HubMethods.moveCompleted);
            });
            connection.on(HubMethods.sendJoinConfirmation, (name: string, color: string, board: Square[]) => {
                setIsMatchMaking(false);
                setInGame(true);
                setYourColor(color);
                setBoard(board);
            });
            connection.on(HubMethods.sendValidMoveLocations, (locations: BoardLocation[]) => {
                setValidMoves(locations);
            });
            connection.on(HubMethods.sendMessage, (sender: string, message: string) => {
                console.log(message);
            });
            connection.on(HubMethods.yourTurnToMove, (board: Square[]) => {
                setYourTurn(true);
                setBoard(board);
            });
            connection.on(HubMethods.gameOver, (winner: string, board: Square[]) => {
                setIsGameOver(true);
                setWinner(winner);
                setBoard(board);
            });

            setIsLoading(false);
        }
    }, [connection])

    const matchMake = async () => {
        try {
            setIsMatchMaking(true);
            await connection?.send(HubMethods.matchMake, undefined);
        }
        catch (e) {
            console.error(e);
        }
    }
    const makeMove = async (moveRequest: MoveRequest) => {
        try {
            if (yourTurn && !isGameOver) {
                await connection?.send(HubMethods.makeMove, moveRequest);
            }
        }
        catch (e) {
            console.error(e);
        }
    }
    const getValidMoves = async (source: BoardLocation) => {
        try {
            if (yourTurn && !isGameOver) {
                await connection?.send(HubMethods.getValidMoves, source);
            }
        }
        catch (e) {
            console.error(e);
        }
    }
    return (
        <>
            {isLoading && <h2>Loading, please wait</h2>}
            {!isLoading && !isMatchMaking && !inGame && <button type="button" onClick={matchMake}>Match Make</button>}
            {isMatchMaking && <h2>You are MatchMaking, please wait</h2>}
            {inGame && <>
                <h2>You are playing {yourColor} pieces</h2>
                <h2>{yourTurn ? "Its your turn" : "Waiting for opponent"}</h2>
                <BoardComponent board={board} validLocations={validLocations} onGetValidMoves={getValidMoves} onMakeMove={makeMove} />
            </>}
            {isGameOver && <h2>Player {winner} won!</h2>}
        </>
    )
}

export default GameView;