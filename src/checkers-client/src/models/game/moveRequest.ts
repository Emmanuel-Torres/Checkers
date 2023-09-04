import BoardLocation from "./location";

export default class MoveRequest {
    public readonly source: BoardLocation;
    public readonly destination: BoardLocation;

    constructor(source: BoardLocation, destination: BoardLocation) {
        this.source = source;
        this.destination = destination;
    }
}
