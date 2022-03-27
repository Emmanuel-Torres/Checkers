export default class User {
  id: number;
  givenName: string;
  familyName: string;
  picture: string;
  email: string;
  bestJoke?: string;
  iceCreamFlavor?: string;
  pizza?: string;
  age?: number;

  constructor(
    id: number,
    givenName: string,
    familyName: string,
    picture: string,
    email: string,
    joke: string,
    iceCream: string,
    pizza: string,
    age: number
  ) {
    this.id = id;
    this.givenName = givenName;
    this.familyName = familyName;
    this.picture = picture;
    this.email = email;
    this.bestJoke = joke;
    this.iceCreamFlavor = iceCream;
    this.pizza = pizza;
    this.age = age;
  }
}
