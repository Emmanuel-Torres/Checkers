import Location from "./location";
import Move from "./move";

export default class ValidMove {
    public readonly destination: Location;
    public readonly moveSequence: Move[];

    constructor (destination: Location, moveSequence: Move[]) {
        this.destination = destination;
        this.moveSequence = moveSequence;
    }
}