import Square from "./square";

export default class MoveResult {
    public readonly gameid: string;
    public readonly wasmovesuccessful: boolean;
    public readonly isgameover: boolean;
    public readonly board: Square[];

    constructor(gameid: string, 
        wasmovesuccessful: boolean,
        isgameover: boolean,
        board: Square[]) {
        this.gameid = gameid;
        this.wasmovesuccessful = wasmovesuccessful;
        this.isgameover = isgameover;
        this.board = board;
    }
}
