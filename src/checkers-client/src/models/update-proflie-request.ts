export default class UpdateProfileRequest {
  picture?: File;
  bestJoke?: string;
  iceCreamFlavor?: string;
  pizza?: string;
  age?: number;

  constructor(
    joke?: string,
    iceCream?: string,
    pizza?: string,
    age?: number,
    picture?: File
  ) {
    this.bestJoke = joke;
    this.iceCreamFlavor = iceCream;
    this.pizza = pizza;
    this.picture = picture;
    this.age = age;
  }
}
