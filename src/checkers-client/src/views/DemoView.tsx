import { FC } from "react";
import BoardComponent from "../components/game/board/BoardComponent";
import BoardLocation from "../models/game/location";
import MoveRequest from "../models/game/moveRequest";
import PlayerIndicatorComponent from "../components/game/player-indicator/PlayerIndicatorComponent";

const DemoView: FC = (): JSX.Element => {
    const board = JSON.parse(`[{"location":{"row":0,"column":0},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":0,"column":1},"color":"White","piece":null,"isOccupied":false},{"location":{"row":0,"column":2},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":0,"column":3},"color":"White","piece":null,"isOccupied":false},{"location":{"row":0,"column":4},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"King"},"isOccupied":true},{"location":{"row":0,"column":5},"color":"White","piece":null,"isOccupied":false},{"location":{"row":0,"column":6},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":0,"column":7},"color":"White","piece":null,"isOccupied":false},{"location":{"row":1,"column":0},"color":"White","piece":null,"isOccupied":false},{"location":{"row":1,"column":1},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":1,"column":2},"color":"White","piece":null,"isOccupied":false},{"location":{"row":1,"column":3},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":1,"column":4},"color":"White","piece":null,"isOccupied":false},{"location":{"row":1,"column":5},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":1,"column":6},"color":"White","piece":null,"isOccupied":false},{"location":{"row":1,"column":7},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":2,"column":0},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":2,"column":1},"color":"White","piece":null,"isOccupied":false},{"location":{"row":2,"column":2},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":2,"column":3},"color":"White","piece":null,"isOccupied":false},{"location":{"row":2,"column":4},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":2,"column":5},"color":"White","piece":null,"isOccupied":false},{"location":{"row":2,"column":6},"color":"Black","piece":{"color":"White","ownerId":"DBDKJUvGs0BvCeQA-chWVA","state":"Regular"},"isOccupied":true},{"location":{"row":2,"column":7},"color":"White","piece":null,"isOccupied":false},{"location":{"row":3,"column":0},"color":"White","piece":null,"isOccupied":false},{"location":{"row":3,"column":1},"color":"Black","piece":null,"isOccupied":false},{"location":{"row":3,"column":2},"color":"White","piece":null,"isOccupied":false},{"location":{"row":3,"column":3},"color":"Black","piece":null,"isOccupied":false},{"location":{"row":3,"column":4},"color":"White","piece":null,"isOccupied":false},{"location":{"row":3,"column":5},"color":"Black","piece":null,"isOccupied":false},{"location":{"row":3,"column":6},"color":"White","piece":null,"isOccupied":false},{"location":{"row":3,"column":7},"color":"Black","piece":null,"isOccupied":false},{"location":{"row":4,"column":0},"color":"Black","piece":null,"isOccupied":false},{"location":{"row":4,"column":1},"color":"White","piece":null,"isOccupied":false},{"location":{"row":4,"column":2},"color":"Black","piece":null,"isOccupied":false},{"location":{"row":4,"column":3},"color":"White","piece":null,"isOccupied":false},{"location":{"row":4,"column":4},"color":"Black","piece":null,"isOccupied":false},{"location":{"row":4,"column":5},"color":"White","piece":null,"isOccupied":false},{"location":{"row":4,"column":6},"color":"Black","piece":null,"isOccupied":false},{"location":{"row":4,"column":7},"color":"White","piece":null,"isOccupied":false},{"location":{"row":5,"column":0},"color":"White","piece":null,"isOccupied":false},{"location":{"row":5,"column":1},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":5,"column":2},"color":"White","piece":null,"isOccupied":false},{"location":{"row":5,"column":3},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":5,"column":4},"color":"White","piece":null,"isOccupied":false},{"location":{"row":5,"column":5},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":5,"column":6},"color":"White","piece":null,"isOccupied":false},{"location":{"row":5,"column":7},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":6,"column":0},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":6,"column":1},"color":"White","piece":null,"isOccupied":false},{"location":{"row":6,"column":2},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":6,"column":3},"color":"White","piece":null,"isOccupied":false},{"location":{"row":6,"column":4},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":6,"column":5},"color":"White","piece":null,"isOccupied":false},{"location":{"row":6,"column":6},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":6,"column":7},"color":"White","piece":null,"isOccupied":false},{"location":{"row":7,"column":0},"color":"White","piece":null,"isOccupied":false},{"location":{"row":7,"column":1},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":7,"column":2},"color":"White","piece":null,"isOccupied":false},{"location":{"row":7,"column":3},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":7,"column":4},"color":"White","piece":null,"isOccupied":false},{"location":{"row":7,"column":5},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"Regular"},"isOccupied":true},{"location":{"row":7,"column":6},"color":"White","piece":null,"isOccupied":false},{"location":{"row":7,"column":7},"color":"Black","piece":{"color":"Black","ownerId":"TyXSydiCF3vrUMltt9B7RQ","state":"King"},"isOccupied":true}]`);
    const validLocations = [new BoardLocation(4, 2), new BoardLocation(4, 4)]

    return (
        <>
            <PlayerIndicatorComponent yourColor={"Black"} opponentColor={"White"}/>
            {/* <PlayerIndicatorComponent yourColor={"White"} opponentColor={"Black"}/> */}
            <BoardComponent board={board} isReversed={false} validLocations={validLocations} onGetValidMoves={(location: BoardLocation) => { }} onMakeMove={(request: MoveRequest) => { }} />
        </>
    )
}

export default DemoView;