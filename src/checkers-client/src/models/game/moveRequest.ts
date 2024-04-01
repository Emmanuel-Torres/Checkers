import Location from "./location";

export default class MoveRequest {
    public readonly source: Location;
    public readonly destination: Location;

    constructor(source: Location, destination: Location) {
        this.source = source;
        this.destination = destination;
    }
}
