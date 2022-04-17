export default class Review {
    id: string;
    playerName: string;
    content: string;
    postedOn: string;

    constructor(id: string, playerName: string, content: string, postedOn: string) {
        this.id = id;
        this.playerName = playerName;
        this.content = content;
        this.postedOn = postedOn;
    }
}