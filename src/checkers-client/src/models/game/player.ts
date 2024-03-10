export default class Player {
    public readonly playerId: string;
    public readonly name: string;

    constructor(playerId: string, name: string) {
        this.playerId = playerId;
        this.name = name;
    }
}