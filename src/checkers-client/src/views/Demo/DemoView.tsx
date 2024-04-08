import { FC } from "react";
import BoardComponent from "../../components/game/board/BoardComponent";
import Location from "../../models/game/location";
import Move from "../../models/game/move";
import PlayerIndicatorComponent from "../../components/game/player-indicator/PlayerIndicatorComponent";
import styles from "./DemoView.module.css";
import JoinRoomComponent from "../../components/room/join-room/JoinRoomComponent";
import CreateRoomComponent from "../../components/room/create-room/CreateRoomComponent";
import RoomView from "../../components/room/room-info/RoomInfoComponent";
import RoomInfo from "../../models/room/roomInfo";
import Player from "../../models/game/player";
import PieceComponent from "../../components/game/piece/PieceComponent";
import ValidMove from "../../models/game/validMove";

const DemoView: FC = (): JSX.Element => {
  // const room = new RoomInfo('AB123', new Player("p1", "Emmanuel"), new Player("p2", "Sally"));
  const gameInfo = JSON.parse(`{"roomId":"LKJ25","isGameOver":false,"nextPlayerTurn":{"playerId":"3l50co-BB6ciXJ8u0KV8gQ","name":"Emmanuel"},"winner":null,"board":
  [[{"ownerId":"3l50co-BB6ciXJ8u0KV8gQ","state":"Regular"},null,null,null,null,null,null,null],
   [null,{"ownerId":"3l50co-BB6ciXJ8u0KV8gQ","state":"Regular"},null,{"ownerId":"3l50co-BB6ciXJ8u0KV8gQ","state":"Regular"},null,{"ownerId":"3l50co-BB6ciXJ8u0KV8gQ","state":"Regular"},null,{"ownerId":"3l50co-BB6ciXJ8u0KV8gQ","state":"Regular"}],
   [{"ownerId":"3l50co-BB6ciXJ8u0KV8gQ","state":"Regular"},null,null,null,null,null,null,null],
   [null,null,null,{"ownerId":"3l50co-BB6ciXJ8u0KV8gQ","state":"Regular"},null,{"ownerId":"3l50co-BB6ciXJ8u0KV8gQ","state":"Regular"},null,null],
   [null,null,null,null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"King"},null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null],
   [null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null,null,null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"}],
   [{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null],
   [null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"},null,{"ownerId":"L3j9lyBJIgaLY-0TTvW1eA","state":"Regular"}]]}`);
  // const currentTurn = "xUgx2RvbinS6RsPX3UBFvg"; //room owner
  const currentTurn = "L3j9lyBJIgaLY-0TTvW1eA";
  const isReversed = false
  const room = new RoomInfo('AB123', new Player("p1", "Emmanuel"));
  const validLocations = [new Location(4, 2), new Location(4, 4)];
  // const moves = [
  //   { destination: new Location(2, 4), moveSeq: [new Move(new Location(4, 6), new Location(2, 4))] },
  //   { destination: new Location(3, 7), moveSeq: [new Move(new Location(4, 6), new Location(3, 7))] },
  //   { destination: new Location(0, 2), moveSeq: [new Move(new Location(4, 6), new Location(2, 4)), new Move(new Location(2, 4), new Location(0, 2))] },
  //   { destination: new Location(0, 6), moveSeq: [new Move(new Location(4, 6), new Location(2, 4)), new Move(new Location(2, 4), new Location(0, 6))] }
  // ]

  const moves: ValidMove[] = [
    new ValidMove(new Location(5, 3), [new Move(new Location(4, 4), new Location(5, 4))]),
    new ValidMove(new Location(2, 6), [new Move(new Location(4, 4), new Location(2, 6))]),
    new ValidMove(new Location(2, 2), [new Move(new Location(4, 4), new Location(2, 2))]),
    new ValidMove(new Location(0, 4), [new Move(new Location(4, 4), new Location(2, 6)), new Move(new Location(2, 6), new Location(0, 4))]),
    new ValidMove(new Location(0, 4), [new Move(new Location(4, 4), new Location(2, 2)), new Move(new Location(2, 2), new Location(0, 4))]),
  ]

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
      <RoomView roomInfo={room} />
      <PlayerIndicatorComponent yourTurn={true} />
      <BoardComponent currentTurnId={currentTurn} yourId={currentTurn} board={gameInfo.board} isReversed={false} validMoves={moves} getValidMoves={() =>{}} makeMove={(moves: Move[]) => {}}/>
    </div>
  );
};

export default DemoView;
