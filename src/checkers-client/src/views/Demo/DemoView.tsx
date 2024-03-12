import { FC } from "react";
import BoardComponent from "../../components/game/board/BoardComponent";
import BoardLocation from "../../models/game/location";
import MoveRequest from "../../models/game/moveRequest";
import PlayerIndicatorComponent from "../../components/game/player-indicator/PlayerIndicatorComponent";
import styles from "./DemoView.module.css";
import JoinRoomComponent from "../../components/room/join-room/JoinRoomComponent";
import CreateRoomComponent from "../../components/room/create-room/CreateRoomComponent";
import RoomView from "../../components/room/room-info/RoomInfoComponent";
import RoomInfo from "../../models/room/roomInfo";
import Player from "../../models/game/player";
import PieceComponent from "../../components/game/piece/PieceComponent";

const DemoView: FC = (): JSX.Element => {
  // const room = new RoomInfo('AB123', new Player("p1", "Emmanuel"), new Player("p2", "Sally"));
  const gameInfo = JSON.parse(`{"roomId":"10E6H","isGameOver":false,"nextPlayerTurn":{"playerId":"xUgx2RvbinS6RsPX3UBFvg","name":"Emmanuel"},"winner":null,"board":[{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,{"ownerId":"xUgx2RvbinS6RsPX3UBFvg","state":"Regular"},null,null,null,null,null,null,null,null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"King"},null,null,null,null,null,null,null,null,null,null,null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},null,null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"},null,{"ownerId":"8YPHrkNXKobAze68aASORQ","state":"Regular"}]}`);
  // const currentTurn = "xUgx2RvbinS6RsPX3UBFvg"; //room owner
  const currentTurn = "8YPHrkNXKobAze68aASORQ";
  const isReversed = false
  const room = new RoomInfo('AB123', new Player("p1", "Emmanuel"));
  const validLocations = [new BoardLocation(4, 2), new BoardLocation(4, 4)];

  /* <PlayerIndicatorComponent
  yourTurn={false} />
<BoardComponent
  board={board}
  isReversed={false}
  validLocations={validLocations}
  onGetValidMoves={(location: BoardLocation) => {}}
  onMakeMove={(request: MoveRequest) => {}} /> */

  return (
    <div>
      <RoomView roomInfo={room}/>
      <PlayerIndicatorComponent yourTurn={false} />
      <BoardComponent currentTurnId={currentTurn} yourId={currentTurn} board={gameInfo.board} isReversed={false}/>
    </div>
  );
};

export default DemoView;
