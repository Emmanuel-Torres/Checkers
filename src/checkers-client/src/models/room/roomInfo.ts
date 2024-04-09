import Player from "../game/player";

export default class RoomInfo {
  public readonly roomId: string;
  public readonly roomOwner: Player;
  public readonly roomGuest?: Player;

  constructor (roomId: string, roomOwner: Player, roomGuest?: Player) {
    this.roomId = roomId;
    this.roomOwner = roomOwner;
    this.roomGuest = roomGuest;
  }
}
