export default class Review {
    id: string;
    playerId?: string;
    content: string;
    postedOn: string;

    constructor(id: string, content: string, postedOn: string, playerId?: string) {
        this.id = id;
        this.playerId = playerId;
        this.content = content;
        this.postedOn = postedOn;
    }
}