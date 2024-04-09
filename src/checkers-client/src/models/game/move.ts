import Location from "./location";

export default class Move {
    public readonly source: Location;
    public readonly destination: Location;

    constructor(source: Location, destination: Location) {
        this.source = source;
        this.destination = destination;
    }
}
