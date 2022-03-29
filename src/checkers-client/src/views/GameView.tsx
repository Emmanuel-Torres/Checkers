import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { FC, useEffect, useState } from "react";
import MatchMake from "../components/matchmake/MatchMake";

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
        console.log("Here")
        console.log(connection);
        if (connection) {
            console.log("Here again")
            connection.start()
                .then(_ => {
                    console.log("Connection Succeeded");
                })
                .catch(err => {
                    console.error("Connection failed: ", err);
                })
        }
    }, [connection])
    return (
        <>
            <MatchMake />
        </>
    )
}

export default GameView;