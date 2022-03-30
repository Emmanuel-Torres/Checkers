import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { FC, useEffect, useState } from "react";
import MatchMake from "../components/matchmake/MatchMake";
import Square from "../game-models/square";
import HubMethods from "../models/hub-methods";

const GameView: FC = (): JSX.Element => {
    const [connection, setConnection] = useState<HubConnection>();

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

            connection.on(HubMethods.sendJoinConfirmation, (name: string, data: Square[]) => {
                console.log(name);
            })

            connection.on(HubMethods.sendMessage, (sender: string, message: string) => {
                console.log(message);
            });
        }
    }, [connection])

    const matchMake = async () => {
        try {
            await connection?.send(HubMethods.matchMake, undefined);
        }
        catch (e) {
            console.error(e);
        }
    }
    return (
        <>
            <button type="button" onClick={matchMake}>Match Make</button>
        </>
    )
}

export default GameView;