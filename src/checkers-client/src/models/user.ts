export default class User {
  id: number;
  givenName: string;
  familyName: string;
  picture: string;
  email: string;
  joke?: string;
  iceCream?: string;
  pizza?: string;
  age?: string;

  constructor(
    id: number,
    givenName: string,
    familyName: string,
    picture: string,
    email: string,
    joke: string,
    iceCream: string,
    pizza: string,
    age: string
  ) {
    this.id = id;
    this.givenName = givenName;
    this.familyName = familyName;
    this.picture = picture;
    this.email = email;
    this.joke = joke;
    this.iceCream = iceCream;
    this.pizza = pizza;
    this.age = age;
  }
}
