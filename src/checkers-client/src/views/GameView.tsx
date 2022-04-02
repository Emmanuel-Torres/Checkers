import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { FC, useEffect, useState } from "react";
import BoardComponent from "../components/game/board/BoardComponent";
import BoardLocation from "../game-models/location";
import MoveRequest from "../game-models/moveRequest";
import MoveResult from "../game-models/moveResult";
import Square from "../game-models/square";
import HubMethods from "../models/hub-methods";

const GameView: FC = (): JSX.Element => {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [isMatchMaking, setIsMatchMaking] = useState<boolean>(false);
    const [inGame, setInGame] = useState<boolean>(false);
    const [connection, setConnection] = useState<HubConnection>();
    const [board, setBoard] = useState<Square[]>([]);
    const [validLocations, setValidMoves] = useState<BoardLocation[]>([]);

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
            connection.on(HubMethods.moveSuccessful, (moveResult: MoveResult) => {
                setBoard(moveResult.board);
            })

            connection.on(HubMethods.sendJoinConfirmation, (name: string, board: Square[]) => {
                console.log(name);
                console.log(JSON.stringify(board));
                setIsMatchMaking(false);
                setInGame(true);
                setBoard(board);
            })

            connection.on(HubMethods.sendMessage, (sender: string, message: string) => {
                console.log(message);
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
            await connection?.send(HubMethods.makeMove, moveRequest);
        }
        catch (e) {
            console.error(e);
        }
    }
    const getValidMoves = async (source: BoardLocation) => {
        try {
            await connection?.send(HubMethods.getValidMoves, source);
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
                <h2>You are currently in a game</h2>
                <BoardComponent board={board} validLocations={validLocations} onGetValidMoves={getValidMoves}/>
            </>}
        </>
    )
}

export default GameView;